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

namespace YOS.ViewModels
{
    class LookControllViewModel
    {
        GenderTypes _g = GenderTypes.Male;
        Weather _w = Weather.Sunny;
        Styles _s = Styles.Formal;

        AlgorytmForWear strategy = new SelectClozzes();

        #region seks

        public string SelectedSexxo1
        {
            set { _g = GenderTypes.Male; }
        }
        public string SelectedSexxo2
        {
            set { _g = GenderTypes.Female; }
        }
        public string SelectedSexxo3
        {
            set { _g = GenderTypes.Unisex; }
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

        private ICommand? _generateLook;
        public ICommand GenerateLook => _generateLook ??= new RelayCommand(OnLookButtonPressed);
        private void OnLookButtonPressed()
        {
            strategy.WearMe(_g, _w, _s);
        }
    }
}
