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
            ["Studded"] = new List<string> { "Studded Chest", "Studded Legs", "Studded Gorget", "Studded Gloves" },
            ["Leather"] = new List<string> { "Leather Chest", "Leather Legs", "Leather Gorget", "Leather Gloves", "Leather Cap" },
            ["Bone"] = new List<string> { "Bone Helm", "Bone Chest", "Bone Arms", "Bone Gloves", "Bone Legs" },
            ["Dragon"] = new List<string> { "Dragon Helm", "Dragon Chest", "Dragon Arms", "Dragon Gloves", "Dragon Legs" },
            ["Tribal"] = new List<string> { "Tribal Mask", "Tribal Chest", "Tribal Legs" }
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

        // Dictionary to map Tribal Mask types to their ItemIDs
        private static readonly Dictionary<string, string> TribalMaskItemIDs = new Dictionary<string, string>
        {
            ["Tribal Mask 1 (0x1549)"] = "0x1549",
            ["Tribal Mask 2 (0x154A)"] = "0x154A",
            ["Tribal Mask 3 (0x154B)"] = "0x154B",
            ["Tribal Mask 4 (0x154C)"] = "0x154C"
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
                backgroundImage = new Bitmap(Properties.Resources.bkd_003); // Replace 'armor_background' with the name of your resource
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

            if (armorType == "Tribal Mask")
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Items.AddRange(TribalMaskItemIDs.Keys.ToArray());
            }
            else if (armorType == "Plate Helm")
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Items.AddRange(PlateHelmItemIDs.Keys.ToArray());
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

            if (selectedArmorType == "Tribal Mask" && selectedArmorTypeDetails != null)
            {
                var match = Regex.Match(selectedArmorTypeDetails, @"\((0x[0-9A-Fa-f]+)\)");
                if (match.Success)
                {
                    int itemID = Convert.ToInt32(match.Groups[1].Value, 16);
                    DisplayArtPreview(itemID, hue);
                }
            }
            else if (selectedArmorType == "Plate Helm" && selectedArmorTypeDetails != null)
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

            if (selectedArmorType == "Tribal Mask" || selectedArmorType == "Plate Helm")
            {
                InitializeArmorTypeDetailsComboBox(selectedArmorType);
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Visible = true;
            }
            else
            {
                armorGenerator_opacityPanel_darkTabControl_tabPage_armorConstructor_opacityPanel_controlA_darkComboBox_multipleArmorTypeList.Visible = false;
            }
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
