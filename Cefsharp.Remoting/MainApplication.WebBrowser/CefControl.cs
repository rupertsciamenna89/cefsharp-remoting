using System.Windows.Forms;
using CefSharp.WinForms;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// WebBrowser control
    /// </summary>
    public partial class CefControl: UserControl {

        public CefControl() {
            CefInitializer.Initialize();

            InitializeComponent();
            var cr = new ChromiumWebBrowser("https://www.google.com");
            cr.Dock = DockStyle.Fill;
            Controls.Add(cr);
        }
    }
}
