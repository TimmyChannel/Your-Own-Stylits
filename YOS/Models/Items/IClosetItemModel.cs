using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;

namespace YOS.Models.Items
{
    public interface IClosetItemModel : ICloneable
    {
        string Name { get; }
        Poses Pose { get; }
        PBRMaterial Material { get; set; }
        Geometry3D Geometry { get; }
        GenderTypes Gender { get; }
        Type Type { get; }
        List<Materials> AvaliableMaterials { get; }
    }
}
