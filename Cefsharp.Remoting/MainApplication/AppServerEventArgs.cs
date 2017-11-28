using System;
using System.Windows.Forms;

namespace MainApplication {

    /// <summary>
    /// Class that contains the app server event arguments
    /// </summary>
    public sealed class AppServerEventArgs : EventArgs {

        /// <summary>
        /// Creates the new event arguments of the server
        /// </summary>
        /// <param name="result">Result of the event</param>
        /// <param name="additionalData">Additional data</param>
        public AppServerEventArgs(DialogResult result, string additionalData) {
            Result = result;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// Get the result of the dialog
        /// </summary>
        public DialogResult Result { get; }

        /// <summary>
        /// Get the additional data of the dialog
        /// </summary>
        public string AdditionalData { get; }
    }

}
