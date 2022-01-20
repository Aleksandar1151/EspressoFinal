using EspressoFinal.Data;
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
using System.Collections.ObjectModel;

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for NalogPage.xaml
    /// </summary>
    public partial class NalogPage : UserControl
    {
        public static ObservableCollection<Nalog> KolekcijaNalog {get;set;}
        int kliknutiNalog;
        public NalogPage()
        {
            InitializeComponent();
            KolekcijaNalog = Nalog.Ucitaj();
            NalogListView.ItemsSource = KolekcijaNalog;
        }

        private void ListElement_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (sender as ListView).SelectedItem; 
                if (item != null)
                {
                    System.Windows.Controls.ListView list = (System.Windows.Controls.ListView)sender;
                    Nalog izabranNalog = (Nalog)list.SelectedItem;
                    kliknutiNalog = izabranNalog.idNalog;

                    AzuriranjeLabel.Content = "AŽURIRANJE NALOGA: [ " + izabranNalog.ime + " ]";
                    AzuriranjeLabel.Foreground  = Brushes.DarkGray;

                    AzurirajNalogButton.IsEnabled = true;
                    ObrisiNalogButton.IsEnabled = true;
                    AzurirajPrivilegije.IsEnabled = true;

                    if(izabranNalog.privilegije == "ima")
                    AzurirajPrivilegije.IsChecked = true;
                    else AzurirajPrivilegije.IsChecked = false;
                }
               (sender as ListView).SelectedItem = null;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void AzurirajNalog_Click(object sender, RoutedEventArgs e)
        {
            #region popup            
            myPopup.AllowsTransparency = true;
            myPopupText.Text = "USPJEŠNO AŽURIRAN NALOG";
            this.myPopup.IsOpen = true;
            #endregion

            string priv = "nema";
            if(AzurirajPrivilegije.IsChecked == true) priv = "ima";

            int index = KolekcijaNalog.ToList().FindIndex(num => num.idNalog == kliknutiNalog);
            KolekcijaNalog[index].privilegije = priv;
            KolekcijaNalog[index].Azuriraj();

            RefreshView();
        }

        private void ObrisiNalog_Click(object sender, RoutedEventArgs e)
        {
            #region popup
            myPopup.AllowsTransparency = true;
            myPopupText.Text = "USPJEŠNO OBRISAN NALOG";
            this.myPopup.IsOpen = true;
            #endregion
            int index = KolekcijaNalog.ToList().FindIndex(num => num.idNalog == kliknutiNalog);
            KolekcijaNalog[index].Obrisi();
            KolekcijaNalog.RemoveAt(index);

            NalogListView.ItemsSource = null;
            NalogListView.ItemsSource = KolekcijaNalog;



            RefreshView();
        }

        private void NoviNalog_Click(object sender, RoutedEventArgs e)
        {
            #region popup
            myPopup.AllowsTransparency = true;
            myPopupText.Text = "USPJEŠNO KREIRAN NALOG";
            this.myPopup.IsOpen = true;
            #endregion
            
            int privilegija_temp = 0;
            if(PrivilegijeToogle.IsChecked == true)  privilegija_temp = 1;

            Nalog noviNalog = new Nalog(NazivBox.Text,LozinkaBox.Text, privilegija_temp);
            KolekcijaNalog.Add(noviNalog);

            NalogListView.ItemsSource = null;
            NalogListView.ItemsSource = KolekcijaNalog;

            noviNalog.Sacuvaj();

            NazivBox.Text = null;
            LozinkaBox.Text = null;
            PrivilegijeToogle.IsChecked = false;

        }

        private void RefreshView()
        {
            NalogListView.ItemsSource = null;
            NalogListView.ItemsSource = KolekcijaNalog;
            AzuriranjeLabel.Content = "AŽURIRANJE NALOGA: ";
            AzuriranjeLabel.Foreground  = Brushes.LightGray;

            AzurirajNalogButton.IsEnabled = false;
            ObrisiNalogButton.IsEnabled = false;
            AzurirajPrivilegije.IsEnabled = false;
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
