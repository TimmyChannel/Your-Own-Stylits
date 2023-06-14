using YOS.Models.Creators;
using YOS.Models.Items;
using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using SharpDX;
using Media3D = System.Windows.Media.Media3D;

namespace YOSTests;

[TestClass]
public class BottomCreatorTests
{
    CloseItemCreator creator;
    IClosetItemModel closetItem;
    [TestInitialize]
    public void Init()
    {
        creator = new BottomCreator();
        creator.SetParams(Poses.Idle, GenderTypes.Male);
        closetItem = creator.CreateClosetItem("Shorts");
    }
    [TestMethod]
    public void GeometryLoadTest()
    {
        var reader = new ObjReader();
        var objCol = reader.Read($"{AppDomain.CurrentDomain.BaseDirectory}TestResources\\BottomModel\\Shorts.obj");
        var meshArray = new MeshGeometry3D[objCol.Count];
        for (int i = 0; i < objCol.Count; i++)
        {
            meshArray[i] = new MeshGeometry3D();
            objCol[i].Geometry.AssignTo(meshArray[i]);
        }
        var expected = MeshGeometry3D.Merge(meshArray).Indices.Average();
        var actual = creator.CreateClosetItem("Shorts").Geometry.Indices.Average();
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void SetParamsTest()
    {
        var expected = Poses.Running;
        creator.SetParams(Poses.Running);
        var actual = creator.Pose;
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void InitializationTest()
    {
        var expected = false;
        creator.SetParams(Poses.A, GenderTypes.Unisex);
        closetItem = creator.CreateClosetItem("Shorts");
        var actual = closetItem.IInitialized;
        Assert.AreEqual(expected, actual);
    }
}

