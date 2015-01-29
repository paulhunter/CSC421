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

namespace AI_SearchAlgos
{
    using Model;
    using Utils;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Utils.Log.Start();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Map m = MapFactory.BuildMap(5, 4, 0.2);
            Log.Info(string.Format("App: Map created has {0:0.00} free paths of target {1:0.00}", m.FreePathPercentage * 100, 0.2*100));
        }
    }
}
