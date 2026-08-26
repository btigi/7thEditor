using CommunityToolkit.Mvvm.ComponentModel;
using ii.EighthSolitude;
using SeventhEditor.Catalog;

namespace SeventhEditor.ViewModels;

public partial class WeaponRowViewModel : ObservableObject
{
    private readonly Action _markDirty;
    private int _bodyRaw;
    private int _lightRaw;
    private int _mediumRaw;
    private int _heavyRaw;
    private int _structureRaw;
    private int[] _extra;
    private bool _suppressDirty;

    public WeaponRowViewModel(int index, StuffDatWeapon weapon, Action markDirty)
    {
        Index = index;
        Name = WeaponNames.GetDamageRowName(index);
        _markDirty = markDirty;
        _extra = new int[StuffDatProcessor.WeaponExtraDwords];
        _suppressDirty = true;
        LoadFrom(weapon);
        _suppressDirty = false;
    }

    public int Index { get; }

    public string Name { get; }

    public string ListLabel => $"{Index:D2} — {Name}";

    [ObservableProperty]
    private string damageVsBodyText = "0";

    [ObservableProperty]
    private string damageVsLightText = "0";

    [ObservableProperty]
    private string damageVsMediumText = "0";

    [ObservableProperty]
    private string damageVsHeavyText = "0";

    [ObservableProperty]
    private string damageVsStructureText = "0";

    partial void OnDamageVsBodyTextChanged(string value) => ApplyDamageText(value, ref _bodyRaw);
    partial void OnDamageVsLightTextChanged(string value) => ApplyDamageText(value, ref _lightRaw);
    partial void OnDamageVsMediumTextChanged(string value) => ApplyDamageText(value, ref _mediumRaw);
    partial void OnDamageVsHeavyTextChanged(string value) => ApplyDamageText(value, ref _heavyRaw);
    partial void OnDamageVsStructureTextChanged(string value) => ApplyDamageText(value, ref _structureRaw);

    public StuffDatWeapon ToWeapon()
    {
        var extra = new int[StuffDatProcessor.WeaponExtraDwords];
        Array.Copy(_extra, extra, extra.Length);
        return new StuffDatWeapon
        {
            DamageVsBody = _bodyRaw,
            DamageVsLight = _lightRaw,
            DamageVsMedium = _mediumRaw,
            DamageVsHeavy = _heavyRaw,
            DamageVsStructure = _structureRaw,
            Extra = extra,
        };
    }

    public void Apply(StuffDatWeapon weapon)
    {
        _suppressDirty = true;
        LoadFrom(weapon);
        _suppressDirty = false;
    }

    private void ApplyDamageText(string value, ref int rawField)
    {
        if (_suppressDirty)
        {
            return;
        }

        if (!FixedPoint.TryParse(value, out var raw) || raw == rawField)
        {
            return;
        }

        rawField = raw;
        NotifyDirty();
    }

    private void LoadFrom(StuffDatWeapon weapon)
    {
        _bodyRaw = weapon.DamageVsBody;
        _lightRaw = weapon.DamageVsLight;
        _mediumRaw = weapon.DamageVsMedium;
        _heavyRaw = weapon.DamageVsHeavy;
        _structureRaw = weapon.DamageVsStructure;
        if (weapon.Extra != null)
        {
            Array.Copy(weapon.Extra, _extra, Math.Min(weapon.Extra.Length, _extra.Length));
        }
        else
        {
            Array.Clear(_extra);
        }

        DamageVsBodyText = FixedPoint.Format(_bodyRaw);
        DamageVsLightText = FixedPoint.Format(_lightRaw);
        DamageVsMediumText = FixedPoint.Format(_mediumRaw);
        DamageVsHeavyText = FixedPoint.Format(_heavyRaw);
        DamageVsStructureText = FixedPoint.Format(_structureRaw);
    }

    private void NotifyDirty()
    {
        if (!_suppressDirty)
        {
            _markDirty();
        }
    }
}
