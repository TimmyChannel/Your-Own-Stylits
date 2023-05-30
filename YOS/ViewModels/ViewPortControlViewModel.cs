﻿using System;
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
namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        public ObservableElement3DCollection MannequinModel { get; } = new ObservableElement3DCollection();
        public ObservableElement3DCollection BottomModel { get; } = new ObservableElement3DCollection();
        public Camera Camera { private set; get; }
        public Light3DCollection Light { private set; get; }

        public ViewPortControlViewModel()
        {
            var vps = new ViewPortSettings();
            Light = vps.LightPresetList[0];
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(0, 0, -320),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(0, 0, 320)
            };


            var man = MannequinSettings.Instance;
            MannequinModel = man.Mannequin.Mannequin;
            var item = man.Bottom;
            var mesh = new MeshGeometryModel3D();
            mesh.Geometry = item.Geometry;
            mesh.Material = item.Material;
            BottomModel.Add(mesh);
            ((MeshGeometryModel3D)BottomModel[0]).DepthBias = -1;
        }
    }
}
