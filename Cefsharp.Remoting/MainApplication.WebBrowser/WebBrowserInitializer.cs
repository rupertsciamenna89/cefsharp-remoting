using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MainApplication.WebBrowser {

    /// <summary>
    /// Class that contains the assembly resolve functions
    /// </summary>
    public static class WebBrowserInitializer {
        private static readonly object _initializer = new object();
        private static bool _initialized;

        /// <summary>
        /// Check if the WebBrowser is initialized
        /// </summary>
        public static bool IsInitialized {
            get {
                lock (_initializer)
                    return _initialized;
            }
        }

        /// <summary>
        /// Initialize the current assembly
        /// </summary>
        public static void Initialize() {
            lock (_initializer) {
                if (!_initialized) {
                    AppDomain.CurrentDomain.AssemblyResolve += CefSharp_AssemblyResolve;
                    _initialized = true;
                }
            }
        }

        /// <summary>
        /// Try to resolve the assembly
        /// </summary>
        /// <param name="sender">Object that has raised the event</param>
        /// <param name="args">Event raised</param>
        /// <returns>Assembly loaded</returns>
        private static Assembly CefSharp_AssemblyResolve(object sender, ResolveEventArgs args) {
            Debug.Print($"Library: {args.Name}");

            if (!args.Name.StartsWith("CefSharp", StringComparison.OrdinalIgnoreCase))
                return null;

            string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";

            foreach (var path in GetAssemblyPaths()) {
                string checkPath = Path.Combine(path, assemblyName);

                if (File.Exists(checkPath)) {
                    Debug.Print($"Relative path FOUND for {args.Name} in {checkPath}");
                    return Assembly.UnsafeLoadFrom(checkPath);
                }

                Debug.Write($"Relative path not found for {args.Name} in {checkPath}");
            }

            return null;
        }

        /// <summary>
        /// Get all possible assembly paths
        /// </summary>
        /// <returns>List of possible assembly paths</returns>
        private static IEnumerable<string> GetAssemblyPaths() {
            string pathPrefix = Environment.Is64BitProcess ? "x64" : "x86";

            if (Directory.Exists(@"C:\Program Files (x86)\CEFRuntime\" + pathPrefix))
                yield return @"C:\Program Files (x86)\CEFRuntime\" + pathPrefix;

            yield return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathPrefix);
            yield return Path.Combine(Environment.CurrentDirectory, pathPrefix);

            Assembly currentAssembly = Assembly.GetAssembly(typeof(CefInitializer));

            if (!string.IsNullOrEmpty(currentAssembly.Location))
                yield return Path.Combine(currentAssembly.Location, pathPrefix);
        }
    }
}
