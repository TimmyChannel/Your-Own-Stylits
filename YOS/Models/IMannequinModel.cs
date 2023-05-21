using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;
namespace YOS.Models
{
    public interface IMannequinModel
    {
        string Name { get; }
        bool IsVisible { get; }
        Poses Pose { get; }
        Somatotypes Somatotype { get; }
        PBRMaterial DefaultMaterial { get; }
        Geometry3D Geometry { get; }
    }
}
