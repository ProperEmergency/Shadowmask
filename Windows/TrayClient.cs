using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class TrayClient : ApplicationContext
    {
        private ConfigurationPane configPane;
        private WallpaperEngine wallpaperEngine = new WallpaperEngine();

        public TrayClient()
        {
            // Setup a system tray icon for the application.
            NotifyIcon notifyIcon = new NotifyIcon();

            // Add a configuration & quit button to the context menu.
            MenuItem configurationButton = new MenuItem("Configure", new EventHandler(ShowConfig));
            MenuItem exitButton = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon.ContextMenu = new ContextMenu(
            
                new MenuItem[]
                {
                    configurationButton,
                    exitButton
                });


            // Match the application theme.
            notifyIcon.Icon = Shadowmask.Properties.Resources.Shadowmask_Icon;

            // Set visible & handle clicks.
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            /*
             * Runs when "Configure" menu item is clicked OR tray icon is left-clicked.
             * Checks for exisiting instances of the config pane, and either reveals or creates one to show.
             */
            void ShowConfig(object sender, EventArgs e)
            {
                if (configPane != null)
                {
                    configPane.Show();
                    configPane.Activate();
                }
                else
                {
                    configPane = new ConfigurationPane(wallpaperEngine);
                    configPane.ShowDialog();
                }
            }

            /*
             * Runs when "Exit" menu item is clicked.
             * Relaunches File Explorer and kills Shadowmask.
             */
            void Exit(object sender, EventArgs e)
            {
                Process.GetProcessesByName("Explorer")[0].Kill();
                Process.Start("Explorer.exe");

                Application.Exit();
            }

            /*
             * Handles clicks to the tray icon.
             * Either launch the context menu by default, or launch the config pane directly.
             */
            void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
            {
                // Handle Left Clicks
                if (e.Button == MouseButtons.Left)
                {
                    ShowConfig(sender, e);
                }

                // Else (for right clicks) exhibit default behavior of opening context menu.
            }
        }
    }
}
