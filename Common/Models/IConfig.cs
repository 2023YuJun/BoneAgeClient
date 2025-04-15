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
        bool DetectStatus { get; set; }
        bool ResultStatus { get; set; }
        // 其他需要监听的字段...
    }
}
