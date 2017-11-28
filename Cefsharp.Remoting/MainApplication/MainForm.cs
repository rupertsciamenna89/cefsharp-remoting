using System;
using System.Windows;
using System.Windows.Forms;
using RedGate.AppHost.Server;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MainApplication {

    /// <summary>
    /// Main application form
    /// </summary>
    public partial class MainForm : Form {
        private readonly IChildProcessHandle _handle;
        private FormServiceLocator _locator;
        private FrameworkElement _element;
        private AppServer _service;

        /// <summary>
        /// Creates the main form
        /// </summary>
        public MainForm() {
            _handle = Program.CreateChildProcessHandle();
            InitializeComponent();
        }

        /// <summary>
        /// Show a new Web Control form
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event arguments</param>
        private void ShowBtn_Click(object sender, EventArgs e) {
            if (_service != null && !_service.IsClosed()) {
                MessageBox.Show(this, "Form already opened", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                _service?.Destroy();

                //Generates client id and server id
                string appId = Guid.NewGuid().ToString("N");
                string controlId = Guid.NewGuid().ToString("N");

                _service = AppServer.Start(appId, controlId);
                _service.FormCompleted += Service_FormCompleted;
                _locator = new FormServiceLocator(appId, controlId);
                _element = _handle.CreateElement(_locator);
                _service.StartRemoteClient();
                _service.Show((long)Handle);
            }
            catch (Exception ex) {
                MessageBox.Show(this, $"Unable to show message: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Show a new Web Control Form as a Dialog
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event arguments</param>
        private void ShowBrowserDlgBtn_Click(object sender, EventArgs e) {
            if (_service != null && !_service.IsClosed()) {
                MessageBox.Show(this, "Form already opened", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                _service?.Destroy();

                //Generates client id and server id
                string appId = Guid.NewGuid().ToString("N");
                string controlId = Guid.NewGuid().ToString("N");

                _service = AppServer.Start(appId, controlId);
                _service.FormCompleted += Service_FormCompleted;
                _locator = new FormServiceLocator(appId, controlId);
                _element = _handle.CreateElement(_locator);
                _service.StartRemoteClient();
                _service.ShowDialog((long)Handle);
            }
            catch (Exception ex) {
                MessageBox.Show(this, $"Unable to show message: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Events generated on form completion
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event arguments</param>
        private void Service_FormCompleted(object sender, AppServerEventArgs e) {

            //Check if invoke is required
            if (InvokeRequired) {
                Invoke(new Action<object, AppServerEventArgs>(Service_FormCompleted), sender, e);
                return;
            }

            _element = null;

            MessageBox.Show(this, $"Result: {e.Result} - Data: {e.AdditionalData}");
        }

        /// <summary>
        /// Raise the FormClosing event
        /// </summary>
        /// <param name="e">Class that contains event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);

            if (_service == null)
                return;

            if (!_service.IsClosed()) {
                var result = MessageBox.Show(this, "Web form window is opened. Do you want to close it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.No) {
                    e.Cancel = true;
                    return;
                }

                _service.Close();
            }

            _service.Destroy();
        }
    }
}
