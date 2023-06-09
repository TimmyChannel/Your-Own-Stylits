using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using SharpDX;
using System.IO;

namespace YOS.Models.Items
{
    public class TopItem : ModelItemBase
    {
        public TopItem(string name,
            GenderTypes gender = GenderTypes.Male,
            Poses pose = Poses.Idle,
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor) : base(name, gender, pose, style, weather)
        {
            _type = Type.Top;
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Items\\{_type}\\{_name}\\{_gender}";
            if (!Directory.Exists(_mainPath))
                throw new ArgumentException("This item has no such gender");
            _texPath = $"{_mainPath}\\Textures";
            _modelPath = $"{_mainPath}\\{_pose}\\{_name}.obj";
            InitAvaliableMaterials();
            InitGeometryAndMaterials();
            _material.AlbedoColor = new Color4(0.561f, 0.561f, 0.561f, 1);
        }
        public override object Clone()
        {
            var clone = new TopItem(Name, Gender, Pose, Style, Weather);
            clone._material = (PBRMaterial)_material.Clone();
            clone._geometry = _geometry;
            clone._type = _type;
            return clone;
        }
    }
}
