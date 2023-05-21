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
        public string Name { get; set; }
        Styles Style { get; set; }
        Weather Weather { get; set; }
        Poses Pose { get; set; }
        Somatotypes Somatotype { get; set; }
        PBRMaterial DefaultMaterial { get; set; }
        Geometry3D Geometry { get; set; }
        bool SomatotypeChangeModel { get; }
        List<PBRMaterial> AvaliableMaterials { get; }
    }
}
