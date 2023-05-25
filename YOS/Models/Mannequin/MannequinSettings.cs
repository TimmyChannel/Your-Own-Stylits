using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using YOS.Models.Items;
using YOS.Models.Creators;

namespace YOS.Models.Mannequin
{
    public class MannequinSettings
    {
        IMannequinModel _mannequin;
        GenderTypes _gender;
        Poses _pose;
        IClosetItem _accessory;
        IClosetItem _headwear;
        IClosetItem _shoes;
        IClosetItem _top;
        IClosetItem _bottom;

        public MannequinSettings(GenderTypes gender)
        {
            _mannequin = new RealisticModel("Joe", true, gender);
            _pose = _mannequin.Pose;
            _gender = gender;
        }
        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                _mannequin.Gender = value;
                _gender = value;
            }
        }
        public Poses Pose
        {
            get => _pose;
            set
            {
                _mannequin.Pose = value;
                _pose = value;
            }
        }
        public void ChangeVisibility(bool visible) => _mannequin.IsVisible = visible;
        public void AddClosetItem(string name, Type type)
        {
            switch (type)
            {
                case Type.Top:
                    var topCreator = new TopCreator();
                    topCreator.SetParams(_pose, _gender);
                    _top = topCreator.CreateClosetItem(name);
                    break;
                case Type.Bottom:
                    var bottomCreator = new BottomCreator();
                    bottomCreator.SetParams(_pose, _gender);
                    _bottom = bottomCreator.CreateClosetItem(name);
                    break;
                case Type.Accessories:
                    var accCreator = new BottomCreator();
                    accCreator.SetParams(_pose);
                    _accessory = accCreator.CreateClosetItem(name);
                    break;
                case Type.Shoes:
                    var shoeCreator = new BottomCreator();
                    shoeCreator.SetParams(_pose);
                    _shoes = shoeCreator.CreateClosetItem(name);
                    break;
                case Type.Headwear:
                    var headCreator = new HeadwearCreator();
                    headCreator.SetParams(_pose);
                    _headwear = headCreator.CreateClosetItem(name);
                    break;
            }
        }
        public void AddClosetItem(Item item)
        {
            switch (item.Type)
            {
                case Type.Top:
                    var topCreator = new TopCreator();
                    topCreator.SetParams(_pose, _gender);
                    _top = topCreator.CreateClosetItem(item.Name);
                    break;
                case Type.Bottom:
                    var bottomCreator = new BottomCreator();
                    bottomCreator.SetParams(_pose, _gender);
                    _bottom = bottomCreator.CreateClosetItem(item.Name);
                    break;
                case Type.Accessories:
                    var accCreator = new BottomCreator();
                    accCreator.SetParams(_pose);
                    _accessory = accCreator.CreateClosetItem(item.Name);
                    break;
                case Type.Shoes:
                    var shoeCreator = new BottomCreator();
                    shoeCreator.SetParams(_pose);
                    _shoes = shoeCreator.CreateClosetItem(item.Name);
                    break;
                case Type.Headwear:
                    var headCreator = new HeadwearCreator();
                    headCreator.SetParams(_pose);
                    _headwear = headCreator.CreateClosetItem(item.Name);
                    break;
            }
        }
    }
}
