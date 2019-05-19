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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3 , Screen.PrimaryScreen.WorkingArea.Height / 2);

            this.Opacity = 0.95;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#1d1d1d");

            Button settingsButton = new Button();
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.BackColor = this.BackColor;
            //settingsButton.FlatAppearance.BorderColor = Color.Transparent;
            settingsButton.BackgroundImage = Properties.Resources.Gear_Dark;
            settingsButton.BackgroundImageLayout = ImageLayout.Center;
            settingsButton.Size = settingsButton.BackgroundImage.Size;
            settingsButton.AutoSize = false;
            settingsButton.Cursor = Cursors.Hand;
            settingsButton.FlatAppearance.CheckedBackColor = settingsButton.BackColor;
            settingsButton.FlatAppearance.MouseOverBackColor = settingsButton.BackColor;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.Location = new Point(8, this.Height - 72);
            settingsButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

            Button aboutButton = new Button();
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.BackColor = this.BackColor;
            //settingsButton.FlatAppearance.BorderColor = Color.Transparent;
            aboutButton.BackgroundImage = Properties.Resources.QuestionMark_Dark;
            aboutButton.BackgroundImageLayout = ImageLayout.Center;
            aboutButton.Size = aboutButton.BackgroundImage.Size;
            aboutButton.AutoSize = false;
            aboutButton.Cursor = Cursors.Hand;
            aboutButton.FlatAppearance.CheckedBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.MouseOverBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.BorderSize = 0;
            aboutButton.Location = new Point(this.Width - 48, this.Height - 72);
            aboutButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);

            Label versionLabel = new Label();
            versionLabel.Text = ProductName + " - " + ProductVersion;
            versionLabel.ForeColor = Color.White;
            versionLabel.AutoSize = false;
            versionLabel.Anchor = AnchorStyles.Bottom;
            versionLabel.Dock = DockStyle.Bottom;
            versionLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(settingsButton);
            this.Controls.Add(aboutButton);
            this.Controls.Add(versionLabel);

        }
    }
}
