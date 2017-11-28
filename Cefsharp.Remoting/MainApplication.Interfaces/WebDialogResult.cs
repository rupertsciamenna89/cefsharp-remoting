using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace MainApplication.Interfaces {

    /// <summary>
    /// Class that contains the web dialog result
    /// </summary>
    [Serializable, DataContract]
    public class WebDialogResult {

        /// <summary>
        /// Get or set the window dialog result
        /// </summary>
        [DataMember] public DialogResult Result { get; set; }

        /// <summary>
        /// Get or set the additional dialog result
        /// </summary>
        [DataMember] public string AdditionalData { get; set; }
    }

}
