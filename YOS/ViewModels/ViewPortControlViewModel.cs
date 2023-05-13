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
            TODO
            var models = reader.Read("C:\\Users\\Аська\\Desktop\\Cursed_Work\\team-1131\\YOS\\Resources\\Tshirt.obj");
            Geometry = models[0].Geometry;

            // Задаем кисть цвета ткани     
            //Создаем материал
            var material = new PhongMaterial { 
                DiffuseColor= Colors.AliceBlue.ToColor4(),
                AmbientColor= Colors.Gray.ToColor4(),
        };

            //Задаем кисть цвета ткани
            material.DiffuseColor = Colors.AliceBlue.ToColor4();

            //Задаем коэффициенты спекулярных отражений
            //Ткань не очень хорошо отражает 
            material.SpecularShininess = 0.1F;
            Material = material;
        }
    }
}
