using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOS.Models.EnumParams
{
    [Flags]
    public enum Materials
    {
        Cotton = 1,
        Linen = 2,
        Wool = 4,
        Denim = 8,
        Polyester = 16,
        Silk = 32,
        Leather = 64,
        Plastic = 128,
        Textile = 256,
    }
}