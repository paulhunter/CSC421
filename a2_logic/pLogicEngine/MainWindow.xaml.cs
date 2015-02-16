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
using System.Threading;

namespace pLogicEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml, interacts with the ParseEngine and
    /// ParseTree classes to allow a user to experiement with Propositional Logic.
    /// </summary>
    public partial class MainWindow : Window
    {

        ParseTree _activeExpression; //The current expression entered by the user. 
        Timer _parseTimer; //A timer used to parse the input after the user has stopped typing. 

        volatile string _input;

        public MainWindow()
        {
            _parseTimer = new Timer(ParseTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            _activeExpression = null;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AttemptParse(); //Called to step initial UI element values. 
        }

        /// <summary>
        /// Attempt to parse the current user input into a parse tree.
        /// </summary>
        private void AttemptParse()
        {
            string input = _input.Substring(0); 
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            {
                string RPN;
                Exception ex;
                int ErrorPoint;
                int ErrorTokenLen;
                if (ParseEngine.TryParse(input, out this._activeExpression, out RPN, out ex, out ErrorPoint, out ErrorTokenLen))
                {
                    //Parse Sucess. 
                    this.tb_infix.Dispatcher.BeginInvoke(new Action(() =>
                    {
#if DEBUG
                        this.tb_rpn.Text = RPN; //If debug, output the Reduced polish notation. 
#else
                        this.tb_rpn.Text = "Parse Complete, use variable assignments and evaluate";
#endif
                    }));
                }
                else
                {
                    //Parse Failure.
                    this.tb_infix.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.tb_rpn.Text = ex.Message;
                        if (ErrorTokenLen != 0)
                        {
                            tb_infix.SelectionStart = ErrorPoint;
                            tb_infix.SelectionLength = ErrorTokenLen;
                        }
                    }));
                }
            }
            else
            {
                //Text box is empty. 
                this.tb_infix.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tb_rpn.Text = "Enter in-fix notation above to get started.";
                }));
                
            }
            //Update other UI elements. 
            UpdateVariableStack();
        }

        /// <summary>
        /// Update the UI to show the variables present in the current expression. 
        /// </summary>
        private void UpdateVariableStack()
        {
            this.sp_variables.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.sp_variables.Children.Clear();
                if (this._activeExpression != null)
                {
                    foreach (string Symbol in _activeExpression.GetSymbols())
                        this.sp_variables.Children.Add(new VariableAssignmentElement(Symbol, _activeExpression));
                }
                this.sp_variables.InvalidateVisual();
                this.lbl_result.Content = "Press Evaluate";
            }));
        }

        /// <summary>
        /// Fired when the user has stopped manipulating the infix textbox. 
        /// </summary>
        /// <param name="state">null</param>
        private void ParseTimerCallback(object state)
        {
            AttemptParse();
        }

        #region Input Changed -> Timer Reset 

        private void tb_infix_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            _input = tb_infix.Text;
            _parseTimer.Change(3500, Timeout.Infinite);
        }

        private void tb_infix_SelectionChanged(object sender, RoutedEventArgs e)
        {
            _parseTimer.Change(3500, Timeout.Infinite);
        }

        #endregion

        private void evaluate_click(object sender, RoutedEventArgs e)
        {
            if(_activeExpression != null)
            {
                TruthValue result = _activeExpression.Evaluate();
                switch(result)
                {
                    case TruthValue.True:
                        lbl_result.Content = "True";
                        break;
                    case TruthValue.False:
                        lbl_result.Content = "False";
                        break;
                    case TruthValue.Unknown:
                        lbl_result.Content = "Unknown";
                        break;

                }
            }
        }




    }
}
