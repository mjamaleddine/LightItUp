using LightItUpMapGenerator.ViewModels;
using System;
using System.Collections.Generic;
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

namespace LightItUpMapGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationViewModel ApplicationViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            ApplicationViewModel = new ApplicationViewModel();
            DataContext = ApplicationViewModel;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            BoardGenerator generator = new BoardGenerator(ApplicationViewModel.BoardSettings);
            var board = generator.Generate();

            ApplicationViewModel.CurrentBoard = new BoardViewModel(board);
        }
    }
}
