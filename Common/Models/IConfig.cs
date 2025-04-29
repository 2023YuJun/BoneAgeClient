using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public interface IConfig
    {
        int FormLocationX { get; set; }
        int FormLocationY { get; set; }
        bool BootUp { get; set; }
        bool StartStatus { get; set; }
        bool DetectStatus { get; set; }
        bool ResultStatus { get; set; }
        bool BrowserStatus { get; set; }
    }
}
