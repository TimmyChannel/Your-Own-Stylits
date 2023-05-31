using HelixToolkit.Wpf.SharpDX;
using Media3D = System.Windows.Media.Media3D;
using SharpDX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace YOS.Models.Mannequin
{
    public class RealisticModel : IMannequinModel
    {
        public string Name { get; init; }
        bool _isVisible;
        Poses _pose;
        GenderTypes _gender;
        private string _mainPath;
        private string _modelPath;
        private ObservableElement3DCollection _mannequin;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible)
                    foreach (var item in _mannequin)
                    {
                        item.Visibility = System.Windows.Visibility.Hidden;
                    }
                else
                    foreach (var item in _mannequin)
                    {
                        item.Visibility = System.Windows.Visibility.Visible;
                    }
                _isVisible = value;
            }
        }
        public Poses Pose
        {
            get => _pose;
            set
            {
                _pose = value;
                UpdatePaths();
                LoadModelAndMaterials();
            }
        }
        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                UpdatePaths();
                LoadModelAndMaterials();
            }
        }

        public ObservableElement3DCollection Mannequin => _mannequin;

        public RealisticModel(string name, bool visible, GenderTypes gender = GenderTypes.Male, Poses pose = Poses.A)
        {
            Name = name;
            _gender = gender;
            _pose = pose;
            _isVisible = visible;
            _mannequin = new ObservableElement3DCollection();
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Mannequins\\{_gender}";
            _modelPath = Directory.GetFiles(_mainPath, "*" + Pose + "*.obj")[0];

            LoadModelAndMaterials();
        }
        private void UpdatePaths()
        {
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Mannequins\\{_gender}";
            _modelPath = Directory.GetFiles(_mainPath, "*" + Pose + "*.obj")[0];
        }
        private void LoadModelAndMaterials()
        {
            var reader = new ObjReader();
            var objCol = reader.Read(_modelPath);
            for (int i = 0; i < objCol.Count; ++i)
            {
                var ob = objCol[i];
                var vertColor = new Color4((float)i / objCol.Count, 0, 1 - (float)i / objCol.Count, 1);
                ob.Geometry.Colors = new Color4Collection(Enumerable.Repeat(vertColor, ob.Geometry.Positions.Count));
                ob.Geometry.UpdateOctree();
                ob.Geometry.UpdateBounds();

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
                var mesh = new MeshGeometryModel3D
                {
                    Geometry = ob.Geometry,
                    CullMode = SharpDX.Direct3D11.CullMode.Back,
                    IsThrowingShadow = true,
                    Transform = scaleTransform,
                    Material = pbrMaterial,
                    DepthBias = i << 2
                };
                _mannequin.Add(mesh);
            }
            //ChangeDepthBias();
        }
        private void ChangeDepthBias()
        {
            for (int i = 0; i < _mannequin.Count; i++)
            {
                var cur = (MeshGeometryModel3D)_mannequin[i];
                cur.DepthBias = i;
            }
        }

    }
}
