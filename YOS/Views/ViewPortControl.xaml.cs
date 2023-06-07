using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YOS.Models.Settings;
using SharpDX;
namespace YOS.Views
{
    /// <summary>
    /// Логика взаимодействия для ViewPortControl.xaml
    /// </summary>
    public partial class ViewPortControl : UserControl
    {
        public ViewPortControl()
        {
            InitializeComponent();
            illuminatedModel = new MeshGeometryModel3D();
            illuminatedModel.Material = new PBRMaterial();
        }
        private MeshGeometryModel3D illuminatedModel;
        private void Move(object sender, MouseEventArgs e)
        {
            var point = view1.CursorOnElementPosition;
            if (point == null)
            {
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                return;
            }
            var pos = point.Value.ToVector3();

            //Проверка на то, находится ли курсор в границах Низа 
            if (Bottom.ItemsSource.Any() && ((MeshGeometryModel3D)Bottom.ItemsSource[0]).Geometry.Bound.Contains(pos) == ContainmentType.Contains)
            {
                var model = ((MeshGeometryModel3D)Bottom.ItemsSource[0]);
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                ((PBRMaterial)model.Material).EmissiveColor = new Color4(1, .831f, .682f, 1);
                illuminatedModel = model;
                Debug.WriteLine("Cursor in the Bottom model boundaries");
                return;
            }
            //Проверка на то, находится ли курсор в границах Верха 
            if (Top.ItemsSource.Any() && ((MeshGeometryModel3D)Top.ItemsSource[0]).Geometry.Bound.Contains(pos) == ContainmentType.Contains)
            {
                var model = ((MeshGeometryModel3D)Top.ItemsSource[0]);
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                ((PBRMaterial)model.Material).EmissiveColor = new Color4(1, .831f, .682f, 1);
                illuminatedModel = model;
                Debug.WriteLine("Cursor in the Top model boundaries");
                return;
            }
            //Проверка на то, находится ли курсор в границах Обуви 
            if (Shoes.ItemsSource.Any() && ((MeshGeometryModel3D)Shoes.ItemsSource[0]).Geometry.Bound.Contains(pos) == ContainmentType.Contains)
            {
                var model = ((MeshGeometryModel3D)Shoes.ItemsSource[0]);
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                ((PBRMaterial)model.Material).EmissiveColor = new Color4(1, .831f, .682f, 1);
                illuminatedModel = model;
                Debug.WriteLine("Cursor in the Shoes model boundaries");
                return;
            }
            //Проверка на то, находится ли курсор в границах Головного убора 
            if (Headwear.ItemsSource.Any() && ((MeshGeometryModel3D)Headwear.ItemsSource[0]).Geometry.Bound.Contains(pos) == ContainmentType.Contains)
            {
                var model = ((MeshGeometryModel3D)Headwear.ItemsSource[0]);
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                ((PBRMaterial)model.Material).EmissiveColor = new Color4(1, .831f, .682f, 1);
                illuminatedModel = model;
                Debug.WriteLine("Cursor in the Headwear model boundaries");
                return;
            }
            //Проверка на то, находится ли курсор в границах Аксессуара 
            if (Accessory.ItemsSource.Any() && ((MeshGeometryModel3D)Accessory.ItemsSource[0]).Geometry.Bound.Contains(pos) == ContainmentType.Contains)
            {
                var model = ((MeshGeometryModel3D)Accessory.ItemsSource[0]);
                ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);
                ((PBRMaterial)model.Material).EmissiveColor = new Color4(1, .831f, .682f, 1);
                illuminatedModel = model;
                Debug.WriteLine("Cursor in the Accessory model boundaries");
                return;
            }

            ((PBRMaterial)illuminatedModel.Material).EmissiveColor = new Color4(0);

        }
    }
}
