using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication.Interfaces {

    /// <summary>
    /// Interface that must be implemented by the web browser
    /// </summary>
    public interface IWebBrowserClient {

        /// <summary>
        /// Show the window as non-dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        [OperationContract(IsOneWay = true)]
        void Show(long handle);

        /// <summary>
        /// Show the window as dialog
        /// </summary>
        /// <param name="handle">Handle of the owner</param>
        [OperationContract(IsOneWay = true)]
        void ShowDialog(long handle);

        /// <summary>
        /// Get the result of the web dialog
        /// </summary>
        /// <returns>Returns the result of the web dialog</returns>
        [OperationContract(IsOneWay = false)]
        WebDialogResult GetResult();

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        [OperationContract(IsOneWay = false)]
        bool IsClosed();

        /// <summary>
        /// Close the remote window
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Close();

        /// <summary>
        /// Destroy the server and release all resources
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Destroy();
    }

}
