using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace EspressoFinal.Data
{
    public class Otpis
    {
        public int idOtpis;
        public int idArtikal;
        public int kolicina;
        public int idNalog;

        public string nazivArtikla;
        public double cijena;
        public string datum;
       

        public Otpis()
        {}
        public Otpis(int idNalog, int idArtikal , int kolicina )
        {
            
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
            this.idNalog = idNalog;           
        }

        public Otpis(int idOtpis,int idArtikal , int kolicina, int idNalog)
        {
            this.idOtpis = idOtpis;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
            this.idNalog = idNalog;           
        }

        public Otpis(int idOtpis, string nazivArtikla, double cijena, int kolicina)
        {
            this.idOtpis = idOtpis;
            this.nazivArtikla = nazivArtikla;
            this.kolicina = kolicina;
            this.cijena = cijena;
        }

        public static ObservableCollection<Otpis> Ucitaj()
        {
            ObservableCollection<Otpis> KolekcijaOtpis = new ObservableCollection<Otpis>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM otpis;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idOtpis = Convert.ToInt32(reader["idOtpis"]);                   
                    int idNalog = Convert.ToInt32(reader["Nalog_idNalog"]);
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                  

                    Otpis element = new Otpis(idOtpis,idArtikal,kolicina, idNalog );

                    KolekcijaOtpis.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja otpisa iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaOtpis;
        }

        public static void Sacuvaj(List<Otpis> ListOtpis)
        {
            Database.InitializeDB();
            try
            {
                foreach(Otpis otpis in ListOtpis)
                {
                    String query = string.Format("INSERT INTO otpis SET " +
                        "Nalog_idNalog = (SELECT idnalog FROM nalog WHERE idnalog = '{0}')," +
                        "Artikal_idArtikal = (SELECT idartikal FROM artikal where idartikal = '{1}'), " +
                        "kolicina = '{2}'" , otpis.idNalog, otpis.idArtikal, otpis.kolicina);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                }      
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa otpisa u bazu.\nRazlog: " + ex.Message); }
        }


        #region Izvještaji

        public static ObservableCollection<Otpis> UcitajZaDnevniIzvjestaj()
        {
            ObservableCollection<Otpis> KolekcijaOtpis = new ObservableCollection<Otpis>();
            Database.InitializeDB();
            string datum = DateTime.Today.ToString("dd-MM-yyyy");
            try
            {
                String query = String.Format("select otpis.idOtpis, artikal.naziv, artikal.cijena, otpis.kolicina from otpis inner join artikal where otpis.Artikal_idArtikal = artikal.idArtikal and otpis.datum = '{0}';", datum);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idOtpis = Convert.ToInt32(reader["idOtpis"]);
                    string nazivArt = reader["naziv"].ToString();
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    int kolicina = Convert.ToInt32(reader["kolicina"]);


                    Otpis element = new Otpis(idOtpis, nazivArt, cijena, kolicina);

                    KolekcijaOtpis.Add(element);

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja otpisa iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaOtpis;
        }

        public static ObservableCollection<Otpis> UcitajZaMjesecniIzvjestaj()
        {
            ObservableCollection<Otpis> KolekcijaOtpis = new ObservableCollection<Otpis>();
            Database.InitializeDB();
            string datum = DateTime.Today.ToString("dd-MM-yyyy");
            string currentMonth = datum.Split('-')[1];
            try
            {
                String query = "select otpis.idOtpis, artikal.naziv, artikal.cijena, otpis.kolicina, otpis.datum from otpis inner join artikal where otpis.Artikal_idArtikal = artikal.idArtikal;";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idOtpis = Convert.ToInt32(reader["idOtpis"]);
                    string nazivArt = reader["naziv"].ToString();
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    string datumIzBaze = reader["datum"].ToString();

                    if (datumIzBaze.Split('-')[1].Equals(currentMonth))
                    {
                        Otpis element = new Otpis(idOtpis, nazivArt, cijena, kolicina);


                        KolekcijaOtpis.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja otpisa iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaOtpis;
        }

        public static ObservableCollection<Otpis> UcitajZaGodisnjiIzvjestaj()
        {
            ObservableCollection<Otpis> KolekcijaOtpis = new ObservableCollection<Otpis>();
            Database.InitializeDB();
            string datum = DateTime.Today.ToString("dd-MM-yyyy");
            string currentYear = datum.Split('-')[2];
            try
            {
                String query = "select otpis.idOtpis, artikal.naziv, artikal.cijena, otpis.kolicina, otpis.datum from otpis inner join artikal where otpis.Artikal_idArtikal = artikal.idArtikal;";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idOtpis = Convert.ToInt32(reader["idOtpis"]);
                    string nazivArt = reader["naziv"].ToString();
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    string datumIzBaze = reader["datum"].ToString();

                    if (datumIzBaze.Split('-')[2].Equals(currentYear))
                    {
                        Otpis element = new Otpis(idOtpis, nazivArt, cijena, kolicina);


                        KolekcijaOtpis.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja otpisa iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaOtpis;
        }

        #endregion


    }
}              
