using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOS.Models.EnumParams
{
    [Flags]
    public enum Weather
    {
        Sunny = 1,
        Cloudy = 2,
        Indoor = 4
    }
}