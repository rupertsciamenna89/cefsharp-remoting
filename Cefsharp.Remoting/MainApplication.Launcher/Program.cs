using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MainApplication.Launcher {

    /// <summary>
    /// Launcher program
    /// </summary>
    internal static class Program {

        /// <summary>
        /// Launcher body
        /// </summary>
        [STAThread, LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        private static void Main() {

            //Initialize path of application
            string startupPath = Environment.CurrentDirectory;
            string cachePath = Path.Combine(Path.GetTempPath(), "Program-" + Guid.NewGuid());
            string assemblyPath = CanonicalizePathCombine(startupPath, @"..\..\..\MainApplication\bin\Debug\");
            string executablePath = Path.Combine(assemblyPath, "MainApplication.exe");
            string configFile = executablePath + ".config";

            //Check if exists Assembly
            if (!File.Exists(executablePath)) {
                MessageBox.Show("Unable to find MainApplication.exe.", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            //Start App Domain
            try {
                var setup = new AppDomainSetup() {
                    ApplicationName = "CefSharp-Remoting",
                    ShadowCopyFiles = "true",
                    ShadowCopyDirectories = assemblyPath,
                    CachePath = cachePath,
                    ConfigurationFile = configFile,
                    ApplicationBase = assemblyPath
                };

                var domain = AppDomain.CreateDomain("CefSharp-Remoting", AppDomain.CurrentDomain.Evidence, setup);
                domain.ExecuteAssembly(executablePath, new[] { $"\"/path:{assemblyPath}\"" });
                AppDomain.Unload(domain);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Empty cache path
            try {
                Directory.Delete(cachePath, true);
            }
            catch (Exception) {
                //DO NOTHING
            }
        }

        private static string CanonicalizePathCombine(string sourcePath, string destPath) {
            string resultPath = Path.Combine(sourcePath, destPath);
            var sb = new StringBuilder(Math.Max(260, 2 * resultPath.Length));
            PathCanonicalize(sb, resultPath);
            return sb.ToString();
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PathCanonicalize([Out] StringBuilder sb, string src);
    }
}
