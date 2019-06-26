using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class ConfigurationPane : Form
    {
        private WallpaperEngine wallpaperEngine;

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
            this.MinimumSize = new Size(640, 440);
            this.MaximumSize = new Size(840, 640);
            this.AutoSize = true;

            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            #region Initialize Controls

            // About Button - Links to further reading about Shadowmask.
            Button aboutButton = new Button();
            aboutButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            aboutButton.AutoSize = false;
            aboutButton.BackColor = this.BackColor;
            aboutButton.BackgroundImage = Properties.Resources.QuestionMark_Dark;
            aboutButton.BackgroundImageLayout = ImageLayout.Center;
            aboutButton.Cursor = Cursors.Hand;
            aboutButton.FlatAppearance.CheckedBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.MouseOverBackColor = aboutButton.BackColor;
            aboutButton.FlatAppearance.BorderSize = 0;
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.Size = aboutButton.BackgroundImage.Size;
            aboutButton.TabStop = false;

            // Activity Indicator - Displays if the configuration application is performing sensative operations (writing to files, lengthy computions).
            PictureBox activityIndicator = new PictureBox();
            activityIndicator.Image = Properties.Resources.Spinner;
            activityIndicator.Name = "progressSpinner";
            activityIndicator.Size = new Size(48, 48);
            activityIndicator.SizeMode = PictureBoxSizeMode.StretchImage;
            activityIndicator.Hide();

            TextBox addressIntake = new TextBox();
            addressIntake.Multiline = true;
            addressIntake.MinimumSize = new Size(this.Width / 3, 24);
            addressIntake.Multiline = false;
            addressIntake.Text = "https://example.com";
            addressIntake.BorderStyle = BorderStyle.FixedSingle;
            addressIntake.TextAlign = HorizontalAlignment.Center;
            addressIntake.TabStop = false;
            addressIntake.Enabled = true;
            addressIntake.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            addressIntake.AutoCompleteSource = AutoCompleteSource.CustomSource;
            addressIntake.AutoCompleteCustomSource = Pull_Chrome__Address_History();

            // Content Type Dropdown - Allows the user to select the type of wallpaper source material.
            ComboBox contentType = new ComboBox();
            contentType.DataSource = new BindingSource(new Dictionary<string,string>
            {
                { "Image", "Image Files (*.PNG;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF" },
                //{ "Video", "Video Files (*.MP4)|*.MP4" },
                { "Webpage", "" },
                //{ "YouTube Video", "" }
            }, null);
            contentType.DisplayMember = "Key";
            contentType.DropDownStyle = ComboBoxStyle.DropDownList;
            contentType.FlatStyle = FlatStyle.Flat;
            contentType.ValueMember = "Value";

            // Content Type Dropdown - Instruction Label
            Label contentType_Label = new Label();
            contentType_Label.Anchor = AnchorStyles.Top;
            contentType_Label.AutoSize = true;
            contentType_Label.ForeColor = Color.White;
            contentType.TabStop = false;
            contentType_Label.Text = "Please select the type of content you wish to display.";
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

            // File Selection Dialog - A explorer dialog that allows the user to input images,videos,etc as wallpaper content.
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Please select the desired wallpaper source...";

            // File Selection Button - Launches the File Selection Dialog.
            Button fileIntake = new Button();
            fileIntake.AutoSize = false;
            fileIntake.BackColor = Color.LightGray;
            fileIntake.FlatAppearance.BorderSize = 0;
            fileIntake.FlatStyle = FlatStyle.Flat;
            fileIntake.TabStop = false;
            fileIntake.Text = "No File";
            fileIntake.TextAlign = ContentAlignment.MiddleCenter;

            // Content Selection Instructions - Assist user with content selection.
            Label intakeLabel = new Label();
            intakeLabel.Anchor = AnchorStyles.Top;
            intakeLabel.AutoSize = true;
            intakeLabel.ForeColor = Color.White;
            intakeLabel.Text = "Please select either the URL hosting the content, or the content's source file";
            intakeLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Screen Selection Instructions - Assist user with monitor selection.
            Label instructionsLabel = new Label();
            instructionsLabel.Anchor = AnchorStyles.Top;
            instructionsLabel.AutoSize = true;
            instructionsLabel.ForeColor = Color.White;
            instructionsLabel.Text = "Please select a screen to configure:";
            instructionsLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Content Layout Selection - Instructions
            Label layoutLabel = new Label();
            layoutLabel.Anchor = AnchorStyles.Top;
            layoutLabel.AutoSize = true;
            layoutLabel.ForeColor = Color.White;
            layoutLabel.Text = "Please select a display layout";
            layoutLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Content Layout Dropdown - Allows the user to select how their content will be displayed (fill, tile, etc).
            ComboBox layoutType = new ComboBox();
            layoutType.DropDownStyle = ComboBoxStyle.DropDownList;
            layoutType.FlatStyle = FlatStyle.Flat;
            layoutType.Items.Add("Fill");
            layoutType.SelectedIndex = 0;
            layoutType.TabStop = false;

            // Monitor Selection - Allows the user to select a monitor to adjust wallpaper settings on. Dynamically built based on currently attached monitors.
            FlowLayoutPanel monitorSelection_Panel = new FlowLayoutPanel();
            monitorSelection_Panel.Anchor = AnchorStyles.None;
            monitorSelection_Panel.AutoSize = true;
            monitorSelection_Panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            monitorSelection_Panel.BackColor = this.BackColor;

            int screenCount = 1;
            foreach (Screen activeDisplay in Screen.AllScreens)
            {
                RadioButton monitorButton = new RadioButton();
                monitorButton.Anchor = AnchorStyles.None;
                monitorButton.Appearance = Appearance.Button;
                monitorButton.BackColor = Color.LightGray;
                monitorButton.Cursor = Cursors.Hand;
                monitorButton.Dock = DockStyle.None;
                monitorButton.FlatAppearance.BorderSize = 0;
                monitorButton.FlatAppearance.MouseOverBackColor = System.Drawing.ColorTranslator.FromHtml("#2d89ef");
                monitorButton.FlatStyle = FlatStyle.Flat;
                monitorButton.Size = new Size(activeDisplay.WorkingArea.Width / 10, activeDisplay.WorkingArea.Height / 10);
                monitorButton.TabStop = true;
                monitorButton.Text = screenCount.ToString();
                monitorButton.TextAlign = ContentAlignment.MiddleCenter;

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

                monitorSelection_Panel.Controls.Add(monitorButton);

                screenCount++;
            }

            // Advanced Settings Button - Brings the user to an advanced configuration window.
            Button settingsButton = new Button();
            settingsButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            settingsButton.AutoSize = false;
            settingsButton.BackColor = this.BackColor;
            settingsButton.BackgroundImage = Properties.Resources.Gear_Dark;
            settingsButton.BackgroundImageLayout = ImageLayout.Center;
            settingsButton.Cursor = Cursors.Hand;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatAppearance.CheckedBackColor = settingsButton.BackColor;
            settingsButton.FlatAppearance.MouseOverBackColor = settingsButton.BackColor;
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.Size = settingsButton.BackgroundImage.Size;
            settingsButton.TabStop = false;

            // Version Label  - Display's the application's current version.
            Label versionLabel = new Label();
            versionLabel.Anchor = AnchorStyles.Bottom;
            versionLabel.AutoSize = false;
            versionLabel.Dock = DockStyle.Bottom;
            versionLabel.ForeColor = Color.White;
            versionLabel.Text = ProductName + " - " + ProductVersion;
            versionLabel.TextAlign = ContentAlignment.MiddleCenter;

            // ToolTip - Enables control tooltip functionality.
            ToolTip toolTip = new ToolTip();
            toolTip.BackColor = Color.White;
            toolTip.SetToolTip(exitButton, "Save & Exit");
            toolTip.SetToolTip(settingsButton, "Advanced Settings");
            #endregion

            #region Add Controls to Form

            this.Controls.Add(aboutButton);
            this.Controls.Add(activityIndicator);
            this.Controls.Add(addressIntake);
            this.Controls.Add(contentType);
            this.Controls.Add(contentType_Label);
            this.Controls.Add(exitButton);
            this.Controls.Add(intakeLabel);
            this.Controls.Add(instructionsLabel);
            this.Controls.Add(fileIntake);
            this.Controls.Add(layoutLabel);
            this.Controls.Add(layoutType);
            this.Controls.Add(monitorSelection_Panel);
            this.Controls.Add(settingsButton);
            this.Controls.Add(versionLabel);

            #endregion

            #region Align Controls on Form

            // Independent Controls
            aboutButton.Location = new Point(this.Width - aboutButton.Width - 10, this.Height - aboutButton.Height - 10);
            activityIndicator.Location = new Point((this.Width / 2 - (activityIndicator.Width / 2)), this.Height - activityIndicator.Height - 50);
            exitButton.Location = new Point((this.Width - exitButton.Width - 10), 10);
            settingsButton.Location = new Point(10, this.Height - settingsButton.Height - 10);
            versionLabel.Location = new Point((this.Width / 2) - (versionLabel.Width / 2), this.Height - versionLabel.Height);
            
            // Location Dependent Controls
            instructionsLabel.Location = new Point((this.Width / 2) - (instructionsLabel.Width / 2), instructionsLabel.Height + exitButton.Height);
            monitorSelection_Panel.Location = new Point((this.Width / 2 - (monitorSelection_Panel.Width / 2)), (instructionsLabel.Location.Y + instructionsLabel.Height + 10));
            contentType_Label.Location = new Point((this.Width / 2) - (contentType_Label.Width / 2), (monitorSelection_Panel.Location.Y + monitorSelection_Panel.Height + 20));
            contentType.Location = new Point((this.Width / 2 - (contentType.Width / 2)), (contentType_Label.Location.Y + contentType_Label.Height + 10));
            intakeLabel.Location = new Point((this.Width / 2) - (intakeLabel.Width / 2), (contentType.Location.Y + contentType.Height + 20));
            addressIntake.Location = new Point(intakeLabel.Location.X + 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));
            fileIntake.Location = new Point(intakeLabel.Location.X + intakeLabel.Width - fileIntake.Width - 5, (intakeLabel.Location.Y + intakeLabel.Height + 15));
            layoutLabel.Location = new Point((this.Width / 2) - (layoutLabel.Width / 2), (fileIntake.Location.Y + fileIntake.Height + 10));
            layoutType.Location = new Point((this.Width / 2 - (layoutType.Width / 2)), (layoutLabel.Location.Y + layoutLabel.Height + 10));
            #endregion

            #region Handle Control Actions
            /* Opens the project github on the default system web browser.
             * Triggered by a click to the "About" Button. */
            void AboutButton_Click(object sender, EventArgs e)
            {
                System.Diagnostics.Process.Start("https://github.com/ProperEmergency/Shadowmask");
            }
            aboutButton.Click += AboutButton_Click;

            /* Detects if a url was entered or modified, and triggers a Settings_Changed event.
             * Triggered by loss of focus to the address bar.*/
            void AddressIntake_LostFocus(object sender, EventArgs e)
            {
                if (addressIntake.Modified)
                {
                    addressIntake.Modified = false;
                    Settings_Changed(sender, e);
                }
            }
            addressIntake.LostFocus += AddressIntake_LostFocus;


            /* Hides the configuration pane, and refreshes the wallpaper engine.
             * Triggered by a click to the exit button */
            void ExitButton_Click(object sender, EventArgs e)
            {
                
                this.Hide();
                Task.Run(() => wallpaperEngine.Change_Displayed_Content());
            }
            exitButton.Click += ExitButton_Click;

            /* Opens a file selection dialog.
             * Once a file is selected, a wallpaper template is built and the Settings_Changed event triggered.
             * Triggered by a click to the file selection button.*/
            void FileIntake_Click(object sender, EventArgs e)
            {
                fileDialog.Filter = contentType.SelectedValue.ToString();

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    TemplateEngine templateEngine = new TemplateEngine();

                    fileIntake.Text = templateEngine.Build_Custom_Template(fileDialog.FileName, fileDialog.SafeFileName, contentType.Text, layoutType.Text);

                    Settings_Changed(sender, e);
                }
            }
            fileIntake.Click += FileIntake_Click;

            /* Pulls autocomplete history from typed Chrome URLs.
             * Used in addressIntake ComboBox.*/
            AutoCompleteStringCollection Pull_Chrome__Address_History()
            {
                AutoCompleteStringCollection urlCollection = new AutoCompleteStringCollection();

                try
                {
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History", Path.GetTempPath() + "Chrome_URL_Entry_History.db", true);

                    string queryString = "SELECT url FROM urls WHERE typed_count > 0;";
                    string connectionString = "Data Source=" + Path.GetTempPath() + "Chrome_URL_Entry_History.db" + "; Version=3";

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand command = new SQLiteCommand(queryString, connection);

                        connection.Open();
                        SQLiteDataReader reader = command.ExecuteReader();

                        try
                        {
                            while (reader.Read())
                            {
                                urlCollection.Add(reader.GetString(0));
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }

                        connection.Close();
                    }
                }
                catch (Exception)
                {
                    //
                }
                return urlCollection;
            }

            /* Disables GUI to process settings changes, and writes/reads new settings values from the user config file.
             * Triggered by all user-modifiable controls.*/
            void Settings_Changed(object sender, EventArgs e, bool writeRequired = true, bool readRequired = false)
            {
                activityIndicator.Show();

                foreach (Control formControl in this.Controls.Cast<Control>().Where(c => !(c is Label) && !(c is PictureBox)))
                {
                    formControl.Enabled = false;
                }

                if (readRequired)
                {
                    Task.Run(() => Read_Settings());
                }

                if (writeRequired)
                {
                    Task.Run(() => Write_Settings());
                }
            }
            contentType.SelectedIndexChanged += (sender, e) => Settings_Changed(sender, e);
            layoutType.SelectedIndexChanged += (sender, e) => Settings_Changed(sender, e);

            /* Reads new wallpaper configuration from user config, and updates GUI to match.
             * Triggered by Settings_Change event.*/
            void Read_Settings()
            {
                var selectedMonitor = monitorSelection_Panel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
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
                    addressIntake.Invoke((MethodInvoker)delegate { addressIntake.Text = currentTheme[2]; });
                    fileIntake.Invoke((MethodInvoker)delegate { fileIntake.Text = "No File"; fileIntake.Enabled = false; });
                }
                else
                {
                    fileIntake.Invoke((MethodInvoker)delegate { fileIntake.Text = currentTheme[2]; });
                    addressIntake.Invoke((MethodInvoker)delegate { addressIntake.Clear(); addressIntake.Enabled = false; });
                }

                Restore_GUI();

            }

            /* Writes new wallpaper configuration to user config based off of GUI status.
             * Triggered by Settings_Change event.*/
            void Write_Settings()
            {
                var selectedMonitor = monitorSelection_Panel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                int monitorIndex = Int32.Parse(selectedMonitor.Text) - 1;

                string displayTheme = "";

                string type = "";
                string content = "";
                string layout = "";

                contentType.Invoke((MethodInvoker)delegate { type = contentType.Text; });
                layoutType.Invoke((MethodInvoker)delegate { layout = layoutType.Text; });

                if (type == "Webpage") { content = addressIntake.Text; }
                else { content = fileIntake.Text; }

                displayTheme = selectedMonitor.Text + ";" + type + ";" + content + ";" + layout;

                try
                {
                    if (Properties.Settings.Default.ThemeLayout.Count > monitorIndex)
                    {
                        Properties.Settings.Default.ThemeLayout.RemoveAt(monitorIndex);
                    }
                    Properties.Settings.Default.ThemeLayout.Insert(monitorIndex, displayTheme);
                }
                catch (NullReferenceException)
                {
                    Properties.Settings.Default.ThemeLayout = new System.Collections.Specialized.StringCollection();
                    Properties.Settings.Default.ThemeLayout.Add(displayTheme);
                }

                Properties.Settings.Default.Save();

                Restore_GUI();
            }

            /* Restores controls disabled by Settings_Changed event.
             * Triggered by Read_Settings or Write_Settings.*/
            void Restore_GUI()
            {
                if (contentType.InvokeRequired)
                {
                    string currentType = "";
                    contentType.Invoke((MethodInvoker)delegate { currentType = contentType.Text; });

                    if (currentType == "Webpage")
                    {
                        addressIntake.Invoke((MethodInvoker)delegate { addressIntake.Enabled = true; });
                    }
                    else
                    {
                        fileIntake.Invoke((MethodInvoker)delegate { fileIntake.Enabled = true; });
                    }

                    foreach (Control formControl in this.Controls.Cast<Control>().Where(c => (c != addressIntake) && (c != fileIntake)))
                    {
                        formControl.Invoke((MethodInvoker)delegate { formControl.Enabled = true; });
                    }
                }

                if (activityIndicator.InvokeRequired)
                {
                    activityIndicator.Invoke((MethodInvoker)delegate { activityIndicator.Hide(); });
                }
            }
            #endregion
        }
    }
}
