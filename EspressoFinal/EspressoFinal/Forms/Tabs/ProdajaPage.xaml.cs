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
using EspressoFinal.Data;
using System.Linq;
using EspressoFinal.Forms.Login;

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for ProdajaPage.xaml
    /// </summary>
    public partial class ProdajaPage : UserControl
    {
        
        public static ObservableCollection<Artikal> KolekcijaArtikal { get;set;}
       
        public static ObservableCollection<KliknutaStavka> RacunStavke {get;set;}

        double UkupnaCijena = 0;
        int kliknutIdStavka;

        List<Button> ListaDugmad = new List<Button>();
       

        int SIRINA = 220;
        int VISINA = 100;
        double KORAK = 2.3;
        int YSkok = 120;

        int trenutniX=5;
        int trenutniY=10;

         string darkColor = "#52796F";
         string lightColor = "#CAD2C5";

        string redColor = "#ae2012";

        public ProdajaPage()
        {
            InitializeComponent();
            KolekcijaArtikal = Artikal.Ucitaj();
            NapraviDugmad(IzdvojiArtikle(Convert.ToInt32(1)));

            RacunStavke = new ObservableCollection<KliknutaStavka>();

            ReceiptListView.ItemsSource = RacunStavke;
        }

        private void NapraviDugmad(ObservableCollection<Artikal> Kolekcija)
        {
            foreach(Artikal artikal in Kolekcija)
            {
                if(artikal.kolicina > 0)
                {
                    Button btnNew = new Button();
                    btnNew.Tag = artikal.idArtikal;
                    btnNew.Content = artikal.naziv.ToString()+"\n"+ artikal.cijena + " KM (" + artikal.kolicina +")";
                    btnNew.Name="Button" + artikal.idArtikal.ToString();
                    btnNew.Width=SIRINA;
                    btnNew.Height = VISINA;
                    btnNew.Click += new RoutedEventHandler(ArtikalButtonClick);

                    BrushConverter bc = new BrushConverter(); 
                    btnNew.Background = (Brush)bc.ConvertFrom("#2F3E46"); 
                    btnNew.Foreground = (Brush)bc.ConvertFrom("#CAD2C5"); 
                    
                    ListaDugmad.Add(btnNew);
                }
                
                

              
            }

            PostaviDugmad();
        }

        private void PostaviDugmad()
        {
            int noviRed = 1;
            foreach(var item in ListaDugmad)
            {
                if(noviRed % 4 ==0)
                {
                    trenutniY+=YSkok;                    
                    trenutniX = 5;
                   
                    Canvas.SetLeft(item,KORAK*trenutniX);
                    Canvas.SetTop(item,trenutniY);
                    trenutniX += 100;
                    noviRed = 1;
                }
                else
                {
                    Canvas.SetTop(item,trenutniY);
                    Canvas.SetLeft(item,KORAK*trenutniX);
                    trenutniX += 100;
                   
                }

                 noviRed++;
                 ButtonCanvas.Children.Add(item);
            }
        }

        private void ChangeMenuClick(object sender, RoutedEventArgs e)
        {
            RefreshCanvas();
            var kliknutoDugmeMeni = (Button)sender;
            ChangeButtonColors(kliknutoDugmeMeni,lightColor,darkColor);
            NapraviDugmad(IzdvojiArtikle(Convert.ToInt32(kliknutoDugmeMeni.Uid)));

        }

        private void ArtikalButtonClick(object sender, RoutedEventArgs e)
        {       
            Button kliknutoDugme = sender as Button;
            int index = Convert.ToInt32(kliknutoDugme.Tag);           
            Artikal kliknutArtikal = KolekcijaArtikal.ToList().Find(a => a.idArtikal == index);

            kliknutArtikal.kolicina -= 1;
            kliknutoDugme.Content = kliknutArtikal.naziv.ToString()+"\n"+ kliknutArtikal.cijena + " KM (" + kliknutArtikal.kolicina +")";
            
            if(kliknutArtikal.kolicina == 0)
            {
                BrushConverter bc = new BrushConverter(); 
                kliknutoDugme.Background = (Brush)bc.ConvertFrom(redColor); 
                kliknutoDugme.IsEnabled = false;
            }

            KliknutaStavka stavka = new KliknutaStavka(kliknutArtikal.idArtikal, kliknutArtikal.naziv.ToString(), 1 ,Convert.ToDouble(kliknutArtikal.cijena));   
            
            if(RacunStavke.Count == 0)
            {
                RacunStavke.Add(stavka);
            }
            else
            {
                int i = RacunStavke.ToList().FindIndex(item => item.idArtikal == index);

                if(i != -1)
                {
                    RacunStavke[i].kolicina += 1;
                    Console.WriteLine("Kolicina="+RacunStavke[i].kolicina);
                    ReceiptListView.ItemsSource = null;
                }
                else
                {
                    RacunStavke.Add(stavka);
                }

            }    
           
            ReceiptListView.ItemsSource = RacunStavke;

            UkupnaCijena += stavka.cijena;
            UkupnoLabel.Content = "Ukupno: " + UkupnaCijena +" KM"; 
        }

        private ObservableCollection<Artikal> IzdvojiArtikle(int kategorija)
        {
            ObservableCollection<Artikal> ListRezultat = new ObservableCollection<Artikal>() ;

            foreach(Artikal artikal in KolekcijaArtikal)
            {
                if(artikal.kategorija == kategorija)
                {
                    ListRezultat.Add(artikal);
                    
                }
                
            }
            return ListRezultat;
        }

        private void RefreshCanvas()
        {
            ButtonCanvas.Children.Clear();
            SIRINA = 220;
            VISINA = 100;
            KORAK = 2.3;
            YSkok = 120;
            trenutniX=5;
            trenutniY=10;

            ListaDugmad.Clear();

            ChangeButtonColors(ButtonMenu1,darkColor,lightColor);
            ChangeButtonColors(ButtonMenu2,darkColor,lightColor);
            ChangeButtonColors(ButtonMenu3,darkColor,lightColor);
            ChangeButtonColors(ButtonMenu4,darkColor,lightColor);
            ChangeButtonColors(ButtonMenu5,darkColor,lightColor);
        }

        private void ChangeButtonColors(Button button, string foreColor, string backColor)
        {
            BrushConverter bc = new BrushConverter(); 
            button.Background = (Brush)bc.ConvertFrom(backColor); 
            button.Foreground = (Brush)bc.ConvertFrom(foreColor); 

        }

        private void StampajClick(object sender, RoutedEventArgs e)
        {
            Racun racun = new Racun();
            racun.Sacuvaj();

            List<Stavka> ListStavke = new List<Stavka>();

            foreach(KliknutaStavka kliknuta_stavka in RacunStavke)
            {
                Stavka stavka = new Stavka(racun.idRacun,kliknuta_stavka.idArtikal,kliknuta_stavka.naziv,kliknuta_stavka.cijena,kliknuta_stavka.kolicina);
                ListStavke.Add(stavka);
            }

            Stavka.Sacuvaj(ListStavke);

            OsvjeziRacun();
        }

        private void OtpisiClick(object sender, RoutedEventArgs e)
        {
            
            List<Otpis> ListOtpis = new List<Otpis>();

            foreach(KliknutaStavka kliknuta_stavka in RacunStavke)
            { 
                Otpis otpis = new Otpis(Login.LoginWindow.IDNalog, kliknuta_stavka.idArtikal, kliknuta_stavka.kolicina);
                ListOtpis.Add(otpis);
            }

            Otpis.Sacuvaj(ListOtpis);            
           
            OsvjeziRacun();

        }

        private void OsvjeziRacun()
        {
            Artikal.Azuriraj(KolekcijaArtikal);
            RacunStavke.Clear();
            UkupnaCijena = 0;
            UkupnoLabel.Content = "Ukupno: " + UkupnaCijena +" KM"; 
        }

        private void ListStavka_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (sender as ListView).SelectedItem; 
                if (item != null)
                {
                    System.Windows.Controls.ListView list = (System.Windows.Controls.ListView)sender;
                    KliknutaStavka izabranaStavka = (KliknutaStavka)list.SelectedItem;
                    kliknutIdStavka = izabranaStavka.idArtikal;

                    int indexArtikal = KolekcijaArtikal.ToList().FindIndex(num => num.idArtikal == kliknutIdStavka);
                    KolekcijaArtikal[indexArtikal].kolicina += 1;


                    int indexDugme = ListaDugmad.FindIndex(num => Convert.ToInt32(num.Tag) == kliknutIdStavka);
                    ListaDugmad[indexDugme].Content = KolekcijaArtikal[indexArtikal].naziv.ToString()+"\n"+ KolekcijaArtikal[indexArtikal].cijena + " KM (" + KolekcijaArtikal[indexArtikal].kolicina +")";

                    if( KolekcijaArtikal[indexArtikal].kolicina == 1)
                    {
                        ListaDugmad[indexDugme].IsEnabled = true;
                        BrushConverter bc = new BrushConverter(); 
                        ListaDugmad[indexDugme].Background = (Brush)bc.ConvertFrom("#2F3E46"); 
                    }

                    int index = RacunStavke.ToList().FindIndex(num => num.idArtikal == kliknutIdStavka);
                    RacunStavke[index].kolicina -= 1;

                    UkupnaCijena -= RacunStavke[index].cijena;

                    if(UkupnaCijena < 0.1)
                    {
                        UkupnaCijena = 0;
                    }
                    UkupnoLabel.Content = "Ukupno: " + UkupnaCijena +" KM";

                    if(RacunStavke[index].kolicina == 0)
                    {
                        RacunStavke.RemoveAt(index);
                    }

                }
               (sender as ListView).SelectedItem = null;
                ReceiptListView.ItemsSource = null;

                ReceiptListView.ItemsSource = RacunStavke;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    
        
        
        
        
    }

}
