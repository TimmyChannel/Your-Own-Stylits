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
    public class ViewPortSettings : OnPropertyChangedClass
    {
        private byte _indexOfCurrentLightPreset;
        private byte _indexOfCurrentEnvironmentMap;
        private byte _indexOfCurrentFloorTexture;
        private MannequinSettings _mannequinSettings;
        private bool _groundIsVisible;
        private List<Light3DCollection> _lightPresets;
        private List<TextureModel> _environmentMaps;
        private List<PBRMaterial> _floorTextures;
        public byte IndexOfCurrentLightPreset
        {
            get => _indexOfCurrentLightPreset;
            set
            {
                if (_indexOfCurrentLightPreset >= _lightPresets.Count)
                    return;
                _indexOfCurrentLightPreset = value;
                OnPropertyChanged(nameof(CurrentLightPreset));
            }
        }
        public byte IndexOfCurrentEnvironmentMap
        {
            get => _indexOfCurrentEnvironmentMap;
            set
            {
                if (_indexOfCurrentEnvironmentMap >= _environmentMaps.Count)
                    return;
                _indexOfCurrentEnvironmentMap = value;
                OnPropertyChanged(nameof(CurrentEnvironmentMap));
            }
        }
        public byte IndexOfCurrentFloorTexture
        {
            get => _indexOfCurrentFloorTexture;
            set
            {
                if (_indexOfCurrentFloorTexture >= _floorTextures.Count)
                    return;
                _indexOfCurrentFloorTexture = value;
                OnPropertyChanged(nameof(CurrentFloorTexture));
            }
        }
        public Light3DCollection CurrentLightPreset
        {
            get => _lightPresets[_indexOfCurrentLightPreset];
        }
        public TextureModel CurrentEnvironmentMap
        {
            get => _environmentMaps[_indexOfCurrentEnvironmentMap];
        }
        public PBRMaterial CurrentFloorTexture
        {
            get => _floorTextures[_indexOfCurrentFloorTexture];
        }

        public Camera Camera { get; private set; }
        public bool GroundIsVisible
        {
            get => _groundIsVisible;
            set =>  SetProperty(ref _groundIsVisible, value); 
        }
        private static readonly Lazy<ViewPortSettings> lazy = new(() => new ViewPortSettings());
        public static ViewPortSettings Instance { get { return lazy.Value; } }
        private ViewPortSettings()
        {
            _mannequinSettings = MannequinSettings.Instance;
            Camera = new PerspectiveCamera
            {
                LookDirection = new Media3D.Vector3D(34, -9, -303),
                UpDirection = new Media3D.Vector3D(0, 1, 0),
                Position = new Media3D.Point3D(-35, 109, 304)
            };

            InitLightPresets();
            InitEnvironmentMapsAndFloorTextures();
        }
        private void InitLightPresets()
        {
            // Дневной пресет
            var dirLight1 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Color.FromRgb(255, 244, 214), Name = "Directional1" };
            var dirLight2 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Color.FromRgb(255, 244, 214), Name = "Directional2" };
            var pointLight1 = new PointLight3D { Position = new Media3D.Point3D(0, 200, 0), Color = Color.FromRgb(255, 244, 214), Name = "Point1", Attenuation = new Media3D.Vector3D(1, 0.02, 0.002) };
            var DayLightPreset = new Light3DCollection();
            DayLightPreset.Children.Add(dirLight1);
            DayLightPreset.Children.Add(dirLight2);
            DayLightPreset.Children.Add(pointLight1);
            DayLightPreset.Tag = "preset_daylight";

            // Ночной пресет
            var dirLight3 = new DirectionalLight3D { Direction = new Media3D.Vector3D(-1, -1, -1), Color = Color.FromRgb(180, 180, 180), Name = "Directional1" };
            var dirLight4 = new DirectionalLight3D { Direction = new Media3D.Vector3D(1, 1, 1), Color = Color.FromRgb(150, 150, 150), Name = "Directional2" };
            var pointLight2 = new PointLight3D { Position = new Media3D.Point3D(0, 200, 0), Color = Color.FromRgb(200, 200, 200), Name = "Point1", 
                Attenuation = new Media3D.Vector3D(1, 0.02, 0.002), Range = 500 };
            var NightLightPreset = new Light3DCollection();
            NightLightPreset.Children.Add(dirLight3);
            NightLightPreset.Children.Add(dirLight4);
            NightLightPreset.Children.Add(pointLight2);
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
        private void InitEnvironmentMapsAndFloorTextures()
        {
            var pathOfMaps = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Environment\\";
            var indoorStudio = TextureModel.Create($"{pathOfMaps}IndoorStudio.dds");
            var Night = TextureModel.Create($"{pathOfMaps}Night.dds");
            var Midday = TextureModel.Create($"{pathOfMaps}Midday.dds");
            _environmentMaps = new List<TextureModel>
            {
                Midday,
                Night,
                indoorStudio,
            };

            var flIndoor = new PBRMaterial()
            {
                AlbedoMap = TextureModel.Create($"{pathOfMaps}\\Floor\\indoor_ny.png"),
                RenderAlbedoMap = true,
                RenderShadowMap = true,
                AlbedoColor = new SharpDX.Color4(1, 1, 1, 1),
                RenderEnvironmentMap = true,
                EnableAutoTangent = true,
                EmissiveColor = new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1),
            };
            var flDay = new PBRMaterial()
            {
                AlbedoMap = TextureModel.Create($"{pathOfMaps}\\Floor\\day_ny.png"),
                RenderAlbedoMap = true,
                RenderShadowMap = true,
                AlbedoColor= new SharpDX.Color4(1,1,1,1),
                RenderEnvironmentMap = true,
                EnableAutoTangent = true,
                EmissiveColor = new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1),

            };
            var flNight = new PBRMaterial()
            {
                AlbedoMap = TextureModel.Create($"{pathOfMaps}\\Floor\\night_ny.png"),
                RenderAlbedoMap = true,
                RenderShadowMap = true,
                AlbedoColor = new SharpDX.Color4(1, 1, 1, 1),
                RenderEnvironmentMap = true,
                EnableAutoTangent = true,
                EnableFlatShading = true,
                EmissiveColor = new SharpDX.Color4(0.1f, 0.1f, 0.1f, 0.9f),
            };
            _floorTextures = new List<PBRMaterial>
            {
                flDay,
                flNight,
                flIndoor,
            };
        }
    }
}
