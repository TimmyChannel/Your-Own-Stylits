using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using YOS.Models.EnumParams;
using YOS.Models.Mannequin;
using SharpDX.Direct3D11;
using System.Windows;
using YOS.Views;
using YOS.Models.Items;
using YOS.Models.Algorithm;
using System.Reflection;
using System.ComponentModel;
using YOS.Models;

namespace YOS.ViewModels
{
    class LookControllViewModel : OnPropertyChangedClass
    {
        GenderTypes _g = GenderTypes.Male;
        Weather _w = Weather.Sunny;
        Styles _s = Styles.Formal;

        AlgorytmForWear strategy = new SelectClozzes();
        MannequinSettings mainMonnequen = MannequinSettings.Instance;
        static GenderTypes monnequin_G = MannequinSettings.Instance.Gender;

        #region Comboboxes

        private string _selectedItemTop;
        private string _selectedItemBottom;
        private string _selectedItemShoe;
        private string _selectedItemAccesory;
        private List<string> _selectedItemToplist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Top);
        private List<string> _selectedItemBottomlist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Bottom);
        private List<string> _selectedItemShoelist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Shoes);
        private List<string> _selectedItemAccesorylist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Accessories);

        public string SelectedTop
        {
            get => _selectedItemTop;
            set
            {
                _selectedItemTop = value;
                if (value == "Нет")
                    mainMonnequen.ResetTop();
                else
                    mainMonnequen.AddClosetItem(value, Models.Type.Top);
            }
        }
        public string SelectedBottom
        {
            get => _selectedItemBottom;
            set
            {
                _selectedItemTop = value;
                if (value == "Нет")
                    mainMonnequen.ResetBottom();
                else
                    mainMonnequen.AddClosetItem(value, Models.Type.Bottom);
            }
        }
        public string SelectedShoe
        {
            get => _selectedItemShoe;
            set
            {
                _selectedItemShoe = value;
                if (value == "Нет")
                    mainMonnequen.ResetShoes();
                else
                    mainMonnequen.AddClosetItem(value, Models.Type.Shoes);
            }
        }
        public string SelectedAccesory
        {
            get => _selectedItemAccesory;
            set
            {
                _selectedItemTop = value;
                if (value == "Нет")
                    mainMonnequen.ResetAccessory();
                else
                    mainMonnequen.AddClosetItem(value, Models.Type.Accessories);
            }
        }

        public List<string> Tops
        {
            get
            {
                return _selectedItemToplist;
            }
        }
        public List<string> Bottoms
        {
            get
            {
                return _selectedItemBottomlist;
            }
        }
        public List<string> Shues
        {
            get
            {
                return _selectedItemShoelist;
            }
        }
        public List<string> Accessory
        {
            get
            {
                return _selectedItemAccesorylist;
            }
        }

        #endregion

        #region seks

        public string SelectedSexxo2
        {
            set { _g = GenderTypes.Female; }
        }
        public string SelectedSexxo3
        {
            set { _g = GenderTypes.Unisex; }
        }
        public string SelectedSexxo1
        {
            set { _g = GenderTypes.Male; }
        }

        #endregion

        #region weather
        public string SelectedW1
        {
            set { _w = Weather.Indoor; }
        }
        public string SelectedW2
        {
            set { _w = Weather.Cloudy; }
        }
        public string SelectedW3
        {
            set { _w = Weather.Sunny; }
        }

        #endregion

        #region Stylek


        public string SelectedSty1
        {
            set { _s = Styles.Casual; }
        }
        public string SelectedSty2
        {
            set { _s = Styles.Summer; }
        }
        public string SelectedSty3
        {
            set { _s = Styles.Formal; }
        }


        #endregion

        #region Command

        private ICommand? _generateLook;
        public ICommand GenerateLook => _generateLook ??= new RelayCommand(OnLookButtonPressed);
        private void OnLookButtonPressed()
        {
            strategy.WearMe(_g, _w, _s);
        }

        #endregion

        public LookControllViewModel()
        {
            mainMonnequen.PropertyChanged += _mannequinPropertyChanged;
        }

        private void _mannequinPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MannequinSettings.Instance.Gender)) return;
            _selectedItemTop = null;
            _selectedItemBottom = null;
            _selectedItemShoe = null;
            _selectedItemAccesory = null;
            monnequin_G = mainMonnequen.Gender;
            _selectedItemToplist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Top);
            _selectedItemBottomlist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Bottom);
            _selectedItemShoelist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Shoes);
            _selectedItemAccesorylist = ClosetItemList.SelectItems(monnequin_G, Models.Type.Accessories);
            OnAllPropertyChanged();
        }
    }
}
