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
        public static ObservableCollection<Kategorija> KategorijaKolekcija { get;set;}
        public SkladistePage()
        {
            InitializeComponent();

            KategorijaKolekcija = Kategorija.Ucitaj();

            foreach(Kategorija kategorija in KategorijaKolekcija)
            {
                ComboBoxItem item = new ComboBoxItem();  
                item.Content = kategorija.naziv;  
                KategorijaCombo.Items.Add(item);
            }

           
        }

        private void DodajArtikal_Click(object sender, RoutedEventArgs e)
        {
             string izabranaKategorija = KategorijaCombo.SelectedItem.ToString();

            

            NazivBox.Text = izabranaKategorija;
        }
    }
}
