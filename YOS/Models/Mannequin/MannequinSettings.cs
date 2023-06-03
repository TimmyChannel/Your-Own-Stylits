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
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;

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
        IClosetItemModel _selectedItem;
        public IClosetItemModel SelectedItem => _selectedItem;
        public IClosetItemModel Bottom => _bottom;
        public IClosetItemModel Top => _top;
        public IClosetItemModel Headwear => _headwear;
        public IClosetItemModel Shoes => _shoes;
        public IClosetItemModel Accessory => _accessory;
        public IMannequinModel Mannequin => _mannequin;
        readonly ObservableCollection<IClosetItemModel> _closetItemList;
        public ObservableCollection<IClosetItemModel> ClosetItems => _closetItemList;
        public ObservableElement3DCollection MannequinModel => _mannequin.Mannequin;
        public bool MannequinIsVisible
        {
            get => _mannequin.IsVisible;
            set
            {
                _mannequin.IsVisible = value;
                OnPropertyChanged();
            }
        }
        private static readonly Lazy<MannequinSettings> lazy = new(() => new MannequinSettings());
        public static MannequinSettings Instance { get { return lazy.Value; } }
        private MannequinSettings()
        {
            _mannequin = new RealisticModel("Joe", true, GenderTypes.Male, Poses.Running);
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
        public void SelectItemByType(Type type)
        {
            switch (type)
            {
                case Type.Top:
                    _selectedItem = _top;
                    break;
                case Type.Bottom:
                    _selectedItem = _bottom;
                    break;
                case Type.Accessories:
                    _selectedItem = _accessory;
                    break;
                case Type.Shoes:
                    _selectedItem = _shoes;
                    break;
                case Type.Headwear:
                    _selectedItem = _headwear;
                    break;
                default:
                    break;
            }
            OnPropertyChanged("SelectedItem");
        }
        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                SetProperty<GenderTypes>(ref _gender, value);
                UpdateItems();
                _mannequin = new RealisticModel("noob", true, _gender, _pose);
            }
        }
        public Poses Pose
        {
            get => _pose;
            set
            {
                _mannequin.Pose = value;
                SetProperty<Poses>(ref _pose, value);
                UpdateItems();
                _mannequin = new RealisticModel("noob", true, _gender, _pose);
            }
        }
        private void UpdateItems()
        {
            for (int i = 0; i < _closetItemList.Count; i++)
            {
                if (_closetItemList[i] != null)
                    AddClosetItem(ClosetItemList.GetItem(_closetItemList[i].Name));
            }
        }
        public void AddClosetItem(string name, Type type)
        {
            try
            {
                switch (type)
                {
                    case Type.Top:
                        var topCreator = new TopCreator();
                        topCreator.SetParams(_pose, _gender);
                        _top = null;
                        _top = topCreator.CreateClosetItem(name);
                        _closetItemList[0] = _top;
                        break;
                    case Type.Bottom:
                        var bottomCreator = new BottomCreator();
                        bottomCreator.SetParams(_pose, _gender);
                        _bottom = null;
                        _bottom = bottomCreator.CreateClosetItem(name);
                        _closetItemList[1] = _bottom;
                        break;
                    case Type.Accessories:
                        var accCreator = new BottomCreator();
                        accCreator.SetParams(_pose);
                        _accessory = null;
                        _accessory = accCreator.CreateClosetItem(name);
                        _closetItemList[2] = _accessory;
                        break;
                    case Type.Shoes:
                        var shoeCreator = new BottomCreator();
                        shoeCreator.SetParams(_pose);
                        _shoes = null;
                        _shoes = shoeCreator.CreateClosetItem(name);
                        _closetItemList[3] = _shoes;
                        break;
                    case Type.Headwear:
                        var headCreator = new HeadwearCreator();
                        headCreator.SetParams(_pose);
                        _headwear = null;
                        _headwear = headCreator.CreateClosetItem(name);
                        _closetItemList[4] = _headwear;
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddClosetItem(Item item)
        {
            try
            {
                switch (item.Type)
                {
                    case Type.Top:
                        var topCreator = new TopCreator();
                        topCreator.SetParams(_pose, _gender);
                        _top = null;
                        _top = topCreator.CreateClosetItem(item.Name);
                        _closetItemList[0] = _top;
                        break;
                    case Type.Bottom:
                        var bottomCreator = new BottomCreator();
                        bottomCreator.SetParams(_pose, _gender);
                        _bottom = null;
                        _bottom = bottomCreator.CreateClosetItem(item.Name);
                        _closetItemList[1] = _bottom;
                        break;
                    case Type.Accessories:
                        var accCreator = new BottomCreator();
                        accCreator.SetParams(_pose);
                        _accessory = null;
                        _accessory = accCreator.CreateClosetItem(item.Name);
                        _closetItemList[2] = _accessory;
                        break;
                    case Type.Shoes:
                        var shoeCreator = new BottomCreator();
                        shoeCreator.SetParams(_pose);
                        _shoes = null;
                        _shoes = shoeCreator.CreateClosetItem(item.Name);
                        _closetItemList[3] = _shoes;
                        break;
                    case Type.Headwear:
                        var headCreator = new HeadwearCreator();
                        headCreator.SetParams(_pose);
                        _headwear = null;
                        _headwear = headCreator.CreateClosetItem(item.Name);
                        _closetItemList[4] = _headwear;
                        break;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void SetMaterialToSelectedItem(PBRMaterial material)
        {
            SelectedItem.Material = material;
            switch (SelectedItem.Type)
            {
                case Type.Top:
                    if (Top == null) return;
                    Top.Material = material;
                    OnPropertyChanged("Top");
                    break;
                case Type.Bottom:
                    if (Bottom == null) return;
                    Bottom.Material = material;
                    OnPropertyChanged("Bottom");
                    break;
                case Type.Accessories:
                    if (Accessory == null) return;
                    Accessory.Material = material;
                    OnPropertyChanged("Accessory");
                    break;
                case Type.Shoes:
                    if (Shoes == null) return;
                    Shoes.Material = material;
                    OnPropertyChanged("Shoes");
                    break;
                case Type.Headwear:
                    if (Headwear == null) return;
                    Headwear.Material = material;
                    OnPropertyChanged("Headwear");
                    break;
                default:
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
