using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelixToolkit.Wpf.SharpDX;
using Media3D = System.Windows.Media.Media3D;
using System.Windows.Media;
using SharpDX.Direct3D9;
using SharpDX;
using System.Windows.Media.Imaging;

namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        public Geometry3D Geometry { private set; get; }
        public PhongMaterial Material { private set; get; }

        public ViewPortControlViewModel()
        {

            var reader = new ObjReader();
            var models = reader.Read($"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Tshirt.obj");
            Geometry = models[0].Geometry;

            //    // Задаем кисть цвета ткани     
            //    //Создаем материал
            //    var material = new PhongMaterial { 
            //        DiffuseColor= Colors.AliceBlue.ToColor4(),
            //        AmbientColor= Colors.Gray.ToColor4(),
            //};

            //    //Задаем кисть цвета ткани
            //    material.DiffuseColor = Colors.AliceBlue.ToColor4();

            //    //Задаем коэффициенты спекулярных отражений
            //    //Ткань не очень хорошо отражает 
            //    material.SpecularShininess = 0.1F;
            //    Material = material;
            var material = new PhongMaterial();

            // Задаем текстуру футболки
            //var brush = new ImageBrush(new BitmapImage());
            material.DiffuseMap = TextureModel.Create($"{AppDomain.CurrentDomain.BaseDirectory}Resources\\temp_tex.jpg");
            material.RenderDiffuseMap = true;
            // Задаем цвет освещения
            material.DiffuseColor = Colors.Gray.ToColor4();

            // Задаем свойства отражения света
            material.SpecularColor = Colors.White.ToColor4();
            material.SpecularShininess = 10F;
            //material.AmbientColor = PhongMaterials.ToColor(0.02, 0.02, 0.02, 1.0);
            //material.DiffuseColor = PhongMaterials.ToColor(0.01, 0.01, 0.01, 1.0);
            //material.SpecularColor = PhongMaterials.ToColor(0.4, 0.4, 0.4, 1.0);
            //material.EmissiveColor = PhongMaterials.ToColor(0.0, 0.0, 0.0, 0.0);

            //Material = PhongMaterials.BlackRubber ;
            Material = material;
        }
    }
}
