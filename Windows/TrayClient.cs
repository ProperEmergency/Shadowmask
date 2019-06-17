using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            notifyIcon.Icon = Icon.FromHandle(Shadowmask.Properties.Resources.StudioLogo.GetHicon());

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
                    configPane = new ConfigurationPane();
                    configPane.ShowDialog();
                }
            }

            void Exit(object sender, EventArgs e)
            {

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
