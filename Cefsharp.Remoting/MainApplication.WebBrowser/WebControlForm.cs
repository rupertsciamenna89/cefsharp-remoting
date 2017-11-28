using System.Windows.Forms;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// Form that contains the webbrowser control
    /// </summary>
    public partial class WebControlForm : Form {

        /// <summary>
        /// Create a new web control form
        /// </summary>
        public WebControlForm() {
            InitializeComponent();
            Controls.Add(new CefControl { Dock = DockStyle.Fill });
        }
    }
}
