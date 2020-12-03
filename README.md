# cefsharp-remoting
This project is archived because it is old and I won't use it anymore. The project is archived.

How to use remoting CefSharp with ShadowCopyFile

This solution gives an example to how launch CefSharp browser from an application launched with ShadowCopy.
Considering that the CefSharp can run only int eh default app domain (https://github.com/cefsharp/CefSharp/wiki/General-Usage#need-to-knowlimitation), I have looked at the project https://github.com/stever/AppHostCefSharp. 

 Although the solution is for a WPF application, I used with success. The solution is composed of 4 projects:
 - MainApplication (base application that I have to launch with ShadowCopy)
 - MainApplication.Launcher (application launcher)
 - MainApplication.WebBrowser (winforms controls library that contains the WebBrowser)
 - MainApplication.Interfaces (interfaces that must be implemented for the operations)

 For remoting I forked the RedGate.AppHost project to add a property ClientExecutablePath that I used to specify the position of the client executables. 
 Now, it use the executing assembly location for find the client path Although we used ShadowCopy in launcher application, the RedGate clients doesn't exists in the assembly location.

 You can see that edits in the follow files:
  - https://github.com/rupertsciamenna89/RedGate.AppHost/blob/master/RedGate.AppHost.Server/ChildProcessFactory.cs
  - https://github.com/rupertsciamenna89/RedGate.AppHost/blob/master/RedGate.AppHost.Server/ProcessStarter.cs

  The Redgate implementation however returns a FrameworkElement object, that cannot be used to interact with the created object. 
  To allow the communications from the main form and the created object I set two WCF NamedPipes services, 
  (one on the main form, another on the created object).
