using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptGenie.Controls.ArmorGenerator
{
    public partial class armorGenerator : UserControl
    {
        // Script Name (Class Name)
        public string ScriptName => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_darkTextBox_scriptName.Text;

        // Armor Display Name (Name property in the script)
        public string ArmorDisplayName => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorName.Text;

        // Armor Type (e.g., Plate Helm, Chain Chest, etc.)
        public string ArmorType => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.SelectedItem?.ToString();

        // Armor Type Details (e.g., Tribal Mask 1, Plate Helm Type 1, etc.)
        public string ArmorTypeDetails => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.SelectedItem?.ToString();

        public string BaseArmorType => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor.SelectedItem?.ToString();
        public string ResourceType => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.SelectedItem?.ToString();
        public decimal Weight => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_weight.Value;
        public int ArmorHue => (int)armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_armorHue.Value;
        public string LootType => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_lootType.SelectedItem?.ToString();
        public bool IsArmorSet => armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkCheckBox_armorSet.Checked;

        // Dictionary to map each BaseArmor type to its valid CraftResource types
        private static readonly Dictionary<string, List<string>> ValidResources = new Dictionary<string, List<string>>
        {
            ["Plate"] = new List<string> { "Iron", "DullCopper", "ShadowIron", "Copper", "Bronze", "Gold", "Agapite", "Verite", "Valorite" },
            ["Chainmail"] = new List<string> { "Iron", "DullCopper", "ShadowIron", "Copper", "Bronze", "Gold", "Agapite", "Verite", "Valorite", "Nepturite", "Orcish", "Bloodrock" },
            ["Ringmail"] = new List<string> { "Iron", "DullCopper", "ShadowIron", "Copper", "Bronze", "Gold", "Agapite", "Verite", "Valorite", "Nepturite", "Orcish", "Bloodrock" },
            ["Studded"] = new List<string> { "RegularLeather", "SpinedLeather", "HornedLeather", "BarbedLeather" },
            ["Leather"] = new List<string> { "RegularLeather", "SpinedLeather", "HornedLeather", "BarbedLeather" },
            ["Bone"] = new List<string> { "RegularLeather", "Bone" },
            ["Dragon"] = new List<string> { "RedScales", "YellowScales", "BlackScales", "GreenScales", "WhiteScales", "BlueScales" },
            ["Tribal"] = new List<string> { "RegularLeather", "SpinedLeather", "HornedLeather", "BarbedLeather" }
        };

        // Dictionary to map each BaseArmor type to its valid Armor pieces
        private static readonly Dictionary<string, List<string>> BaseArmorTypesToPieces = new Dictionary<string, List<string>>
        {
            ["Plate"] = new List<string> { "Plate Helm", "Plate Gorget", "Plate Chest", "Plate Arms", "Plate Gloves", "Plate Legs" },
            ["Chainmail"] = new List<string> { "Chain Chest", "Chain Legs", "Chain Coif" },
            ["Ringmail"] = new List<string> { "Ringmail Chest", "Ringmail Legs", "Ringmail Gloves" },
            ["Studded"] = new List<string> { "Studded Chest", "Studded Legs", "Studded Gloves", "Studded Gorget" },
            ["Leather"] = new List<string> { "Leather Chest", "Leather Legs", "Leather Gloves", "Leather Cap" },
            ["Bone"] = new List<string> { "Bone Helm", "Bone Chest", "Bone Arms", "Bone Gloves", "Bone Legs" },
            ["Dragon"] = new List<string> { "Dragon Helm", "Dragon Chest", "Dragon Arms", "Dragon Gloves", "Dragon Legs" },
            ["Tribal"] = new List<string> { "Tribal Mask", "Tribal Chest", "Tribal Arms", "Tribal Legs" }
        };

        // Dictionary to map Plate Helm types to their ItemIDs
        private static readonly Dictionary<string, string> PlateHelmItemIDs = new Dictionary<string, string>
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

        // Dictionary to map Plate Chest types to their ItemIDs
        private static readonly Dictionary<string, string> PlateChestItemIDs = new Dictionary<string, string>
        {
            ["Plate Chest 1 (0x1415)"] = "0x1415",
            ["Plate Chest 2 (0x1416)"] = "0x1416"
        };

        // Dictionary to map Plate Arms types to their ItemIDs
        private static readonly Dictionary<string, string> PlateArmsItemIDs = new Dictionary<string, string>
        {
            ["Plate Arms 1 (0x1410)"] = "0x1410",
            ["Plate Arms 2 (0x1417)"] = "0x1417"
        };

        // Dictionary to map Plate Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> PlateGlovesItemIDs = new Dictionary<string, string>
        {
            ["Plate Gloves 1 (0x1414)"] = "0x1414",
            ["Plate Gloves 2 (0x1418)"] = "0x1418"
        };

        // Dictionary to map Plate Legs types to their ItemIDs
        private static readonly Dictionary<string, string> PlateLegsItemIDs = new Dictionary<string, string>
        {
            ["Plate Legs 1 (0x1411)"] = "0x1411",
            ["Plate Legs 2 (0x141A)"] = "0x141A"
        };

        // Dictionary to map Tribal Mask types to their ItemIDs
        private static readonly Dictionary<string, string> TribalMaskItemIDs = new Dictionary<string, string>
        {
            ["Tribal Mask 1 (0x1549)"] = "0x1549",
            ["Tribal Mask 2 (0x154A)"] = "0x154A",
            ["Tribal Mask 3 (0x154B)"] = "0x154B",
            ["Tribal Mask 4 (0x154C)"] = "0x154C"
        };

        // Dictionary to map Chain Chest types to their ItemIDs
        private static readonly Dictionary<string, string> ChainChestItemIDs = new Dictionary<string, string>
        {
            ["Chain Chest 1 (0x13BF)"] = "0x13BF",
            ["Chain Chest 2 (0x13C4)"] = "0x13C4"
        };

        // Dictionary to map Chain Legs types to their ItemIDs
        private static readonly Dictionary<string, string> ChainLegsItemIDs = new Dictionary<string, string>
        {
            ["Chain Legs 1 (0x13BE)"] = "0x13BE",
            ["Chain Legs 2 (0x13C3)"] = "0x13C3"
        };

        // Dictionary to map Chain Coif types to their ItemIDs
        private static readonly Dictionary<string, string> ChainCoifItemIDs = new Dictionary<string, string>
        {
            ["Chain Coif 1 (0x13BB)"] = "0x13BB",
            ["Chain Coif 2 (0x13C0)"] = "0x13C0"
        };

        // Dictionary to map Ringmail Chest types to their ItemIDs
        private static readonly Dictionary<string, string> RingmailChestItemIDs = new Dictionary<string, string>
        {
            ["Ringmail Chest 1 (0x13EC)"] = "0x13EC",
            ["Ringmail Chest 2 (0x13ED)"] = "0x13ED"
        };

        // Dictionary to map Ringmail Legs types to their ItemIDs
        private static readonly Dictionary<string, string> RingmailLegsItemIDs = new Dictionary<string, string>
        {
            ["Ringmail Legs 1 (0x13F0)"] = "0x13F0",
            ["Ringmail Legs 2 (0x13F1)"] = "0x13F1"
        };

        // Dictionary to map Ringmail Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> RingmailGlovesItemIDs = new Dictionary<string, string>
        {
            ["Ringmail Gloves 1 (0x13EB)"] = "0x13EB",
            ["Ringmail Gloves 2 (0x13F2)"] = "0x13F2"
        };

        // Dictionary to map Leather Chest types to their ItemIDs
        private static readonly Dictionary<string, string> LeatherChestItemIDs = new Dictionary<string, string>
        {
            ["Leather Chest 1 (0x13CC)"] = "0x13CC",
            ["Leather Chest 2 (0x13D3)"] = "0x13D3"
        };

        // Dictionary to map Leather Legs types to their ItemIDs
        private static readonly Dictionary<string, string> LeatherLegsItemIDs = new Dictionary<string, string>
        {
            ["Leather Legs 1 (0x13CB)"] = "0x13CB",
            ["Leather Legs 2 (0x13D2)"] = "0x13D2"
        };

        // Dictionary to map Leather Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> LeatherGlovesItemIDs = new Dictionary<string, string>
        {
            ["Leather Gloves 1 (0x13C6)"] = "0x13C6",
            ["Leather Gloves 2 (0x13CE)"] = "0x13CE"
        };

        // Dictionary to map Leather Cap types to their ItemIDs
        private static readonly Dictionary<string, string> LeatherCapItemIDs = new Dictionary<string, string>
        {
            ["Leather Cap 1 (0x1DB9)"] = "0x1DB9",
            ["Leather Cap 2 (0x1DBA)"] = "0x1DBA"
        };

        // Dictionary to map Studded Chest types to their ItemIDs
        private static readonly Dictionary<string, string> StuddedChestItemIDs = new Dictionary<string, string>
        {
            ["Studded Chest 1 (0x13DB)"] = "0x13DB",
            ["Studded Chest 2 (0x13E2)"] = "0x13E2"
        };

        // Dictionary to map Studded Legs types to their ItemIDs
        private static readonly Dictionary<string, string> StuddedLegsItemIDs = new Dictionary<string, string>
        {
            ["Studded Legs 1 (0x13DA)"] = "0x13DA",
            ["Studded Legs 2 (0x13E1)"] = "0x13E1"
        };

        // Dictionary to map Studded Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> StuddedGlovesItemIDs = new Dictionary<string, string>
        {
            ["Studded Gloves 1 (0x13D5)"] = "0x13D5",
            ["Studded Gloves 2 (0x13DD)"] = "0x13DD"
        };

        // Dictionary to map Bone Helm types to their ItemIDs
        private static readonly Dictionary<string, string> BoneHelmItemIDs = new Dictionary<string, string>
        {
            ["Bone Helm 1 (0x1451)"] = "0x1451",
            ["Bone Helm 2 (0x1456)"] = "0x1456"
        };

        // Dictionary to map Bone Chest types to their ItemIDs
        private static readonly Dictionary<string, string> BoneChestItemIDs = new Dictionary<string, string>
        {
            ["Bone Chest 1 (0x144F)"] = "0x144F",
            ["Bone Chest 2 (0x1454)"] = "0x1454"
        };

        // Dictionary to map Bone Arms types to their ItemIDs
        private static readonly Dictionary<string, string> BoneArmsItemIDs = new Dictionary<string, string>
        {
            ["Bone Arms 1 (0x144E)"] = "0x144E",
            ["Bone Arms 2 (0x1453)"] = "0x1453"
        };

        // Dictionary to map Bone Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> BoneGlovesItemIDs = new Dictionary<string, string>
        {
            ["Bone Gloves 1 (0x1450)"] = "0x1450",
            ["Bone Gloves 2 (0x1455)"] = "0x1455"
        };

        // Dictionary to map Bone Legs types to their ItemIDs
        private static readonly Dictionary<string, string> BoneLegsItemIDs = new Dictionary<string, string>
        {
            ["Bone Legs 1 (0x1452)"] = "0x1452",
            ["Bone Legs 2 (0x1457)"] = "0x1457"
        };

        // Dictionary to map Dragon Helm types to their ItemIDs
        private static readonly Dictionary<string, string> DragonHelmItemIDs = new Dictionary<string, string>
        {
            ["Dragon Helm 1 (0x2645)"] = "0x2645",
            ["Dragon Helm 2 (0x2646)"] = "0x2646"
        };

        // Dictionary to map Dragon Chest types to their ItemIDs
        private static readonly Dictionary<string, string> DragonChestItemIDs = new Dictionary<string, string>
        {
            ["Dragon Chest 1 (0x2641)"] = "0x2641",
            ["Dragon Chest 2 (0x2642)"] = "0x2642"
        };

        // Dictionary to map Dragon Arms types to their ItemIDs
        private static readonly Dictionary<string, string> DragonArmsItemIDs = new Dictionary<string, string>
        {
            ["Dragon Arms 1 (0x2657)"] = "0x2657",
            ["Dragon Arms 2 (0x2658)"] = "0x2658"
        };

        // Dictionary to map Dragon Gloves types to their ItemIDs
        private static readonly Dictionary<string, string> DragonGlovesItemIDs = new Dictionary<string, string>
        {
            ["Dragon Gloves 1 (0x2643)"] = "0x2643",
            ["Dragon Gloves 2 (0x2644)"] = "0x2644"
        };

        // Dictionary to map Dragon Legs types to their ItemIDs
        private static readonly Dictionary<string, string> DragonLegsItemIDs = new Dictionary<string, string>
        {
            ["Dragon Legs 1 (0x2647)"] = "0x2647",
            ["Dragon Legs 2 (0x2648)"] = "0x2648"
        };

        // Dictionary to map Tribal Chest types to their ItemIDs
        private static readonly Dictionary<string, string> TribalChestItemIDs = new Dictionary<string, string>
        {
            ["Tribal Chest 1 (0x144F)"] = "0x144F",
            ["Tribal Chest 2 (0x1454)"] = "0x1454"
        };

        // Dictionary to map Tribal Arms types to their ItemIDs
        private static readonly Dictionary<string, string> TribalArmsItemIDs = new Dictionary<string, string>
        {
            ["Tribal Arms 1 (0x144E)"] = "0x144E",
            ["Tribal Arms 2 (0x1453)"] = "0x1453"
        };

        // Dictionary to map Tribal Legs types to their ItemIDs
        private static readonly Dictionary<string, string> TribalLegsItemIDs = new Dictionary<string, string>
        {
            ["Tribal Legs 1 (0x1452)"] = "0x1452",
            ["Tribal Legs 2 (0x1457)"] = "0x1457"
        };

        // Dictionary to map armor types to their corresponding item ID dictionaries
        private static readonly Dictionary<string, Dictionary<string, string>> ArmorTypeItemIDMappings = new Dictionary<string, Dictionary<string, string>>
        {
            ["Plate Helm"] = PlateHelmItemIDs,
            ["Plate Chest"] = PlateChestItemIDs,
            ["Plate Arms"] = PlateArmsItemIDs,
            ["Plate Gloves"] = PlateGlovesItemIDs,
            ["Plate Legs"] = PlateLegsItemIDs,
            ["Tribal Mask"] = TribalMaskItemIDs,
            ["Chain Chest"] = ChainChestItemIDs,
            ["Chain Legs"] = ChainLegsItemIDs,
            ["Chain Coif"] = ChainCoifItemIDs,
            ["Ringmail Chest"] = RingmailChestItemIDs,
            ["Ringmail Legs"] = RingmailLegsItemIDs,
            ["Ringmail Gloves"] = RingmailGlovesItemIDs,
            ["Leather Chest"] = LeatherChestItemIDs,
            ["Leather Legs"] = LeatherLegsItemIDs,
            ["Leather Gloves"] = LeatherGlovesItemIDs,
            ["Leather Cap"] = LeatherCapItemIDs,
            ["Studded Chest"] = StuddedChestItemIDs,
            ["Studded Legs"] = StuddedLegsItemIDs,
            ["Studded Gloves"] = StuddedGlovesItemIDs,
            ["Bone Helm"] = BoneHelmItemIDs,
            ["Bone Chest"] = BoneChestItemIDs,
            ["Bone Arms"] = BoneArmsItemIDs,
            ["Bone Gloves"] = BoneGlovesItemIDs,
            ["Bone Legs"] = BoneLegsItemIDs,
            ["Dragon Helm"] = DragonHelmItemIDs,
            ["Dragon Chest"] = DragonChestItemIDs,
            ["Dragon Arms"] = DragonArmsItemIDs,
            ["Dragon Gloves"] = DragonGlovesItemIDs,
            ["Dragon Legs"] = DragonLegsItemIDs,
            ["Tribal Chest"] = TribalChestItemIDs,
            ["Tribal Arms"] = TribalArmsItemIDs,
            ["Tribal Legs"] = TribalLegsItemIDs
        };

        private float zoomFactor = 1.0f;
        private Bitmap backgroundImage;
        private Bitmap currentArmorImage;

        public armorGenerator()
        {
            InitializeComponent();
            // TabControl Hidden On Load
            armorGenerator_opacityPanel_darkTabControl.TabPages.Remove(armorGenerator_opacityPanel_darkTabControl_tabPage_setProperties);
            // Stop Control Flicker
            ControlHelper.EnableDoubleBuffering(this);
            // OpacityPanel Is @ 30%
            armorGenerator_opacityPanel.Opacity = 0.3f;
            // ComboBox Configuration
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor.Items.AddRange(new object[] { "Plate", "Chainmail", "Ringmail", "Studded", "Leather", "Bone", "Dragon", "Tribal" });
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor.SelectedIndexChanged += armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor_SelectedIndexChanged;
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.SelectedIndexChanged += armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType_SelectedIndexChanged;
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.SelectedIndexChanged += armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList_SelectedIndexChanged;
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_armorHue.ValueChanged += armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_armorHue_ValueChanged;
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_lootType.Items.AddRange(new object[] { "Regular", "Blessed", "Cursed", "Newbied" });
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.Items.AddRange(new object[] { /*Metallic...*/  "Iron", "DullCopper", "ShadowIron", "Copper", "Bronze", "Gold", "Agapite", "Verite", "Valorite",
                                                                                                                                                                   /*Leather....*/  "RegularLeather", "SpinedLeather", "HornedLeather", "BarbedLeather",
                                                                                                                                                                   /*Bone.......*/  "Bone",
                                                                                                                                                                   /*Scales.....*/  "RedScales", "YellowScales", "BlackScales", "GreenScales", "WhiteScales", "BlueScales",
                                                                                                                                                                   /*Wooden.....*/  "RegularWood", "OakWood", "AshWood", "YewWood", "HeartWood", "BloodWood", "FrostWood",
                                                                                                                                                                   /*Stone......*/  "Granite", "DullCopperGranite", "ShadowIronGranite", "CopperGranite", "BronzeGranite", "GoldGranite", "AgapiteGranite", "VeriteGranite", "ValoriteGranite",
                                                                                                                                                                   /*Rock.......*/  "BlackRock" });
            // Set the Multiple Armor Type List ComboBox to be invisible initially
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Visible = false;

            // Enable MouseWheel event for the preview panel
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.MouseWheel += armorTypePreview_MouseWheel;
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Paint += armorTypePreview_Paint;

            // Load the background image
            LoadBackgroundImage();

            // TabPages Configuration
            armorGenerator_opacityPanel_darkTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            armorGenerator_opacityPanel_darkTabControl.ItemSize = new Size(140, 30);
            armorGenerator_opacityPanel_darkTabControl.SizeMode = TabSizeMode.Fixed;
            armorGenerator_opacityPanel_darkTabControl.DrawItem += ArmorGenerator_OpacityPanel_darkTabControl_DrawItem;
        }

        private void LoadBackgroundImage()
        {
            try
            {
                // Load the background image from Resources.resx
                backgroundImage = new Bitmap(Properties.Resources.bkd_003); // Replace 'bkd_003' with the name of your resource
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundImage = new Bitmap(512, 512);
                using (Graphics g = Graphics.FromImage(backgroundImage))
                {
                    g.Clear(Color.LightGray);
                }
            }
        }

        private void InitializeArmorTypeDetailsComboBox(string armorType)
        {
            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Items.Clear();

            // Check if the selected armor type has multiple pieces
            if (ArmorTypeItemIDMappings.TryGetValue(armorType, out var itemIDMappings))
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Items.AddRange(itemIDMappings.Keys.ToArray());
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Visible = true;
            }
            else
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Visible = false;
            }

            if (armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Items.Count > 0)
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.SelectedIndex = 0;
            }
        }

        private void DisplayArtPreview(int itemID, int hue)
        {
            try
            {
                Bitmap artBitmap = ScriptGenie.UltimaSDK.Art.GetStatic(itemID);
                if (artBitmap != null)
                {
                    Bitmap displayBitmap = new Bitmap(artBitmap);

                    // Only apply hue if it's not the default preview (hue != 0)
                    if (hue != 0)
                    {
                        Bitmap coloredBitmap = ApplyHue(artBitmap, hue);
                        if (coloredBitmap != null)
                        {
                            displayBitmap.Dispose();
                            displayBitmap = coloredBitmap;
                        }
                    }

                    // Dispose of the old armor image if it exists
                    if (currentArmorImage != null)
                    {
                        currentArmorImage.Dispose();
                    }
                    currentArmorImage = displayBitmap;

                    armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Refresh();
                }
                else
                {
                    MessageBox.Show($"Failed to load art for item ID: {itemID:X4}");
                    currentArmorImage = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading preview: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                currentArmorImage = null;
            }
        }

        private Bitmap ApplyHue(Bitmap bitmap, int hue)
        {
            if (bitmap == null)
                return null;

            Bitmap coloredBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

            try
            {
                using (Graphics g = Graphics.FromImage(coloredBitmap))
                {
                    g.DrawImage(bitmap, Point.Empty);
                }

                BitmapData bd = coloredBitmap.LockBits(new Rectangle(0, 0, coloredBitmap.Width, coloredBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                int bytes = Math.Abs(bd.Stride) * coloredBitmap.Height;
                byte[] rgbValues = new byte[bytes];

                Marshal.Copy(bd.Scan0, rgbValues, 0, bytes);

                ScriptGenie.UltimaSDK.Hue hueObj = ScriptGenie.UltimaSDK.Hues.GetHue(hue);

                for (int i = 0; i < rgbValues.Length; i += 4)
                {
                    byte b = rgbValues[i];
                    byte g = rgbValues[i + 1];
                    byte r = rgbValues[i + 2];
                    byte a = rgbValues[i + 3];

                    if (a != 0)
                    {
                        int index = (r >> 3) & 0x1F;
                        if (index >= 0 && index < hueObj.Colors.Length)
                        {
                            short colorEntry = hueObj.Colors[index];
                            rgbValues[i] = (byte)((colorEntry & 0x1F) * 8); // B
                            rgbValues[i + 1] = (byte)(((colorEntry >> 5) & 0x1F) * 8); // G
                            rgbValues[i + 2] = (byte)(((colorEntry >> 10) & 0x1F) * 8); // R
                        }
                    }
                }

                Marshal.Copy(rgbValues, 0, bd.Scan0, bytes);
                coloredBitmap.UnlockBits(bd);
            }
            catch (Exception ex)
            {
                coloredBitmap.Dispose();
                throw new Exception($"Error applying hue: {ex.Message}", ex);
            }

            return coloredBitmap;
        }

        private void UpdatePreview()
        {
            string selectedArmorType = armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.SelectedItem?.ToString();
            string selectedArmorTypeDetails = armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.SelectedItem?.ToString();
            int hue = (int)armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_armorHue.Value;

            if (selectedArmorTypeDetails != null && ArmorTypeItemIDMappings.TryGetValue(selectedArmorType, out var itemIDMappings))
            {
                var match = Regex.Match(selectedArmorTypeDetails, @"\((0x[0-9A-Fa-f]+)\)");
                if (match.Success)
                {
                    int itemID = Convert.ToInt32(match.Groups[1].Value, 16);
                    DisplayArtPreview(itemID, hue);
                }
            }
            else if (Exporters.Armor.ArmorTypeItemIDs.TryGetValue(selectedArmorType, out string itemIDStr))
            {
                int itemID = Convert.ToInt32(itemIDStr, 16);
                DisplayArtPreview(itemID, hue);
            }
        }

        private void armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedArmorType = armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.SelectedItem?.ToString();

            InitializeArmorTypeDetailsComboBox(selectedArmorType);
            UpdatePreview();
        }

        private void armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkNumericUpDown_armorHue_ValueChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBaseArmor = armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_baseArmor.SelectedItem?.ToString();

            if (selectedBaseArmor != null && ValidResources.ContainsKey(selectedBaseArmor))
            {
                // Clear the current items in CraftResourceType ComboBox
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.Items.Clear();
                // Add the valid resources for the selected base armor type
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.Items.AddRange(ValidResources[selectedBaseArmor].ToArray());
                // Select the first item by default
                if (armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.Items.Count > 0)
                {
                    armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_craftResourceType.SelectedIndex = 0;
                }
            }
            // Update ArmorType ComboBox based on selected BaseArmorType
            if (selectedBaseArmor != null && BaseArmorTypesToPieces.ContainsKey(selectedBaseArmor))
            {
                // Clear the current items in ArmorType ComboBox
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.Items.Clear();
                // Add the valid armor pieces for the selected base armor type
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.Items.AddRange(BaseArmorTypesToPieces[selectedBaseArmor].ToArray());
                // Select the first item by default
                if (armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.Items.Count > 0)
                {
                    armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkTextBox_armorType.SelectedIndex = 0;
                }
            }
            UpdatePreview();
        }

        private void ArmorGenerator_OpacityPanel_darkTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl == null || e.Index < 0)
                return;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabBounds = tabControl.GetTabRect(e.Index);
            // Fill the tab background
            using (Brush backBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            {
                e.Graphics.FillRectangle(backBrush, tabBounds);
            }
            // Draw centered text
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabBounds, sf);
            }
            // Optional: Draw a border
            using (Pen borderPen = new Pen(Color.DimGray))
            {
                e.Graphics.DrawRectangle(borderPen, tabBounds);
            }
        }

        private void armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkCheckBox_armorSet_CheckedChanged(object sender, EventArgs e)
        {
            if (armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkCheckBox_armorSet.Checked)
            {
                // Show the tab if checkbox is checked
                armorGenerator_opacityPanel_darkTabControl.TabPages.Add(armorGenerator_opacityPanel_darkTabControl_tabPage_setProperties);
            }
            else
            {
                // Clears the controls when unchecked
                armorGenerator_opacityPanel_darkTabControl_tabPage_setProperties.Controls.Clear();
                // Hide the tab if checkbox is unchecked
                armorGenerator_opacityPanel_darkTabControl.TabPages.Remove(armorGenerator_opacityPanel_darkTabControl_tabPage_setProperties);
            }
        }

        private void armorTypePreview_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomFactor *= 1.1f; // Zoom in
            }
            else if (e.Delta < 0)
            {
                zoomFactor *= 0.9f; // Zoom out
                if (zoomFactor < 0.1f) zoomFactor = 0.1f; // Minimum zoom level
            }

            armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Refresh();
        }

        private void armorTypePreview_Paint(object sender, PaintEventArgs e)
        {
            // Draw the background image stretched to fit the preview panel
            if (backgroundImage != null)
            {
                e.Graphics.DrawImage(backgroundImage, 0, 0,
                    armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Width,
                    armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Height);
            }

            // Draw the current armor image on top of the background image
            if (currentArmorImage != null)
            {
                int newWidth = (int)(currentArmorImage.Width * zoomFactor);
                int newHeight = (int)(currentArmorImage.Height * zoomFactor);

                // Calculate the center of the preview panel
                Point center = new Point(armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Width / 2,
                                          armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_opacityPanel_armorTypePreview.Height / 2);

                // Center the image
                Rectangle destRect = new Rectangle(center.X - newWidth / 2, center.Y - newHeight / 2, newWidth, newHeight);

                e.Graphics.DrawImage(currentArmorImage, destRect);
            }
        }
    }
}
