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
namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        public ObservableElement3DCollection MannequinModel { get; } = new ObservableElement3DCollection();
        private SynchronizationContext context = SynchronizationContext.Current;

        public Geometry3D Geometry { private set; get; }
        public PBRMaterial Material { private set; get; }
        public Camera Camera { private set; get; }
        public Light3DCollection Light { private set; get; }
        public ViewPortControlViewModel()
        {
            var vps = new ViewPortSettings();
            Light = vps.CurrentLightPreset;
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(0, 0, -320),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(0, 0, 320)
            };
            var resourcePath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\";
            var reader = new ObjReader();
            //var models = reader.Read($"{resourcePath}Avatars\\Female\\Models\\APose.obj");
            LoadObj($"{resourcePath}Avatars\\Female\\Models\\APose.obj");
        }
        public void LoadObj(string path)
        {
            var reader = new ObjReader();
            var objCol = reader.Read(path);
            AttachModelList(objCol);
        }

        public void AttachModelList(List<Object3D> objs)
        {
            for (int i = 0; i < objs.Count; ++i)
            {
                var ob = objs[i];
                var vertColor = new Color4((float)i / objs.Count, 0, 1 - (float)i / objs.Count, 1);
                ob.Geometry.Colors = new Color4Collection(Enumerable.Repeat(vertColor, ob.Geometry.Positions.Count));
                ob.Geometry.UpdateOctree();
                ob.Geometry.UpdateBounds();

                context.Post((o) =>
                {
                    var scaleTransform = new Media3D.ScaleTransform3D(1, 1, 1);
                    var s = new MeshGeometryModel3D
                    {
                        Geometry = ob.Geometry,
                        CullMode = SharpDX.Direct3D11.CullMode.Back,
                        IsThrowingShadow = true,
                        Transform = scaleTransform,
                    };

                    var diffuseMaterial = new DiffuseMaterial();
                    PBRMaterial pbrMaterial = null;
                    if (ob.Material is PhongMaterialCore p)
                    {
                        var phong = p.ConvertToPhongMaterial();
                        phong.RenderEnvironmentMap = true;
                        phong.RenderShadowMap = true;
                        phong.RenderSpecularColorMap = false;
                        s.Material = phong;
                        diffuseMaterial.DiffuseColor = p.DiffuseColor;
                        diffuseMaterial.DiffuseMap = p.DiffuseMap;
                        pbrMaterial = new PBRMaterial()
                        {
                            AlbedoColor = p.DiffuseColor,
                            AlbedoMap = p.DiffuseMap,
                            NormalMap = p.NormalMap,
                            RoughnessMetallicMap = p.SpecularColorMap,
                            AmbientOcculsionMap = p.SpecularColorMap,
                            RenderShadowMap = true,
                            RenderEnvironmentMap = true,
                            MetallicFactor = 1, // Set to 1 if using RMA Map
                            RoughnessFactor = 1 // Set to 1 if using RMA Map
                        };
                    }

                    //if (ob.Transform != null && ob.Transform.Count > 0)
                    //{
                    //    s.Instances = ob.Transform;
                    //}
                    this.MannequinModel.Add(new MeshGeometryModel3D
                    {
                        Geometry = ob.Geometry,
                        CullMode = SharpDX.Direct3D11.CullMode.Back,
                        IsThrowingShadow = true,
                        Transform = scaleTransform,
                        Material = pbrMaterial
                    }); ;
                    

                }, null);
            }
        }
    }
}
