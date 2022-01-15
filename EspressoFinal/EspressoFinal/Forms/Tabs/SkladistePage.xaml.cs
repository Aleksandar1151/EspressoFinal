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
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using EspressoFinal.Data;

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for SkladistePage.xaml
    /// </summary>
    public partial class SkladistePage : UserControl
    {
        public static ObservableCollection<Kategorija> KolekcijaKategorija { get;set;}
        public static ObservableCollection<Artikal> KolekcijaArtikal { get;set;}
        int kliknutIdArtikal;
        public SkladistePage()
        {
            InitializeComponent();

            KolekcijaKategorija = Kategorija.Ucitaj();
            KolekcijaArtikal = Artikal.Ucitaj();

            foreach(Kategorija kategorija in KolekcijaKategorija)
            {
                ComboBoxItem item = new ComboBoxItem();  
                item.Content = kategorija.naziv;  
                KategorijaCombo.Items.Add(item);
            }

            //KolekcijaArtikal = new ObservableCollection<Artikal>();
            SkladisteListView.ItemsSource = KolekcijaArtikal;

           
        }

        private void DodajArtikal_Click(object sender, RoutedEventArgs e)
        {
            
            //ComboBoxItem comboBoxItem = (ComboBoxItem)KategorijaCombo.SelectedIndex;
            //string izabranaKategorija = comboBoxItem.Content.ToString();   
        

            Artikal artikal = new Artikal(NazivBox.Text,Convert.ToDouble(CijenaBox.Text), Convert.ToInt32(KolicinaBox.Text), (KategorijaCombo.SelectedIndex + 1) );
            
            KolekcijaArtikal.Add(artikal);
            Artikal.Dodaj(artikal);

            NazivBox.Text = null;
            CijenaBox.Text = null;
            KolicinaBox.Text = null;

            KategorijaCombo.SelectedItem = null;

            
        }

        private void ListElement_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (sender as ListView).SelectedItem; 
                if (item != null)
                {
                    System.Windows.Controls.ListView list = (System.Windows.Controls.ListView)sender;
                    Artikal izabranArtikal = (Artikal)list.SelectedItem;
                    kliknutIdArtikal = izabranArtikal.idArtikal;

                    LabelArtikal.Content = "DODAVANJE KOLIČINE ARTIKLU: [ " + izabranArtikal.naziv + " ]";
                    LabelArtikal.Foreground  = Brushes.Gray;

                    KolicinaDodajBox.IsEnabled = true;
                    DodajKolicinuButton.IsEnabled = true;

                    

                }
               (sender as ListView).SelectedItem = null;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DodajKolicinu_Click(object sender, RoutedEventArgs e)
        {
            SkladisteListView.ItemsSource = null;            

            int index = KolekcijaArtikal.ToList().FindIndex(num => num.idArtikal == kliknutIdArtikal);
            KolekcijaArtikal[index].kolicina += Convert.ToInt32(KolicinaDodajBox.Text); 

            Artikal.Azuriraj(KolekcijaArtikal);

            SkladisteListView.ItemsSource = KolekcijaArtikal;
            KolicinaDodajBox.IsEnabled = false;
            DodajKolicinuButton.IsEnabled = false;
            KolicinaDodajBox.Text = null;
            LabelArtikal.Foreground  = Brushes.LightGray;

        }
    }
}
