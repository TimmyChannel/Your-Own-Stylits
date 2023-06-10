using CommunityToolkit.Mvvm.Input;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YOS.Models.Settings;
using YOS.Models;
namespace YOS.ViewModels
{
    public class EnvironmentPanelControlViewModel : OnPropertyChangedClass
    {
        public bool GroundIsVisible => ViewPortSettings.Instance.GroundIsVisible;

        #region LightPanelParams

        private bool _lightStackPanelIsVisible;
        public bool LightStackPanelIsVisible
        {
            get => _lightStackPanelIsVisible;
            set => _lightStackPanelIsVisible = value;
        }

        public void OnOpenLightsPanel()
        {
            if (LightStackPanelIsVisible)
                SetProperty(ref _lightStackPanelIsVisible, false, nameof(LightStackPanelIsVisible));
            else
            {
                SetProperty(ref _lightStackPanelIsVisible, true, nameof(LightStackPanelIsVisible));
            }

        }
        private ICommand _openLightsPanelComm;
        public ICommand OpenLightsPanelComm => _openLightsPanelComm ??= new RelayCommand(OnOpenLightsPanel);

        public void OnLightDayChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 0)
            {
                return;
            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 0;
                ViewPortSettings.Instance.IndexOfCurrentEnvironmentMap = 0;
                ViewPortSettings.Instance.IndexOfCurrentFloorTexture = 0;
                Debug.WriteLine($"Enviroment Preset was changed to Day");
            }
        }
        private ICommand _changeviewPortLightToDayComm;
        public ICommand ChangeviewPortLightToDayComm => _changeviewPortLightToDayComm ??= new RelayCommand(OnLightDayChange);
        public void OnLightNightChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 1)
            {
                return;
            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 1;
                ViewPortSettings.Instance.IndexOfCurrentEnvironmentMap = 1;
                ViewPortSettings.Instance.IndexOfCurrentFloorTexture = 1;
                Debug.WriteLine($"Enviroment Preset was changed to Night");
            }
        }
        private ICommand _changeviewPortLightToNightComm;
        public ICommand ChangeviewPortLightToNightComm => _changeviewPortLightToNightComm ??= new RelayCommand(OnLightNightChange);
        public void OnLight3PointChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 2)
            {
                return;
            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 2;
                ViewPortSettings.Instance.IndexOfCurrentEnvironmentMap = 2;
                ViewPortSettings.Instance.IndexOfCurrentFloorTexture = 2;
                Debug.WriteLine($"Enviroment Preset was changed to Studio");
            }
        }
        private ICommand _changeviewPortLightTo3PointComm;
        public ICommand ChangeviewPortLightTo3PointComm => _changeviewPortLightTo3PointComm ??= new RelayCommand(OnLight3PointChange);
        #endregion
        #region GroundParams
        public void OnChangeGroundVisibility()
        {
            if (ViewPortSettings.Instance.GroundIsVisible)
                ViewPortSettings.Instance.GroundIsVisible = false;
            else
                ViewPortSettings.Instance.GroundIsVisible = true;
            OnPropertyChanged(nameof(GroundIsVisible));
            Debug.WriteLine("Ground visibility was changed");
        }
        private ICommand _changeGroundVisibilityComm;
        public ICommand ChangeGroundVisibilityComm => _changeGroundVisibilityComm ??= new RelayCommand(OnChangeGroundVisibility);
        #endregion

    }
}
