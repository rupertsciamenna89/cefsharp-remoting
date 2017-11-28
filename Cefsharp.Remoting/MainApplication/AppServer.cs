using System;
using MainApplication.Interfaces;
using System.ServiceModel;

namespace MainApplication {
    /// <summary>
    /// Class that implements an App Server
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class AppServer : IAppServer, IDisposable {

        /// <summary>
        /// Event raised when the app has got a result
        /// </summary>
        public event EventHandler<AppServerEventArgs> FormCompleted;

        /// <summary>
        /// Generates the event form completed
        /// </summary>
        /// <param name="e">Event arguments</param>
        private void OnFormCompleted(AppServerEventArgs e) {
            FormCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        public void FormClosed() {
            var result = GetResult();
            OnFormCompleted(new AppServerEventArgs(result.Result, result.AdditionalData));
        }

        /// <summary>
        /// Show the window as non-dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        public void Show(long handle) {
            _webServer.Show(handle);
        }

        /// <summary>
        /// Show the window as dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        public void ShowDialog(long handle) {
            _webServer.ShowDialog(handle);
        }

        /// <summary>
        /// Get the result of the web dialog
        /// </summary>
        /// <returns>Returns the result of the web dialog</returns>
        public WebDialogResult GetResult() {
            return _webServer.GetResult();
        }

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        public bool IsClosed() {
            return _webServer.IsClosed();
        }

        /// <summary>
        /// Close the remote window
        /// </summary>
        public void Close() {
            _webServer.Close();
        }

        /// <summary>
        /// Destroy the server and release all resources
        /// </summary>
        public void Destroy() {
            _webServer.Destroy();
        }

        /// <summary>
        /// Start remote client
        /// </summary>
        public void StartRemoteClient() {
            if (_webServer != null)
                throw new InvalidOperationException("Remote client has been started");

            _webServer = new WebBrowserClient(ClientId);
        }

        /// <summary>
        /// Esegue attività definite dall'applicazione, ad esempio libera, rilascia o reimposta risorse non gestite.
        /// </summary>
        public void Dispose() {
            if (_host != null) {
                _host.Close();
                ((IDisposable)_host).Dispose();
            }
        }

        #region Server properties and functionalities

        private IWebBrowserClient _webServer;
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
        public IAppClient Callback { get; private set; }

        /// <summary>
        /// Check if the web browser exists
        /// </summary>
        public void Connect() {
            Callback = OperationContext.Current.GetCallbackChannel<IAppClient>();
        }

        /// <summary>
        /// Start a new server for web browser interop
        /// </summary>
        /// <param name="serverId">Id of the current web browser server</param>
        /// <param name="clientId">Id of the remote app client</param>
        /// <returns>Returns the instance of the web server</returns>
        public static AppServer Start(string serverId, string clientId) {
            var w = new AppServer();
            w.ServerId = serverId;
            w.ServerUri = new Uri("net.pipe://localhost/" + w.ServerId);
            w.ClientId = clientId;

            //Start current web server
            w._host = new ServiceHost(w, w.ServerUri);
            w._host.AddServiceEndpoint(typeof(IAppServer), new NetNamedPipeBinding(), "App");
            w._host.Open();

            return w;
        }

        private AppServer() { }

        #endregion

        #region Client implementation

        /// <summary>
        /// Class that implements the web browser client
        /// </summary>
        private sealed class WebBrowserClient : IWebBrowserClient {
            private readonly IWebBrowserServer _webServer;

            /// <summary>
            /// Create a new instance of the current web browser client
            /// </summary>
            /// <param name="clientId">Id client of the web browser server</param>
            public WebBrowserClient(string clientId) {
                Uri clientUri = new Uri("net.pipe://localhost/" + clientId + "/Web");
                var context = new InstanceContext(this);
                var pipeFactory = new DuplexChannelFactory<IWebBrowserServer>(context, new NetNamedPipeBinding(), new EndpointAddress(clientUri));
                _webServer = pipeFactory.CreateChannel();
            }

            void IWebBrowserClient.Close() {
                _webServer.Close();
            }

            void IWebBrowserClient.Destroy() {
                _webServer.Destroy();
            }

            WebDialogResult IWebBrowserClient.GetResult() { return _webServer.GetResult(); }

            bool IWebBrowserClient.IsClosed() {
                return _webServer.IsClosed();
            }

            void IWebBrowserClient.Show(long handle) {
                _webServer.Show(handle);
            }

            void IWebBrowserClient.ShowDialog(long handle) {
                _webServer.ShowDialog(handle);
            }
        }

        #endregion
    }
}
