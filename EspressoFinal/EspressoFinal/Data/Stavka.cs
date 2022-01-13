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
        public string kolicina;

        public Stavka(){}

        public Stavka(int idRacun, int idArtikal, string kolicina)
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
                    
                    string kolicina = reader["kolicina"].ToString();
                   
                    

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
