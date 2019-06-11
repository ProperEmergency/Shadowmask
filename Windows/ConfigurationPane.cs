using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class ConfigurationPane : Form
    {
        public ConfigurationPane()
        {
            this.Name = "Configuration Panel";
            this.Text = "Shadowmask - Settings";
            this.Icon = Icon.FromHandle(Properties.Resources.StudioLogo.GetHicon());
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;

            

            this.Opacity = 0.95;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#1d1d1d");

            this.Deactivate += new EventHandler(Form_LostFocus);

            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3, Screen.PrimaryScreen.WorkingArea.Height / 3);

            Button settingsButton = new Button();
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.BackColor = this.BackColor;
            settingsButton.BackgroundImage = Properties.Resources.Gear_Dark;
            settingsButton.BackgroundImageLayout = ImageLayout.Center;
            settingsButton.Size = settingsButton.BackgroundImage.Size;
            settingsButton.AutoSize = false;
            settingsButton.Cursor = Cursors.Hand;
            settingsButton.FlatAppearance.CheckedBackColor = settingsButton.BackColor;
            settingsButton.FlatAppearance.MouseOverBackColor = settingsButton.BackColor;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.Location = new Point(10, this.Height - settingsButton.Height - 10);
            settingsButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

            Button aboutButton = new Button();
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.BackColor = this.BackColor;
            aboutButton.BackgroundImage = Properties.Resources.QuestionMark_Dark;
            aboutButton.BackgroundImageLayout = ImageLayout.Center;
            aboutButton.Size = aboutButton.BackgroundImage.Size;
            aboutButton.AutoSize = false;
            aboutButton.Cursor = Cursors.Hand;
            aboutButton.FlatAppearance.CheckedBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.MouseOverBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.BorderSize = 0;
            aboutButton.Location = new Point(this.Width - aboutButton.Width - 10, this.Height - aboutButton.Height - 10);
            aboutButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            aboutButton.Click += AboutButton_HandleClick;

            Label versionLabel = new Label();
            versionLabel.Text = ProductName + " - " + ProductVersion;
            versionLabel.ForeColor = Color.White;
            versionLabel.AutoSize = false;
            versionLabel.Anchor = AnchorStyles.Bottom;
            versionLabel.Dock = DockStyle.Bottom;
            versionLabel.Location = new Point((this.Width / 2) - (versionLabel.Width / 2), this.Height - versionLabel.Height);
            versionLabel.TextAlign = ContentAlignment.MiddleCenter;

            FlowLayoutPanel monitor_selectionPanel = new FlowLayoutPanel();
            monitor_selectionPanel.BackColor = this.BackColor;
            monitor_selectionPanel.Anchor = AnchorStyles.None;
            monitor_selectionPanel.AutoSize = true;
            monitor_selectionPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            int screenCount = 1;

            foreach (Screen activeDisplay in Screen.AllScreens)
            {
                Button monitor = new Button();
                monitor.Text = screenCount.ToString();
                monitor.Anchor = AnchorStyles.None;
                monitor.Dock = DockStyle.None;
                monitor.Cursor = Cursors.Hand;
                monitor.FlatAppearance.BorderColor = Color.White;
                monitor.FlatStyle = FlatStyle.Flat;
                monitor.FlatAppearance.MouseOverBackColor = Color.Blue;


                monitor.Size = new Size(activeDisplay.WorkingArea.Width / 10, activeDisplay.WorkingArea.Height / 10);

                monitor_selectionPanel.Controls.Add(monitor);

                screenCount++;
            }

            this.Controls.Add(monitor_selectionPanel);
            monitor_selectionPanel.Location = new Point((this.Width / 2 - (monitor_selectionPanel.Width / 2)), (this.Height / 2 - (monitor_selectionPanel.Height / 2)));

            this.Controls.Add(settingsButton);
            this.Controls.Add(aboutButton);
            this.Controls.Add(versionLabel);
        }

        private void InitalizeApplicationSettings()
        {

        }

        private void AboutButton_HandleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ProperEmergency/Shadowmask");
        }

        private void Form_LostFocus(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("hide");
            this.Hide();
        }
    }
}
