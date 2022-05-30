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
using System.IO;
using iTextSharp.text;
using Paragraph = iTextSharp.text.Paragraph;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

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
       

        int SIRINA = 210;
        int VISINA = 100;
        double KORAK = 2.3;
        int YSkok = 120;

        int trenutniX=5;
        int trenutniY=10;
        double canvas_dynamic_height = 0;

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
           // button_height = ButtonCanvas.Height;
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

            if(canvas_dynamic_height < ListaDugmad.Count * 40 && ListaDugmad.Count > 18)
            {
                for (int i = 0; i*3 < ListaDugmad.Count; i++)
                {
                     canvas_dynamic_height += YSkok;
                }
                ButtonCanvas.Height = canvas_dynamic_height;
            }
            else
            {

            }
            

           // ButtonCanvas.Height = button_height;
            int broj_reda=1;
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

                    broj_reda++;
                   // if(ListaDugmad.Count > broj_reda*3)
                    //ButtonCanvas.Height += YSkok;

                    Console.WriteLine("ButtonCanvas.Height: "+ButtonCanvas.Height +" ListaDugmad.Count "+ ListaDugmad.Count);
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
            UkupnoLabel.Content = "Ukupno: " + UkupnaCijena.ToString("0.00") +" KM"; 
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
            if(RacunStavke.Count > 0)
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

                StampaPDF(racun.idRacun);

                OsvjeziRacun();
            }
            else
            {
                MessageBox.Show("Unesite stavke.");
            }
            
        }

        private void OtpisiClick(object sender, RoutedEventArgs e)
        {
            string datum = DateTime.Today.ToString("dd-MM-yyyy");
            List<Otpis> ListOtpis = new List<Otpis>();

            foreach(KliknutaStavka kliknuta_stavka in RacunStavke)
            { 
                Otpis otpis = new Otpis(Login.LoginWindow.IDNalog, kliknuta_stavka.idArtikal, kliknuta_stavka.kolicina, datum);
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
            UkupnoLabel.Content = "Ukupno: " + UkupnaCijena.ToString("0.00") +" KM"; 
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
                    UkupnoLabel.Content = "Ukupno: " + UkupnaCijena.ToString("0.00") +" KM";

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

        #region Stampa

        private void StampaPDF(int id)
        {
            Chunk glue = new Chunk(new VerticalPositionMark());


            string dir = @"C:\\Racuni";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string datum = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string path = "C:\\Racuni\\" + datum + ".pdf";
            Document doc1 = new Document();
            doc1.SetMargins(5, 0, 0, 0);
            doc1.SetPageSize(new iTextSharp.text.Rectangle(100, 300));
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(path, FileMode.Create));


            

            doc1.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font font_naslov = new Font(bf,5);
            Font font = new Font(bf,6);


            //Font font = FontFactory.GetFont("TIMES", 6);
            //Font font_naslov = FontFactory.GetFont("TIMES", 5);
            iTextSharp.text.Paragraph p1 = new Paragraph(new Chunk("YOKOSP", font));
           // iTextSharp.text.Paragraph p2 = new Paragraph(new Chunk("Konobar: "+Properties.Settings.Default.Nalog_Naziv , font));
            iTextSharp.text.Paragraph p2 = new Paragraph(new Chunk("IVE ANDRIĆA 3", font));
            iTextSharp.text.Paragraph p3 = new Paragraph(new Chunk("78000 BANJA LUKA", font));
            
            iTextSharp.text.Paragraph p4 = new Paragraph(new Chunk("MALOPRODAJNI FISKALNI RACUN", font_naslov));

            //Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1) + "  "));
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F,95.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)) );
           p.Leading = 5 ;


            #region Header
            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;
            p3.Alignment = Element.ALIGN_CENTER;
            p4.Alignment = Element.ALIGN_CENTER;
           // p.Alignment = Element.ALIGN_CENTER;

            doc1.Add(p1);
            doc1.Add(p2);
            doc1.Add(p3);
            doc1.Add(p);
            doc1.Add(p4);            
            doc1.Add(p);
            #endregion


            foreach (KliknutaStavka stavka in RacunStavke)
            {
                 Paragraph p_stavke = new Paragraph();
                 Paragraph p_stavke_naziv = new Paragraph();

                 p_stavke_naziv.Leading = 8;
                 p_stavke.Leading = 6;

                UkupnaCijena += stavka.kolicina * stavka.cijena;
                p_stavke_naziv.Add(new Chunk((""+stavka.naziv),font));
                p_stavke.Add(new Chunk("  "+stavka.kolicina+"x",font));
                p_stavke.Add(new Chunk(glue));
                //Console.WriteLine("{0,-15}x{1,-5}{2,0:0.00} KM",imena[i],kolicine[i],cijene[i]);
               // p_stavke.Add(new Chunk(stavka.kolicina+"x" + "          " + stavka.cijena+" KM  \n",font));

                string kolicina_cijena = String.Format("{0,-15:0.00} {1,3:0.00} KM",stavka.cijena, stavka.cijena*stavka.kolicina);
                p_stavke.Add(new Chunk(kolicina_cijena,font));
                
                
                //p_stavke.Add(new Chunk(glue));
                //p_stavke.Add(new Chunk(stavka.cijena+"KM  \n",font));
                
                //document.add(p);
               // stavke += stavka.naziv + "              " + stavka.kolicina + "x                " + stavka.cijena + "\n";

                doc1.Add(p_stavke_naziv);
                
                doc1.Add(p_stavke);
               
            }
            doc1.Add(p);
            UkupnaCijena /= 2;
            double pdvDio = UkupnaCijena * 0.17;
            double nesto = UkupnaCijena - pdvDio;
            //iTextSharp.text.Paragraph p6 = new Paragraph(new Chunk(stavke, font));
            //iTextSharp.text.Paragraph p7 = new Paragraph(new Chunk("CE:             17.00%", font));
            iTextSharp.text.Paragraph p7 = new Paragraph(new Chunk("CE:", font));
            p7.Add(new Chunk(glue));
            p7.Add("17.00%");

            //iTextSharp.text.Paragraph p8 = new Paragraph(new Chunk("VE:             " + , font));
            iTextSharp.text.Paragraph p8 = new Paragraph(new Chunk("VE:", font));
            p8.Add(new Chunk(glue));
            p8.Add(new Chunk(pdvDio.ToString()+"  ",font));

            iTextSharp.text.Paragraph p9 = new Paragraph(new Chunk("VU:", font));
            p9.Add(new Chunk(glue));
            p9.Add(new Chunk(pdvDio.ToString("0.00")+"  ",font));

            iTextSharp.text.Paragraph p10 = new Paragraph(new Chunk("PE:", font));
            p10.Add(new Chunk(glue));
            p10.Add(new Chunk(UkupnaCijena.ToString("0.00")+"  ",font));

            iTextSharp.text.Paragraph p11 = new Paragraph(new Chunk("PU:", font));
            p11.Add(new Chunk(glue));
            p11.Add(new Chunk(UkupnaCijena.ToString("0.00")+"  ",font));

            iTextSharp.text.Paragraph p12 = new Paragraph(new Chunk("CE:", font));
            p12.Add(new Chunk(glue));
            p12.Add(new Chunk(nesto.ToString("0.00")+"  ",font));

            iTextSharp.text.Paragraph p13 = new Paragraph(new Chunk("ZA UPLATU:", font));
            p13.Add(new Chunk(glue));
            p13.Add(new Chunk(UkupnaCijena.ToString("0.00")+" KM  ",font));

            iTextSharp.text.Paragraph p14 = new Paragraph(new Chunk("GOTOVINA:", font));
            p14.Add(new Chunk(glue));
            p14.Add(new Chunk(UkupnaCijena.ToString("0.00")+" KM  ",font));

            iTextSharp.text.Paragraph p15 = new Paragraph(new Chunk("UPLACENO:", font));
            p15.Add(new Chunk(glue));
            p15.Add(new Chunk(UkupnaCijena.ToString("0.00")+" KM  ",font));

            iTextSharp.text.Paragraph p16 = new Paragraph(new Chunk("POVRAT:", font));
            p16.Add(new Chunk(glue));
            p16.Add(new Chunk("0.00"+" KM  ",font));

            iTextSharp.text.Paragraph p17 = new Paragraph(new Chunk("BROJ RACUNA:", font));
            p17.Add(new Chunk(glue));
            p17.Add(new Chunk(id.ToString()+"  ",font));

            iTextSharp.text.Paragraph p18 = new Paragraph(new Chunk("KONOBAR:", font));
            p18.Add(new Chunk(glue));
            p18.Add(new Chunk(Properties.Settings.Default.Nalog_Naziv+"  ",font));


          
            

            //p6.Alignment = Element.ALIGN_LEFT;
            //p7.Alignment = Element.ALIGN_LEFT;
            //p8.Alignment = Element.ALIGN_LEFT;
            //p9.Alignment = Element.ALIGN_LEFT;
            //p10.Alignment = Element.ALIGN_LEFT;
            //p11.Alignment = Element.ALIGN_LEFT;
            //p12.Alignment = Element.ALIGN_LEFT;
            //p13.Alignment = Element.ALIGN_LEFT;
            //p14.Alignment = Element.ALIGN_LEFT;
            //p15.Alignment = Element.ALIGN_LEFT;
            //p16.Alignment = Element.ALIGN_LEFT;
            //p17.Alignment = Element.ALIGN_LEFT;
            //p18.Alignment = Element.ALIGN_LEFT;

           
           
           
           
            //doc1.Add(p);
            doc1.Add(p7);
            doc1.Add(p8);
            doc1.Add(p9);
            doc1.Add(p10);
            doc1.Add(p11);
            doc1.Add(p12);
            doc1.Add(p);
            doc1.Add(p13);
            doc1.Add(p14);
            doc1.Add(p15);
            doc1.Add(p16);
            doc1.Add(p17);
            doc1.Add(p18);
            doc1.Add(p);
            doc1.Close();
            System.Diagnostics.Process.Start("C:\\Racuni\\" + datum + ".pdf");
        }

        #endregion





    }

}
