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

namespace EspressoFinal.Forms.Tabs
{
    /// <summary>
    /// Interaction logic for ProdajaPage.xaml
    /// </summary>
    public partial class ProdajaPage : UserControl
    {
        public static ObservableCollection<Artikal> KolekcijaArtikal { get;set;}
        List<Button> ListaDugmad = new List<Button>();
        int SIRINA = 200;
        int VISINA = 100;
        double KORAK = 1;

        int trenutniX=1;
        int trenutniY=1;

        public ProdajaPage()
        {
            InitializeComponent();
            KolekcijaArtikal = Artikal.Ucitaj();
            NapraviDugmad();

            visinaBox.Text = VISINA.ToString();
            sirinaBox.Text = SIRINA.ToString();
            korakBox.Text = KORAK.ToString();
        }

        private void NapraviDugmad()
        {
            foreach(Artikal artikal in KolekcijaArtikal)
            {
                if(artikal.kategorija == 1)
                {
                    
                    Button btnNew = new Button();
                    btnNew.Tag = artikal.idArtikal;
                    btnNew.Content = artikal.naziv.ToString();
                    btnNew.Name="Button" + artikal.idArtikal.ToString();
                    btnNew.Width=SIRINA;
                    btnNew.Height = VISINA;
                   // btnNew.Background = Brushes.White;
                    ListaDugmad.Add(btnNew);

                    
                }
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
                         trenutniY+=50;
                    }
                    trenutniX = 10;
                   
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

        private void Click(object sender, RoutedEventArgs e)
        {
            SIRINA = Convert.ToInt32(sirinaBox.Text);
            VISINA = Convert.ToInt32(visinaBox.Text);
            KORAK = Convert.ToDouble(korakBox.Text);

            trenutniX = 1;
            trenutniY = 1;

            ListaDugmad.Clear();

            ButtonCanvas.Children.Clear();
            
            NapraviDugmad();
            
        }
    }
}
