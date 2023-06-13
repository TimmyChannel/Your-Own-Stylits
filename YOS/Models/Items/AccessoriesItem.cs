using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using SharpDX;

namespace YOS.Models.Items
{
    public class AccessoriesItem : ModelItemBase
    {
        public AccessoriesItem(string name,
            GenderTypes gender = GenderTypes.Male,
            Poses pose = Poses.Idle,
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor) : base(name, gender, pose, style, weather)
        {
            _type = Type.Accessories;
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Items\\{_type}\\{_name}\\{_gender}";
            if (Directory.Exists(_mainPath))
            {
                _texPath = $"{_mainPath}\\Textures";
                _modelPath = $"{_mainPath}\\{_pose}\\{_name}.obj";
                InitAvaliableMaterials();
                InitGeometryAndMaterials();
                _material.AlbedoColor = Color4.White;
            }
        }

        public override object Clone()
        {
            var clone = new AccessoriesItem(Name, Gender, Pose, Style, Weather);
            clone._material = (PBRMaterial)_material.Clone();
            clone._geometry = _geometry;
            clone._type = _type;
            return clone;
        }
    }

}
