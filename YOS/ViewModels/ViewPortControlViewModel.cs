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
using YOS.Models.EnumParams;
namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        public ObservableElement3DCollection MannequinModel { get; } = new ObservableElement3DCollection();
        private SynchronizationContext context = SynchronizationContext.Current;
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


            var resourcePath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\";
            var reader = new ObjReader();
            //var models = reader.Read($"{resourcePath}Avatars\\Female\\Models\\APose.obj");
            var strPose = Poses.Idle.ToString();
            LoadObj($"{resourcePath}Mannequins\\Male\\{strPose}Pose.obj", MannequinModel);
            //LoadObj($"{resourcePath}Items\\Shorts\\{strPose}\\Shorts.obj", BottomModel);
            var curPath = $"{resourcePath}Items\\Bottom\\Shorts\\{strPose}\\";
            BottomModel.Add(CreateMesh($"{curPath}Shorts.obj",
                $"{curPath}Shorts_diffuse_1001.png",
                $"{curPath}Shorts_normal_1001.png"
                ));
        }
        private MeshGeometryModel3D CreateMesh(string meshPath,
            string diffusePath,
            string normalPath
            //string displacementPath,
            //string metallicRoughnessPath
            )
        {
            var reader = new ObjReader();
            var objCol = reader.Read(meshPath);
            var meshArray = new MeshGeometry3D[objCol.Count];
            for (int i = 0; i < meshArray.Length; i++)
            {
                meshArray[i] = new MeshGeometry3D();
            }
            var result = new MeshGeometryModel3D();
            for (int i = 0; i < objCol.Count; i++)
            {
                objCol[i].Geometry.AssignTo(meshArray[i]);
            }
            var m = MeshGeometry3D.Merge(meshArray);
            result.Geometry = m;
            result.Material = new PBRMaterial
            {
                AlbedoColor = new Color4(0.3f, 0.3f, 0.8f, 1f),
                AlbedoMap = TextureModel.Create(diffusePath),
                NormalMap = TextureModel.Create(normalPath),
                //RoughnessMetallicMap = TextureModel.Create(metallicRoughnessPath),
                //AmbientOcculsionMap = TextureModel.Create(metallicRoughnessPath),
                //DisplacementMap = TextureModel.Create(displacementPath),
                RenderShadowMap = true,
                RenderEnvironmentMap = true,
                RenderAlbedoMap = true,
                RenderDisplacementMap = true,
                RenderNormalMap = true,
                RenderAmbientOcclusionMap = true,
                EnableTessellation = false
            };
            return result;
        }
        public void LoadObj(string path, ObservableElement3DCollection elements)
        {
            var reader = new ObjReader();
            var objCol = reader.Read(path);
            AttachModelList(objCol, elements);

        }

        public void AttachModelList(List<Object3D> objs, ObservableElement3DCollection elements)
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
                    PhongMaterial phong = null;
                    if (ob.Material is PhongMaterialCore p)
                    {
                        phong = p.ConvertToPhongMaterial();
                        phong.RenderEnvironmentMap = true;
                        phong.RenderShadowMap = true;
                        phong.RenderSpecularColorMap = false;
                        phong.RenderDiffuseAlphaMap = false;
                        phong.EnableAutoTangent = true;
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
                            RenderAlbedoMap = true,
                            RenderDisplacementMap = true,
                            RenderNormalMap = true,
                            RenderAmbientOcclusionMap = true,
                            EnableTessellation = false

                        };
                    }

                    //if (ob.Transform != null && ob.Transform.Count > 0)
                    //{
                    //    s.Instances = ob.Transform;
                    //}
                    elements.Add(new MeshGeometryModel3D
                    {
                        Geometry = ob.Geometry,
                        CullMode = SharpDX.Direct3D11.CullMode.Back,
                        IsThrowingShadow = true,
                        Transform = scaleTransform,
                        Material = pbrMaterial,
                    }); ;


                }, null);
            }
        }
    }
}
