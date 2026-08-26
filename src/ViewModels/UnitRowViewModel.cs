using CommunityToolkit.Mvvm.ComponentModel;
using ii.EighthSolitude;
using SeventhEditor.Catalog;

namespace SeventhEditor.ViewModels;

public partial class UnitRowViewModel : ObservableObject
{
    private readonly Action _markDirty;
    private bool _suppressDirty;

    public UnitRowViewModel(StuffDatTypeId typeId, StuffDatEntry entry, bool isEnabled, Action markDirty)
    {
        TypeId = typeId;
        DisplayName = UnitCatalog.GetDisplayName(typeId);
        IsBuilding = typeId >= StuffDatTypeId.Base;
        _markDirty = markDirty;
        _suppressDirty = true;
        IsEnabled = isEnabled;
        LoadFrom(entry);
        _suppressDirty = false;
    }

    public StuffDatTypeId TypeId { get; }

    public string DisplayName { get; }

    public bool IsBuilding { get; }

    public string ListLabel => IsEnabled ? DisplayName : $"{DisplayName} (EXE default)";

    public bool CanEdit => IsEnabled;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ListLabel))]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    private bool isEnabled;

    [ObservableProperty]
    private int cost;

    [ObservableProperty]
    private int health;

    [ObservableProperty]
    private int buildTime;

    [ObservableProperty]
    private int armour;

    [ObservableProperty]
    private int weapon;

    [ObservableProperty]
    private int weapon2;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SpeedPreview))]
    private int speed;

    public string SpeedPreview => $"= {FixedPoint.Format(Speed)}  (÷ {FixedPoint.Scale})";

    [ObservableProperty]
    private int turnSpeed;

    [ObservableProperty]
    private int reloadTime;

    partial void OnIsEnabledChanged(bool value) => NotifyDirty();
    partial void OnCostChanged(int value) => NotifyDirty();
    partial void OnHealthChanged(int value) => NotifyDirty();
    partial void OnBuildTimeChanged(int value) => NotifyDirty();
    partial void OnArmourChanged(int value) => NotifyDirty();
    partial void OnWeaponChanged(int value) => NotifyDirty();
    partial void OnWeapon2Changed(int value) => NotifyDirty();
    partial void OnSpeedChanged(int value) => NotifyDirty();
    partial void OnTurnSpeedChanged(int value) => NotifyDirty();
    partial void OnReloadTimeChanged(int value) => NotifyDirty();

    public void EnableFromCurrent()
    {
        if (IsEnabled)
        {
            return;
        }

        IsEnabled = true;
    }

    public void DisableToExeDefault()
    {
        if (!IsEnabled)
        {
            return;
        }

        Apply(UnitCatalog.CreateExeDefault(TypeId), isEnabled: false);
        NotifyDirty();
    }

    public void Apply(StuffDatEntry entry, bool isEnabled)
    {
        _suppressDirty = true;
        IsEnabled = isEnabled;
        LoadFrom(entry);
        _suppressDirty = false;
    }

    public StuffDatEntry ToEntry()
    {
        var entry = new StuffDatEntry
        {
            TypeId = TypeId,
            Cost = Cost,
            Armour = Armour,
            Weapon = Weapon,
            Health = Health,
            BuildTime = BuildTime,
        };

        if (!IsBuilding)
        {
            entry.Weapon2 = Weapon2;
            entry.Speed = Speed;
            entry.TurnSpeed = TurnSpeed;
            entry.ReloadTime = ReloadTime;
        }

        return entry;
    }

    private void LoadFrom(StuffDatEntry entry)
    {
        Cost = entry.Cost;
        Health = entry.Health;
        BuildTime = entry.BuildTime;
        Armour = entry.Armour;
        Weapon = entry.Weapon;
        Weapon2 = entry.Weapon2;
        Speed = entry.Speed;
        TurnSpeed = entry.TurnSpeed;
        ReloadTime = entry.ReloadTime;
    }

    private void NotifyDirty()
    {
        if (!_suppressDirty)
        {
            _markDirty();
        }
    }
}
