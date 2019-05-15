using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class WallpaperInstance : Form
    {
        private readonly ChromiumWebBrowser browser;

        public WallpaperInstance()
        {
            InitializeComponent();

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            browser = new ChromiumWebBrowser("")
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(browser);

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);

            IntPtr progman = Window_API_Wrapper.FindWindow("progman", null);

            IntPtr zero = IntPtr.Zero;
            Window_API_Wrapper.SendMessageTimeout(progman, 0x052C, zero, zero, 0x0, 1000, out zero);

            IntPtr workerw = IntPtr.Zero;

            Window_API_Wrapper.EnumWindows(new Window_API_Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = Window_API_Wrapper.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                if (p != IntPtr.Zero) { workerw = Window_API_Wrapper.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", IntPtr.Zero); }

                return true;
            }), IntPtr.Zero);

            Window_API_Wrapper.SetParent(this.Handle, workerw);
        }
    }
}
