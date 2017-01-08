using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightItUpMapGenerator.ViewModels
{
    public class ApplicationViewModel : ViewModel
    {

        public BoardSettingsViewModel BoardSettings { get; private set; }
        private BoardViewModel _CurrentBoard;
        public BoardViewModel CurrentBoard
        {
            get
            {
                return _CurrentBoard;
            }
            set
            {
                _CurrentBoard = value;
                FirePropertyChanged("CurrentBoard");
            }
        }

        public ApplicationViewModel()
        {
            BoardSettings = new BoardSettingsViewModel();
        }



    }
}
