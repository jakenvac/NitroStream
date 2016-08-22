using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Octokit;
using System.Windows;

namespace Nitro_Stream.Model
{
    class Updater : ModelBase
    {
        private bool _UpdateAvailable;
        public bool UpdateAvailable
        {
            get { return _UpdateAvailable; }
        }

        private string _LatestUrl;

        public Updater()
        {
            CheckUpdate();
        }

        private async void CheckUpdate()
        {
            var client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("NitroStream"));
            var Latest = await client.Repository.Release.GetLatest("JakeHL", "NitroStream");

            if (System.Diagnostics.Debugger.IsAttached)
                GetStats(client);            

            _LatestUrl = Latest.HtmlUrl;
            string serverVer, localVer;
            serverVer = Latest.TagName.Replace(".", "").Replace("v", "0");
            localVer = ViewModel.NitroStreamViewModel.Version.Replace(".", "");
            int sver, lver;

            if (int.TryParse(serverVer, out sver) && int.TryParse(localVer, out lver))
                _UpdateAvailable = sver > lver;
            OnPropertyChanged("UpdateAvailable");            
        }

        private async void GetStats(GitHubClient client)
        {
            var releases = await client.Repository.Release.GetAll("JakeHL", "NitroStream");
            int total = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var r in releases)
            {
                int itotal = 0;
                StringBuilder ib = new StringBuilder();
                ib.Append(string.Format("Version {0} {1} \n", r.TagName, r.Name));
                foreach (var a in r.Assets)
                {
                    total += a.DownloadCount;
                    itotal += a.DownloadCount;
                    ib.Append(string.Format("{0} : {1} \n", a.Name, a.DownloadCount));
                }
                ib.Append(string.Format("Version total: {0} \n \n", itotal));
                sb.Append(ib.ToString());
            }
            sb.Append(string.Format("Running total: {0}", total));
            MessageBox.Show(sb.ToString());
        }

        public void GetUpdate()
        {
            System.Diagnostics.Process.Start(_LatestUrl);
        }

    }
}
