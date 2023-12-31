﻿using System;
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
using SharpDX;
using System.Diagnostics;
using System.Xml.Linq;

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
        private bool _iLoadModels;
        public bool ILoadModels
        {
            get => _iLoadModels;
            private set
            {
                SetProperty<bool>(ref _iLoadModels, value);
                Debug.WriteLine($"ILoadModels {value}");
            }
        }
        private static readonly Lazy<MannequinSettings> lazy = new(() => new MannequinSettings());
        public static MannequinSettings Instance { get { return lazy.Value; } }
        private MannequinSettings()
        {
            _mannequin = new RealisticModel("Joe", true, GenderTypes.Male, Poses.A);
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
            OnPropertyChanged(nameof(SelectedItem));
        }
        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                SetProperty<GenderTypes>(ref _gender, value);
                ILoadModels = true;
                SetProperty<IMannequinModel>(ref _mannequin, new RealisticModel("noob", true, _gender, _pose), nameof(MannequinModel));
                ILoadModels = false;
                UpdateItems();
            }
        }
        public Poses Pose
        {
            get => _pose;
            set
            {
                SetProperty<Poses>(ref _pose, value);
                ILoadModels = true;
                SetProperty<IMannequinModel>(ref _mannequin, new RealisticModel("noob", true, _gender, _pose), nameof(MannequinModel));
                ILoadModels = false;
                UpdateItems();
            }
        }
        private void UpdateItems()
        {
            for (int i = 0; i < _closetItemList.Count; i++)
            {
                if (_closetItemList[i] != null)
                {
                    var prevColor = _closetItemList[i].Color;
                    var prevMat = _closetItemList[i].TextureMaterial;
                    AddClosetItem(ClosetItemList.GetItem(_closetItemList[i].Name));
                    if (_closetItemList[i] != null)
                    {
                        _closetItemList[i].SetColor(prevColor);
                        _closetItemList[i].SetMaterial(prevMat);
                    }
                }
            }
        }
        public void AddClosetItem(string name, Type type)
        {
            try
            {
                ILoadModels = true;
                switch (type)
                {
                    case Type.Top:
                        var topCreator = new TopCreator();
                        topCreator.SetParams(_pose, _gender);
                        _top = topCreator.CreateClosetItem(name);
                        if (_top == null || !_top.IInitialized) _top = null;
                        _closetItemList[0] = _top;
                        OnPropertyChanged(nameof(Top));
                        break;
                    case Type.Bottom:
                        var bottomCreator = new BottomCreator();
                        bottomCreator.SetParams(_pose, _gender);
                        _bottom = bottomCreator.CreateClosetItem(name);
                        if (_bottom == null || !_bottom.IInitialized) _bottom = null;
                        _closetItemList[1] = _bottom;
                        OnPropertyChanged(nameof(Bottom));
                        break;
                    case Type.Accessories:
                        var accCreator = new AccessoriesCreator();
                        accCreator.SetParams(_pose, _gender);
                        _accessory = accCreator.CreateClosetItem(name);
                        if (_accessory == null || !_accessory.IInitialized) _accessory = null;
                        _closetItemList[2] = _accessory;
                        OnPropertyChanged(nameof(Accessory));
                        break;
                    case Type.Shoes:
                        var shoeCreator = new ShoesCreator();
                        shoeCreator.SetParams(_pose, _gender);
                        _shoes = shoeCreator.CreateClosetItem(name);
                        if (_shoes == null || !_shoes.IInitialized) _shoes = null;
                        _closetItemList[3] = _shoes;
                        OnPropertyChanged(nameof(Shoes));
                        break;
                    case Type.Headwear:
                        var headCreator = new HeadwearCreator();
                        headCreator.SetParams(_pose, _gender);
                        _headwear = headCreator.CreateClosetItem(name);
                        if (_headwear == null || !_headwear.IInitialized) _headwear = null;
                        _closetItemList[4] = _headwear;
                        OnPropertyChanged(nameof(Headwear));
                        break;
                }
                ILoadModels = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
        public void AddClosetItem(Item item)
        {
            try
            {
                ILoadModels = true;
                switch (item.Type)
                {
                    case Type.Top:
                        var topCreator = new TopCreator();
                        topCreator.SetParams(_pose, _gender);
                        _top = topCreator.CreateClosetItem(item.Name);
                        if (_top == null || !_top.IInitialized) _top = null;
                        _closetItemList[0] = _top;
                        OnPropertyChanged(nameof(Top));
                        break;
                    case Type.Bottom:
                        var bottomCreator = new BottomCreator();
                        bottomCreator.SetParams(_pose, _gender);
                        _bottom = bottomCreator.CreateClosetItem(item.Name);
                        if (_bottom == null || !_bottom.IInitialized) _bottom = null;
                        _closetItemList[1] = _bottom;
                        OnPropertyChanged(nameof(Bottom));
                        break;
                    case Type.Accessories:
                        var accCreator = new AccessoriesCreator();
                        accCreator.SetParams(_pose, _gender);
                        _accessory = accCreator.CreateClosetItem(item.Name);
                        if (_accessory == null || !_accessory.IInitialized) _accessory = null;
                        _closetItemList[2] = _accessory;
                        OnPropertyChanged(nameof(Accessory));
                        break;
                    case Type.Shoes:
                        var shoeCreator = new ShoesCreator();
                        shoeCreator.SetParams(_pose, _gender);
                        _shoes = shoeCreator.CreateClosetItem(item.Name);
                        if (_shoes == null || !_shoes.IInitialized) _shoes = null;
                        _closetItemList[3] = _shoes;
                        OnPropertyChanged(nameof(Shoes));
                        break;
                    case Type.Headwear:
                        var headCreator = new HeadwearCreator();
                        headCreator.SetParams(_pose, _gender);
                        _headwear = headCreator.CreateClosetItem(item.Name);
                        if (_headwear == null || !_headwear.IInitialized) _headwear = null;
                        _closetItemList[4] = _headwear;
                        OnPropertyChanged(nameof(Headwear));
                        break;
                }
                ILoadModels = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
        public void SetMaterialToSelectedItem(Materials material, Color4 color)
        {
            SelectedItem.SetColor(color);
            SelectedItem.SetMaterial(material);
            switch (SelectedItem.Type)
            {
                case Type.Top:
                    if (Top == null) return;
                    Top.SetColor(color);
                    Top.SetMaterial(material);
                    OnPropertyChanged(nameof(Top));
                    break;
                case Type.Bottom:
                    if (Bottom == null) return;
                    Bottom.SetColor(color);
                    Bottom.SetMaterial(material);
                    OnPropertyChanged(nameof(Bottom));
                    break;
                case Type.Accessories:
                    if (Accessory == null) return;
                    Accessory.SetColor(color);
                    Accessory.SetMaterial(material);
                    OnPropertyChanged(nameof(Accessory));
                    break;
                case Type.Shoes:
                    if (Shoes == null) return;
                    Shoes.SetColor(color);
                    Shoes.SetMaterial(material);
                    OnPropertyChanged(nameof(Shoes));
                    break;
                case Type.Headwear:
                    if (Headwear == null) return;
                    Headwear.SetColor(color);
                    Headwear.SetMaterial(material);
                    OnPropertyChanged(nameof(Headwear));
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
            OnPropertyChanged();
        }
        public void ResetTop()
        {
            _top = null;
            OnPropertyChanged(nameof(Top));
        }
        public void ResetBottom()
        {
            _bottom = null;
            OnPropertyChanged(nameof(Bottom));
        }
        public void ResetShoes()
        {
            _shoes = null;
            OnPropertyChanged(nameof(Shoes));
        }
        public void ResetAccessory()
        {
            _accessory = null;
            OnPropertyChanged(nameof(Accessory));
        }
    }
}
