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
using System.IO;
using iTextSharp.text;
using Paragraph = iTextSharp.text.Paragraph;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

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

        string idRacuna;
        int id_storniranog_racuna;

        public StornirajPage()
        {
            InitializeComponent();

            KolekcijaStorniran = new ObservableCollection<Stavka>();
        }

        private void PretraziRacun_Click(object sender, RoutedEventArgs e)
        {
            idRacuna = RacunBox.Text;                  
            racun.PretraziRacun(Convert.ToInt32(idRacuna));

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
            id_storniranog_racuna = storniranRacun.idStorniranRacun;
            

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

            Stampa_PDF(ListStavke);
        }

        private void Stampa_PDF(List<Stavka> ListStavke)
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
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc1.PageSize.Height, 0.75f);


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
            
            iTextSharp.text.Paragraph p4 = new Paragraph(new Chunk("MALOPRODAJNI REKLAMIRANI RACUN", font_naslov));

            iTextSharp.text.Paragraph p5 = new Paragraph(new Chunk("BROJ RAČUNA:" + id_storniranog_racuna.ToString(), font_naslov));
            
            //Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1) + "  "));
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F,95.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)) );
            p.Leading = 5 ;


            #region Header
            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;
            p3.Alignment = Element.ALIGN_CENTER;
            p4.Alignment = Element.ALIGN_CENTER;
            //p5.Alignment = Element.ALIGN_CENTER;
           // p.Alignment = Element.ALIGN_CENTER;

            doc1.Add(p1);
            doc1.Add(p2);
            doc1.Add(p3);
            doc1.Add(p);
            doc1.Add(p4);            
            doc1.Add(p5);  
            doc1.Add(p);
            #endregion




            double UkupnaCijena = 0;
            
            foreach (Stavka stavka in ListStavke)
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
            double pdvDio = UkupnaCijena * 0.17;
            double nesto = UkupnaCijena - pdvDio;
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

            iTextSharp.text.Paragraph p17 = new Paragraph(new Chunk("BR:", font));
            p17.Add(new Chunk(glue));
            p17.Add(new Chunk(idRacuna.ToString()+"  ",font));

            iTextSharp.text.Paragraph p18 = new Paragraph(new Chunk("KONOBAR:", font));
            p18.Add(new Chunk(glue));
            p18.Add(new Chunk(Properties.Settings.Default.Nalog_Naziv+"  ",font));

            

   
          
            
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
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, pdfWriter);

            //set the open action for our writer object
            pdfWriter.SetOpenAction(action);
            doc1.Close();
            System.Diagnostics.Process.Start("C:\\Racuni\\" + datum + ".pdf");
        }

        private void AzurirajArtikle()
        {
            KolekcijaArtikal = Artikal.Ucitaj();

            foreach(Stavka stornirana_stavka in KolekcijaStorniran)
            { 
                int index = KolekcijaArtikal.ToList().FindIndex(num => num.idArtikal == stornirana_stavka.idArtikal);
                KolekcijaArtikal[index].kolicina += stornirana_stavka.kolicina;
                Console.WriteLine("Stavka stornirana količina: "+stornirana_stavka.kolicina);
            }

            Artikal.Azuriraj(KolekcijaArtikal);
        }
    }
}
