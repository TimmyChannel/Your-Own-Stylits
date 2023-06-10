using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Algorithm
{
    interface AlgorytmForWear
    {
        void WearMe(GenderTypes _g, Weather _w, Styles _s);
    }
}
