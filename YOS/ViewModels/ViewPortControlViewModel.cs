using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelixToolkit.Wpf.SharpDX;
using Media3D = System.Windows.Media.Media3D;
using System.Windows.Media;
using SharpDX.Direct3D9;
using SharpDX;
using System.Windows.Media.Imaging;
using System.Drawing;
using YOS.Models;
using YOS.Models.EnumParams;
using System.Diagnostics;
using HelixToolkit.Wpf.SharpDX.Model;
using SharpDX.Toolkit.Graphics;
using SharpDX.Direct3D11;
using System.Threading;
using YOS.Models.Settings;
using YOS.Models.Mannequin;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;
using YOS.Models.Items;

namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : OnPropertyChangedClass
    {
        public MeshGeometryModel3D Target { set; get; }
        public ObservableElement3DCollection MannequinModel { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection BottomModel { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection TopModel { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection AccessoryModel { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection ShoesModel { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection HeadwearModel { get; private set; } = new ObservableElement3DCollection();
        public Camera Camera { private set; get; }
        public Light3DCollection Light { private set; get; }
        public bool MannequinIsVisible
        {
            get => MannequinSettings.Instance.MannequinIsVisible;
            set
            {
                MannequinSettings.Instance.MannequinIsVisible = value;
                OnPropertyChanged();
                Debug.WriteLine($"MannequinIsVisible changed to {value}");
            }
        }
        public GenderTypes MannequinGender
        {
            get => MannequinSettings.Instance.Gender;
            set
            {
                MannequinSettings.Instance.Gender = value;
                OnPropertyChanged();
                Debug.WriteLine($"MannequinGender changed to {value}");
                UpdateMannequinModel();
            }
        }
        private void UpdateLight()
        {
            Light = ViewPortSettings.Instance.CurrentLightPreset;
            Debug.WriteLine($"Light has changed to {Light.Tag}");
            OnAllPropertyChanged();
        }
        private void UpdateMannequinModel()
        {
            MannequinModel = MannequinSettings.Instance.MannequinModel;
            UpdateBottom();
            OnAllPropertyChanged();
        }
        public ViewPortControlViewModel()
        {
            Light = ViewPortSettings.Instance.CurrentLightPreset;
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(34, -9, -303),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(-35, 109, 304)
            };
            var man = MannequinSettings.Instance;
            MannequinModel = man.MannequinModel;
            BottomItem item = (BottomItem)man.Bottom;
            var mesh = new MeshGeometryModel3D();
            mesh.Geometry = item.Geometry;
            mesh.Material = item.Material;
            BottomModel.Add(mesh);
            item.PropertyChanged += Item_PropertyChanged;
            BottomModel[0].Mouse3DDown += ViewPortControlViewModel_Mouse3DDown;
            ((MeshGeometryModel3D)BottomModel[0]).DepthBias = -1;
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateBottom();
        }
        private void UpdateBottom()
        {
            BottomModel.Clear();
            var item = MannequinSettings.Instance.Bottom;
            if (item == null) return;
            var mesh = new MeshGeometryModel3D();
            mesh.Geometry = item.Geometry;
            mesh.Material = item.Material;
            BottomModel.Add(mesh);
            OnPropertyChanged(nameof(BottomModel));
        }
        private void ViewPortControlViewModel_Mouse3DDown(object? sender, MouseDown3DEventArgs e)
        {
            Debug.WriteLine("Disney");
        }

        public void OnMouseDown3DHandler(object sender, MouseDown3DEventArgs e)
        {
            if (e.HitTestResult?.ModelHit is MeshGeometryModel3D m)
            {
                if (BottomModel.Any() && m.Geometry == ((MeshGeometryModel3D)BottomModel.First()).Geometry)
                {
                    //Target = null;
                    //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
                    Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
                    MannequinSettings.Instance.SelectItemByType(Models.Type.Bottom);
                    Debug.WriteLine($"Hit to BottomModel");
                    return;
                }
                if (TopModel.Any() && m.Geometry == ((MeshGeometryModel3D)TopModel.First()).Geometry)
                {
                    //Target = null;
                    //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
                    Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
                    MannequinSettings.Instance.SelectItemByType(Models.Type.Top);
                    Debug.WriteLine($"Hit to TopModel");
                    return;
                }
                if (ShoesModel.Any() && m.Geometry == ((MeshGeometryModel3D)ShoesModel.First()).Geometry)
                {
                    //Target = null;
                    //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
                    Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
                    MannequinSettings.Instance.SelectItemByType(Models.Type.Shoes);
                    Debug.WriteLine($"Hit to ShoesModel");
                    return;
                }
                if (HeadwearModel.Any() && m.Geometry == ((MeshGeometryModel3D)HeadwearModel.First()).Geometry)
                {
                    //Target = null;
                    //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
                    Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
                    MannequinSettings.Instance.SelectItemByType(Models.Type.Headwear);
                    Debug.WriteLine($"Hit to HeadwearModel");
                    return;
                }
                if (AccessoryModel.Any() && m.Geometry == ((MeshGeometryModel3D)AccessoryModel.First()).Geometry)
                {
                    //Target = null;
                    //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
                    Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
                    MannequinSettings.Instance.SelectItemByType(Models.Type.Accessories);
                    Debug.WriteLine($"Hit to HeadwearModel");
                    return;
                }
            }
        }
        private void MannequinChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("Model changed");
            Debug.WriteLine(MannequinSettings.Instance.Gender);
            UpdateMannequinModel();
        }
        #region MannequinSimpleParams

        public void OnVisibilityChange()
        {
            if (MannequinIsVisible)
                MannequinIsVisible = false;
            else
                MannequinIsVisible = true;
        }
        private ICommand _changeMannequinVisibilityComm;
        public ICommand ChangeMannequinVisibilityComm => _changeMannequinVisibilityComm ??= new RelayCommand(OnVisibilityChange);
        public void OnGenderChange()
        {
            if (MannequinGender == GenderTypes.Male)
                MannequinGender = GenderTypes.Female;
            else
                MannequinGender = GenderTypes.Male;
        }
        private ICommand _changeMannequinGenderComm;
        public ICommand ChangeMannequinGenderComm => _changeMannequinGenderComm ??= new RelayCommand(OnGenderChange);
        #endregion
        #region PosePanelParams

        private bool _poseStackPanelIsVisible;
        public bool PoseStackPanelIsVisible
        {
            get => _poseStackPanelIsVisible;
            set => _poseStackPanelIsVisible = value;
        }

        public void OnOpenPosesPanel()
        {
            if (PoseStackPanelIsVisible)
                SetProperty(ref _poseStackPanelIsVisible, false, nameof(PoseStackPanelIsVisible));
            else
            {
                SetProperty(ref _lightStackPanelIsVisible, false, nameof(LightStackPanelIsVisible));
                SetProperty(ref _poseStackPanelIsVisible, true, nameof(PoseStackPanelIsVisible));
            }

        }
        private ICommand _openPosesPanelComm;
        public ICommand OpenPosesPanelComm => _openPosesPanelComm ??= new RelayCommand(OnOpenPosesPanel);

        public void OnPoseAChange()
        {
            if (MannequinSettings.Instance.Pose == Poses.A)
                return;
            else
            {
                MannequinSettings.Instance.Pose = Poses.A;
                UpdateMannequinModel();
            }
        }
        private ICommand _changeMannequinPoseToAComm;
        public ICommand ChangeMannequinPoseToAComm => _changeMannequinPoseToAComm ??= new RelayCommand(OnPoseAChange);
        public void OnPoseIdleChange()
        {
            if (MannequinSettings.Instance.Pose == Poses.Idle)
                return;
            else
            {
                MannequinSettings.Instance.Pose = Poses.Idle;
                UpdateMannequinModel();
            }
        }
        private ICommand _changeMannequinPoseToIdleComm;
        public ICommand ChangeMannequinPoseToIdleComm => _changeMannequinPoseToIdleComm ??= new RelayCommand(OnPoseIdleChange);
        public void OnPoseRunningChange()
        {
            if (MannequinSettings.Instance.Pose == Poses.Running)
                return;
            else
            {
                MannequinSettings.Instance.Pose = Poses.Running;
                UpdateMannequinModel();
            }
        }
        private ICommand _changeMannequinPoseToRunningComm;
        public ICommand ChangeMannequinPoseToRunningComm => _changeMannequinPoseToRunningComm ??= new RelayCommand(OnPoseRunningChange);
        #endregion
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
                SetProperty(ref _poseStackPanelIsVisible, false, nameof(PoseStackPanelIsVisible));
                SetProperty(ref _lightStackPanelIsVisible, true, nameof(LightStackPanelIsVisible));
            }

        }
        private ICommand _openLightsPanelComm;
        public ICommand OpenLightsPanelComm => _openLightsPanelComm ??= new RelayCommand(OnOpenLightsPanel);

        public void OnLightDayChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 0)
            {

                UpdateLight();
                return;
            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 0;
                OnPropertyChanged(nameof(Light));
                UpdateLight();
            }
        }
        private ICommand _changeviewPortLightToDayComm;
        public ICommand ChangeviewPortLightToDayComm => _changeviewPortLightToDayComm ??= new RelayCommand(OnLightDayChange);
        public void OnLightNightChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 1)
            {

                UpdateLight();
                return;

            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 1;
                OnPropertyChanged(nameof(Light));
                UpdateLight();
            }
        }
        private ICommand _changeviewPortLightToNightComm;
        public ICommand ChangeviewPortLightToNightComm => _changeviewPortLightToNightComm ??= new RelayCommand(OnLightNightChange);
        public void OnLight3PointChange()
        {
            if (ViewPortSettings.Instance.IndexOfCurrentLightPreset == 2)
            {

                UpdateLight();
                return;

            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 2;
                OnPropertyChanged(nameof(Light));
                UpdateLight();
            }
        }
        private ICommand _changeviewPortLightTo3PointComm;
        public ICommand ChangeviewPortLightTo3PointComm => _changeviewPortLightTo3PointComm ??= new RelayCommand(OnLight3PointChange);
        #endregion


    }
}
