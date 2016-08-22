using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nitro_Stream.Model
{
    class Updater
    {
        private string _BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        //private
        public bool UpdateAvailable { get; set; }

        public Updater()
        {
            if (File.Exists(Path.Combine(_BaseDirectory, "Octokit.dll")))
            {
                LoadAssembly();
                CheckForUpdate();
            }
        }

        private void LoadAssembly()
        {
            throw new NotImplementedException();
        }

        private  bool CheckForUpdate()
        {
            throw new NotImplementedException();
        }

    }
}
