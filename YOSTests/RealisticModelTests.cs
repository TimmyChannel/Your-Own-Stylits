using YOS.Models.Mannequin;
using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using SharpDX;
using Media3D = System.Windows.Media.Media3D;

namespace YOSTests;

[TestClass]
public class RealisticModelTests
{
    IMannequinModel mannequinModel;
    ObservableElement3DCollection expectedMannequinModel;
    [TestInitialize]
    public void Init()
    {
        mannequinModel = new RealisticModel("Joe", true, GenderTypes.Male, Poses.Idle);
        expectedMannequinModel = LoadModelAndMaterials($"{AppDomain.CurrentDomain.BaseDirectory}TestResources\\MaleMannequin\\IdlePose.obj");
    }
    private ObservableElement3DCollection LoadModelAndMaterials(string modelPath)
    {
        ObservableElement3DCollection mannequin = new();
        var reader = new ObjReader();
        var objCol = reader.Read(modelPath);
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
            mannequin.Add(mesh);
        }
        return mannequin;
    }

    [TestMethod]
    public void MaterialLoadTest()
    {
        var expected = ((PBRMaterial)((MeshGeometryModel3D)expectedMannequinModel[0]).Material).AlbedoColor;
        var actual = ((PBRMaterial)((MeshGeometryModel3D)mannequinModel.Mannequin[0]).Material).AlbedoColor;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GeometryLoadTest()
    {
        var expected = ((MeshGeometryModel3D)expectedMannequinModel[0]).Geometry.Indices.Average();
        var actual = ((MeshGeometryModel3D)mannequinModel.Mannequin[0]).Geometry.Indices.Average();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void NameSetTest()
    {
        var expected = "Joe";
        var actual = mannequinModel.Name;
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void PoseSetTest()
    {
        var expected = Poses.Idle;
        var actual = mannequinModel.Pose;
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void GenderSetTest()
    {
        var expected = GenderTypes.Male;
        var actual = mannequinModel.Gender;
        Assert.AreEqual(expected, actual);
    }
}
