using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AppSettings : IConfig
    {
        public string AppName { get; set; } = "";
        public string AppPath { get; set; } = "";
        public string DefaultBrowserPath { get; set; } = "";
        public string OurBrowserUrl { get; } = "https://dl.google.com/chrome/install/latest/chrome_installer.exe";
        public string OurBrowserPath { get; set; } = "";
        public bool StartStatus { get; set; }
        public bool DetectStatus { get; set; }
        public bool ResultStatus { get; set; }
        public bool BrowserStatus { get; set; }
        public string CurrentBrowser { get; set; } = "";
        public string ServiceIP { get; set; } = "";
        public string IFZ { get; set; } = "";
        public int MinArea { get; set; } = 800000;
        public int MaxArea { get; set; } = 900000;
        public int MaxAspectRatio { get; set; } = 4;
        public int ColumnToTable { get; set; }
        public string RE { get; set; } = "\\d+";
        public string? DetectData { get; set; }
        public string? ResultUrl { get; set; }
        public int FormLocationX { get; set; } = 800;
        public int FormLocationY { get; set; } = 50;
        public bool BootUp { get; set; }
        public bool AutoOpen { get; set; }
        public bool ResultResident { get; set; }

        public string FaultFeedBack { get; set; } = "";
    }
}
