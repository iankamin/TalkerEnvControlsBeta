using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timer = System.Timers.Timer;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Button = System.Windows.Controls.Button;
using System;
using System.ComponentModel;

namespace TalkerEnvControlsBETA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConsoleControls cc = new ConsoleControls();
        private List<Button> thisButtons = new List<Button>();
        private List<Button> currentButtons = new List<Button>();

        private Timer aTimer;
        private Button highlightedButton;
        private int indexHighlighted;

        Remote_popup rp = new Remote_popup();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            GetLogicalChildCollection(this, thisButtons);
            currentButtons = thisButtons;
            runTimer();

            this.Closed += terminate_program;

        }
        //  creates a list from all buttons on the page
        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
           
            rp.Show();
            List<Button> temp = new List<Button>();
            GetLogicalChildCollection(rp, temp);
            currentButtons = temp;
            rp.Closing += popup_closed;
            rp.KeyDown += Key_down;
            indexHighlighted = 0;
        }
        private void popup_closed(object sender, CancelEventArgs e)
        {
            currentButtons = thisButtons;
            rp.KeyDown -= Key_down;
            indexHighlighted = 0;

            // Cancel Window closing 
            e.Cancel = true;
            // Hide Window instead
            rp.Hide();
        }

        private void relayControl(object sender, RoutedEventArgs e)
        {
            string _tag = (((Button)sender).Tag).ToString();
            int _t = int.Parse(_tag);
            Debug.Print(_t.ToString());
            cc.relayControl(_t);
        }

        private void powerControl(object sender,RoutedEventArgs e)
        {
            cc.remoteControlSend("STV", "KEY_POWER");
        }

        //using System.Timers
        public void runTimer()
        {
            aTimer = new Timer(1000);

            aTimer.Elapsed += new ElapsedEventHandler(autoscaningButtons);
            aTimer.AutoReset = true;
            aTimer.Enabled = false;
            indexHighlighted = 0;


        }

        // This method will get called by the timer until the timer stops or the program exits.
        // it changes the currently highlighted button for autoscanning
        public void autoscaningButtons(object source, ElapsedEventArgs e)
        {
            // increments index for next button
            if (indexHighlighted < currentButtons.Count - 1) {   indexHighlighted++;   }
            else  {   indexHighlighted = 0;  }

            //currently highlighted button reverts to black background
            if (highlightedButton != null)
            {   this.Dispatcher.Invoke(() =>  {
                    highlightedButton.Background = Brushes.Black;
                });
            }
            // change to next highlighted button
            highlightedButton = currentButtons[indexHighlighted];    
            this.Dispatcher.Invoke(() =>  {
                highlightedButton.Background = Brushes.Red;
            });            
        }

        private void Autoscan_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = !aTimer.Enabled;
            // enable key listeners
            if (aTimer.Enabled)
            {
                this.KeyDown += Key_down;

            } else
            {
                this.KeyDown -= Key_down;
            }
            //restore the highlighted key to original color
            if (highlightedButton != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    highlightedButton.Background = Brushes.Black;
                });
            }
        }

        // key press eventHandler
        private void Key_down(object sender, KeyEventArgs e)
        {
            Key k = e.Key;
            switch (k)
            {
                case Key.Q:
                    indexHighlighted -= 2;
                    if (indexHighlighted < 0)
                    {
                        aTimer.Stop();
                        indexHighlighted += currentButtons.Count;
                        aTimer.Start();
                    }
                    break;
                case Key.E:
                    highlightedButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }

        }


        private void terminate_program(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }


    }
}
