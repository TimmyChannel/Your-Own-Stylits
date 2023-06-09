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
using Haley.Models;

namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : OnPropertyChangedClass
    {
        public MeshGeometryModel3D Target { set; get; }
        public ObservableElement3DCollection MannequinModel => MannequinSettings.Instance.MannequinModel;
        public ObservableElement3DCollection Bottom { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection Top { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection Accessory { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection Shoes { get; private set; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection Headwear { get; private set; } = new ObservableElement3DCollection();
        public MeshGeometry3D Floor { get; private set; }
        public PBRMaterial FloorMaterial => ViewPortSettings.Instance.CurrentFloorTexture;
        public Camera Camera { init; get; }
        public Light3DCollection Light => ViewPortSettings.Instance.CurrentLightPreset;
        public TextureModel EnvironmentMap => ViewPortSettings.Instance.CurrentEnvironmentMap;
        public bool GroundIsVisible => ViewPortSettings.Instance.GroundIsVisible;
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
                OnAllPropertyChanged();
                UpdateAllItems();
                Debug.WriteLine($"MannequinGender changed to {value}");
            }
        }

        public ViewPortControlViewModel()
        {
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(34, -9, -303),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(-35, 109, 304)
            };
            MannequinSettings.Instance.AddClosetItem(ClosetItemList.GetItem("Trousers"));
            MannequinSettings.Instance.AddClosetItem(ClosetItemList.GetItem("Tshirt"));
            MannequinSettings.Instance.AddClosetItem(ClosetItemList.GetItem("RoundGlasses"));
            MannequinSettings.Instance.AddClosetItem(ClosetItemList.GetItem("Sneakers"));
            MannequinSettings.Instance.PropertyChanged += MannequinSettings_PropertyChanged;
            UpdateAllItems();
            ViewPortSettings.Instance.IndexOfCurrentEnvironmentMap = 0;
            ViewPortSettings.Instance.IndexOfCurrentFloorTexture = 0;
            ViewPortSettings.Instance.IndexOfCurrentLightPreset = 0;
            var b2 = new MeshBuilder(true, true, true);
            //b2.AddCylinder(new Vector3(0.0f, -100f, 0.0f), new Vector3(0.0f, 0f, 0.0f), 400);
            b2.AddBox(new Vector3(0.0f, 0f, 0.0f), 800, 1, 800, BoxFaces.All);
            Floor = b2.ToMeshGeometry3D();
            FloorMaterial.UVTransform = new UVTransform()
            { Scaling = new Vector2(10, 10) };
        }

        private void MannequinSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
            Debug.WriteLine("Property " + e.PropertyName + " changed");
            UpdateAllItems();
        }
        private void UpdateAllItems()
        {
            UpdateBottom();
            UpdateTop();
            UpdateAccessory();
            UpdateShoes();
        }
        private void Bottom_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateBottom();
        }
        private void Top_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateTop();
        }
        private void UpdateBottom()
        {
            Bottom.Clear();
            BottomItem item = (BottomItem)MannequinSettings.Instance.Bottom;
            if (item != null)
            {
                var mesh = new MeshGeometryModel3D
                {
                    Geometry = item.Geometry,
                    Material = item.Material,
                    IsThrowingShadow = true
                };
                Bottom.Add(mesh);
                item.PropertyChanged += Bottom_PropertyChanged;
                Bottom[0].Mouse3DDown += Bottom_Mouse3DDown;
                ((MeshGeometryModel3D)Bottom[0]).DepthBias = -1;
            }
            OnPropertyChanged(nameof(Bottom));
        }    
        private void Accessory_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateAccessory();
        }
        private void UpdateAccessory()
        {
            Accessory.Clear();
            AccessoriesItem item = (AccessoriesItem)MannequinSettings.Instance.Accessory;
            if (item != null)
            {
                var mesh = new MeshGeometryModel3D
                {
                    Geometry = item.Geometry,
                    Material = item.Material,
                    IsThrowingShadow = true
                };
                Accessory.Add(mesh);
                item.PropertyChanged += Accessory_PropertyChanged;
                Accessory[0].Mouse3DDown += Accessory_Mouse3DDown;
                ((MeshGeometryModel3D)Accessory[0]).DepthBias = -1;
            }
            OnPropertyChanged(nameof(Accessory));
        }
        private void Shoes_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateShoes();
        }
        private void UpdateShoes()
        {
            Shoes.Clear();
            ShoesItem item = (ShoesItem)MannequinSettings.Instance.Shoes;
            if (item != null)
            {
                var mesh = new MeshGeometryModel3D
                {
                    Geometry = item.Geometry,
                    Material = item.Material,
                    IsThrowingShadow = true
                };
                Shoes.Add(mesh);
                item.PropertyChanged += Shoes_PropertyChanged;
                Shoes[0].Mouse3DDown += Shoes_Mouse3DDown;
                ((MeshGeometryModel3D)Shoes[0]).DepthBias = 0;
            }
            OnPropertyChanged(nameof(Shoes));
        }
        private void UpdateTop()
        {
            Top.Clear();
            TopItem item = (TopItem)MannequinSettings.Instance.Top;
            if (item != null)
            {
                var mesh = new MeshGeometryModel3D
                {
                    Geometry = item.Geometry,
                    Material = item.Material,
                    IsThrowingShadow = true
                };
                Top.Add(mesh);
                item.PropertyChanged += Top_PropertyChanged;
                Top[0].Mouse3DDown += Top_Mouse3DDown;
                ((MeshGeometryModel3D)Top[0]).DepthBias = -2;
            }
            OnPropertyChanged(nameof(Top));
        }
        private void Bottom_Mouse3DDown(object? sender, MouseDown3DEventArgs e)
        {
            MannequinSettings.Instance.SelectItemByType(Models.Type.Bottom);
            Debug.WriteLine($"Hit to Bottom");
        }
        private void Top_Mouse3DDown(object? sender, MouseDown3DEventArgs e)
        {
            MannequinSettings.Instance.SelectItemByType(Models.Type.Top);
            Debug.WriteLine($"Hit to Top");
        }    
        private void Accessory_Mouse3DDown(object? sender, MouseDown3DEventArgs e)
        {
            MannequinSettings.Instance.SelectItemByType(Models.Type.Accessories);
            Debug.WriteLine($"Hit to Accessory");
        }
        private void Shoes_Mouse3DDown(object? sender, MouseDown3DEventArgs e)
        {
            MannequinSettings.Instance.SelectItemByType(Models.Type.Shoes);
            Debug.WriteLine($"Hit to Shoes");
        }

        //public void OnMouseDown3DHandler(object sender, MouseDown3DEventArgs e)
        //{
        //    if (e.HitTestResult?.ModelHit is MeshGeometryModel3D m)
        //    {
        //        if (Bottom.Any() && m.Geometry == ((MeshGeometryModel3D)Bottom.First()).Geometry)
        //        {
        //            //Target = null;
        //            //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
        //            Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
        //            MannequinSettings.Instance.SelectItemByType(Models.Type.Bottom);
        //            Debug.WriteLine($"Hit to Bottom");
        //            return;
        //        }
        //        if (Top.Any() && m.Geometry == ((MeshGeometryModel3D)Top.First()).Geometry)
        //        {
        //            //Target = null;
        //            //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
        //            Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
        //            MannequinSettings.Instance.SelectItemByType(Models.Type.Top);
        //            Debug.WriteLine($"Hit to Top");
        //            return;
        //        }
        //        if (ShoesModel.Any() && m.Geometry == ((MeshGeometryModel3D)ShoesModel.First()).Geometry)
        //        {
        //            //Target = null;
        //            //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
        //            Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
        //            MannequinSettings.Instance.SelectItemByType(Models.Type.Shoes);
        //            Debug.WriteLine($"Hit to ShoesModel");
        //            return;
        //        }
        //        if (HeadwearModel.Any() && m.Geometry == ((MeshGeometryModel3D)HeadwearModel.First()).Geometry)
        //        {
        //            //Target = null;
        //            //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
        //            Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
        //            MannequinSettings.Instance.SelectItemByType(Models.Type.Headwear);
        //            Debug.WriteLine($"Hit to HeadwearModel");
        //            return;
        //        }
        //        if (AccessoryModel.Any() && m.Geometry == ((MeshGeometryModel3D)AccessoryModel.First()).Geometry)
        //        {
        //            //Target = null;
        //            //CenterOffset = m.Geometry.Bound.Center; // Must update this before updating target
        //            Target = e.HitTestResult.ModelHit as MeshGeometryModel3D;
        //            MannequinSettings.Instance.SelectItemByType(Models.Type.Accessories);
        //            Debug.WriteLine($"Hit to HeadwearModel");
        //            return;
        //        }
        //    }
        //}

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
                UpdateAllItems();
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
                UpdateAllItems();
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
                UpdateAllItems();
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
                return;
            }
            else
            {
                ViewPortSettings.Instance.IndexOfCurrentLightPreset = 0;
                ViewPortSettings.Instance.IndexOfCurrentEnvironmentMap = 0;
                ViewPortSettings.Instance.IndexOfCurrentFloorTexture = 0;
                OnPropertyChanged(nameof(Light));
                OnPropertyChanged(nameof(EnvironmentMap));
                OnPropertyChanged(nameof(FloorMaterial));
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
                OnPropertyChanged(nameof(Light));
                OnPropertyChanged(nameof(EnvironmentMap));
                OnPropertyChanged(nameof(FloorMaterial));
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
                OnPropertyChanged(nameof(Light));
                OnPropertyChanged(nameof(EnvironmentMap));
                OnPropertyChanged(nameof(FloorMaterial));
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

        }
        private ICommand _changeGroundVisibilityComm;
        public ICommand ChangeGroundVisibilityComm => _changeGroundVisibilityComm ??= new RelayCommand(OnChangeGroundVisibility);
        #endregion

    }
}
