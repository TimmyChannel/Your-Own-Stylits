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
        public Light3DCollection Light => ViewPortSettings.Instance.CurrentLightPreset;
        public TextureModel EnvironmentMap => ViewPortSettings.Instance.CurrentEnvironmentMap;
        public bool GroundIsVisible => ViewPortSettings.Instance.GroundIsVisible;

        public Camera Camera { init; get; }
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
            Camera = ViewPortSettings.Instance.Camera;
            MannequinSettings.Instance.PropertyChanged += MannequinSettings_PropertyChanged;
            ViewPortSettings.Instance.PropertyChanged += ViewPortSettings_PropertyChanged;
            UpdateAllItems();
            var builder = new MeshBuilder(true, true, true);
            builder.AddBox(new Vector3(0.0f, 0f, 0.0f), 800, 1, 800, BoxFaces.All);
            Floor = builder.ToMeshGeometry3D();
            FloorMaterial.UVTransform = new UVTransform()
            { Scaling = new Vector2(10, 10) };
        }

        private void ViewPortSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewPortSettings.Instance.CurrentEnvironmentMap):
                    OnPropertyChanged(nameof(EnvironmentMap));
                    return;
                case nameof(ViewPortSettings.Instance.CurrentFloorTexture):
                    OnPropertyChanged(nameof(FloorMaterial));
                    return;
                case nameof(ViewPortSettings.Instance.CurrentLightPreset):
                    OnPropertyChanged(nameof(Light));
                    return;
                case nameof(ViewPortSettings.Instance.GroundIsVisible):
                    OnPropertyChanged(nameof(GroundIsVisible));
                    return;
                default:
                    break;
            }
        }

        private void MannequinSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MannequinSettings.Instance.Accessory):
                    UpdateAccessory();
                    OnPropertyChanged(nameof(Accessory));
                    return;
                case nameof(MannequinSettings.Instance.Bottom):
                    UpdateBottom();
                    OnPropertyChanged(nameof(Bottom));
                    return;
                case nameof(MannequinSettings.Instance.Shoes):
                    UpdateShoes();
                    OnPropertyChanged(nameof(Shoes));
                    return;
                case nameof(MannequinSettings.Instance.Top):
                    UpdateTop();
                    OnPropertyChanged(nameof(Top));
                    return;
                default:
                    UpdateAllItems();
                    return;
            }
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
            Debug.WriteLine("Property " + e.PropertyName + " changed");
        }
        private void Top_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateTop();
            Debug.WriteLine("Property " + e.PropertyName + " changed");
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
            Debug.WriteLine("Property " + e.PropertyName + " changed");
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
            Debug.WriteLine("Property " + e.PropertyName + " changed");
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
                Debug.WriteLine($"Mannequin pose was changed to {MannequinSettings.Instance.Pose}");
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
                Debug.WriteLine($"Mannequin pose was changed to {MannequinSettings.Instance.Pose}");
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
                Debug.WriteLine($"Mannequin pose was changed to {MannequinSettings.Instance.Pose}");
            }
        }
        private ICommand _changeMannequinPoseToRunningComm;
        public ICommand ChangeMannequinPoseToRunningComm => _changeMannequinPoseToRunningComm ??= new RelayCommand(OnPoseRunningChange);
        #endregion

    }
}
