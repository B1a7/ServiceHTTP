using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Modules
{
    public class TangibleModule : ModuleBase
    {
        public string Object { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public int RotationAngle { get; set; }

    }
}
