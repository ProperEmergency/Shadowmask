using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class TrayClient : ApplicationContext
    {
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

            notifyIcon.Visible = true;

            void ShowConfig(object sender, EventArgs e)
            {

            }

            void Exit(object sender, EventArgs e)
            {

            }
        }
    }
}
