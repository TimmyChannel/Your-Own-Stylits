using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YOS.Models;
using YOS.Models.Mannequin;

namespace YOS.ViewModels
{
    public class MainWindowViewModel : OnPropertyChangedClass
    {
        private Cursor _cursor;
        public Cursor CurrentCursor
        {
            get => _cursor;
            set => SetProperty<Cursor>(ref _cursor, value);
        }
        public MainWindowViewModel()
        {
            _cursor = Cursors.Arrow;
            MannequinSettings.Instance.PropertyChanged += Mannequin_PropertyChanged;
        }

        private void Mannequin_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MannequinSettings.Instance.ILoadModels))
            {
                if (MannequinSettings.Instance.ILoadModels)
                    CurrentCursor = Cursors.Wait;
                else
                    CurrentCursor = Cursors.Arrow;
            }
        }
    }
}
