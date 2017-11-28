using System;
using System.Diagnostics;
using System.Runtime.Remoting.Lifetime;
using MainApplication.Interfaces;

namespace MainApplication {

    /// <summary>
    /// Class that contains a service that allows to show a WebControlForm
    /// </summary>
    public sealed class FormService : MarshalByRefObject, IFormService, ISponsor {

        /// <summary>
        /// Get the app server Id
        /// </summary>
        public string AppServerId { get; }

        /// <summary>
        /// Get the webcontrol server Id
        /// </summary>
        public string ControlServerId { get; }

        /// <summary>
        /// Creates the current form service
        /// </summary>
        /// <param name="appServerId">Application Server Id</param>
        /// <param name="controlServerId">WebControl Server Id</param>
        public FormService(string appServerId, string controlServerId) {
            AppServerId = appServerId;
            ControlServerId = controlServerId;
        }

        /// <summary>
        /// Richiede a un client di patrocinio a rinnovare la lease per l'oggetto specificato.
        /// </summary>
        /// <param name="lease">La lease della durata che richiede il rinnovo della lease. </param>
        /// <returns>Il ciclo di vita di lease aggiuntivo per l'oggetto specificato.</returns>
        public TimeSpan Renewal(ILease lease) {
            return TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Ottiene un oggetto di servizio di durata per controllare i criteri di durata dell'istanza.
        /// </summary>
        /// <returns>
        /// Un oggetto di tipo ILease utilizzato per controllare i criteri di durata dell'istanza.
        /// Si tratta dell'oggetto corrente del servizio di durata di tale istanza se disponibile; 
        /// in caso contrario, un nuovo oggetto del servizio di durata inizializzato sul valore di 
        /// LeaseManagerPollTime proprietà.
        /// </returns>
        public override object InitializeLifetimeService() {
            var l = (ILease)base.InitializeLifetimeService();
            Debug.Assert(l != null, "l != null");
            l.SponsorshipTimeout = TimeSpan.FromMinutes(2);
            l.Register(this);
            return l;
        }
    }

}
