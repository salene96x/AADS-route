using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    class MapMode
    {
        public string Name { get; set; }
        public GMapProvider MapProvider { get; set; }
        public int MinZoom { get; set; }
        public int MaxZoom { get; set; }
    }
}
