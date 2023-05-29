using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using YOS.Models.Items;
using YOS.Models.Creators;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace YOS.Models.Mannequin
{
    public class MannequinSettings : OnPropertyChangedClass
    {

        IMannequinModel _mannequin;
        GenderTypes _gender;
        Poses _pose;
        IClosetItemModel? _accessory;
        IClosetItemModel? _headwear;
        IClosetItemModel? _shoes;
        IClosetItemModel? _top;
        IClosetItemModel? _bottom;
        public IClosetItemModel Bottom => _bottom;
        public IClosetItemModel Top => _top;
        public IClosetItemModel Headwear => _headwear;
        public IClosetItemModel Shoes => _shoes;
        public IClosetItemModel Accessory => _accessory;
        public IMannequinModel Mannequin => _mannequin;
        readonly ObservableCollection<IClosetItemModel> _closetItemList;
        public ObservableCollection<IClosetItemModel> ClosetItems => _closetItemList;

        private static readonly Lazy<MannequinSettings> lazy = new(() => new MannequinSettings());
        public static MannequinSettings Instance { get { return lazy.Value; } }
        private MannequinSettings()
        {
            _mannequin = new RealisticModel("Joe", true);
            _pose = _mannequin.Pose;
            _gender = _mannequin.Gender;
            _closetItemList = new ObservableCollection<IClosetItemModel>()
            {
                _top,
                _bottom,
                _accessory,
                _shoes,
                _headwear,
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
                SetProperty<GenderTypes>(ref _gender, value);
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
                SetProperty<Poses>(ref _pose, value);
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
