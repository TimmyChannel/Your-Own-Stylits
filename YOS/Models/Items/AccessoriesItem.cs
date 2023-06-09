﻿using HelixToolkit.Wpf.SharpDX;
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
            if (!Directory.Exists(_mainPath))
                throw new ArgumentException("This item has no such gender");
            _texPath = $"{_mainPath}\\Textures";
            _modelPath = $"{_mainPath}\\{_pose}\\{_name}.obj";
            InitAvaliableMaterials();
            InitGeometryAndMaterials();
        }
    }

}
