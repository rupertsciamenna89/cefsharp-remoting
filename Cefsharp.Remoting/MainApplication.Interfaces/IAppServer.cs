using System.ServiceModel;

namespace MainApplication.Interfaces {

    /// <summary>
    /// Interface that must be implemented by the server
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAppClient))]
    public interface IAppServer {

        /// <summary>
        /// Check if the web browser exists
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Connect();

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        [OperationContract(IsOneWay = true)]
        void FormClosed();
    }
}
