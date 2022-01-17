using EspressoFinal.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for StornirajPage.xaml
    /// </summary>
    public partial class StornirajPage : UserControl
    {
        public static ObservableCollection<Stavka> RacunStavke { get;set;}
        public static ObservableCollection<Stavka> KolekcijaStorniran { get;set;}
         public static ObservableCollection<Artikal> KolekcijaArtikal { get;set;}
        Racun racun = new Racun(); 
        int kliknutIdStavka;

        public StornirajPage()
        {
            InitializeComponent();

            KolekcijaStorniran = new ObservableCollection<Stavka>();
        }

        private void PretraziRacun_Click(object sender, RoutedEventArgs e)
        {
                                  
            racun.PretraziRacun(Convert.ToInt32(RacunBox.Text));

            RacunStavke = Stavka.UcitajStavkeRacuna(racun.idRacun);

            RacunListView.ItemsSource = RacunStavke;

            
        }

        private void ListStavka_Click(object sender, MouseButtonEventArgs e)
        {
             try
            {
                var item = (sender as ListView).SelectedItem; 
                if (item != null)
                {
                    System.Windows.Controls.ListView list = (System.Windows.Controls.ListView)sender;
                    Stavka izabranaStavka = (Stavka)list.SelectedItem;
                    Stavka novaStavka = new Stavka(izabranaStavka);
                    kliknutIdStavka = izabranaStavka.idArtikal;

                    int index = RacunStavke.ToList().FindIndex(num => num.idArtikal == kliknutIdStavka);
                    RacunStavke[index].kolicina -= 1;

                    int i = KolekcijaStorniran.ToList().FindIndex(num2 => num2.idArtikal == kliknutIdStavka);
                    if(i != -1)
                    {
                       KolekcijaStorniran[i].kolicina += 1;                        
                       StorniranListView.ItemsSource = null;
                    }
                    else
                    {
                         novaStavka.kolicina = 1;
                         KolekcijaStorniran.Add(novaStavka);
                    }                    
                    
                    if(RacunStavke[index].kolicina == 0)
                    {
                        RacunStavke.RemoveAt(index);
                    }

                }
               (sender as ListView).SelectedItem = null;
                RacunListView.ItemsSource = null;
                StorniranListView.ItemsSource = null;

                RacunListView.ItemsSource = RacunStavke;
                StorniranListView.ItemsSource = KolekcijaStorniran;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void Stampa_Click(object sender, RoutedEventArgs e)
        {            
            StorniranRacun storniranRacun = new StorniranRacun();
            racun.Sacuvaj();
            storniranRacun.Sacuvaj(racun.idRacun);
            
            

            List<Stavka> ListStavke = new List<Stavka>();

            foreach(Stavka kliknuta_stavka in KolekcijaStorniran)
            {
                Stavka stavka = new Stavka(racun.idRacun,kliknuta_stavka.idArtikal,kliknuta_stavka.naziv,kliknuta_stavka.cijena,kliknuta_stavka.kolicina, storniranRacun.idStorniranRacun);
                ListStavke.Add(stavka);
            }

            Stavka.SacuvajStorniran(ListStavke);
            AzurirajArtikle();

            RacunStavke.Clear();
            KolekcijaStorniran.Clear();

            RacunListView.ItemsSource = null;
            StorniranListView.ItemsSource = null;

            RacunListView.ItemsSource = RacunStavke;
            StorniranListView.ItemsSource = KolekcijaStorniran;

            RacunBox.Text = null;
        }

        private void AzurirajArtikle()
        {
            KolekcijaArtikal = Artikal.Ucitaj();


        }
    }
}
