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

namespace YOS.ViewModels
{
    class LookControllViewModel
    {
        Random random = new();
        private MannequinSettings _mannequinSettings = MannequinSettings.Instance;

        GenderTypes _g = GenderTypes.Male;
        Weather _w = Weather.Sunny;
        Styles _s = Styles.Formal;

        #region seks

        private string _selectedSexxo1 = "false";
        private string _selectedSexxo2 = "false";
        private string _selectedSexxo3 = "false";
        public string SelectedSexxo1
        {
            get => _selectedSexxo1;
            set { _selectedSexxo1 = value; _g = GenderTypes.Male; }
        }
        public string SelectedSexxo2
        {
            get => _selectedSexxo2;
            set { _selectedSexxo2 = value; _g = GenderTypes.Female; }
        }
        public string SelectedSexxo3
        {
            get => _selectedSexxo3;
            set { _selectedSexxo3 = value; _g = GenderTypes.Unisex; }
        }

        #endregion

        #region weather

        private string _selectedW1 = "false";
        private string _selectedW2 = "false";
        private string _selectedW3 = "false";
        public string SelectedW1
        {
            get => _selectedW1;
            set { _selectedW1 = value; _w = Weather.Indoor; }
        }
        public string SelectedW2
        {
            get => _selectedW2;
            set { _selectedW2 = value; _w = Weather.Cloudy; }
        }
        public string SelectedW3
        {
            get => _selectedW3;
            set { _selectedW3 = value; _w = Weather.Sunny; }
        }

        #endregion

        #region Stylek

        private string _selectedSty1 = "false";
        private string _selectedSty2 = "false";
        private string _selectedSty3 = "false";
        public string SelectedSty1
        {
            get => _selectedSty1;
            set { _selectedSty1 = value; _s = Styles.Casual; }
        }
        public string SelectedSty2
        {
            get => _selectedSty2;
            set { _selectedSty2 = value; _s = Styles.Summer; }
        }
        public string SelectedSty3
        {
            get => _selectedSty3;
            set { _selectedSty3 = value; _s = Styles.Formal; }
        }


        #endregion

        private ICommand? _generateLook;
        public ICommand GenerateLook => _generateLook ??= new RelayCommand(OnLookButtonPressed);
        private void OnLookButtonPressed()
        {
            List<Item> Wordrobe = ClosetItemList.SelectItems(_g, _w, _s);
            List<Item> Top = new();
            List<Item> Bottom = new();
            List<Item> Shoes = new();
            List<Item> Accessory = new();
            List<Item> HeadWear = new();

            _mannequinSettings.ResetItems();

            foreach (var itm in Wordrobe)
            {
                if (itm.Type == Models.Type.Top) 
                {
                    Top.Add(itm); 
                }
                if (itm.Type == Models.Type.Accessories)
                {
                    Accessory.Add(itm); 
                }
                if (itm.Type == Models.Type.Shoes)
                {
                    Shoes.Add(itm);
                }
                if (itm.Type == Models.Type.Bottom)
                {
                    Bottom.Add(itm);
                }
                if (itm.Type == Models.Type.Headwear)
                {
                    HeadWear.Add(itm);
                }
            }

            if (Top.Count > 0)
                _mannequinSettings.AddClosetItem(Top[random.Next(Top.Count)]);
            if (Accessory.Count > 0)
                _mannequinSettings.AddClosetItem(Accessory[random.Next(Accessory.Count)]);
            if (Bottom.Count > 0)
                _mannequinSettings.AddClosetItem(Bottom[random.Next(Bottom.Count)]);
            if (HeadWear.Count > 0)
                _mannequinSettings.AddClosetItem(HeadWear[random.Next(HeadWear.Count)]);
            if (Shoes.Count > 0)
                _mannequinSettings.AddClosetItem(Shoes[random.Next(Shoes.Count)]);
        }
    }
}
