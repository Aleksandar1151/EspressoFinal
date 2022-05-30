using EspressoFinal.Forms.Login;
using EspressoFinal.Forms.MainPage;
using EspressoFinal.Forms.Tabs;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.ObjectModel;
using EspressoFinal.Data;
using System.IO;

namespace EspressoFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static MainPage MainPage = new MainPage();

        #region PDF Varijable
        public static ObservableCollection<Racun> Racuni { get; set; }
        public static ObservableCollection<Racun> StorniraniRacuni { get; set; }

        public static ObservableCollection<Artikal> Artikli { get; set; }

        public static ObservableCollection<Stavka> prodaneStavke = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> ProdaneStavkeRacuna = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> ProdaneStavkeZaIspis = new ObservableCollection<Stavka>(); //stavke za dnevni izvjestaj

        public static ObservableCollection<Stavka> StorniraneStavke = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> StorniraneStavkeRacuna = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> StorniraneStavkeZaIspis = new ObservableCollection<Stavka>(); //stornirane stavke za dnevni izvjestaj
        public static ObservableCollection<Otpis> otpis = new ObservableCollection<Otpis>(); // otpis
        public static ObservableCollection<Stavka> MjesecneStavke = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> MjesecneStorniraneStavke = new ObservableCollection<Stavka>();

        public static ObservableCollection<Stavka> GodisnjeStavke = new ObservableCollection<Stavka>();
        public static ObservableCollection<Stavka> GodisnjeStorniraneStavke = new ObservableCollection<Stavka>();
        public static ObservableCollection<Artikal> ArtikliSkladiste = new ObservableCollection<Artikal>();
        #endregion


        public MainWindow()
        {
            InitializeComponent();


            if(LoginWindow.NalogPrivilegije == "nema")
            {
                ButtonTab3.IsEnabled = false;
                ButtonTab4.IsEnabled = false;
            }

            Title ="Esspreso - Nalog: " + Properties.Settings.Default.Nalog_Naziv;


            MainGrid.Children.Clear();
            ProdajaPage prodajaPage = new ProdajaPage();
            //Login LoginPage = new Login();
            MainGrid.Children.Add(prodajaPage);
        }

        private void Button_Transition(object sender, RoutedEventArgs e)
        {
            var pressedButton = (Button)sender;
            int index = int.Parse(pressedButton.Uid);

            string lightColor = "#CAD2C5";
            string darkColor = "#52796F";

            BrushConverter bc = new BrushConverter(); 
            
            ChangeButtonColors(ButtonTab1,darkColor,lightColor);
            ChangeButtonColors(ButtonTab2,darkColor,lightColor);
            ChangeButtonColors(ButtonTab3,darkColor,lightColor);
            ChangeButtonColors(ButtonTab4,darkColor,lightColor);
           // ChangeButtonColors(ButtonTab5,darkColor,lightColor);

            MainGrid.Children.Clear();
            switch (index)
            {
                 case 1:
                    {
                        ProdajaPage prodajaPage = new ProdajaPage();
                        MainGrid.Children.Add(prodajaPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 2:
                    {
                        StornirajPage stornirajPage = new StornirajPage();
                        MainGrid.Children.Add(stornirajPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 3:
                    {
                       
                        SkladistePage skladistePage = new SkladistePage();
                        MainGrid.Children.Add(skladistePage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 4:
                    {
                        NalogPage naloziPage = new NalogPage();
                        MainGrid.Children.Add(naloziPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    
            }

        }

        private void ChangeButtonColors(Button button, string foreColor, string backColor)
        {
            BrushConverter bc = new BrushConverter(); 
            button.Background = (Brush)bc.ConvertFrom(backColor); 
            button.Foreground = (Brush)bc.ConvertFrom(foreColor); 
            

        }
    
        private void ProdaneStavkeZaDnevniIzvjestaj()
        {
            string datum = DateTime.Today.ToString("dd-MM-yyyy"); 
            ProdaneStavkeZaIspis.Clear();
             

            Racuni = Racun.UcitajStornirane();

            Artikli = Artikal.Ucitaj();

            prodaneStavke = Stavka.UcitajDnevneStavkeBezStorniranih();



            foreach (Artikal artikal in Artikli)
            {
                Stavka stavka = new Stavka(0, artikal.idArtikal, artikal.naziv, artikal.cijena, 0);
                
                foreach(Stavka stavka1 in prodaneStavke)
                {

                    if(stavka1.idArtikal == artikal.idArtikal)
                    {
  
                        stavka.kolicina = stavka1.kolicina + stavka.kolicina;
                        
                    } 
                }

                if (stavka.kolicina != 0)
                {
                    ProdaneStavkeZaIspis.Add(stavka);
                }
            }

            StorniraneStavke = Stavka.UcitajStavkeRacunaSaStorniranimStavkama();

            otpis = Otpis.UcitajZaDnevniIzvjestaj();
            
        }

        private void MejsecniIzvjestaj()
        {
            ProdaneStavkeRacuna = Stavka.UcitajMjesecneStavkeBezStorniranih();
            Artikli = Artikal.Ucitaj();
            foreach (Artikal artikal in Artikli)
            {
                Stavka stavka = new Stavka(0, artikal.idArtikal, artikal.naziv, artikal.cijena, 0);

                foreach (Stavka stavka1 in ProdaneStavkeRacuna)
                {

                    if (stavka1.idArtikal == artikal.idArtikal)
                    {

                        stavka.kolicina = stavka1.kolicina + stavka.kolicina;

                    }
                }
                if (stavka.kolicina != 0)
                {
                    MjesecneStavke.Add(stavka);
                }
            }
            MjesecneStorniraneStavke = Stavka.UcitajMjesecneStavkeSaStorniranim();
            otpis = Otpis.UcitajZaMjesecniIzvjestaj();
        }

        private void GodisnjiIzvjestaj()
        {
            ProdaneStavkeRacuna = Stavka.UcitajGodisnjeStavkeBezStorniranih();
            Artikli = Artikal.Ucitaj();
            foreach (Artikal artikal in Artikli)
            {
                Stavka stavka = new Stavka(0, artikal.idArtikal, artikal.naziv, artikal.cijena, 0);

                foreach (Stavka stavka1 in ProdaneStavkeRacuna)
                {

                    if (stavka1.idArtikal == artikal.idArtikal)
                    {

                        stavka.kolicina = stavka1.kolicina + stavka.kolicina;

                    }
                }
                GodisnjeStavke.Add(stavka);
            }
            GodisnjeStorniraneStavke = Stavka.UcitajGodisnjeStavkeSaStorniranim();
            otpis = Otpis.UcitajZaGodisnjiIzvjestaj();
        }

        private void DnevniIzvjestaj_Click(object sender, RoutedEventArgs e)
        {
            ProdaneStavkeZaDnevniIzvjestaj();
            string dir = @"C:\\Izvjestaji";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string datum = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string path = "C:\\Izvjestaji\\DI-" + datum + ").pdf";
            Document doc1 = new Document();
            doc1.SetMargins(5, 0, 0, 0);
            doc1.SetPageSize(new iTextSharp.text.Rectangle(595, 842));
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(path, FileMode.Create));
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc1.PageSize.Height, 0.75f);





            doc1.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            //Font font = FontFactory.GetFont("HELVETICA", 6);
             
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font font1 = new Font(bf,20);
            Font font2 = new Font(bf,15);
            Font font3 = new Font(bf,10);



            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph(new Chunk("ESPRESSO DB", font1));
            iTextSharp.text.Paragraph p11 = new iTextSharp.text.Paragraph();
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(new Chunk("DNEVNI IZVJESTAJ ZA DATUM " + DateTime.Today.ToString("dd-MM-yyyy"), font2));
            iTextSharp.text.Paragraph p2_1 = new iTextSharp.text.Paragraph(new Chunk("VRIJEME: " + DateTime.Now.ToString("HH:mm"), font2));
            iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph(new Chunk("TABELA PRODANIH STAVKI", font2));
            PdfPTable table1 = new PdfPTable(5);
            table1.AddCell("ID artikla");
            table1.AddCell("Naziv");
            table1.AddCell("Cijena [KM]");
            table1.AddCell("Kolicina [kom.]");
            table1.AddCell("Ukupno [KM]");
            double Ukupno1 = 0;
            foreach(Stavka stavka in ProdaneStavkeZaIspis)
            {
                table1.AddCell(new PdfPCell(new Phrase(stavka.idArtikal.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase((stavka.kolicina * stavka.cijena).ToString(),font3)));
                Ukupno1 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell.Colspan = 4;
            table1.AddCell(cell);
            table1.AddCell(new PdfPCell(new Phrase(Ukupno1.ToString(),font2)));
            iTextSharp.text.Paragraph p4 = new iTextSharp.text.Paragraph(new Chunk("TABELA STORNIRANIH STAVKI", font2));
            PdfPTable table2 = new PdfPTable(5);
            table2.AddCell("ID storniranog racuna");
            table2.AddCell("ID racuna");
            table2.AddCell("Naziv");
            table2.AddCell("Cijena [KM]");
            table2.AddCell("Kolicina [kom.]");
            double Ukupno2 = 0;
            foreach (Stavka stavka in StorniraneStavke)
            {
                table2.AddCell(new PdfPCell(new Phrase(stavka.idStorniranRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.idRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));
    
                Ukupno2 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell1 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell1.Colspan = 4;
            table2.AddCell(cell1);
            table2.AddCell(new PdfPCell(new Phrase(Ukupno2.ToString(),font2)));
            iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph(new Chunk("TABELA OTPISA", font2));
            PdfPTable table3 = new PdfPTable(4);
            table3.AddCell("ID ");            
            table3.AddCell("Naziv");
            table3.AddCell("Cijena [KM]");
            table3.AddCell("Kolicina");
            double Ukupno3 = 0;
            foreach (Otpis stavka in otpis)
            {
               table3.AddCell(new PdfPCell(new Phrase(stavka.idOtpis.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.nazivArtikla.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));

                Ukupno3 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell2 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell2.Colspan = 3;
            table3.AddCell(cell2);

            table3.AddCell(Ukupno3.ToString());
            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;
            p2_1.Alignment = Element.ALIGN_CENTER;
            p3.Alignment = Element.ALIGN_CENTER;
            p4.Alignment = Element.ALIGN_CENTER;
            p5.Alignment = Element.ALIGN_CENTER;
            doc1.Add(p1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p2);            
            doc1.Add(p2_1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p3);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE); 
            doc1.Add(p4);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table2);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p5);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table3);

            doc1.Close();
            System.Diagnostics.Process.Start("C:\\Izvjestaji\\DI-" + datum + ").pdf");
        }

        private void Mjesecni_izvjestaj(object sender, RoutedEventArgs e)
        {
            MejsecniIzvjestaj();
            string dir = @"C:\\Izvjestaji";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string datum = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string path = "C:\\Izvjestaji\\MI-(" + datum + ").pdf";
            Document doc1 = new Document();
            doc1.SetMargins(5, 0, 0, 0);
            doc1.SetPageSize(new iTextSharp.text.Rectangle(595, 842));
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(path, FileMode.Create));
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc1.PageSize.Height, 0.75f);





            doc1.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            //Font font = FontFactory.GetFont("HELVETICA", 6);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font font1 = new Font(bf,20);
            Font font2 = new Font(bf,15);
            Font font3 = new Font(bf,10);
            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph(new Chunk("ESPRESSO DB", font1));
            iTextSharp.text.Paragraph p11 = new iTextSharp.text.Paragraph();
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(new Chunk("MJESECNI IZVJESTAJ ZA " + DateTime.Today.ToString("dd-MM-yyyy").Split('-')[1] + ". MJESEC", font2));
            iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph(new Chunk("TABELA PRODANIH STAVKI", font2));
            PdfPTable table1 = new PdfPTable(5);
            table1.AddCell("ID artikla");
            table1.AddCell("Naziv");
            table1.AddCell("Cijena [KM]");
            table1.AddCell("Kolicina [kom.]");
            table1.AddCell("Ukupno [KM]");
            double Ukupno1 = 0;
            foreach (Stavka stavka in MjesecneStavke)
            {
                table1.AddCell(new PdfPCell(new Phrase(stavka.idArtikal.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase((stavka.kolicina * stavka.cijena).ToString(),font3)));

                Ukupno1 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell.Colspan = 4;
            table1.AddCell(cell);
            table1.AddCell(new PdfPCell(new Phrase(Ukupno1.ToString(),font2)));
            
            iTextSharp.text.Paragraph p4 = new iTextSharp.text.Paragraph(new Chunk("TABELA STORNIRANIH STAVKI", font2));
            PdfPTable table2 = new PdfPTable(5);
            table2.AddCell("ID storniranog racuna");
            table2.AddCell("ID racuna");
            table2.AddCell("Naziv");
            table2.AddCell("Cijena [KM]");
            table2.AddCell("Kolicina [kom.]");
            double Ukupno2 = 0;
            foreach (Stavka stavka in MjesecneStorniraneStavke)
            {
                table2.AddCell(new PdfPCell(new Phrase(stavka.idStorniranRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.idRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));
    
                Ukupno2 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell1 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell1.Colspan = 4;
            table2.AddCell(cell1);
            table2.AddCell(new PdfPCell(new Phrase(Ukupno2.ToString(),font2)));
            

            iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph(new Chunk("TABELA OTPISA", font2));
            PdfPTable table3 = new PdfPTable(4);
            table3.AddCell("ID storniranog racuna");
            table3.AddCell("ID racuna");
            table3.AddCell("Naziv");
            table3.AddCell("Cijena [KM]");
            double Ukupno3 = 0;
            foreach (Otpis stavka in otpis)
            {
                table3.AddCell(new PdfPCell(new Phrase(stavka.idOtpis.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.nazivArtikla.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));

                Ukupno3 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell2 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell2.Colspan = 3;
            table3.AddCell(cell2);            
            table3.AddCell(new PdfPCell(new Phrase(Ukupno3.ToString(),font2)));

            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;
            p3.Alignment = Element.ALIGN_CENTER;
            p4.Alignment = Element.ALIGN_CENTER;
            p5.Alignment = Element.ALIGN_CENTER;
            doc1.Add(p1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p2);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p3);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p4);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table2);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p5);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table3);
            doc1.Close();
            System.Diagnostics.Process.Start("C:\\Izvjestaji\\MI-(" + datum + ").pdf");
        }

        private void GodisnjiIzvjestaj_Click(object sender, RoutedEventArgs e)
        {
            GodisnjiIzvjestaj();
            string dir = @"C:\\Izvjestaji";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string datum = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string path = "C:\\Izvjestaji\\GI-(" + datum + ").pdf";
            Document doc1 = new Document();
            doc1.SetMargins(5, 0, 0, 0);
            doc1.SetPageSize(new iTextSharp.text.Rectangle(595, 842));
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(path, FileMode.Create));
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc1.PageSize.Height, 0.75f);





            doc1.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            //Font font = FontFactory.GetFont("HELVETICA", 6);
         BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font font1 = new Font(bf,20);
            Font font2 = new Font(bf,15);
            Font font3 = new Font(bf,10);
            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph(new Chunk("ESPRESSO DB", font1));
            iTextSharp.text.Paragraph p11 = new iTextSharp.text.Paragraph();
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(new Chunk("GODISNJI IZVJESTAJ ZA " + DateTime.Today.ToString("dd-MM-yyyy").Split('-')[2] + ". GODINU", font2));
            iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph(new Chunk("TABELA PRODANIH STAVKI", font2));
            PdfPTable table1 = new PdfPTable(5);
            table1.AddCell("ID artikla");
            table1.AddCell("Naziv");
            table1.AddCell("Cijena [KM]");
            table1.AddCell("Kolicina [kom.]");
            table1.AddCell("Ukupno [KM]");
            double Ukupno1 = 0;
            foreach (Stavka stavka in GodisnjeStavke)
            {
               table1.AddCell(new PdfPCell(new Phrase(stavka.idArtikal.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase((stavka.kolicina * stavka.cijena).ToString(),font3)));
                Ukupno1 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell.Colspan = 4;
            table1.AddCell(cell);
             table1.AddCell(new PdfPCell(new Phrase(Ukupno1.ToString(),font2)));

            iTextSharp.text.Paragraph p4 = new iTextSharp.text.Paragraph(new Chunk("TABELA STORNIRANIH STAVKI", font2));
            PdfPTable table2 = new PdfPTable(5);
            table2.AddCell("ID storniranog racuna");
            table2.AddCell("ID racuna");
            table2.AddCell("Naziv");
            table2.AddCell("Cijena [KM]");
            table2.AddCell("Kolicina [kom.]");
            double Ukupno2 = 0;
            foreach (Stavka stavka in GodisnjeStorniraneStavke)
            {
                 table2.AddCell(new PdfPCell(new Phrase(stavka.idStorniranRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.idRacun.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.naziv.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table2.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));

                Ukupno2 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell1 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell1.Colspan = 4;
            table2.AddCell(cell1);
            table2.AddCell(new PdfPCell(new Phrase(Ukupno2.ToString(),font2)));

            iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph(new Chunk("TABELA OTPISA", font2));
            PdfPTable table3 = new PdfPTable(4);
            table3.AddCell("ID storniranog racuna");
            table3.AddCell("ID racuna");
            table3.AddCell("Naziv");
            table3.AddCell("Cijena [KM]");
            double Ukupno3 = 0;
            foreach (Otpis stavka in otpis)
            {
                table3.AddCell(new PdfPCell(new Phrase(stavka.idOtpis.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.nazivArtikla.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.cijena.ToString(),font3)));
                table3.AddCell(new PdfPCell(new Phrase(stavka.kolicina.ToString(),font3)));

                Ukupno3 += stavka.cijena * stavka.kolicina;
            }
            PdfPCell cell2 = new PdfPCell(new Phrase("Ukupno [KM]"));
            cell2.Colspan = 3;
            table3.AddCell(cell2);
            table3.AddCell(new PdfPCell(new Phrase(Ukupno3.ToString(),font2)));

            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;
            p3.Alignment = Element.ALIGN_CENTER;
            p4.Alignment = Element.ALIGN_CENTER;
            p5.Alignment = Element.ALIGN_CENTER;
            doc1.Add(p1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p2);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p3);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p4);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table2);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p5);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(table3);
            doc1.Close();
            System.Diagnostics.Process.Start("C:\\Izvjestaji\\GI-(" + datum + ").pdf");
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            Window loginWindow = new EspressoFinal.Forms.Login.LoginWindow();
            loginWindow.Show();
            this.Close();

        }

        private void Popis_Click(object sender, RoutedEventArgs e)
        {
            ArtikliSkladiste = Artikal.Ucitaj();
            GodisnjiIzvjestaj();

            string dir = @"C:\\Izvjestaji";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string datum = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
            string path = "C:\\Izvjestaji\\" + "Popis("+datum + ").pdf";
            Document doc1 = new Document();
            doc1.SetMargins(5, 0, 0, 0);
            doc1.SetPageSize(new iTextSharp.text.Rectangle(595, 842));
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(path, FileMode.Create));
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc1.PageSize.Height, 0.75f);





            doc1.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            //Font font = FontFactory.GetFont("HELVETICA", 6);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font font1 = new Font(bf,20);
            Font font2 = new Font(bf,15);
            Font font3 = new Font(bf,10);
            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph(new Chunk("ESPRESSO DB", font1));
           
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(new Chunk("POPIS ZA DATUM " + DateTime.Today.ToString("dd-MM-yyyy"), font2));
            PdfPTable table1 = new PdfPTable(4);

            table1.AddCell("ID artikla");
            table1.AddCell("Naziv");
            table1.AddCell("Cijena [KM]");
            table1.AddCell("Kolicina [kom.]");           
           
            foreach (Artikal artikal in ArtikliSkladiste)
            {
                table1.AddCell(new PdfPCell(new Phrase(artikal.idArtikal.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(artikal.naziv.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(artikal.cijena.ToString(),font3)));
                table1.AddCell(new PdfPCell(new Phrase(artikal.kolicina.ToString(),font3)));
                
              
            }

            p1.Alignment = Element.ALIGN_CENTER;
            p2.Alignment = Element.ALIGN_CENTER;           
            doc1.Add(p1);
            doc1.Add(Chunk.NEWLINE);
            doc1.Add(p2);
            doc1.Add(Chunk.NEWLINE);          
            doc1.Add(table1);
             doc1.Close();
            System.Diagnostics.Process.Start("C:\\Izvjestaji\\" + "Popis("+datum + ").pdf");
        }
    }
}
