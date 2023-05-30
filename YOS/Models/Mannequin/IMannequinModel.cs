using YOS.Models.EnumParams;
using HelixToolkit.Wpf.SharpDX;

namespace YOS.Models.Mannequin
{
    public interface IMannequinModel
    {
        string Name { get; }
        bool IsVisible { get; set; }
        Poses Pose { get; set; }
        GenderTypes Gender { get; set; }
        ObservableElement3DCollection Mannequin { get; }
    }
}
