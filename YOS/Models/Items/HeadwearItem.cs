using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Items
{
    public class HeadwearItem : IClosetItemModel
    {
        public HeadwearItem(string name,
            Poses pose = Poses.Idle,
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor)
        {
            var i = ClosetItemList.GetItem(name);
            _name = i.Name;
            _gender = i.Gender;
            _type = i.Type;
            _style = style;
            _weather = weather;
            _pose = pose;
            _genderChangeModel = false;
        }
        private string _name;
        private Styles _style;
        private Weather _weather;
        private Poses _pose;
        private GenderTypes _gender;
        private Type _type;
        private PBRMaterial _material;
        private Geometry3D _geometry;
        private bool _genderChangeModel;
        public string Name { get => _name; private set => _name = value; }
        public Styles Style { get => _style; private set => _style = value; }
        public Weather Weather { get => _weather; private set => _weather = value; }
        public Poses Pose { get => _pose; private set => _pose = value; }
        public PBRMaterial Material { get => _material; private set => _material = value; }
        public Geometry3D Geometry { get => _geometry; private set => _geometry = value; }
        public bool GenderChangeModel { get => _genderChangeModel; private set => _genderChangeModel = value; }
        public GenderTypes Gender { get => _gender; private set => _gender = value; }
        public Type Type { get => _type; private set => _type = value; }
    }

}
