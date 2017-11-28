using System.Windows;
using MainApplication.Interfaces;
using RedGate.AppHost.Interfaces;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// Class that allows to create a WebControlForm
    /// </summary>
    public sealed class OutOfProcessEntryPoint : IOutOfProcessEntryPoint {

        /// <summary>
        /// Create che web control form handler
        /// </summary>
        /// <param name="service">Host services</param>
        /// <returns>Returns the framework element</returns>
        public FrameworkElement CreateElement(IAppHostServices service) {
            return new FormFrameworkElement(service.GetService<IFormService>());
        }

        /// <summary>
        /// Class that allows to show the WebControl Form
        /// </summary>
        public sealed class FormFrameworkElement : FrameworkElement {
            private readonly WebBrowserServer _server;

            /// <summary>
            /// Element that allows to show the service control
            /// </summary>
            /// <param name="service">Service to show</param>
            public FormFrameworkElement(IFormService service) {
                _server = WebBrowserServer.Start(service.ControlServerId, service.AppServerId);
                WebBrowserInitializer.Initialize();
            }
        }
    }

}
