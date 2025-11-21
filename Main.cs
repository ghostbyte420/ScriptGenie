using ScriptGenie.Controls.ArmorGenerator;
using ScriptGenie.Controls.ItemGenerator;
using ScriptGenie.Controls.MobileGenerator;
using ScriptGenie.Controls.QuestGenerator;
using ScriptGenie.Controls.WeaponGenerator;
using ScriptGenie.Exporters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptGenie
{
    public partial class scriptGenieMain : Form
    {
        public scriptGenieMain()
        {
            InitializeComponent();

            PopulateGenerationTypeComboBox();
            PopulateServerTypeComboBox();
        }

        private void scriptGenieMain_Load(object sender, EventArgs e)
        {
            scriptGenieMain_splitDisplay.Panel2.Controls.Clear();
        }

        #region Load Generators Into scriptGenieMain_splitDisplay.Panel1 (ComboBox Selections)

        private void PopulateGenerationTypeComboBox()
        {
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Items.Clear();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Items.Add(" "); // Default prompt

            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Items.AddRange(new object[]
            {
                "Armor",
                "Weapon",
                "Item",
                "Mobile",
                "Quest"
            });

            if (scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Items.Count > 0)
            {
                scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.SelectedIndex = 0;
            }
        }

        private void PopulateServerTypeComboBox()
        {
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Items.Clear();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Items.Add(" "); // Default prompt

            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Items.AddRange(new object[]
            {
                "RunUO",
                "ServUO"
            });

            if (scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Items.Count > 0)
            {
                scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.SelectedIndex = 0;
            }
        }

        #endregion

        #region Load Generators Into scriptGenieMain_splitDisplay.Panel2 (Selection Functions)

        private void scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGenerator = scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.SelectedItem?.ToString();

            if (selectedGenerator == null)
            {
                return;
            }

            scriptGenieMain_splitDisplay.Panel2.Controls.Clear();

            switch (selectedGenerator)
            {
                case "Armor":
                    scriptGenieMain_splitDisplay.Panel2.Controls.Add(new armorGenerator { Dock = DockStyle.Fill });
                    break;
                case "Weapon":
                    scriptGenieMain_splitDisplay.Panel2.Controls.Add(new weaponGenerator { Dock = DockStyle.Fill });
                    break;
                case "Item":
                    scriptGenieMain_splitDisplay.Panel2.Controls.Add(new itemGenerator { Dock = DockStyle.Fill });
                    break;
                case "Mobile":
                    scriptGenieMain_splitDisplay.Panel2.Controls.Add(new mobileGenerator { Dock = DockStyle.Fill });
                    break;
                case "Quest":
                    scriptGenieMain_splitDisplay.Panel2.Controls.Add(new questGenerator { Dock = DockStyle.Fill });
                    break;
            }
        }

        private void scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGenerator = scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.SelectedItem?.ToString();

            if (selectedGenerator == null)
            {
                return;
            }

            switch (selectedGenerator)
            {
                case "RunUO":
                    // Exports To RunUO Script Logic - maybe put the RunUO script template into a seperate class file
                    break;
                case "ServUO":
                    // Exports To ServUO Script Logic - maybe put the ServUO script template into a seperate class file
                    break;
            }
        }


        #endregion

        private void scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export_Click(object sender, EventArgs e)
        {
            string selectedGenerator = scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.SelectedItem?.ToString();
            string selectedServer = scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.SelectedItem?.ToString();

            #region Armor Export

            if (selectedGenerator != "Armor" || selectedServer == null || selectedServer == " ")
            {
                MessageBox.Show("Please pick 'Armor' and a server (RunUO or ServUO).");
                return;
            }

            if (!(scriptGenieMain_splitDisplay.Panel2.Controls[0] is armorGenerator armorGen))
            {
                MessageBox.Show("Oops! The armor generator isn't ready.");
                return;
            }

            bool exportSuccess = Armor.Export(armorGen, selectedServer);
            if (!exportSuccess)
            {
                // The Export method already shows a message box for errors, so no further action is needed here.
            }

            #endregion
        }
    }
}
