namespace MainApplication.Interfaces {

    /// <summary>
    /// Interface that must be implemented by a form service
    /// </summary>
    public interface IFormService {

        /// <summary>
        /// Get the app server Id
        /// </summary>
        string AppServerId { get; }

        /// <summary>
        /// Get the client server Id
        /// </summary>
        string ControlServerId { get; }

    }

}
