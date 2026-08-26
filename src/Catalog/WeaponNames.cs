namespace SeventhEditor.Catalog;

public sealed record NamedValue(int Value, string Name)
{
    public override string ToString() => Name;
}

public static class WeaponNames
{
    public static IReadOnlyList<NamedValue> ArmourClasses { get; } =
    [
        new(-1, "None"),
        new(0, "Body"),
        new(1, "Light"),
        new(2, "Medium"),
        new(3, "Heavy"),
        new(4, "Structure"),
    ];

    public static IReadOnlyList<NamedValue> WeaponTypes { get; } =
    [
        new(0, "None"),
        new(29, "Bomb"),
        new(39, "Fast Laser"),
        new(41, "Flame Thrower"),
        new(33, "Grenade"),
        new(2, "Laser"),
        new(35, "Laser Bolt"),
        new(38, "Missile Pack"),
        new(43, "Mortar Round"),
        new(18, "Plasma Rifle"),
        new(42, "Priest thing"),
        new(31, "Rocket"),
        new(15, "Shell 1"),
        new(20, "Shell 2"),
        new(34, "Zapper"),
    ];

    public static string GetDamageRowName(int index) => index switch
    {
        0 => "Plasma Rifle",
        1 => "Laser",
        2 => "Shell 1",
        3 => "Shell 2",
        4 => "Rocket",
        5 => "Grenade",
        6 => "Zapper",
        7 => "Laser Bolt",
        8 => "Missile Pack",
        9 => "Fast Laser",
        10 => "Flame Thrower",
        11 => "Priest thing",
        12 => "Weapon 12",
        13 => "Mortar Round",
        _ => $"Unused {index}",
    };

    public static string GetArmourName(int value)
    {
        foreach (var item in ArmourClasses)
        {
            if (item.Value == value)
            {
                return item.Name;
            }
        }

        return value.ToString();
    }

    public static string GetWeaponTypeName(int value)
    {
        foreach (var item in WeaponTypes)
        {
            if (item.Value == value)
            {
                return item.Name;
            }
        }

        return value.ToString();
    }
}
