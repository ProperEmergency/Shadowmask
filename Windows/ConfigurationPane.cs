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
            this.TopMost = true;

            this.Opacity = 0.98;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#1d1d1d");

            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3, Screen.PrimaryScreen.WorkingArea.Height / 2);

            Button exitButton = new Button();
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.BackColor = this.BackColor;
            exitButton.BackgroundImage = Properties.Resources.Exit;
            exitButton.BackgroundImageLayout = ImageLayout.Center;
            exitButton.Size = exitButton.BackgroundImage.Size;
            exitButton.AutoSize = false;
            exitButton.Cursor = Cursors.Hand;
            exitButton.FlatAppearance.CheckedBackColor = exitButton.BackColor;
            exitButton.FlatAppearance.MouseOverBackColor = exitButton.BackColor;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.Location = new Point((this.Width - exitButton.Width - 10) , 10);
            exitButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            exitButton.TabStop = false;

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
            settingsButton.TabStop = false;

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
            aboutButton.TabStop = false;

            Label instructionsLabel = new Label();
            instructionsLabel.Text = "Please select a screen to configure:";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            instructionsLabel.ForeColor = Color.White;
            instructionsLabel.AutoSize = true;
            instructionsLabel.Anchor = AnchorStyles.Top;
            instructionsLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(instructionsLabel);
            instructionsLabel.Location = new Point((this.Width / 2) - (instructionsLabel.Width / 2), instructionsLabel.Height + exitButton.Height);

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
                Button monitorButton = new Button();
                monitorButton.Text = screenCount.ToString();
                monitorButton.Anchor = AnchorStyles.None;
                monitorButton.Dock = DockStyle.None;
                monitorButton.Cursor = Cursors.Hand;
                monitorButton.FlatStyle = FlatStyle.Flat;
                monitorButton.FlatAppearance.MouseOverBackColor = System.Drawing.ColorTranslator.FromHtml("#2d89ef");
                monitorButton.BackColor = Color.LightGray;
                monitorButton.FlatAppearance.BorderSize = 0;

                monitorButton.TabStop = false;


                monitorButton.Size = new Size(activeDisplay.WorkingArea.Width / 10, activeDisplay.WorkingArea.Height / 10);

                monitorButton.GotFocus += MonitorButton_GotFocus;
                monitorButton.LostFocus += MonitorButton_LostFocus;
                monitorButton.Click += Monitor_Click;


                void MonitorButton_GotFocus(object sender, EventArgs e)
                {
                    monitorButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#2d89ef");
                    monitorButton.FlatAppearance.BorderSize = 0;
                }

                void MonitorButton_LostFocus(object sender, EventArgs e)
                {
                    monitorButton.BackColor = Color.LightGray;
                    monitorButton.FlatAppearance.BorderSize = 0;
                }

                void Monitor_Click(object sender, EventArgs e)
                {
                }

                monitor_selectionPanel.Controls.Add(monitorButton);

                screenCount++;
            }

            this.Controls.Add(monitor_selectionPanel);
            monitor_selectionPanel.Location = new Point((this.Width / 2 - (monitor_selectionPanel.Width / 2)), (instructionsLabel.Location.Y + instructionsLabel.Height + 10));

            Label contentType_Label = new Label();
            contentType_Label.Text = "Please select the type of content you wish to display.";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            contentType_Label.ForeColor = Color.White;
            contentType_Label.AutoSize = true;
            contentType_Label.Anchor = AnchorStyles.Top;
            contentType_Label.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(contentType_Label);
            contentType_Label.Location = new Point((this.Width / 2) - (contentType_Label.Width / 2), (monitor_selectionPanel.Location.Y + monitor_selectionPanel.Height + 20));

            ComboBox contentType = new ComboBox();
            contentType.DropDownStyle = ComboBoxStyle.DropDownList;
            contentType.FlatStyle = FlatStyle.Flat;
            contentType.Items.Add("None");
            contentType.Items.Add("Webpage");
            contentType.Items.Add("Image");
            contentType.Items.Add("YouTube Video");
            contentType.Items.Add("Video File");
            contentType.SelectedIndex = 0;
            contentType.TabStop = false;
            contentType.Location = new Point((this.Width / 2 - (contentType.Width / 2)), (contentType_Label.Location.Y + contentType_Label.Height + 10));
            this.Controls.Add(contentType);

            Label intakeLabel = new Label();
            intakeLabel.Text = "Please select either the URL hosting the content, or the content's source file";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            intakeLabel.ForeColor = Color.White;
            intakeLabel.AutoSize = true;
            intakeLabel.Anchor = AnchorStyles.Top;
            intakeLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(intakeLabel);
            intakeLabel.Location = new Point((this.Width / 2) - (intakeLabel.Width / 2), (contentType.Location.Y + contentType.Height + 20));

            TextBox urlIntake = new TextBox();
            urlIntake.Multiline = true;
            urlIntake.MinimumSize = new Size(this.Width / 3, 24);
            urlIntake.Multiline = false;
            urlIntake.Location = new Point(intakeLabel.Location.X + 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));
            urlIntake.Text = "https://example.com";
            urlIntake.BorderStyle = BorderStyle.FixedSingle;
            urlIntake.TextAlign = HorizontalAlignment.Center;
            urlIntake.TabStop = false;
            urlIntake.Enabled = true;
            this.Controls.Add(urlIntake);

            Button fileIntake = new Button();
            fileIntake.Text = "No File Chosen";
            fileIntake.AutoSize = true;
            fileIntake.BackColor = Color.LightGray;
            fileIntake.FlatStyle = FlatStyle.Flat;
            fileIntake.FlatAppearance.BorderSize = 0;
            fileIntake.TextAlign = ContentAlignment.MiddleCenter;
            fileIntake.TabStop = false;
            fileIntake.Enabled = true;
            this.Controls.Add(fileIntake);
            fileIntake.Location = new Point(intakeLabel.Location.X + intakeLabel.Width - fileIntake.Width - 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));

            Label dividerLabel = new Label();
            dividerLabel.Text = "or";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            dividerLabel.ForeColor = Color.White;
            dividerLabel.AutoSize = true;
            dividerLabel.Anchor = AnchorStyles.Top;
            dividerLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(dividerLabel);
            dividerLabel.Location = new Point(fileIntake.Location.X - ((fileIntake.Location.X - (urlIntake.Location.X + urlIntake.Width)) / 2) - dividerLabel.Width / 2, (intakeLabel.Location.Y + intakeLabel.Height + 15));

            Label layoutLabel = new Label();
            layoutLabel.Text = "Please select a display layout";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            layoutLabel.ForeColor = Color.White;
            layoutLabel.AutoSize = true;
            layoutLabel.Anchor = AnchorStyles.Top;
            layoutLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(layoutLabel);
            layoutLabel.Location = new Point((this.Width / 2) - (layoutLabel.Width / 2), (fileIntake.Location.Y + fileIntake.Height + 10));

            ComboBox layoutType = new ComboBox();
            layoutType.DropDownStyle = ComboBoxStyle.DropDownList;
            layoutType.FlatStyle = FlatStyle.Flat;
            layoutType.Items.Add("Fill");
            layoutType.SelectedIndex = 0;
            layoutType.TabStop = false;
            layoutType.Location = new Point((this.Width / 2 - (layoutType.Width / 2)), (layoutLabel.Location.Y + layoutLabel.Height + 10));
            this.Controls.Add(layoutType);

            PictureBox progressSpinner = new PictureBox();
            progressSpinner.Image = Properties.Resources.Spinner;
            progressSpinner.Size = new Size(48, 48);
            progressSpinner.SizeMode = PictureBoxSizeMode.StretchImage;
            progressSpinner.Location = new Point((this.Width / 2 - (progressSpinner.Width / 2)), (layoutType.Location.Y + layoutType.Height + 10));
            this.Controls.Add(progressSpinner);


            this.Controls.Add(exitButton);
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
            /*System.Diagnostics.Debug.Print("hide");
            this.Hide();*/
        }
    }
}
