using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitro_Stream.Model
{

    public enum Orientations
    {
        Horizontal = 0,
        Vertical = 1
    }

    public class ViewSettings : ModelBase
    {
        string _IPAddress;
        public string IPAddress
        {
            get { return _IPAddress; }
            set
            {
                _IPAddress = value;
                OnPropertyChanged("IPAddress");
            }
        }

        bool _ShowLog;
        public bool ShowLog
        {
            get { return _ShowLog; }
            set
            {
                _ShowLog = value;
                OnPropertyChanged("ShowLog");
            }
        }

        double _TopScale;
        public double TopScale
        {
            get { return _TopScale; }
            set
            {
                _TopScale = value;
                OnPropertyChanged("TopScale");
            }
        }

        double _BottomScale;
        public double BottomScale
        {
            get { return _BottomScale; }
            set
            {
                _BottomScale = value;
                OnPropertyChanged("BottomScale");
            }
        }

        Orientations _ViewMode;
        public Orientations ViewMode
        {
            get { return _ViewMode; }
            set
            {
                _ViewMode = value;
                OnPropertyChanged("ViewMode");
            }
        }

        uint _PictureQuality;
        public uint PictureQuality
        {
            get { return _PictureQuality; }
            set
            {
                _PictureQuality = value;
                OnPropertyChanged("PictureQuality");
            }
        }

        uint _PriorityFactor;
        public uint PriorityFactor
        {
            get { return _PriorityFactor; }
            set
            {
                _PriorityFactor = value;
                OnPropertyChanged("PriorityFactor");
            }
        }

        double _QosValue;
        public double QosValue
        {
            get { return _QosValue; }
            set
            {
                _QosValue = value;
                OnPropertyChanged("QosValue");
            }
        }

        bool _PriorityMode;
        public bool PriorityMode
        {
            get { return _PriorityMode; }
            set
            {
                _PriorityMode = value;
                OnPropertyChanged("PriorityMode");
            }
        }

        string _ViewerPath;
        public string ViewerPath
        {
            get { return _ViewerPath; }
            set
            {
                _ViewerPath = value;
                OnPropertyChanged("ViewerPath");
            }
        }

        public ViewSettings()
        {

        }

        public ViewSettings(bool loadDefaults)
        {
            if (loadDefaults)
            {
                _IPAddress = "AAA.BBB.YYY.ZZZ";
                _TopScale = 1;
                _BottomScale = 1;
                _ViewMode = 0;
                _PriorityMode = false;
                _PriorityFactor = 5;
                _PictureQuality = 90;
                _QosValue = 15;
                _ViewerPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NTRViewer.exe");
                _ShowLog = true;
            }
        }

        public static void Save(string path, ViewSettings vs)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(vs.GetType());

            using (System.IO.StreamWriter s = new System.IO.StreamWriter(path))
            {
                xs.Serialize(s, vs);
            }
        }

        public static ViewSettings Load(string path)
        {
            ViewSettings vs = new ViewSettings(true);
            if (System.IO.File.Exists(path))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(vs.GetType());

                using (System.IO.StreamReader s = new System.IO.StreamReader(path))
                {

                    vs = (ViewSettings)xs.Deserialize(s);
                    return vs;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
