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
        private byte _indexOfCurrentEnvironmentMap;
        private MannequinSettings _mannequinSettings;
        private bool _groundIsVisible;
        private List<Light3DCollection> _lightPresets;
        private List<TextureModel> _enviromentMaps;
        public byte IndexOfCurrentLightPreset
        {
            get => _indexOfCurrentLightPreset;
            set
            {
                if (_indexOfCurrentLightPreset >= _lightPresets.Count)
                    return;
                _indexOfCurrentLightPreset = value;
            }
        }
        public byte IndexOfCurrentEnvironmentMap
        {
            get => _indexOfCurrentEnvironmentMap;
            set
            {
                if (_indexOfCurrentEnvironmentMap >= _enviromentMaps.Count)
                    return;
                _indexOfCurrentEnvironmentMap = value;
            }
        }
        public Light3DCollection CurrentLightPreset
        {
            get => _lightPresets[_indexOfCurrentLightPreset];
        }
        public TextureModel CurrentEnvironmentMap
        {
            get => _enviromentMaps[_indexOfCurrentEnvironmentMap];
        }

        public Viewport3DX Viewport { get; private set; }
        public bool GroundIsVisible
        {
            get => _groundIsVisible;
            set => _groundIsVisible = value;
        }
        private static readonly Lazy<ViewPortSettings> lazy = new(() => new ViewPortSettings());
        public static ViewPortSettings Instance { get { return lazy.Value; } }
        private ViewPortSettings()
        {
            _mannequinSettings = MannequinSettings.Instance;
            _mannequinSettings.AddClosetItem("Shorts", Type.Bottom);
            InitLightPresets();
            InitViewport();
            InitEnviromentMaps();
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
            Viewport.Items.Add(CurrentLightPreset);
        }
        private void InitLightPresets()
        {
            // Дневной пресет
            var dirLight1 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Colors.White, Name = "Directional1" };
            var dirLight2 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Colors.LightGray, Name = "Directional2" };
            var pointLight1 = new PointLight3D { Position = new Media3D.Point3D(0, 200, 0), Color = Colors.White, Name = "Point1" };
            var DayLightPreset = new Light3DCollection();
            DayLightPreset.Children.Add(dirLight1);
            DayLightPreset.Children.Add(dirLight2);
            DayLightPreset.Children.Add(pointLight1);
            DayLightPreset.Tag = "preset_daylight";

            // Ночной пресет
            var dirLight3 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Color.FromRgb(0, 0, 30), Name = "Directional1" };
            var dirLight4 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Color.FromRgb(30, 30, 50), Name = "Directional2" };
            var spotLight1 = new SpotLight3D { Position = new Media3D.Point3D(-500, 1000, 0), Direction = new Media3D.Vector3D(0, 1, 0), 
                InnerAngle = 30, OuterAngle = 45, Color = Colors.White, Name = "Spot1", Range = 2000 };
            var NightLightPreset = new Light3DCollection();
            NightLightPreset.Children.Add(dirLight3);
            NightLightPreset.Children.Add(dirLight4);
            NightLightPreset.Children.Add(spotLight1);
            NightLightPreset.Tag = "preset_nightlight";

            var light7 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Colors.White, Name = "Directional1" };
            var light8 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Colors.White, Name = "Directional2" };
            var light9 = new PointLight3D { Position = new Media3D.Point3D(0, 0, 500), Color = Colors.White, Name = "Point" };
            var collection = new Light3DCollection();
            collection.Children.Add(light7);
            collection.Children.Add(light8);
            collection.Children.Add(light9);
            collection.Tag = "preset_classic3pointlight";

            _lightPresets = new List<Light3DCollection>
            {
                DayLightPreset,
                NightLightPreset,
                collection
            };
        }
        private void InitEnviromentMaps()
        {
            var pathOfMaps = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\HDRIs\\";
            var indoorStudio = TextureModel.Create($"{pathOfMaps}indoorStudio.dds");
            var Cloudy = TextureModel.Create($"{pathOfMaps}Cloudy.dds");
            var Midday = TextureModel.Create($"{pathOfMaps}Midday.dds");
            _enviromentMaps = new List<TextureModel>
            {
                indoorStudio,
                Midday,
                Cloudy,
            };
        }
    }
}
