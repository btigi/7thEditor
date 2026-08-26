using ii.EighthSolitude;

namespace SeventhEditor.Catalog;

public static class UnitCatalog
{
    public static string GetDisplayName(StuffDatTypeId typeId) => typeId switch
    {
        StuffDatTypeId.OreCarrier => "Ore Carrier",
        StuffDatTypeId.OreTruck => "Ore Truck",
        StuffDatTypeId.Marauder => "Marauder",
        StuffDatTypeId.Oppressor => "Oppressor",
        StuffDatTypeId.Crucifier => "Crucifier",
        StuffDatTypeId.Apc => "APC",
        StuffDatTypeId.Tormentor => "Tormentor",
        StuffDatTypeId.Avenger => "Avenger",
        StuffDatTypeId.FaithHammer => "Faith Hammer",
        StuffDatTypeId.Annihilator => "Annihilator",
        StuffDatTypeId.Purifier => "Purifier",
        StuffDatTypeId.LandMine => "Land Mine",
        StuffDatTypeId.Missile => "Missile",
        StuffDatTypeId.Infantry => "Infantry",
        StuffDatTypeId.MachineGunner => "Machine Gunner",
        StuffDatTypeId.MachineGunnerCh => "Machine Gunner (CH)",
        StuffDatTypeId.MortarUnit => "Mortar Unit",
        StuffDatTypeId.Priest => "Priest",
        StuffDatTypeId.Medic => "Medic",
        StuffDatTypeId.SlavenRider => "Slaven Rider",
        StuffDatTypeId.Marine => "Marine",
        StuffDatTypeId.Commander => "Commander",
        StuffDatTypeId.AirTransport => "Air Transport",
        StuffDatTypeId.Sparrow => "Sparrow",
        StuffDatTypeId.Eagle => "Eagle",
        StuffDatTypeId.Hovercraft => "Hovercraft",
        StuffDatTypeId.SpySatellite => "Spy Satellite",
        StuffDatTypeId.PowerUpgrade => "Power Upgrade",
        StuffDatTypeId.LaserUpgrade => "Laser Upgrade",
        StuffDatTypeId.ShellUpgrade => "Shell Upgrade",
        StuffDatTypeId.RifleUpgrade => "Rifle Upgrade",
        StuffDatTypeId.BodyArmourUpgrade => "Body Armour Upgrade",
        StuffDatTypeId.ArmourPlatingUpgrade => "Armour Plating Upgrade",
        StuffDatTypeId.Dominator => "Dominator",
        StuffDatTypeId.Obliterator => "Obliterator",
        StuffDatTypeId.LightMech => "Light Mech",
        StuffDatTypeId.Nova => "Nova",
        StuffDatTypeId.VenomTyphoon => "Venom Typhoon",
        StuffDatTypeId.Redeemer => "Redeemer",
        StuffDatTypeId.Bomb => "Bomb",
        StuffDatTypeId.Stealth => "Stealth",
        StuffDatTypeId.Trueseeing => "Trueseeing",
        StuffDatTypeId.MobileBase => "Mobile Base",
        StuffDatTypeId.Pyroclast => "Pyroclast",
        StuffDatTypeId.Base => "Base",
        StuffDatTypeId.Foundation => "Foundation",
        StuffDatTypeId.PowerPlant => "Power Plant",
        StuffDatTypeId.Mine => "Mine",
        StuffDatTypeId.Refinery => "Refinery",
        StuffDatTypeId.Barracks => "Barracks",
        StuffDatTypeId.Wall => "Wall",
        StuffDatTypeId.RadarStation => "Radar Station",
        StuffDatTypeId.Hospital => "Hospital",
        StuffDatTypeId.VehicleFactory => "Vehicle Factory",
        StuffDatTypeId.GunEmplacement => "Gun Emplacement",
        StuffDatTypeId.HiTechLab => "Hi Tech Lab",
        StuffDatTypeId.RepairBay => "Repair Bay",
        StuffDatTypeId.RobotHangar => "Robot Hangar",
        StuffDatTypeId.AdvancedMine => "Advanced Mine",
        StuffDatTypeId.Reactor => "Reactor",
        StuffDatTypeId.MissileSilo => "Missile Silo",
        StuffDatTypeId.ShieldGenerator => "Shield Generator",
        StuffDatTypeId.NavalYard => "Naval Yard",
        StuffDatTypeId.AirHangar => "Air Hangar",
        StuffDatTypeId.LandingPad => "Landing Pad",
        StuffDatTypeId.ChemicalPlant => "Chemical Plant",
        StuffDatTypeId.SuperGun => "Super Gun",
        _ => typeId.ToString(),
    };

    public static IEnumerable<StuffDatTypeId> AllUnits => Enum.GetValues<StuffDatTypeId>().Where(id => (int)id < 1000).OrderBy(GetDisplayName);

    public static IEnumerable<StuffDatTypeId> AllBuildings => Enum.GetValues<StuffDatTypeId>().Where(id => (int)id >= 1000).OrderBy(GetDisplayName);

    public static StuffDatEntry CreateExeDefault(StuffDatTypeId typeId)
    {
        var entry = Clone(ExeDefaults[typeId]);
        entry.TypeId = typeId;
        return entry;
    }

    public static StuffDatWeapon CreateExeWeaponDefault(int index)
    {
        if (index < 0 || index >= ExeWeapons.Length)
        {
            return new StuffDatWeapon();
        }

        return Clone(ExeWeapons[index]);
    }

    public static StuffDatEntry Clone(StuffDatEntry source) => new()
    {
        TypeId = source.TypeId,
        Cost = source.Cost,
        Armour = source.Armour,
        Weapon = source.Weapon,
        Weapon2 = source.Weapon2,
        Speed = source.Speed,
        TurnSpeed = source.TurnSpeed,
        Health = source.Health,
        BuildTime = source.BuildTime,
        ReloadTime = source.ReloadTime,
    };

    public static StuffDatWeapon Clone(StuffDatWeapon source)
    {
        var extra = new int[StuffDatProcessor.WeaponExtraDwords];
        if (source.Extra != null)
        {
            Array.Copy(source.Extra, extra, Math.Min(source.Extra.Length, extra.Length));
        }

        return new StuffDatWeapon
        {
            DamageVsBody = source.DamageVsBody,
            DamageVsLight = source.DamageVsLight,
            DamageVsMedium = source.DamageVsMedium,
            DamageVsHeavy = source.DamageVsHeavy,
            DamageVsStructure = source.DamageVsStructure,
            Extra = extra,
        };
    }

    private static StuffDatEntry V(StuffDatTypeId typeId, int health, int cost, int build, int weapon, int weapon2, int reload, int armour, int speed, int turn) => new()
    {
        TypeId = typeId,
        Health = health,
        Cost = cost,
        BuildTime = build,
        Weapon = weapon,
        Weapon2 = weapon2,
        ReloadTime = reload,
        Armour = armour,
        Speed = speed,
        TurnSpeed = turn,
    };

    private static StuffDatEntry B(StuffDatTypeId typeId, int health, int cost, int build, int weapon, int armour) => new()
    {
        TypeId = typeId,
        Health = health,
        Cost = cost,
        BuildTime = build,
        Weapon = weapon,
        Armour = armour,
    };

    private static StuffDatWeapon W(int body, int light, int medium, int heavy, int structure) => new()
    {
        DamageVsBody = body << 16,
        DamageVsLight = light << 16,
        DamageVsMedium = medium << 16,
        DamageVsHeavy = heavy << 16,
        DamageVsStructure = structure << 16,
        Extra = new int[StuffDatProcessor.WeaponExtraDwords],
    };

    // Defaults from legion.exe
    private static readonly Dictionary<StuffDatTypeId, StuffDatEntry> ExeDefaults = new()
    {
        [StuffDatTypeId.OreCarrier] = V(StuffDatTypeId.OreCarrier, 10, 5000, 350, 0, 0, 0, 2, 65536, 8),
        [StuffDatTypeId.OreTruck] = V(StuffDatTypeId.OreTruck, 20, 3500, 300, 0, 0, 0, 3, 126976, 15),
        [StuffDatTypeId.Marauder] = V(StuffDatTypeId.Marauder, 300, 3000, 200, 15, 0, 38, 2, 131072, 15),
        [StuffDatTypeId.Oppressor] = V(StuffDatTypeId.Oppressor, 400, 4000, 240, 15, 0, 70, 2, 118784, 14),
        [StuffDatTypeId.Crucifier] = V(StuffDatTypeId.Crucifier, 400, 4000, 250, 15, 0, 70, 2, 118784, 11),
        [StuffDatTypeId.Apc] = V(StuffDatTypeId.Apc, 16, 1000, 300, 15, 0, 70, 1, 131072, 15),
        [StuffDatTypeId.Tormentor] = V(StuffDatTypeId.Tormentor, 75, 2250, 340, 20, 0, 150, 2, 110592, 10),
        [StuffDatTypeId.Avenger] = V(StuffDatTypeId.Avenger, 75, 2250, 350, 20, 0, 160, 2, 110592, 10),
        [StuffDatTypeId.FaithHammer] = V(StuffDatTypeId.FaithHammer, 600, 7500, 500, 15, 0, 95, 3, 114688, 10),
        [StuffDatTypeId.Annihilator] = V(StuffDatTypeId.Annihilator, 600, 7500, 490, 15, 0, 84, 3, 114688, 10),
        [StuffDatTypeId.Purifier] = V(StuffDatTypeId.Purifier, 120, 3750, 600, 34, 0, 120, 2, 98304, 8),
        [StuffDatTypeId.LandMine] = V(StuffDatTypeId.LandMine, 0, 0, 0, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.Missile] = V(StuffDatTypeId.Missile, 0, 10000, 1200, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.Infantry] = V(StuffDatTypeId.Infantry, 3, 500, 50, 18, 0, 45, 0, 98304, -1),
        [StuffDatTypeId.MachineGunner] = V(StuffDatTypeId.MachineGunner, 50, 500, 85, 18, 0, 38, 0, 98304, -1),
        [StuffDatTypeId.MachineGunnerCh] = V(StuffDatTypeId.MachineGunnerCh, 50, 500, 85, 18, 0, 38, 0, 98304, -1),
        [StuffDatTypeId.MortarUnit] = V(StuffDatTypeId.MortarUnit, 70, 1500, 160, 43, 0, 150, 0, 73728, -1),
        [StuffDatTypeId.Priest] = V(StuffDatTypeId.Priest, 40, 2500, 160, 18, 42, 60, 0, 73728, -1),
        [StuffDatTypeId.Medic] = V(StuffDatTypeId.Medic, 3, 800, 160, 0, 0, 999, 0, 90112, -1),
        [StuffDatTypeId.SlavenRider] = V(StuffDatTypeId.SlavenRider, 25, 1500, 200, 18, 0, 60, 0, 196608, -1),
        [StuffDatTypeId.Marine] = V(StuffDatTypeId.Marine, 50, 500, 160, 18, 0, 40, 0, 157286, -1),
        [StuffDatTypeId.Commander] = V(StuffDatTypeId.Commander, 80, 4250, 260, 18, 15, 33, 0, 98304, -1),
        [StuffDatTypeId.AirTransport] = V(StuffDatTypeId.AirTransport, 9, 3000, 200, 18, 0, 45, 1, 122880, 14),
        [StuffDatTypeId.Sparrow] = V(StuffDatTypeId.Sparrow, 9, 3000, 200, 18, 0, 45, 1, 122880, 14),
        [StuffDatTypeId.Eagle] = V(StuffDatTypeId.Eagle, 9, 3000, 200, 18, 0, 45, 1, 122880, 14),
        [StuffDatTypeId.Hovercraft] = V(StuffDatTypeId.Hovercraft, 9, 3000, 200, 18, 0, 45, 1, 122880, 14),
        [StuffDatTypeId.SpySatellite] = V(StuffDatTypeId.SpySatellite, 0, 50000, 2000, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.PowerUpgrade] = V(StuffDatTypeId.PowerUpgrade, 0, 1000, 1300, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.LaserUpgrade] = V(StuffDatTypeId.LaserUpgrade, 0, 4000, 2200, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.ShellUpgrade] = V(StuffDatTypeId.ShellUpgrade, 0, 3500, 1800, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.RifleUpgrade] = V(StuffDatTypeId.RifleUpgrade, 0, 2000, 1000, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.BodyArmourUpgrade] = V(StuffDatTypeId.BodyArmourUpgrade, 0, 1200, 800, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.ArmourPlatingUpgrade] = V(StuffDatTypeId.ArmourPlatingUpgrade, 0, 1750, 1500, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.Dominator] = V(StuffDatTypeId.Dominator, 400, 5000, 700, 2, 0, 60, 2, 122880, -1),
        [StuffDatTypeId.Obliterator] = V(StuffDatTypeId.Obliterator, 600, 6000, 900, 2, 0, 60, 3, 126976, -1),
        [StuffDatTypeId.LightMech] = V(StuffDatTypeId.LightMech, 32, 3000, 500, 2, 0, 60, 2, 131072, -1),
        [StuffDatTypeId.Nova] = V(StuffDatTypeId.Nova, 400, 7000, 700, 38, 0, 110, 2, 126976, -1),
        [StuffDatTypeId.VenomTyphoon] = V(StuffDatTypeId.VenomTyphoon, 400, 7000, 900, 35, 41, 80, 2, 126976, -1),
        [StuffDatTypeId.Redeemer] = V(StuffDatTypeId.Redeemer, 300, 10000, 1000, 31, 0, 650, 2, 122880, -1),
        [StuffDatTypeId.Bomb] = V(StuffDatTypeId.Bomb, 0, 50000, 2000, 29, 0, 0, 1, 0, 0),
        [StuffDatTypeId.Stealth] = V(StuffDatTypeId.Stealth, 0, 25000, 1500, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.Trueseeing] = V(StuffDatTypeId.Trueseeing, 0, 22500, 1200, 0, 0, 0, 1, 0, 0),
        [StuffDatTypeId.MobileBase] = V(StuffDatTypeId.MobileBase, 30, 25000, 750, 0, 0, 0, 3, 126976, 15),
        [StuffDatTypeId.Pyroclast] = V(StuffDatTypeId.Pyroclast, 300, 8000, 1000, 38, 0, 200, 3, 196608, -1),

        [StuffDatTypeId.Base] = B(StuffDatTypeId.Base, 400, 0, 1, 0, 1),
        [StuffDatTypeId.Foundation] = B(StuffDatTypeId.Foundation, 9999, 0, 0, 0, -1),
        [StuffDatTypeId.PowerPlant] = B(StuffDatTypeId.PowerPlant, 200, 1500, 150, 0, 1),
        [StuffDatTypeId.Mine] = B(StuffDatTypeId.Mine, 60, 2000, 220, 0, -1),
        [StuffDatTypeId.Refinery] = B(StuffDatTypeId.Refinery, 60, 6000, 220, 0, -1),
        [StuffDatTypeId.Barracks] = B(StuffDatTypeId.Barracks, 200, 1500, 180, 0, 1),
        [StuffDatTypeId.Wall] = B(StuffDatTypeId.Wall, 70, 500, 100, 0, 2),
        [StuffDatTypeId.RadarStation] = B(StuffDatTypeId.RadarStation, 30, 250, 100, 0, -1),
        [StuffDatTypeId.Hospital] = B(StuffDatTypeId.Hospital, 200, 5000, 190, 0, 1),
        [StuffDatTypeId.VehicleFactory] = B(StuffDatTypeId.VehicleFactory, 300, 10000, 220, 0, 2),
        [StuffDatTypeId.GunEmplacement] = B(StuffDatTypeId.GunEmplacement, 600, 1250, 300, 35, 3),
        [StuffDatTypeId.HiTechLab] = B(StuffDatTypeId.HiTechLab, 500, 15000, 220, 0, 1),
        [StuffDatTypeId.RepairBay] = B(StuffDatTypeId.RepairBay, 400, 6000, 220, 0, 1),
        [StuffDatTypeId.RobotHangar] = B(StuffDatTypeId.RobotHangar, 500, 20000, 220, 0, 3),
        [StuffDatTypeId.AdvancedMine] = B(StuffDatTypeId.AdvancedMine, 1, 250, 10, 0, -1),
        [StuffDatTypeId.Reactor] = B(StuffDatTypeId.Reactor, 35, 12000, 120, 0, -1),
        [StuffDatTypeId.MissileSilo] = B(StuffDatTypeId.MissileSilo, 40, 1000, 150, 0, -1),
        [StuffDatTypeId.ShieldGenerator] = B(StuffDatTypeId.ShieldGenerator, 50, 2000, 175, 0, -1),
        [StuffDatTypeId.NavalYard] = B(StuffDatTypeId.NavalYard, 60, 5000, 220, 0, -1),
        [StuffDatTypeId.AirHangar] = B(StuffDatTypeId.AirHangar, 60, 5000, 220, 0, -1),
        [StuffDatTypeId.LandingPad] = B(StuffDatTypeId.LandingPad, 35, 500, 120, 0, -1),
        [StuffDatTypeId.ChemicalPlant] = B(StuffDatTypeId.ChemicalPlant, 35, 500, 120, 0, -1),
        [StuffDatTypeId.SuperGun] = B(StuffDatTypeId.SuperGun, 1200, 3250, 500, 35, -1),
    };

    // Defaults from legion.exe weapon damage table (high word of each dword)
    private static readonly StuffDatWeapon[] ExeWeapons =
    [
        W(16, 8, 8, 4, 4),
        W(90, 80, 40, 40, 40),
        W(6, 19, 19, 25, 19),
        W(10, 30, 30, 40, 60),
        W(30, 100, 90, 50, 100),
        W(39, 30, 26, 13, 30),
        W(90, 80, 40, 40, 80),
        W(90, 80, 40, 40, 80),
        W(8, 22, 22, 30, 22),
        W(68, 60, 30, 30, 60),
        W(50, 35, 35, 13, 35),
        W(50, 35, 35, 13, 35),
        W(50, 35, 35, 13, 35),
        W(6, 19, 19, 25, 19),
        W(0, 0, 0, 0, 0),
        W(0, 0, 0, 0, 0),
        W(0, 0, 0, 0, 0),
        W(0, 0, 0, 0, 0),
        W(0, 0, 0, 0, 0),
        W(0, 0, 0, 0, 0),
    ];
}