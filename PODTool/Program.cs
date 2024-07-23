using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Windows.Forms;

namespace PODTool
{
    static class Program
    {
        const string AppId = "Local\\3f6134bc-1f49-4582-a4ee-3d7044294e98";
        
        static MainForm mainForm;
        static Process mainProcess;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (var mutex = new Mutex(false, AppId))
            {
                if (!mutex.WaitOne(0))
                {
                    // app already running, forward to existing instance
                    IpcChannel clientChannel = new IpcChannel();
                    ChannelServices.RegisterChannel(clientChannel, false);
                    SingleInstance app = (SingleInstance)Activator.GetObject(typeof(SingleInstance), string.Format("ipc://{0}/RemotingServer", AppId));
                    app.Execute(args);
                    return;
                }

                // register the IPC server
                IpcChannel channel = new IpcChannel(AppId);
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingleInstance), "RemotingServer", WellKnownObjectMode.Singleton);

                // start the application
                mainProcess = Process.GetCurrentProcess();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                Environment.CurrentDirectory = strWorkPath;

                ProgramSettings.Init();

                mainForm = new MainForm();
                mainForm.OpenFilesFromArgs(args);
                Application.Run(mainForm);
            }
        }

        private class SingleInstance : MarshalByRefObject
        {   
            private void OpenSingleInstanceStageOne(string[] args)
            {
                if (mainForm.WindowState == FormWindowState.Minimized)
                    mainForm.WindowState = FormWindowState.Normal;
                mainForm.OpenFilesFromArgs(args);
            }

            private void OpenSingleInstanceStageTwo()
            {
                mainForm.Activate();
            }

            public void Execute(string[] args)
            {
                // this is split because if the mainForm is doing work when Activate is called it's not moved to the foreground
                mainForm.Invoke((Action)(() => { OpenSingleInstanceStageOne(args); } ));
                mainForm.Invoke((Action)OpenSingleInstanceStageTwo);
            }
        }
    }
}
