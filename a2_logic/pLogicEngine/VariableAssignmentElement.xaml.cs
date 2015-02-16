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

namespace pLogicEngine
{
    /// <summary>
    /// Interaction logic for VariableAssignmentElement.xaml
    /// this UI elemnt was created to allow a user to change the 
    /// truth value assignment of a variable within an expression. 
    /// </summary>
    public partial class VariableAssignmentElement : UserControl
    {
        string _symbol;
        ParseTree _source;
        private VariableAssignmentElement()
        {
            InitializeComponent();
        }

        public VariableAssignmentElement(string Symbol, ParseTree Source)
        {
            InitializeComponent();
            this.lbl_symbol.Content = _symbol = Symbol;
            _source = Source;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Default to the Unknow value (what the ParseTree defaults to)
            this.cmb_value.ItemsSource = Enum.GetValues(typeof(TruthValue));
            this.cmb_value.SelectedIndex = 2; //Set to unknown, their default value.
        }

        private void cmb_value_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Assign the value in the tree. 
            _source.AssignValue(_symbol, (TruthValue)this.cmb_value.SelectedValue);
        }


    }
}
