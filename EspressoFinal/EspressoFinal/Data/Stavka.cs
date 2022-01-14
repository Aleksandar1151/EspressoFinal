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
        public int idRacun;
        public int idArtikal;
        public int kolicina;

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

        public Stavka(int idRacun, int idArtikal, int kolicina)
        {
            this.idRacun = idRacun;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
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
                   
                    

                    Stavka element = new Stavka(idRacun,idArtikal,kolicina);

                    KolekcijaStavka.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }
    }
}
