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
    public class Stavka
    {
        public int idRacun  { get;set;}
        public int idArtikal { get;set;}
        public int kolicina { get;set;}
        public double cijena { get;set;}

        public string naziv { get;set;}


        public Stavka()
        {
            idRacun = 0;
            idArtikal = 0;
            kolicina = 0;
        }

        public Stavka(int idArtikal, int kolicina)
        {           
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
        }

        public Stavka(int idRacun, int idArtikal, string naziv, double cijena, int kolicina)
        {
            this.idRacun = idRacun;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.naziv = naziv;
        }

        public static ObservableCollection<Stavka> Ucitaj()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM stavka;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();
                   
                    

                    Stavka element = new Stavka(idRacun,idArtikal,naziv,cijena,kolicina);

                    KolekcijaStavka.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }

        public static void Sacuvaj(List<Stavka> ListStavka)
        {
            Database.InitializeDB();

            try
            {
                foreach(Stavka stavka in ListStavka)
                {
                    

                    //Čuvanje u bazi Stavke
                    String query = string.Format("INSERT INTO stavka SET " +
                        "Racun_idRacun = (SELECT idracun FROM racun WHERE idracun = '{0}')," +
                        "Artikal_idArtikal = (SELECT idartikal FROM artikal where idartikal = '{1}'), " +
                        "cijena = '{2}', kolicina = '{3}', naziv = '{4}'" , stavka.idRacun, stavka.idArtikal, stavka.cijena, stavka.kolicina, stavka.naziv);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                }


                           
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa racuna u bazu.\nRazlog: " + ex.Message); }
        }
    }
}
