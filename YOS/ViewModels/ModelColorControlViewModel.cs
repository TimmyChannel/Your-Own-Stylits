using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.Mannequin;
using YOS.Models.Items;
namespace YOS.ViewModels
{
    public class ModelColorControlViewModel
    {
	  private MannequinSettings _mannequinSettings;
	  private IClosetItemModel _currentClosetItemModel;
	  public ModelColorControlViewModel()
	  {
		_mannequinSettings = MannequinSettings.Instance;
	  }

    }
}
