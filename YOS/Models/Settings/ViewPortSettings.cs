using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Media3D = System.Windows.Media.Media3D;
using System.Windows.Media;
using HelixToolkit.Wpf.SharpDX;
using YOS.Models.Mannequin;
using System.Windows.Input;

namespace YOS.Models.Settings
{
    public class ViewPortSettings
    {
        private byte _indexOfCurrentLightPreset;
        private MannequinSettings _mannequinSettings;
        private bool _groundIsVisible;
        private List<Light3DCollection> _lightPresets;
        public Light3DCollection CurrentLightPreset
        {
            get => _lightPresets[_indexOfCurrentLightPreset];
        }
        public List<Light3DCollection> LightPresetList
        {
            get => _lightPresets;
        }
        public Viewport3DX Viewport { get; private set; }
        public EnvironmentMap3D Environment { get; init; }
        public bool GroundIsVisible
        {
            get => _groundIsVisible;
            set => _groundIsVisible = value;
        }
        public ViewPortSettings()
        {
            InitLightPresets();
            InitViewport();
        }
        private void InitViewport()
        {
            Viewport = new Viewport3DX()
            {
                Name = "Viewport",
                ShowViewCube = false,
                FXAALevel = FXAALevel.Ultra,
                MSAA = HelixToolkit.Wpf.SharpDX.MSAALevel.Two,
                ShowFrameRate = true,
                Camera = new PerspectiveCamera
                {
                    LookDirection = new Media3D.Vector3D(0, 0, -320),
                    UpDirection = new Media3D.Vector3D(0, 1, 0),
                    Position = new Media3D.Point3D(0, 0, 320)
                },
                PinchZoomAtCenter = true,
                EffectsManager = new DefaultEffectsManager()
            };
            Viewport.InputBindings.Clear();
            Viewport.InputBindings.Add(new MouseBinding(ViewportCommands.Rotate, new MouseGesture(MouseAction.RightClick)));
            Viewport.InputBindings.Add(new MouseBinding(ViewportCommands.Pan, new MouseGesture(MouseAction.MiddleClick)));

        }
        private void InitLightPresets()
        {

            var light1 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Colors.White, Name = "Directional1" };
            var light2 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Colors.LightGray, Name = "Directional2" };
            var light3 = new AmbientLight3D { Color = Colors.White, Name = "Ambient" };
            var DayLightPreset = new Light3DCollection();
            DayLightPreset.Children.Add(light1);
            DayLightPreset.Children.Add(light2);
            DayLightPreset.Children.Add(light3);
            DayLightPreset.Name = "Дневной свет";
            DayLightPreset.Tag = "preset_daylight";

            var light4 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Colors.DarkBlue, Name = "Directional1" };
            var light5 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Colors.DarkSlateBlue, Name = "Directional2" };
            var light6 = new AmbientLight3D { Color = Colors.White, Name = "Ambient" };
            var NightLightPreset = new Light3DCollection();
            NightLightPreset.Children.Add(light4);
            NightLightPreset.Children.Add(light5);
            NightLightPreset.Children.Add(light6);
            NightLightPreset.Name = "Ночной свет";
            NightLightPreset.Tag = "preset_nightlight";

            var light7 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Colors.White, Name = "Directional1" };
            var light8 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Colors.White, Name = "Directional2" };
            var light9 = new PointLight3D { Position = new Media3D.Point3D(0, 0, 500), Color = Colors.White, Name = "Point" };
            var collection = new Light3DCollection();
            collection.Children.Add(light7);
            collection.Children.Add(light8);
            collection.Children.Add(light9);
            collection.Name = "Классический 3-точечный свет";
            collection.Tag = "preset_classic3pointlight";

            _lightPresets = new List<Light3DCollection>
            {
                collection,
                DayLightPreset,
                NightLightPreset
            };
        }
    }
}
