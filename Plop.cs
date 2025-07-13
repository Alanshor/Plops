using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;

namespace Happy_Plops
{
    public class Plop
    {
        private string _name;
        public string Name { 
            get => _name;
            set => _name = string.IsNullOrEmpty(value) ? _name : value;
        }
        public int Level { get; set; } = 1;
        public int HP { get; set; }  = 10;

        public int MaxHP { get; set; } = 10;
        public int Damage { get; set; }  = 1;
        public int EXP { get; set; } = 0;
        public int EXP_for_next_LevelUp { get; private set; } = 10;
        public List<Plop> Parents { get; set; }

        private PlopGroup _group = null;
        public PlopGroup Group
        {
            get => _group ?? new PlopGroup(true);
            set => _group = value;
        }


        public Plop(string name = "Plop", int start_level = 1, List<Plop> start_parents = null)
        {
            Name = name;
            Parents = start_parents ?? new List<Plop>();

            if (start_level < 1)
            {
                start_level = Level;
                Console.WriteLine($"Ungültiges Level übergeben, Standardwert {Level} wird verwendet.");
            }

            while (start_level > 1)
            {
                LevelUp(false);
                start_level--;
            }
        }

        [JsonConstructor]
        public Plop(string name, PlopGroup group /*, weitere Parameter, wenn nötig */)
        {
            Name = name;
            _group = group;
        }

        public override string ToString()
        {
            string elternNamen = Parents.Count > 0
            ? string.Join(", ", Parents.Select(p => p.Name))
            : "Keine";

            return $"Plop \"{Name}\" (Level {Level})\n" +
                    $"- HP: {HP}/{MaxHP}\n" +
                    $"- Damage: {Damage}\n" +
                    $"- EXP: {EXP} / {EXP_for_next_LevelUp}\n" +
                    $"- Eltern ({Parents.Count}): {elternNamen}\n"+
                    $"- Gruppe: {Group.Name} mit {Group.Members.Count} Plops";
        }

        public void Heal(int heal, bool isPercent=false)
        {
            if (heal < 0) return;
            if (isPercent)
            {
                heal = (int)(MaxHP * (heal/100.0));
            }
            ChangeHP(heal);
        }

        public void HealFull()
        {
            HP = MaxHP;
        }

        public void TakeDamage(int damage, bool isPercent = false)
        {
            if (damage < 0) return;
            if (isPercent)
            {
                damage = (int)(MaxHP * (damage/100.0));
            }
            ChangeHP(damage *-1);
        }

        private void ChangeHP(int value)
        {
            HP += value;
            if (HP < 0) { HP = 0; }
            else if (HP > MaxHP) { HP = MaxHP; }
        }


        private void Rise_Damage_After_LevelUp()
        {
            Damage = (int)Math.Round(Damage * 1.1) + 1;
        }

        private void Rise_HP_After_LevelUp()
        {
            MaxHP += (int)(Level * 1.25);
        }

        private void Calculate_EXP_for_next_LevelUp()
        {
            EXP_for_next_LevelUp = (int)(EXP_for_next_LevelUp * 1.5);
        }

        private void ShowLevelMessage()
        {
            Console.WriteLine($"{_name} hat Level {Level} erreicht!");
        }

        public void LevelUp(bool print_levelup_text = true)
        {
            Level++;
            Rise_HP_After_LevelUp();
            Rise_Damage_After_LevelUp();

            EXP = 0;
            Calculate_EXP_for_next_LevelUp();

            if (print_levelup_text)
            {
                ShowLevelMessage();
            }
        }
        public int EarnEXP(int newEXP, bool print_levelup_text = true)
        {
            if (newEXP < 1)
            {
                return 0;
            }
            int rest_EXP = newEXP;
            int LevelUps = 0;
            int needed = EXP_for_next_LevelUp - EXP;
            while (rest_EXP > 0)
            {
                if (rest_EXP >= needed)
                {
                    rest_EXP -= needed;
                    LevelUps++;
                    LevelUp(print_levelup_text);
                    needed = EXP_for_next_LevelUp - EXP;
                }
                else
                {
                    EXP += rest_EXP;
                    break;
                }
            }
            return LevelUps;
        }
    }
    public class PlopGroup
    {
        public int ID { get; set; }

        public string Name { get; set; } = "";

        public List<Plop> Members { get; set; } = new List<Plop>();

        private static readonly Random Random = new Random();

        private readonly bool isUngrouped;
        public PlopGroup(string name = null, List<Plop> ploplist = null, bool isUngrouped = false)
        {
            if (isUngrouped)
            {
                this.Name = "Keine Gruppe";
            }
            else if (string.IsNullOrEmpty(name))
            {
                this.Name = RandomName();
            }
            else
            {
                this.Name = name;
            }

            this.Members = ploplist ?? new List<Plop>();
            this.isUngrouped=isUngrouped;
        }
        public PlopGroup(bool isUngrouped = false) : this(null, null, isUngrouped) { }

        public PlopGroup() { }

        private string[] NameList() {
            return new[] { "ShadowCore", "IronFangs", "NebulaPack", "StormRiders", "EchoUnit",
                            "BlackVortex", "SolarSyndicate", "CrystalClaws", "Frostbyte", "NovaForce",
                            "QuantumTribe", "SilentReign", "SteelPulse", "OblivionWard", "FireNest",
                            "LunarEcho", "ZeroSignal", "CrimsonWings", "FrozenHorde", "DarkTide",
                            "VenomCrew", "Skybreakers", "NightSparks", "ChaosHive", "BlitzUnit",
                            "Starlance", "WarpHunters", "ThunderCove", "AshLeague", "CyberHowl",
                            "PulsePack", "DuneRunners", "RadiantPack", "VortexKnights", "NeonShroud",
                            "FeralSons", "NovaTide", "BladeNest", "PyroSync", "DustTribe",
                            "IronHalo", "BlackOrbit", "DreadWardens", "FrozenFury", "HexFaction",
                            "GrimFleet", "VoidHowl", "SnareClaw", "StormKind", "MythCore",
                            "WraithLegion", "DarkCircuit", "Ashborn", "ColdSnap", "FireHounds",
                            "PulseRaiders", "SkyNomads", "NeonBand", "PhantomSquad", "BrimstoneCrew",
                            "TalonOrder", "GlitchBrood", "SpecterLine", "The Red Fold", "CraterWolves",
                            "ThunderNest", "PlasmaFang", "IceGrinders", "DoomCircle", "BinaryPack",
                            "NightCradle", "SunShard", "BlackFlare", "DustWings", "VoidSignal",
                            "FangWard", "SteelNest", "InfernalCode", "SilentWing", "CircuitSyndicate",
                            "StarForge", "ObsidianClaw", "PlasmaReign", "AshFangs", "SkyBurners",
                            "Frostborn", "CyberShade", "ChaosCradle", "PyreCrest", "StaticCoven",
                            "TwilightPack", "ShadowScript", "ZeroPack", "ArcRaiders", "DriftWard",
                            "RustFang", "EchoBurn", "SignalNest", "NeonFangs", "Plopchaos" };
        }

        private string RandomName()
        {
            var names = NameList();
            return names[Random.Next(names.Length)];
        }

        public void Add(Plop plop)
        {
            if (this.isUngrouped || plop==null)
                return;
            
            this.Members.Add(plop);
            plop.Group = this;
        }

        public void Add(PlopGroup plopGroup)
        {
            if (this.isUngrouped || plopGroup==null || plopGroup.Members==null)
                return;

            foreach (var plop in plopGroup.Members)
            {
                if (plop != null)
                {
                    this.Members.Add(plop);
                    plop.Group = this;
                }
            }
        }

        public void Remove(Plop plop)
        {
            if (this.isUngrouped || plop == null)
                return;

            if (this.Members.Remove(plop))
            {
                plop.Group = null;
            }
        }

        public void Remove(PlopGroup plopGroup)
        {
            if (this.isUngrouped || plopGroup == null || plopGroup.Members == null)
                return;

            foreach (var plop in plopGroup.Members)
            {
                if (plop != null && this.Members.Remove(plop))
                {
                    plop.Group = null;
                }
            }
        }

        public override string ToString()
        {
            if (Members.Count == 0)
            {
                return $"{Name}-Group hat keine Mitglieder.";
            }

            return $"{Name}-Group Members:\n" + string.Join("\n", Members);
        }

    }
}
