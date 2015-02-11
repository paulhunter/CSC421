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

namespace prositional_logic_engine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tb_infix_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = (sender as TextBox).Text;
            if(!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            {
                string RPN;
                Exception ex;
                ParseTree pt;
                int ErrorPoint;
                int ErrorTokenLen;
                if (ParseEngine.TryParse(input, out pt, out RPN, out ex, out ErrorPoint, out ErrorTokenLen))
                {
                    this.tb_rpn.Text = RPN;
                }
                else
                {
                    this.tb_rpn.Text = ex.Message;
                    if(ErrorTokenLen != 0)
                    {
                        tb_infix.SelectionStart = ErrorPoint;
                        tb_infix.SelectionLength = ErrorTokenLen;
                    }
                }
            }
            else
            {
                this.tb_rpn.Text = "";
            }

        }
    }
}
