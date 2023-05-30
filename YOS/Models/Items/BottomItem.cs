using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Items
{
    public class BottomItem : IClosetItemModel
    {
        private string _mainPath;
        private string _texPath;
        private string _modelPath;
        private List<Materials> _materials;
        public BottomItem(string name,
            GenderTypes gender = GenderTypes.Male,
            Poses pose = Poses.Idle,
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor)
        {
            var i = ClosetItemList.GetItem(name);
            _name = i.Name;
            _type = i.Type;
            _gender = gender;
            _style = style;
            _weather = weather;
            _pose = pose;
            _genderChangeModel = true;
            _mainPath = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Items\\{_type}\\{_name}\\{_gender}";
            if (!Directory.Exists(_mainPath))
                return;
            _texPath = $"{_mainPath}\\Textures";
            _modelPath = $"{_mainPath}\\{_pose}\\{_name}.obj";
            InitAvaliableMaterials();
            InitGeometryAndMaterials();
        }
        private void InitAvaliableMaterials()
        {
            _materials = new List<Materials>();
            foreach (var mat in (Materials[])Enum.GetValues(typeof(Materials)))
                if (Directory.Exists($"{_texPath}\\{mat}"))
                    _materials.Add(mat);
        }
        public void SetColor(Color color) => _material.AlbedoColor = color;

        private void InitGeometryAndMaterials()
        {
            var reader = new ObjReader();

            var objCol = reader.Read(_modelPath);
            var meshArray = new MeshGeometry3D[objCol.Count];
            for (int i = 0; i < objCol.Count; i++)
            {
                meshArray[i] = new MeshGeometry3D();
                objCol[i].Geometry.AssignTo(meshArray[i]);
            }
            var m = MeshGeometry3D.Merge(meshArray);
            _geometry = m;
            _material = new PBRMaterial
            {
                AlbedoColor = new Color(255,128,0),
                RenderShadowMap = true,
                RenderEnvironmentMap = true,
                RenderAlbedoMap = true,
                RenderDisplacementMap = true,
                RenderNormalMap = true,
                RenderAmbientOcclusionMap = true,
                EnableTessellation = false
            };
            SetMaterialMaps();
            GC.Collect();
        }
        private void SetMaterialMaps()
        {
            string diffuseFileName = "diffuse";
            string normalFileName = "normal";
            string metallicRoughnessFileName = "metallicroughness";
            string ambientOcculsionFileName = "ambientocculsion";
            string displacementFileName = "displacement";
            _texPath += "\\"+_materials[0].ToString();
            string[] diffuseFiles = Directory.GetFiles(_texPath, "*" + diffuseFileName + "*.*");
            string[] normalFiles = Directory.GetFiles(_texPath, "*" + normalFileName + "*.*");
            string[] metallicRoughnessFiles = Directory.GetFiles(_texPath, "*" + metallicRoughnessFileName + "*.*");
            string[] ambientOcculsionFiles = Directory.GetFiles(_texPath, "*" + ambientOcculsionFileName + "*.*");
            string[] displacementFiles = Directory.GetFiles(_texPath, "*" + displacementFileName + "*.*");

            if (diffuseFiles.Length > 0)
            {
                _material.AlbedoMap = TextureModel.Create(diffuseFiles[0]);
            }

            if (normalFiles.Length > 0)
            {
                _material.NormalMap = TextureModel.Create(normalFiles[0]);
            }

            if (metallicRoughnessFiles.Length > 0)
            {
                _material.RoughnessMetallicMap = TextureModel.Create(metallicRoughnessFiles[0]);
            }

            if (ambientOcculsionFiles.Length > 0)
            {
                _material.AmbientOcculsionMap = TextureModel.Create(ambientOcculsionFiles[0]);
            }

            if (displacementFiles.Length > 0)
            {
                _material.DisplacementMap = TextureModel.Create(displacementFiles[0]);
            }
        }

        private string _name;
        private Styles _style;
        private Weather _weather;
        private Poses _pose;
        private GenderTypes _gender;
        private Type _type;
        private PBRMaterial _material;
        private Geometry3D _geometry;
        private bool _genderChangeModel;
        public string Name { get => _name; private set => _name = value; }
        public Styles Style { get => _style; private set => _style = value; }
        public Weather Weather { get => _weather; private set => _weather = value; }
        public Poses Pose { get => _pose; private set => _pose = value; }
        public PBRMaterial Material { get => _material; private set => _material = value; }
        public Geometry3D Geometry { get => _geometry; private set => _geometry = value; }
        public bool GenderChangeModel { get => _genderChangeModel; private set => _genderChangeModel = value; }
        public GenderTypes Gender { get => _gender; private set => _gender = value; }
        public Type Type { get => _type; private set => _type = value; }
    }

}
