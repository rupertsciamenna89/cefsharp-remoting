using System;
using System.Collections.Generic;
using RedGate.AppHost.Interfaces;

namespace MainApplication {

    /// <summary>
    /// Class that contains the form service locator functionality
    /// </summary>
    public sealed class FormServiceLocator : MarshalByRefObject, IAppHostServices {
        private readonly List<FormService> _services = new List<FormService>();

        /// <summary>
        /// Get the app server Id
        /// </summary>
        public string AppServerId { get; }

        /// <summary>
        /// Get the webcontrol server Id
        /// </summary>
        public string ControlServerId { get; }

        /// <summary>
        /// Create a new service locator for the web control
        /// </summary>
        /// <param name="appServerId">Application Server Id</param>
        /// <param name="controlServerId">WebControl Server Id</param>
        public FormServiceLocator(string appServerId, string controlServerId) {
            AppServerId = appServerId;
            ControlServerId = controlServerId;
        }

        /// <summary>
        /// Get the service for the current object
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <returns>Reeturns the service</returns>
        public T GetService<T>() where T : class {
            var service = new FormService(AppServerId, ControlServerId);
            _services.Add(service);
            return service as T;
        }
    }
}
