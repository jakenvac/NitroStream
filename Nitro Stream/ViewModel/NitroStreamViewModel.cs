using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Permissions;

namespace Nitro_Stream.ViewModel
{
    class NitroStreamViewModel : ViewModelBase
    {

        public static string Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        Model.NtrClient _NtrClient;
        System.Timers.Timer _DisconnectTimeout;
        bool _PatchMem;
        bool _Connected;

        public string configPath { get { return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml"); } }

        public Model.Updater Updater { get; set; }

        private StringBuilder _RunningLog;
        public string runningLog
        {
            get { return _RunningLog.ToString(); }
        }

        Model.ViewSettings _ViewSettings;
        public Model.ViewSettings ViewSettings { get { return _ViewSettings; } set { _ViewSettings = value; } }

        public NitroStreamViewModel()
        {
            _ViewSettings = new Model.ViewSettings(true);
            _NtrClient = new Model.NtrClient();
            _DisconnectTimeout = new System.Timers.Timer(10000);
            _DisconnectTimeout.Elapsed += _disconnectTimeout_Elapsed;
            if (System.IO.File.Exists(configPath))
                _ViewSettings = Model.ViewSettings.Load(configPath);

            _NtrClient.onLogArrival += WriteToLog;
            _NtrClient.Connected += _ntrClient_Connected;
            AppDomain.CurrentDomain.UnhandledException += ExceptionToLog;

            _RunningLog = new StringBuilder("");

            Updater = new Model.Updater();
        }

        internal void Donate()
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ASKURE99X999W");
        }

        private void _ntrClient_Connected(bool Connected)
        {
            if (Connected)
            {
                _Connected = true;
                if (_PatchMem)
                {
                    byte[] bytes = { 0x70, 0x47 };
                    _WriteToDeviceMemory(0x00105B00, bytes, 0x1a);
                    _PatchMem = false;
                }
                else
                {
                    uint pm = (uint)(_ViewSettings.PriorityMode ? 1 : 0);
                    remoteplay(pm, _ViewSettings.PriorityFactor, _ViewSettings.PictureQuality, _ViewSettings.QosValue);
                    _DisconnectTimeout.Start();

                    if (System.IO.File.Exists(_ViewSettings.ViewerPath))
                    {
                        StringBuilder args = new StringBuilder();

                        args.Append("-l ");
                        args.Append(((_ViewSettings.ViewMode == Model.Orientations.Vertical) ? "0" : "1") + " ");
                        args.Append("-t " + _ViewSettings.TopScale.ToString() + " ");
                        args.Append("-b " + _ViewSettings.BottomScale.ToString());

                        System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(_ViewSettings.ViewerPath);
                        p.Verb = "runas";
                        p.Arguments = args.ToString().Replace(',','.');
                        Process.Start(p);
                    }
                    else
                        WriteToLog("NTRViewer not found, please run it manually as admin");
                }
            }
        }

        private void ExceptionToLog(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                throw ex;
            }
            WriteToLog("ERR:" + ex.Message.ToString());
        }

        public void WriteToLog(string msg)
        {
            _RunningLog.Append(msg);
            _RunningLog.Append("\n");
            OnPropertyChanged("RunningLog");
        }

        private void _disconnectTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Disconnect();
            _DisconnectTimeout.Stop();
        }

        public void InitiateRemotePlay()
        {
            Connect(_ViewSettings.IPAddress);
        }

        public void Connect(string host)
        {
            _NtrClient.setServer(host, 8000);
            _NtrClient.connectToServer();
        }

        public void MemPatch()
        {
            _PatchMem = true; 
            Connect(_ViewSettings.IPAddress);            
            WriteToLog("OK: Memory patch applied");
        }

        public void Disconnect()
        {
            _NtrClient.disconnect();
        }

        private void _WriteToDeviceMemory(uint addr, byte[] buf, int pid = -1)
        {
            _NtrClient.sendWriteMemPacket(addr, (uint)pid, buf);
        }

        public void remoteplay(uint priorityMode = 0, uint priorityFactor = 5, uint quality = 90, double qosValue = 15)
        {
            uint num = 1;
            if (priorityMode == 1)
            {
                num = 0;
            }
            uint qosval = (uint)(qosValue * 1024 * 1024 / 8);
            _NtrClient.sendEmptyPacket(901, num << 8 | priorityFactor, quality, qosval);
            WriteToLog("OK: Remoteplay initiated. This client will disconnect in 10 seconds.");
        }

    }
}
