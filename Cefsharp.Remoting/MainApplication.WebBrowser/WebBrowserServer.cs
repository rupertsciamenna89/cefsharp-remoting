using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MainApplication.Interfaces;
using System.ServiceModel;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// Class that implements the web browser server
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class WebBrowserServer : IWebBrowserServer, IDisposable {
        private WebControlForm _form;
        private DialogResult _result;

        /// <summary>
        /// Show the window as non-dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        public void Show(long handle) {
            var ownerForm = GetWindowByHandle(handle);
            ShowInternal(ownerForm, false);
        }

        /// <summary>
        /// Show the window as dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        public void ShowDialog(long handle) {
            var ownerForm = GetWindowByHandle(handle);
            ShowInternal(ownerForm, true);
        }

        /// <summary>
        /// Show internally the current window
        /// </summary>
        /// <param name="ownerForm">Owner window</param>
        /// <param name="asDialog">Check if show the window as a dialog</param>
        private void ShowInternal(NativeWindow ownerForm, bool asDialog) {
            _form = new WebControlForm();
            _form.FormClosed += Form_FormClosed;

            //SetParent(_form.Handle, ownerForm.Handle);

            if (asDialog)
                _form.ShowDialog(ownerForm);
            else
                _form.Show(ownerForm);
        }

        /// <summary>
        /// Event generated on form closed
        /// </summary>
        /// <param name="sender">Object that has generated the event</param>
        /// <param name="e">Event arguments</param>
        private void Form_FormClosed(object sender, EventArgs e) {
            _result = _form.DialogResult;
            _form = null;

            //Send notification to the server
            IAppClient appClient = new AppClient(ClientId);
            appClient.FormClosed();
        }

        /// <summary>
        /// Get the result of the web dialog
        /// </summary>
        /// <returns>Returns the result of the web dialog</returns>
        public WebDialogResult GetResult() {
            return new WebDialogResult() {
                Result = _result,
                AdditionalData = $"Generated on {DateTime.Now:u}"
            };
        }

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        public bool IsClosed() {
            return _form == null;
        }

        /// <summary>
        /// Close the remote window
        /// </summary>
        public void Close() {
            _form?.Close();
        }

        /// <summary>
        /// Destroy the server and release all resources
        /// </summary>
        public void Destroy() {
            Dispose();
        }

        /// <summary>
        /// Esegue attività definite dall'applicazione, ad esempio libera, rilascia o reimposta risorse non gestite.
        /// </summary>
        public void Dispose() {
            if (_host != null) {
                _host.Close();
                ((IDisposable)_host).Dispose();
            }

            _form?.Dispose();
        }

        /// <summary>
        /// Get the window id handle
        /// </summary>
        /// <param name="handle">Handle of the owning window</param>
        /// <returns>Returns the owning window</returns>
        private static NativeWindow GetWindowByHandle(long handle) {
            var ptr = new IntPtr(handle);

            var owner = new NativeWindow();
            owner.AssignHandle(ptr);
            return owner;
        }

        #region Native methods

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #endregion

        #region Server properties and functionalities

        private ServiceHost _host;

        /// <summary>
        /// Get the id of the current Server
        /// </summary>
        public string ServerId { get; private set; }

        /// <summary>
        /// Get the id of the remote Client
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// Get the current server URI
        /// </summary>
        public Uri ServerUri { get; private set; }

        /// <summary>
        /// Get the client callback
        /// </summary>
        public IWebBrowserClient Callback { get; private set; }

        /// <summary>
        /// Check if the web browser exists
        /// </summary>
        public void Connect() {
            Callback = OperationContext.Current.GetCallbackChannel<IWebBrowserClient>();
        }

        /// <summary>
        /// Start a new server for web browser interop
        /// </summary>
        /// <param name="serverId">Id of the current web browser server</param>
        /// <param name="clientId">Id of the remote app client</param>
        /// <returns>Returns the instance of the web server</returns>
        public static WebBrowserServer Start(string serverId, string clientId) {
            var w = new WebBrowserServer();
            w.ServerId = serverId;
            w.ServerUri = new Uri("net.pipe://localhost/" + w.ServerId);
            w.ClientId = clientId;

            //Start current web server
            w._host = new ServiceHost(w, w.ServerUri);
            w._host.AddServiceEndpoint(typeof(IWebBrowserServer), new NetNamedPipeBinding(), "Web");
            w._host.Open();

            return w;
        }

        private WebBrowserServer() { }

        #endregion

        #region AppClient implementation

        private sealed class AppClient : IAppClient {
            private readonly IAppServer _server;

            public AppClient(string clientId) {
                Uri clientUri = new Uri("net.pipe://localhost/" + clientId + "/App");
                var context = new InstanceContext(this);
                var pipeFactory = new DuplexChannelFactory<IAppServer>(context, new NetNamedPipeBinding(), new EndpointAddress(clientUri));
                _server = pipeFactory.CreateChannel();
                _server.Connect();
            }

            void IAppClient.FormClosed() {
                _server.FormClosed();
            }
        }

        #endregion
    }
}
