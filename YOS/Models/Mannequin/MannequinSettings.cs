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
        IClosetItem? _accessory;
        IClosetItem? _headwear;
        IClosetItem? _shoes;
        IClosetItem? _top;
        IClosetItem? _bottom;
        List<IClosetItem> _closetItemList;
        public List<IClosetItem> ClosetItems => _closetItemList;

        private static readonly Lazy<MannequinSettings> lazy =
       new Lazy<MannequinSettings>(() => new MannequinSettings());
        public static MannequinSettings Instance { get { return lazy.Value; } }
        public MannequinSettings()
        {
            _mannequin = new RealisticModel("Joe", true);
            _pose = _mannequin.Pose;
            _gender = _mannequin.Gender;
            _closetItemList = new List<IClosetItem>()
            {
                _accessory,
                _headwear,
                _shoes,
                _top,
                _bottom,
            };
        }
        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                _mannequin.Gender = value;
                _gender = value;
                UpdateItems();
            }
        }
        public Poses Pose
        {
            get => _pose;
            set
            {
                _mannequin.Pose = value;
                _pose = value;
                UpdateItems();
            }
        }
        private void UpdateItems()
        {
            foreach (var item in _closetItemList)
                AddClosetItem(ClosetItemList.GetItem(item.Name));
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
        public void ResetItems()
        {
            _accessory = null;
            _shoes = null;
            _headwear = null;
            _top = null;
            _bottom = null;
        }

    }
}
