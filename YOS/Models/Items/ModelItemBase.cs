using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using YOS.Models.EnumParams;

namespace YOS.Models.Items
{
    public abstract class ModelItemBase : OnPropertyChangedClass, IClosetItemModel
    {
        protected string _mainPath;
        protected string _texPath;
        protected string _modelPath;
        protected List<Materials> _materials;
        protected string _name;
        protected Styles _style;
        protected Weather _weather;
        protected Poses _pose;
        protected GenderTypes _gender;
        protected Type _type;
        protected PBRMaterial _material;
        protected Geometry3D _geometry;
        public string Name { get => _name; protected set => _name = value; }
        public Styles Style { get => _style; protected set => _style = value; }
        public Weather Weather { get => _weather; protected set => _weather = value; }
        public Poses Pose { get => _pose; protected set => _pose = value; }
        public PBRMaterial Material { get => _material; set { _material = value; OnPropertyChanged(); } }
        public Geometry3D Geometry { get => _geometry; protected set => _geometry = value; }
        public GenderTypes Gender { get => _gender; protected set => _gender = value; }
        public Type Type { get => _type; protected set => _type = value; }
        public List<Materials> AvaliableMaterials => _materials;

        public Materials TextureMaterial { get; protected set; }

        public Color4 Color => _material.AlbedoColor;

        public bool IInitialized { get; private set; }

        public ModelItemBase(string name,
          GenderTypes gender = GenderTypes.Male,
          Poses pose = Poses.Idle,
          Styles style = Styles.Casual,
          Weather weather = Weather.Indoor)
        {
            IInitialized = false;
            _name = name;
            _gender = gender;
            _style = style;
            _weather = weather;
            _pose = pose;
            _material = new PBRMaterial();
        }
        public abstract object Clone();
       
        public void SetColor(Color4 color) => _material.AlbedoColor = color;
        public void SetMaterial(Materials material)
        {
            _texPath = $"{_mainPath}\\Textures\\{material}";
            TextureMaterial = material;
            SetMaterialMaps();
        }
        protected void InitAvaliableMaterials()
        {
            _materials = new List<Materials>();
            foreach (var mat in (Materials[])Enum.GetValues(typeof(Materials)))
                if (Directory.Exists($"{_texPath}\\{mat}"))
                    _materials.Add(mat);
        }
        protected void InitGeometryAndMaterials()
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
                AlbedoColor = new Color(255, 128, 0),
                RenderShadowMap = true,
                RenderEnvironmentMap = true,
                RenderAlbedoMap = true,
                RenderDisplacementMap = true,
                RenderNormalMap = true,
                RenderAmbientOcclusionMap = true,
                EnableTessellation = false,

            };
            _texPath += "\\" + _materials[0].ToString();
            TextureMaterial = _materials[0];
            SetMaterialMaps();
            IInitialized = true;
            GC.Collect();
        }
        protected void SetMaterialMaps()
        {
            string diffuseFileName = "diffuse";
            string normalFileName = "normal";
            string metallicRoughnessFileName = "metallicroughness";
            string ambientOcculsionFileName = "ambientocculsion";
            string displacementFileName = "displacement";
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
    }
}