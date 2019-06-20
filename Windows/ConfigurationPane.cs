using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class ConfigurationPane : Form
    {
        WallpaperEngine wallpaperEngine;
        public ConfigurationPane(WallpaperEngine wallpaperEngine)
        {
            this.wallpaperEngine = wallpaperEngine;

            this.Name = "Configuration Panel";
            this.Text = "Shadowmask - Settings";
            this.Icon = Icon.FromHandle(Properties.Resources.StudioLogo.GetHicon());
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.98;
            this.BackColor = ColorTranslator.FromHtml("#1d1d1d");

            this.ShowInTaskbar = false;

            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(640, 640);

            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            /*
             * Initialize Controls
             * 
             */

            // About Button - Links to further reading about Shadowmask.
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
            aboutButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            aboutButton.Click += AboutButton_HandleClick;
            aboutButton.TabStop = false;

            // Activity Indicator - Displays if the configuration application is performing sensative operations (writing to files, lengthy computions).
            PictureBox activityIndicator = new PictureBox();
            activityIndicator.Image = Properties.Resources.Spinner;
            activityIndicator.Name = "progressSpinner";
            activityIndicator.Size = new Size(48, 48);
            activityIndicator.SizeMode = PictureBoxSizeMode.StretchImage;

            // Content Type Dropdown - Allows the user to select the type of wallpaper source material.
            ComboBox contentType = new ComboBox();
            contentType.DropDownStyle = ComboBoxStyle.DropDownList;
            contentType.FlatStyle = FlatStyle.Flat;
            contentType.Items.AddRange(new string[] { "None" , "Image" , "Video File" , "Webpage" , "YouTube Video" });
            contentType.SelectedIndex = 0;

            // Content Type Dropdown - Instruction Label
            Label contentType_Label = new Label();
            contentType_Label.Text = "Please select the type of content you wish to display.";
            contentType_Label.ForeColor = Color.White;
            contentType_Label.AutoSize = true;
            contentType_Label.Anchor = AnchorStyles.Top;
            contentType_Label.TextAlign = ContentAlignment.MiddleCenter;

            // Exit Button - Closes the configuration pane (not the entire application).
            Button exitButton = new Button();
            exitButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            exitButton.AutoSize = false;
            exitButton.BackColor = this.BackColor;
            exitButton.BackgroundImage = Properties.Resources.Exit;
            exitButton.BackgroundImageLayout = ImageLayout.Center;
            exitButton.Cursor = Cursors.Hand;
            exitButton.FlatAppearance.CheckedBackColor = exitButton.BackColor;
            exitButton.FlatAppearance.MouseOverBackColor = exitButton.BackColor;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Size = exitButton.BackgroundImage.Size;
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
            settingsButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            settingsButton.TabStop = false;

            Label instructionsLabel = new Label();
            instructionsLabel.Text = "Please select a screen to configure:";
            instructionsLabel.ForeColor = Color.White;
            instructionsLabel.AutoSize = true;
            instructionsLabel.Anchor = AnchorStyles.Top;
            instructionsLabel.TextAlign = ContentAlignment.MiddleCenter;


            Label versionLabel = new Label();
            versionLabel.Text = ProductName + " - " + ProductVersion;
            versionLabel.ForeColor = Color.White;
            versionLabel.AutoSize = false;
            versionLabel.Anchor = AnchorStyles.Bottom;
            versionLabel.Dock = DockStyle.Bottom;
            versionLabel.TextAlign = ContentAlignment.MiddleCenter;

            FlowLayoutPanel monitor_selectionPanel = new FlowLayoutPanel();
            monitor_selectionPanel.BackColor = this.BackColor;
            monitor_selectionPanel.Anchor = AnchorStyles.None;
            monitor_selectionPanel.AutoSize = true;
            monitor_selectionPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;


            Label intakeLabel = new Label();
            intakeLabel.Text = "Please select either the URL hosting the content, or the content's source file";
            intakeLabel.ForeColor = Color.White;
            intakeLabel.AutoSize = true;
            intakeLabel.Anchor = AnchorStyles.Top;
            intakeLabel.TextAlign = ContentAlignment.MiddleCenter;

            TextBox urlIntake = new TextBox();
            urlIntake.Multiline = true;
            urlIntake.MinimumSize = new Size(this.Width / 3, 24);
            urlIntake.Multiline = false;
            urlIntake.Text = "https://example.com";
            urlIntake.BorderStyle = BorderStyle.FixedSingle;
            urlIntake.TextAlign = HorizontalAlignment.Center;
            urlIntake.TabStop = false;
            urlIntake.Enabled = true;

            Button fileIntake = new Button();
            fileIntake.Text = "No File Chosen";
            fileIntake.AutoSize = true;
            fileIntake.BackColor = Color.LightGray;
            fileIntake.FlatStyle = FlatStyle.Flat;
            fileIntake.FlatAppearance.BorderSize = 0;
            fileIntake.TextAlign = ContentAlignment.MiddleCenter;
            fileIntake.TabStop = false;
            fileIntake.Enabled = true;


            Label layoutLabel = new Label();
            layoutLabel.Text = "Please select a display layout";
            //instructionsLabel.Font = new Font("Segoe UI", 12);
            layoutLabel.ForeColor = Color.White;
            layoutLabel.AutoSize = true;
            layoutLabel.Anchor = AnchorStyles.Top;
            layoutLabel.TextAlign = ContentAlignment.MiddleCenter;


            ComboBox layoutType = new ComboBox();
            layoutType.DropDownStyle = ComboBoxStyle.DropDownList;
            layoutType.FlatStyle = FlatStyle.Flat;
            layoutType.Items.Add("Fill");
            layoutType.SelectedIndex = 0;
            layoutType.TabStop = false;

            layoutType.SelectedIndexChanged += (sender, e) => Settings_Changed(sender, e);
            this.Controls.Add(layoutType);

            int screenCount = 1;

            foreach (Screen activeDisplay in Screen.AllScreens)
            {
                RadioButton monitorButton = new RadioButton();
                monitorButton.Text = screenCount.ToString();
                monitorButton.Anchor = AnchorStyles.None;
                monitorButton.Dock = DockStyle.None;
                monitorButton.Cursor = Cursors.Hand;
                monitorButton.FlatStyle = FlatStyle.Flat;
                monitorButton.FlatAppearance.MouseOverBackColor = System.Drawing.ColorTranslator.FromHtml("#2d89ef");
                monitorButton.BackColor = Color.LightGray;
                monitorButton.FlatAppearance.BorderSize = 0;
                monitorButton.Appearance = Appearance.Button;
                monitorButton.TextAlign = ContentAlignment.MiddleCenter;

                monitorButton.TabStop = false;


                monitorButton.Size = new Size(activeDisplay.WorkingArea.Width / 10, activeDisplay.WorkingArea.Height / 10);

                monitorButton.CheckedChanged += MonitorButton_Checked;

                void MonitorButton_Checked(object sender, EventArgs e)
                {
                    if (monitorButton.Checked)
                    {
                        monitorButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#2d89ef");
                        monitorButton.FlatAppearance.BorderSize = 0;
                    }
                    else
                    {
                        monitorButton.BackColor = Color.LightGray;
                        monitorButton.FlatAppearance.BorderSize = 0;
                    }

                    Settings_Changed(sender, e, false, true);
                }

                monitor_selectionPanel.Controls.Add(monitorButton);

                screenCount++;
            }

            // ToolTip
            ToolTip toolTip = new ToolTip();
            toolTip.BackColor = Color.White;

            this.Controls.Add(activityIndicator);
            this.Controls.Add(exitButton);
            this.Controls.Add(settingsButton);
            this.Controls.Add(aboutButton);
            this.Controls.Add(versionLabel);
            this.Controls.Add(monitor_selectionPanel);
            this.Controls.Add(contentType_Label);
            this.Controls.Add(contentType);
            this.Controls.Add(intakeLabel);
            this.Controls.Add(urlIntake);
            this.Controls.Add(fileIntake);
            this.Controls.Add(layoutLabel);
            this.Controls.Add(instructionsLabel);

            exitButton.Location = new Point((this.Width - exitButton.Width - 10), 10);
            monitor_selectionPanel.Location = new Point((this.Width / 2 - (monitor_selectionPanel.Width / 2)), (instructionsLabel.Location.Y + instructionsLabel.Height + 10));
            instructionsLabel.Location = new Point((this.Width / 2) - (instructionsLabel.Width / 2), instructionsLabel.Height + exitButton.Height);
            contentType_Label.Location = new Point((this.Width / 2) - (contentType_Label.Width / 2), (monitor_selectionPanel.Location.Y + monitor_selectionPanel.Height + 20));
            contentType.Location = new Point((this.Width / 2 - (contentType.Width / 2)), (contentType_Label.Location.Y + contentType_Label.Height + 10));
            intakeLabel.Location = new Point((this.Width / 2) - (intakeLabel.Width / 2), (contentType.Location.Y + contentType.Height + 20));
            urlIntake.Location = new Point(intakeLabel.Location.X + 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));
            fileIntake.Location = new Point(intakeLabel.Location.X + intakeLabel.Width - fileIntake.Width - 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));
            layoutLabel.Location = new Point((this.Width / 2) - (layoutLabel.Width / 2), (fileIntake.Location.Y + fileIntake.Height + 10));
            layoutType.Location = new Point((this.Width / 2 - (layoutType.Width / 2)), (layoutLabel.Location.Y + layoutLabel.Height + 10));
            settingsButton.Location = new Point(10, this.Height - settingsButton.Height - 10);
            activityIndicator.Location = new Point((this.Width / 2 - (activityIndicator.Width / 2)), this.Height - activityIndicator.Height - 50);
            versionLabel.Location = new Point((this.Width / 2) - (versionLabel.Width / 2), this.Height - versionLabel.Height);
            aboutButton.Location = new Point(this.Width - aboutButton.Width - 10, this.Height - aboutButton.Height - 10);





            void UrlIntake_LostFocus(object sender, EventArgs e)
            {
                if(urlIntake.Modified)
                {
                    urlIntake.Modified = false;
                    Settings_Changed(sender, e);
                }
            }








            contentType.SelectedIndexChanged += (sender, e) => Settings_Changed(sender, e);
            urlIntake.LostFocus += UrlIntake_LostFocus;

            exitButton.Click += ExitButton_Click;

            toolTip.SetToolTip(exitButton, "Save & Exit");
            toolTip.SetToolTip(settingsButton, "Advanced Settings");



            activityIndicator.Hide();

            void Settings_Changed(object sender, EventArgs e, bool writeRequired = true, bool readRequired = false)
            {
                activityIndicator.Show();

                foreach (Control formControl in this.Controls.Cast<Control>().Where(c => !(c is Label) && !(c is PictureBox)))
                {
                    formControl.Enabled = false;
                }

                if (readRequired)
                {
                    Thread workerThread = new Thread(Read_Settings);
                    workerThread.Start();
                }

                if (writeRequired)
                {
                    Thread workerThread = new Thread(Write_Settings);
                    workerThread.Start();
                }
            }

            void Read_Settings()
            {
                var selectedMonitor = monitor_selectionPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                int monitorIndex = Int32.Parse(selectedMonitor.Text) - 1;

                String[] currentTheme;

                try
                {
                    currentTheme = Properties.Settings.Default.ThemeLayout[monitorIndex].Split(';');
                }

                catch
                {
                    currentTheme = new string[] { selectedMonitor.Text, "Webpage", Properties.Settings.Default.DefaultContent, "Fill" };
                }

                contentType.Invoke((MethodInvoker)delegate { contentType.Text = currentTheme[1]; });
                if (currentTheme[1] == "Webpage")
                {
                    urlIntake.Invoke((MethodInvoker)delegate { urlIntake.Text = currentTheme[2]; });
                    fileIntake.Invoke((MethodInvoker)delegate { fileIntake.ResetText(); fileIntake.Enabled = false; });
                }
                else
                {
                    fileIntake.Invoke((MethodInvoker)delegate { fileIntake.Text = currentTheme[2]; });
                    urlIntake.Invoke((MethodInvoker)delegate { urlIntake.Clear(); urlIntake.Enabled = false; });
                }

                Restore_GUI();

            }

            void Write_Settings()
            {
                var selectedMonitor = monitor_selectionPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                int monitorIndex = Int32.Parse(selectedMonitor.Text) - 1;

                string displayTheme = "";

                string type = "";
                string content = "";
                string layout = "";

                contentType.Invoke((MethodInvoker)delegate { type = contentType.Text; });
                layoutType.Invoke((MethodInvoker)delegate { layout = layoutType.Text; });

                if (type == "Webpage"){content = urlIntake.Text;}
                else{content = fileIntake.Text;}

                displayTheme = selectedMonitor.Text + ";" + type + ";" + content + ";" + layout;

                try
                {
                    if(Properties.Settings.Default.ThemeLayout.Count > monitorIndex)
                    {
                        Properties.Settings.Default.ThemeLayout.RemoveAt(monitorIndex);
                    }
                    Properties.Settings.Default.ThemeLayout.Insert(monitorIndex, displayTheme);
                }
                catch(NullReferenceException nullException)
                {
                    Properties.Settings.Default.ThemeLayout = new System.Collections.Specialized.StringCollection();
                    Properties.Settings.Default.ThemeLayout.Add(displayTheme);
                }

                Properties.Settings.Default.Save();

                Restore_GUI();
            }

            void Restore_GUI()
            {
                foreach (Control formControl in this.Controls)
                {
                    if (formControl.InvokeRequired)
                    {
                        formControl.Invoke((MethodInvoker)delegate { formControl.Enabled = true; });
                    }
                }

                if (activityIndicator.InvokeRequired)
                {
                    activityIndicator.Invoke((MethodInvoker)delegate { activityIndicator.Hide(); });
                }
            }

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Thread workerThread = new Thread(wallpaperEngine.Change_Displayed_Content);
            workerThread.Start();
        }

        private void AboutButton_HandleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ProperEmergency/Shadowmask");
        }

    }
}
