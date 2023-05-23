using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;
namespace YOS.Models
{
    public interface IClosetItem
    {
        string Name { get; }
        Poses Pose { get; }
        Somatotypes Somatotype { get; }
        PBRMaterial DefaultMaterial { get; }
        Geometry3D Geometry { get; }
        bool SomatotypeChangeModel { get; }
        GenderTypes Gender { get; }
        Type Type { get; }
    }
}
