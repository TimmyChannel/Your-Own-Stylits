using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Items
{
    public class BottomItem : ModelItemBase
    {
	  public BottomItem(string name,
            GenderTypes gender = GenderTypes.Male,
            Poses pose = Poses.Idle,
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor) : base(name, gender, pose, style, weather)
        {
            _type = Type.Bottom;
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Items\\{_type}\\{_name}\\{_gender}";
            if (!Directory.Exists(_mainPath))
                throw new ArgumentException("This item has no such gender");
            _texPath = $"{_mainPath}\\Textures";
            _modelPath = $"{_mainPath}\\{_pose}\\{_name}.obj";
            InitAvaliableMaterials();
            InitGeometryAndMaterials();
        }
        public override object Clone()
        {
            var clone = new BottomItem(Name, Gender, Pose, Style, Weather);
            clone._material = (PBRMaterial)_material.Clone();
            clone._geometry = _geometry;
            clone._type = _type;
            return clone;
        }
    }
}
