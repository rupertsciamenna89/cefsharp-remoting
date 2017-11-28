using System.ServiceModel;

namespace MainApplication.Interfaces
{
    /// <summary>
    /// Interface that must be implemented by the web browser
    /// </summary>
    public interface IAppClient {

        /// <summary>
        /// Check if the remote window is closed
        /// </summary>
        /// <returns>True if the remote window is closed</returns>
        [OperationContract(IsOneWay = true)] void FormClosed();
    }
}
