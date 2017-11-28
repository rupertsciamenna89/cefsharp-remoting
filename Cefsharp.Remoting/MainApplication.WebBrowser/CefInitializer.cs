using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CefSharp;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// Class that contains the base methods for CEF initializations
    /// </summary>
    public static class CefInitializer {

        /// <summary>
        /// Initialize properties
        /// </summary>
        static CefInitializer() {
            CachePath = Path.Combine(Path.GetTempPath(), "SOIssue", "Cache");
            LogFile = Path.Combine(Path.GetTempPath(), "SOIssue", "Logs");
            UserDataPath = Path.Combine(Path.GetTempPath(), "SOIssue", "Data");

            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);
            if (!Directory.Exists(LogFile))
                Directory.CreateDirectory(LogFile);
            if (!Directory.Exists(UserDataPath))
                Directory.CreateDirectory(UserDataPath);

            //Complete the files combine
            LogFile = Path.Combine(LogFile, "WebBrowser.log");

            AppDomain.CurrentDomain.DomainUnload += (sender, args) => Shutdown();
        }

        /// <summary>
        /// Shutdown all CEF instances
        /// </summary>
        internal static void Shutdown() {
            using (var syncObj = new WindowsFormsSynchronizationContext()) {
                syncObj.Send(o => {
                    if (Cef.IsInitialized)
                        Cef.Shutdown();
                }, new object());
            }
        }

        /// <summary>
        /// Initialize CEF libraries
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Initialize() {
            if (Cef.IsInitialized)
                return;

            //Get proxy properties
            WebProxy proxy = WebRequest.DefaultWebProxy as WebProxy;
            string cefPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Cef)).Location);
            Debug.Print($"CEF Library Path: {cefPath}");
            Debug.Assert(cefPath != null, nameof(cefPath) + " != null");

            var settings = new CefSettings() {
                BrowserSubprocessPath = Path.Combine(cefPath, "CefSharp.BrowserSubprocess.exe"),
                LocalesDirPath = Path.Combine(cefPath, "locales"),
                ResourcesDirPath = cefPath,
                Locale = CultureInfo.CurrentCulture.Name,
                CachePath = CachePath,
                LogFile = LogFile,
                UserDataPath = UserDataPath
            };

            if (proxy == null || proxy.Address.AbsoluteUri != string.Empty)
                settings.CefCommandLineArgs.Add("no-proxy-server", string.Empty);

            Cef.Initialize(settings);
        }

        internal static readonly string CachePath;
        internal static readonly string LogFile;
        internal static readonly string UserDataPath;
    }
}
