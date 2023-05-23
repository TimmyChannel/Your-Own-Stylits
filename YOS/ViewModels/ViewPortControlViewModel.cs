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

namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        public Geometry3D Geometry { private set; get; }
        public PBRMaterial Material { private set; get; }
        public Camera Camera { private set; get; }
        public ViewPortControlViewModel()
        {
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(0, 0, -320),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(0, 0, 320)
            };
            var resourcePath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\";
            var reader = new ObjReader();
            
            var models = reader.Read($"{resourcePath}Tshirt.obj");
            Geometry = models[0].Geometry;
            for (int i = 0; i < Geometry.Positions.Count; i++)
            {
                Geometry.Positions[i] += new Vector3(0, -40, 0);
            }
            var tshirtuv = new UVTransform();
            tshirtuv.Scaling = new Vector2(10F, 10F);
            var material = new PBRMaterial
            {
                RenderAlbedoMap = true,
                RenderAmbientOcclusionMap = true,
                RenderNormalMap = true,
                AmbientOcculsionMap = TextureModel.Create($"{resourcePath}Textures\\GrayFabricTex\\ambientocclusion.png"),
                NormalMap = TextureModel.Create($"{resourcePath}Textures\\GrayFabricTex\\normal.png"),
                RoughnessMetallicMap = TextureModel.Create($"{resourcePath}Textures\\GrayFabricTex\\roughness.png"),
                AlbedoMap = TextureModel.Create($"{resourcePath}Textures\\GrayFabricTex\\Tshirt.png"),
                RoughnessFactor = 0.2f,
                AlbedoColor = new Color4(1, 0.1f, 0.2f, 1)
            };
            Material = material;
        }
    }
}
