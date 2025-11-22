using ScriptGenie.Controls.ArmorGenerator;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ScriptGenie.Exporters
{
    public static class Armor
    {
        // Map BaseArmorType to MaterialType
        private static readonly Dictionary<string, string> BaseArmorMaterialTypes = new Dictionary<string, string>
        {
            ["Plate"] = "ArmorMaterialType.Plate",
            ["Chainmail"] = "ArmorMaterialType.Chainmail",
            ["Ringmail"] = "ArmorMaterialType.Ringmail",
            ["Studded"] = "ArmorMaterialType.Studded",
            ["Leather"] = "ArmorMaterialType.Leather",
            ["Bone"] = "ArmorMaterialType.Bone",
            ["Dragon"] = "ArmorMaterialType.Dragon",
            ["Tribal"] = "ArmorMaterialType.Tribal"
        };

        // Map ResourceType to CraftResource
        private static readonly Dictionary<string, string> ResourceTypes = new Dictionary<string, string>
        {
            ["Iron"] = "CraftResource.Iron",
            ["DullCopper"] = "CraftResource.DullCopper",
            ["ShadowIron"] = "CraftResource.ShadowIron",
            ["Copper"] = "CraftResource.Copper",
            ["Bronze"] = "CraftResource.Bronze",
            ["Gold"] = "CraftResource.Gold",
            ["Agapite"] = "CraftResource.Agapite",
            ["Verite"] = "CraftResource.Verite",
            ["Valorite"] = "CraftResource.Valorite",
            ["Nepturite"] = "CraftResource.Nepturite",
            ["Orcish"] = "CraftResource.Orcish",
            ["Bloodrock"] = "CraftResource.Bloodrock",
            ["RegularLeather"] = "CraftResource.RegularLeather",
            ["SpinedLeather"] = "CraftResource.SpinedLeather",
            ["HornedLeather"] = "CraftResource.HornedLeather",
            ["BarbedLeather"] = "CraftResource.BarbedLeather",
            ["Bone"] = "CraftResource.RegularLeather",
            ["RedScales"] = "CraftResource.RedScales",
            ["YellowScales"] = "CraftResource.YellowScales",
            ["BlackScales"] = "CraftResource.BlackScales",
            ["GreenScales"] = "CraftResource.GreenScales",
            ["WhiteScales"] = "CraftResource.WhiteScales",
            ["BlueScales"] = "CraftResource.BlueScales"
        };

        // Map ResourceType to Hue
        public static readonly Dictionary<string, int> ResourceHues = new Dictionary<string, int>
        {
            ["Iron"] = 0,
            ["DullCopper"] = 0x973,
            ["ShadowIron"] = 0x966,
            ["Copper"] = 0x96D,
            ["Bronze"] = 0x972,
            ["Gold"] = 0x8A5,
            ["Agapite"] = 0x979,
            ["Verite"] = 0x89F,
            ["Valorite"] = 0x8AB,
            ["RegularLeather"] = 0,
            ["SpinedLeather"] = 0x8AC,
            ["HornedLeather"] = 0x845,
            ["BarbedLeather"] = 0x851,
            ["Bone"] = 0,
            ["RedScales"] = 0x66D,
            ["YellowScales"] = 0x8A8,
            ["BlackScales"] = 0x455,
            ["GreenScales"] = 0x851,
            ["WhiteScales"] = 0x8FD,
            ["BlueScales"] = 0x8B0
        };

        // Map ArmorType to ItemID
        public static readonly Dictionary<string, string> ArmorTypeItemIDs = new Dictionary<string, string>
        {
            ["Plate Helm"] = "0x1408",
            ["Plate Gorget"] = "0x1413",
            ["Plate Chest"] = "0x1415",
            ["Plate Arms"] = "0x1410",
            ["Plate Gloves"] = "0x1413",
            ["Plate Legs"] = "0x1411",
            ["Chain Chest"] = "0x13BF",
            ["Chain Legs"] = "0x13BE",
            ["Chain Coif"] = "0x13BB",
            ["Ringmail Chest"] = "0x13EC",
            ["Ringmail Legs"] = "0x13F0",
            ["Ringmail Gloves"] = "0x13EB",
            ["Leather Chest"] = "0x13CC",
            ["Leather Legs"] = "0x13CB",
            ["Leather Gorget"] = "0x13C7",
            ["Leather Gloves"] = "0x13C6",
            ["Leather Cap"] = "0x1DB9",
            ["Studded Chest"] = "0x13DB",
            ["Studded Legs"] = "0x13DA",
            ["Studded Gorget"] = "0x13D6",
            ["Studded Gloves"] = "0x13D5",
            ["Bone Helm"] = "0x1451",
            ["Bone Chest"] = "0x144F",
            ["Bone Arms"] = "0x144E",
            ["Bone Gloves"] = "0x1450",
            ["Bone Legs"] = "0x1452",
            ["Dragon Helm"] = "0x2645",
            ["Dragon Chest"] = "0x2641",
            ["Dragon Arms"] = "0x2657",
            ["Dragon Gloves"] = "0x2643",
            ["Dragon Legs"] = "0x2647",
            ["Tribal Mask"] = "0x1549",
            ["Tribal Chest"] = "0x154C",
            ["Tribal Arms"] = "0x144E",
            ["Tribal Legs"] = "0x1452"
        };

        // Map Tribal Mask types to ItemIDs
        public static readonly Dictionary<string, string> TribalMaskItemIDs = new Dictionary<string, string>
        {
            ["Tribal Mask 1 (0x1549)"] = "0x1549",
            ["Tribal Mask 2 (0x154A)"] = "0x154A",
            ["Tribal Mask 3 (0x154B)"] = "0x154B",
            ["Tribal Mask 4 (0x154C)"] = "0x154C"
        };

        // Map Plate Helm types to ItemIDs
        public static readonly Dictionary<string, string> PlateHelmItemIDs = new Dictionary<string, string>
        {
            ["Plate Helm 1 (0x1408)"] = "0x1408",
            ["Plate Helm 2 (0x1409)"] = "0x1409",
            ["Plate Helm 3 (0x140A)"] = "0x140A",
            ["Plate Helm 4 (0x140B)"] = "0x140B",
            ["Plate Helm 5 (0x140C)"] = "0x140C",
            ["Plate Helm 6 (0x140D)"] = "0x140D",
            ["Plate Helm 7 (0x140E)"] = "0x140E",
            ["Plate Helm 8 (0x140F)"] = "0x140F",
            ["Plate Helm 9 (0x1412)"] = "0x1412",
            ["Plate Helm 10 (0x1419)"] = "0x1419"
        };

        // List of profane words
        private static readonly List<string> ProfaneWords = new List<string>
        {
            "jigaboo", "chigaboo", "wop", "kyke", "kike", "tit", "spic", "prick", "piss", "lezbo", "lesbo",
            "felatio", "dyke", "dildo", "chinc", "chink", "cunnilingus", "cum", "cocksucker", "cock", "clitoris",
            "clit", "ass", "hitler", "penis", "nigga", "nigger", "klit", "kunt", "jiz", "jism", "jerkoff",
            "jackoff", "goddamn", "fag", "blowjob", "bitch", "asshole", "dick", "pussy", "snatch", "cunt",
            "twat", "shit", "fuck"
        };

        // Profanity filter method
        private static bool ContainsProfanity(string input)
        {
            foreach (string word in ProfaneWords)
            {
                if (Regex.IsMatch(input, $"\\b{Regex.Escape(word)}\\b", RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // Method to check if input is alphanumeric
        private static bool IsAlphanumeric(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        }

        // Helper method to map layer names to enums
        private static string MapLayerToEnum(string layerType)
        {
            if (string.IsNullOrEmpty(layerType))
                return "OuterTorso"; // Default layer

            switch (layerType)
            {
                case "Neck": return "Neck";
                case "Helm": return "Helmet";
                case "Gloves": return "Gloves";
                case "Arms": return "Arms";
                case "InnerTorso": return "InnerTorso";
                case "OuterTorso": return "OuterTorso";
                case "InnerLegs": return "InnerLegs";
                case "OuterLegs": return "OuterLegs";
                case "Shoes": return "Shoes";
                case "TwoHanded": return "TwoHanded";
                default: return "OuterTorso"; // Default layer
            }
        }

        public static string GenerateScript(armorGenerator armorGen, string serverType, out bool containsProfanity)
        {
            string scriptName = armorGen.ScriptName;
            string armorDisplayNameInput = armorGen.ArmorDisplayName;
            string baseArmorType = armorGen.BaseArmorType;
            string resourceType = armorGen.ResourceType;
            string armorType = armorGen.ArmorType;
            string armorTypeDetails = armorGen.ArmorTypeDetails;
            string layerType = armorGen.LayerType;
            decimal weight = armorGen.Weight;
            int armorHue = armorGen.ArmorHue;
            string lootType = armorGen.LootType;

            // Get resistance values from the generator
            int physicalResistance = armorGen.PhysicalResistance;
            int fireResistance = armorGen.FireResistance;
            int coldResistance = armorGen.ColdResistance;
            int poisonResistance = armorGen.PoisonResistance;
            int energyResistance = armorGen.EnergyResistance;

            // Check for profanity in both ScriptName and ArmorDisplayName
            containsProfanity = ContainsProfanity(scriptName) || ContainsProfanity(armorDisplayNameInput);

            string armorDisplayName;
            if (ContainsProfanity(armorDisplayNameInput))
            {
                // Replace profanity with asterisks for display name
                armorDisplayName = armorDisplayNameInput;
                foreach (string word in ProfaneWords)
                {
                    armorDisplayName = Regex.Replace(armorDisplayName, $"\\b{Regex.Escape(word)}\\b", new string('*', word.Length), RegexOptions.IgnoreCase);
                }
            }
            else
            {
                armorDisplayName = armorDisplayNameInput;
            }

            // Get ItemID based on ArmorType or ArmorTypeDetails
            string itemID;
            if (armorType == "Tribal Mask" && armorTypeDetails != null)
            {
                itemID = TribalMaskItemIDs.TryGetValue(armorTypeDetails, out var id) ? id : "0x154B";
            }
            else if (armorType == "Plate Helm" && armorTypeDetails != null)
            {
                itemID = PlateHelmItemIDs.TryGetValue(armorTypeDetails, out var id) ? id : "0x1412";
            }
            else
            {
                itemID = ArmorTypeItemIDs.TryGetValue(armorType, out var id) ? id : "0x1415";
            }

            // Get MaterialType based on BaseArmorType
            string materialType = BaseArmorMaterialTypes.TryGetValue(baseArmorType, out var armorInfo) ? armorInfo : "ArmorMaterialType.Leather";

            // Get CraftResource based on ResourceType
            string craftResource = ResourceTypes.TryGetValue(resourceType, out var resource) ? resource : "CraftResource.RegularLeather";

            // Get Layer based on LayerType
            string layerEnum = MapLayerToEnum(layerType);

            string script = $@"using Server;
using Server.Items;
namespace Server.Items
{{
    public class {scriptName} : BaseArmor
    {{
        public override string Name {{ get {{ return ""{armorDisplayName}""; }} }}
        public override int BasePhysicalResistance {{ get {{ return {physicalResistance}; }} }}
        public override int BaseFireResistance {{ get {{ return {fireResistance}; }} }}
        public override int BaseColdResistance {{ get {{ return {coldResistance}; }} }}
        public override int BasePoisonResistance {{ get {{ return {poisonResistance}; }} }}
        public override int BaseEnergyResistance {{ get {{ return {energyResistance}; }} }}
        public override int InitMinHits {{ get {{ return 30; }} }}
        public override int InitMaxHits {{ get {{ return 40; }} }}
        public override int AosStrReq {{ get {{ return 20; }} }}
        public override int OldStrReq {{ get {{ return 10; }} }}
        public override int ArmorBase {{ get {{ return 22; }} }}
        public override ArmorMaterialType MaterialType {{ get {{ return {materialType}; }} }}
        public override CraftResource DefaultResource {{ get {{ return {craftResource}; }} }}

        [Constructable]
        public {scriptName}() : base({itemID})
        {{
            Weight = {weight};
            Hue = {armorHue};
            Name = ""{armorDisplayName}"";
            Layer = Layer.{layerEnum};
            LootType = LootType.{lootType};
        }}

        public {scriptName}(Serial serial) : base(serial)
        {{}}

        public override void Serialize(GenericWriter writer)
        {{
            base.Serialize(writer);
            writer.Write(0); // version
        }}

        public override void Deserialize(GenericReader reader)
        {{
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }}
    }}
}}";
            return script;
        }

        public static bool Export(armorGenerator armorGen, string serverType)
        {
            string scriptName = armorGen.ScriptName;
            string armorDisplayNameInput = armorGen.ArmorDisplayName;

            // Check for alphanumeric characters in ScriptName
            if (!IsAlphanumeric(scriptName))
            {
                MessageBox.Show("The script name can only contain alphanumeric characters (A-Z, a-z, 0-9). Please remove any spaces or special characters before exporting.", "Invalid Script Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool containsProfanity;
            string script = GenerateScript(armorGen, serverType, out containsProfanity);

            if (containsProfanity)
            {
                MessageBox.Show("The script name or armor name contains inappropriate language. Please change the name before exporting.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                SaveScriptToFile(script, serverType, scriptName);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oops! Something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static void SaveScriptToFile(string script, string serverType, string scriptName)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "C# Script|*.cs";
                saveFileDialog.Title = $"Save Armor Script for {serverType}";
                saveFileDialog.FileName = $"{scriptName}_Script.cs";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, script);
                    MessageBox.Show($"Script exported successfully to:\n{saveFileDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}