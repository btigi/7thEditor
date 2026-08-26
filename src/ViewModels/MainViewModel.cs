using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ii.EighthSolitude;
using Microsoft.Win32;
using SeventhEditor.Catalog;

namespace SeventhEditor.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly List<StuffDatTypeId> _fileOrder = [];
    private bool _weaponsFromFile;

    public MainViewModel()
    {
        Units = new ObservableCollection<UnitRowViewModel>(
            UnitCatalog.AllUnits.Select(id => CreateUnitRow(id, UnitCatalog.CreateExeDefault(id), enabled: false)));
        Buildings = new ObservableCollection<UnitRowViewModel>(
            UnitCatalog.AllBuildings.Select(id => CreateUnitRow(id, UnitCatalog.CreateExeDefault(id), enabled: false)));
        Weapons = new ObservableCollection<WeaponRowViewModel>(
            Enumerable.Range(0, StuffDatProcessor.WeaponRecordCount)
                .Select(i => new WeaponRowViewModel(i, UnitCatalog.CreateExeWeaponDefault(i), MarkDirty)));

        SelectedUnit = Units.FirstOrDefault();
        SelectedBuilding = Buildings.FirstOrDefault();
        SelectedWeapon = Weapons.FirstOrDefault();
        UpdateWindowTitle();
    }

    public ObservableCollection<UnitRowViewModel> Units { get; }

    public ObservableCollection<UnitRowViewModel> Buildings { get; }

    public ObservableCollection<WeaponRowViewModel> Weapons { get; }

    public IReadOnlyList<NamedValue> ArmourClasses => WeaponNames.ArmourClasses;

    public IReadOnlyList<NamedValue> WeaponTypes => WeaponNames.WeaponTypes;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsCommand))]
    private string? filePath;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WindowTitle))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private bool isDirty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EnableUnitCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisableUnitCommand))]
    private UnitRowViewModel? selectedUnit;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EnableBuildingCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisableBuildingCommand))]
    private UnitRowViewModel? selectedBuilding;

    [ObservableProperty]
    private WeaponRowViewModel? selectedWeapon;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WindowTitle))]
    [NotifyCanExecuteChangedFor(nameof(EnableUnitCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisableUnitCommand))]
    [NotifyCanExecuteChangedFor(nameof(EnableBuildingCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisableBuildingCommand))]
    private bool hasFile;

    public string WindowTitle
    {
        get
        {
            var name = string.IsNullOrEmpty(FilePath) ? "7thEditor" : $"7thEditor — {FilePath}";
            return IsDirty ? name + " *" : name;
        }
    }

    [RelayCommand]
    private void Open()
    {
        if (!ConfirmDiscardChanges())
        {
            return;
        }

        var dialog = new OpenFileDialog
        {
            Title = "Open stuff.dat",
            Filter = "STUFF files (stuff.dat)|stuff.dat|DAT files (*.dat)|*.dat|All files (*.*)|*.*",
            FileName = "stuff.dat",
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            LoadFile(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open file:\n{ex.Message}", "7thEditor", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Exit()
    {
        if (!ConfirmDiscardChanges())
        {
            return;
        }

        Application.Current.Shutdown();
    }

    [RelayCommand]
    private void About()
    {
        var about = new AboutWindow
        {
            Owner = Application.Current.MainWindow,
        };
        about.ShowDialog();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            SaveAs();
            return;
        }

        try
        {
            WriteFile(FilePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save file:\n{ex.Message}", "7thEditor", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanSave() => HasFile && IsDirty;

    [RelayCommand(CanExecute = nameof(CanSaveAs))]
    private void SaveAs()
    {
        if (!HasFile)
        {
            return;
        }

        var dialog = new SaveFileDialog
        {
            Title = "Save stuff.dat",
            Filter = "STUFF files (stuff.dat)|stuff.dat|DAT files (*.dat)|*.dat|All files (*.*)|*.*",
            FileName = string.IsNullOrEmpty(FilePath) ? "stuff.dat" : Path.GetFileName(FilePath),
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            WriteFile(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save file:\n{ex.Message}", "7thEditor", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanSaveAs() => HasFile;

    [RelayCommand(CanExecute = nameof(CanEnableUnit))]
    private void EnableUnit()
    {
        SelectedUnit?.EnableFromCurrent();
        EnableUnitCommand.NotifyCanExecuteChanged();
        DisableUnitCommand.NotifyCanExecuteChanged();
    }

    private bool CanEnableUnit() => HasFile && SelectedUnit is { IsEnabled: false };

    [RelayCommand(CanExecute = nameof(CanDisableUnit))]
    private void DisableUnit()
    {
        SelectedUnit?.DisableToExeDefault();
        EnableUnitCommand.NotifyCanExecuteChanged();
        DisableUnitCommand.NotifyCanExecuteChanged();
    }

    private bool CanDisableUnit() => HasFile && SelectedUnit is { IsEnabled: true };

    [RelayCommand(CanExecute = nameof(CanEnableBuilding))]
    private void EnableBuilding()
    {
        SelectedBuilding?.EnableFromCurrent();
        EnableBuildingCommand.NotifyCanExecuteChanged();
        DisableBuildingCommand.NotifyCanExecuteChanged();
    }

    private bool CanEnableBuilding() => HasFile && SelectedBuilding is { IsEnabled: false };

    [RelayCommand(CanExecute = nameof(CanDisableBuilding))]
    private void DisableBuilding()
    {
        SelectedBuilding?.DisableToExeDefault();
        EnableBuildingCommand.NotifyCanExecuteChanged();
        DisableBuildingCommand.NotifyCanExecuteChanged();
    }

    private bool CanDisableBuilding() => HasFile && SelectedBuilding is { IsEnabled: true };

    partial void OnSelectedUnitChanged(UnitRowViewModel? value)
    {
        EnableUnitCommand.NotifyCanExecuteChanged();
        DisableUnitCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedBuildingChanged(UnitRowViewModel? value)
    {
        EnableBuildingCommand.NotifyCanExecuteChanged();
        DisableBuildingCommand.NotifyCanExecuteChanged();
    }

    partial void OnFilePathChanged(string? value) => UpdateWindowTitle();

    partial void OnIsDirtyChanged(bool value) => UpdateWindowTitle();

    private void LoadFile(string path)
    {
        var processor = new StuffDatProcessor();
        var entries = processor.Read(path);

        _fileOrder.Clear();
        var byId = new Dictionary<StuffDatTypeId, StuffDatEntry>();
        foreach (var entry in entries)
        {
            byId[entry.TypeId] = entry;
            _fileOrder.Add(entry.TypeId);
        }

        foreach (var unit in Units)
        {
            if (byId.TryGetValue(unit.TypeId, out var overlay))
            {
                unit.Apply(overlay, isEnabled: true);
            }
            else
            {
                unit.Apply(UnitCatalog.CreateExeDefault(unit.TypeId), isEnabled: false);
            }
        }

        foreach (var building in Buildings)
        {
            if (byId.TryGetValue(building.TypeId, out var overlay))
            {
                building.Apply(overlay, isEnabled: true);
            }
            else
            {
                building.Apply(UnitCatalog.CreateExeDefault(building.TypeId), isEnabled: false);
            }
        }

        _weaponsFromFile = processor.Weapons.Count > 0;
        for (var i = 0; i < Weapons.Count; i++)
        {
            var source = i < processor.Weapons.Count
                ? processor.Weapons[i]
                : UnitCatalog.CreateExeWeaponDefault(i);
            Weapons[i].Apply(source);
        }

        SelectedUnit ??= Units.FirstOrDefault();
        SelectedBuilding ??= Buildings.FirstOrDefault();
        SelectedWeapon ??= Weapons.FirstOrDefault();
        FilePath = path;
        HasFile = true;
        IsDirty = false;
        SaveCommand.NotifyCanExecuteChanged();
        SaveAsCommand.NotifyCanExecuteChanged();
        EnableUnitCommand.NotifyCanExecuteChanged();
        DisableUnitCommand.NotifyCanExecuteChanged();
        EnableBuildingCommand.NotifyCanExecuteChanged();
        DisableBuildingCommand.NotifyCanExecuteChanged();
    }

    private void WriteFile(string path)
    {
        var enabled = Units.Concat(Buildings).Where(u => u.IsEnabled).ToDictionary(u => u.TypeId);

        var ordered = new List<StuffDatEntry>();
        var written = new HashSet<StuffDatTypeId>();

        foreach (var typeId in _fileOrder)
        {
            if (enabled.TryGetValue(typeId, out var row) && written.Add(typeId))
            {
                ordered.Add(row.ToEntry());
            }
        }

        foreach (var row in Units.Concat(Buildings).Where(u => u.IsEnabled))
        {
            if (written.Add(row.TypeId))
            {
                ordered.Add(row.ToEntry());
            }
        }

        var processor = new StuffDatProcessor
        {
            Weapons = Weapons.Select(w => w.ToWeapon()).ToList(),
        };
        _weaponsFromFile = true;

        processor.Write(ordered, path);

        _fileOrder.Clear();
        _fileOrder.AddRange(ordered.Select(e => e.TypeId));
        FilePath = path;
        HasFile = true;
        IsDirty = false;
        SaveCommand.NotifyCanExecuteChanged();
        SaveAsCommand.NotifyCanExecuteChanged();
    }

    private UnitRowViewModel CreateUnitRow(StuffDatTypeId typeId, StuffDatEntry entry, bool enabled)
        => new(typeId, entry, enabled, OnUnitChanged);

    private void OnUnitChanged()
    {
        MarkDirty();
        EnableUnitCommand.NotifyCanExecuteChanged();
        DisableUnitCommand.NotifyCanExecuteChanged();
        EnableBuildingCommand.NotifyCanExecuteChanged();
        DisableBuildingCommand.NotifyCanExecuteChanged();
    }

    private void MarkDirty()
    {
        if (!HasFile)
        {
            return;
        }

        IsDirty = true;
    }

    private bool ConfirmDiscardChanges()
    {
        if (!IsDirty)
        {
            return true;
        }

        var result = MessageBox.Show(
            "You have unsaved changes. Discard them?",
            "7thEditor",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);
        return result == MessageBoxResult.Yes;
    }

    private void UpdateWindowTitle() => OnPropertyChanged(nameof(WindowTitle));
}
