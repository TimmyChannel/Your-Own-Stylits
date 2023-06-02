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
        private void UpdateMannequinModel()
        {
            MannequinModel = MannequinSettings.Instance.MannequinModel;
            BottomModel.Clear();
            var item = MannequinSettings.Instance.Bottom;
            var mesh = new MeshGeometryModel3D();
            mesh.Geometry = item.Geometry;
            mesh.Material = item.Material;
            BottomModel.Add(mesh);
            OnAllPropertyChanged();
        }
        public ViewPortControlViewModel()
        {
            var vps = new ViewPortSettings();
            Light = vps.LightPresetList[0];
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(34, -9, -303),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(-35, 109, 304)
            };
            var man = MannequinSettings.Instance;
            MannequinModel = man.MannequinModel;
            MannequinModel.CollectionChanged += MannequinChanged;
            var item = man.Bottom;
            var mesh = new MeshGeometryModel3D();
            mesh.Geometry = item.Geometry;
            mesh.Material = item.Material;
            BottomModel.Add(mesh);
            ((MeshGeometryModel3D)BottomModel[0]).DepthBias = -1;
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
        }

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

    }
}
