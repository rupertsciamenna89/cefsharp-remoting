using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MainApplication.WebBrowser;
using RedGate.AppHost.Server;

namespace MainApplication {

    /// <summary>
    /// Main application program
    /// </summary>
    internal static class Program {
        private static string _sourcePath;

        /// <summary>
        /// Main application program.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread] private static void Main(string[] args) {

            //Get the path of the program client
            if (args.Length > 0) {
                foreach (string arg in args) {
                    string tmpArg = arg;

                    if (tmpArg[0] == '"' && tmpArg[tmpArg.Length - 1] == '"')
                        tmpArg = tmpArg.Substring(1, tmpArg.Length - 2);

                    if (tmpArg.StartsWith("/path:", StringComparison.OrdinalIgnoreCase)) {
                        _sourcePath = tmpArg.Substring(6);
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Create a basic child process handle
        /// </summary>
        /// <returns>Returns the child process handle</returns>
        public static IChildProcessHandle CreateChildProcessHandle() {
            string assemblyPath = _sourcePath ?? Path.GetDirectoryName(Assembly.GetAssembly(typeof(WebBrowserInitializer)).Location);
            Debug.Assert(assemblyPath != null, "assemblyPath != null");
            var al = new ChildProcessFactory() { ClientExecutablePath = _sourcePath };
            return al.Create(Path.Combine(assemblyPath, "MainApplication.WebBrowser.dll"), false, Environment.Is64BitProcess);
        }
    }
}
