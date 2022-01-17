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
using System.Windows.Threading;

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for NalogPage.xaml
    /// </summary>
    public partial class NalogPage : UserControl
    {
        public NalogPage()
        {
            InitializeComponent();
        }

        private void ListElement_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void AzurirajNalog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ObrisiNalog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NoviNalog_Click(object sender, RoutedEventArgs e)
        {
            myPopup.AllowsTransparency = true;
            this.myPopup.IsOpen = true;
        }

        private void myPopup_Opened(object sender, EventArgs e)
        {
            StartCloseTimer();
        }

        private void StartCloseTimer()
         {
             DispatcherTimer timer = new DispatcherTimer();
             timer.Interval = TimeSpan.FromSeconds(3d);
             timer.Tick += TimerTick;
             timer.Start();
         }
    
         private void TimerTick(object sender, EventArgs e)
         {
             DispatcherTimer timer = (DispatcherTimer)sender;
             timer.Stop();
             timer.Tick -= TimerTick;
             this.myPopup.IsOpen = false;
         }
    }
}
