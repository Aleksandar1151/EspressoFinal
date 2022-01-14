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

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for ProdajaPage.xaml
    /// </summary>
    public partial class ProdajaPage : UserControl
    {
        public class KliknutaStavka
        {
            public int idartikla { get;set;}   
            public string naziv { get;set;}       
            public double cijena { get;set;}
            public int kolicina { get;set;}
            //public string ispisKolicina { get;set;}

            public KliknutaStavka()
            {

            }

            public KliknutaStavka(int id, string naziv, int kolicina, double cijena)
            {
                this.idartikla = id;
                this.naziv = naziv;               
                this.cijena = cijena;
                this.kolicina = kolicina;
               // this.ispisKolicina = "x"+kolicina;

            }
        }
        public static ObservableCollection<Artikal> KolekcijaArtikal { get;set;}
       // public static ObservableCollection<Stavka> RacunList { get;set;}
        public static ObservableCollection<KliknutaStavka> RacunStavke {get;set;}

        double UkupnaCijena = 0;

        List<Button> ListaDugmad = new List<Button>();

        int SIRINA = 220;
        int VISINA = 100;
        double KORAK = 2.3;
        int YSkok = 120;

        int trenutniX=5;
        int trenutniY=10;

         string darkColor = "#941B0C";
         string lightColor = "#C3B299";

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
                Button btnNew = new Button();
                btnNew.Tag = artikal.idArtikal;
                btnNew.Content = artikal.naziv.ToString()+"\n"+ artikal.cijena + " KM (" + artikal.kolicina +")";
                btnNew.Name="Button" + artikal.idArtikal.ToString();
                btnNew.Width=SIRINA;
                btnNew.Height = VISINA;
                btnNew.Click += new RoutedEventHandler(ArtikalButtonClick);
                // btnNew.Background = Brushes.White;
                ListaDugmad.Add(btnNew);

              
            }

            PostaviDugmad();
        }

        private void PostaviDugmad()
        {

            foreach(var item in ListaDugmad)
            {
                if(Convert.ToInt32(item.Tag) % 4 ==0)
                {
                    if(Convert.ToInt32(item.Tag) == 0)
                    {

                    }
                    else
                    {
                         trenutniY+=YSkok;
                    }
                    trenutniX = 5;
                   
                    Canvas.SetLeft(item,KORAK*trenutniX);
                    Canvas.SetTop(item,trenutniY);
                    trenutniX += 100;
                    
                }
                else
                {
                    Canvas.SetTop(item,trenutniY);
                    Canvas.SetLeft(item,KORAK*trenutniX);
                    trenutniX += 100;
                   
                }

                 
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
            Button kliknutoDugme=sender as Button;
            int index = Convert.ToInt32(kliknutoDugme.Tag);           
            Artikal kliknutArtikal = KolekcijaArtikal.ToList().Find(a => a.idArtikal == index);


            

            KliknutaStavka stavka = new KliknutaStavka(kliknutArtikal.idArtikal, kliknutArtikal.naziv.ToString(), 1 ,Convert.ToDouble(kliknutArtikal.cijena));   
            

            if(RacunStavke.Count == 0)
            {
                RacunStavke.Add(stavka);
            }
            else
            {
                int i = RacunStavke.ToList().FindIndex(item => item.idartikla == index);

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
            
          

            
            UkupnaCijena += stavka.cijena;
            ReceiptListView.ItemsSource = RacunStavke;
        
            UkupnoLabel.Content = "Ukupno: " + UkupnaCijena +" KM"; 
            
            

        }

    

        private ObservableCollection<Artikal> IzdvojiArtikle(int kategorija)
        {
            ObservableCollection<Artikal> ListRezultat = new ObservableCollection<Artikal>() ;

            foreach(Artikal artikal in KolekcijaArtikal)
            {
                if(artikal.kategorija == kategorija) ListRezultat.Add(artikal);
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

        }
    }
}
