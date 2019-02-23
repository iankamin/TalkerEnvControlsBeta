using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Button = System.Windows.Controls.Button;
using System;
using System.Reflection;

namespace TalkerEnvControlsBETA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConsoleControls cc = new ConsoleControls();
        private List<Button> listButtons = new List<Button>();
        private Timer aTimer;
        private Button highlightedButton;
        private int indexHighlighted;

        public MainWindow()
        {
            InitializeComponent();
            GetLogicalChildCollection(this, listButtons);
           
            runTimer();
        }

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

        private void relayControl(object sender, RoutedEventArgs e)
        {
            string _tag = (((Button)sender).Tag).ToString();
            int _t = int.Parse(_tag);
            Debug.Print(_t.ToString());
            cc.relayControl(_t);
        }

        private void OnClick1(object sender,RoutedEventArgs e)
        {
            relayControl(this, e);
        }

        //using System.Timers
        public void runTimer()
        {
            aTimer = new Timer(1000);

            aTimer.Elapsed += new ElapsedEventHandler(RunEvent);
            aTimer.AutoReset = true;
            aTimer.Enabled = false;
            indexHighlighted = 0;


        }

        //This method will get called every second until the timer stops or the program exits.
        public void RunEvent(object source, ElapsedEventArgs e)
        {
            if (highlightedButton != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    highlightedButton.Background = Brushes.Black;
                });
            }

            highlightedButton = listButtons[indexHighlighted];
            this.Dispatcher.Invoke(() =>
            {
                highlightedButton.Background = Brushes.Red;
            });
            if (indexHighlighted < listButtons.Count - 1)
            {
                indexHighlighted++;
            }
            else
            {
                indexHighlighted = 0;
            }
        }

        private void Autoscan_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = !aTimer.Enabled;
            // enable key listeners
            layoutGrid.KeyDown += Key_down;
            //restore the highlighted key to original color
            if (highlightedButton != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    highlightedButton.Background = Brushes.Black;
                });
            }
        }

        private void Key_down(object sender, KeyEventArgs e)
        {
            Key k = e.Key;
            if (k == Key.Q)
            {
                highlightedButton.Tag()
                MethodInfo clickMethodInfo = typeof(Button).GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
                clickMethodInfo.Invoke(highlightedButton, new object[] { EventArgs.Empty });
            }

            if (k == Key.E)
            {
            
            }
        }
    }
}
