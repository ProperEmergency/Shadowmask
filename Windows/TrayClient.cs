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
            NotifyIcon notifyIcon = new NotifyIcon();

            MenuItem configurationButton = new MenuItem("Configure", new EventHandler(ShowConfig));
            MenuItem exitButton = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon.Icon = Shadowmask.Properties.Resources.Shadowmask_Icon;

            notifyIcon.ContextMenu = new ContextMenu
            (
                new MenuItem[]
                {
                    configurationButton,
                    exitButton
                }
            );

            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            notifyIcon.Visible = true;

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

            void Exit(object sender, EventArgs e)
            {
                Process.GetProcessesByName("Explorer")[0].Kill();
                Process.Start("Explorer.exe");

                Application.Exit();
            }

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
