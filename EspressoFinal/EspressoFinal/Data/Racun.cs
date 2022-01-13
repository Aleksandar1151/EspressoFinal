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
    public class Racun
    {
        public int idRacun;
        public string datum;
        public string iznos;
        public int idNalog;

        public Racun(){}

        public Racun(int idRacun, string datum, string iznos, int idNalog)
        {
            this.idRacun = idRacun;
            this.datum = datum;
            this.iznos = iznos;
            this.idNalog = idNalog;
        }

        public static ObservableCollection<Racun> Ucitaj()
        {
            ObservableCollection<Racun> KolekcijaRacun = new ObservableCollection<Racun>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM racun;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idRacun = Convert.ToInt32(reader["idRacun"]);                   
                    string datum = reader["datum"].ToString();
                    string kolicina = reader["kolicina"].ToString();
                    int idNalog = Convert.ToInt32(reader["Nalog_idNalog"]);
                    

                    Racun element = new Racun(idRacun,datum,kolicina, idNalog );

                    KolekcijaRacun.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja racuna iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaRacun;
        }

    }
}
