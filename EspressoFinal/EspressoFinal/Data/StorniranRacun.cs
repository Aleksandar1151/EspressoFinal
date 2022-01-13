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
    public class StorniranRacun
    {
        public int idStorniranRacun;
        public int idRacun;
        public int idArtikal;
        public string kolicina;

        public StorniranRacun(){}

        public StorniranRacun(int idStorniranRacun, int idRacun, int idArtikal, string kolicina )
        {
            this.idStorniranRacun = idStorniranRacun;
            this.idRacun = idRacun;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
        }

         public static ObservableCollection<StorniranRacun> Ucitaj()
        {
            ObservableCollection<StorniranRacun> KolekcijaStorniranRacun = new ObservableCollection<StorniranRacun>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM storniranracun;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idStorniranRacun = Convert.ToInt32(reader["idStorniranRacun"]);
                    int idRacun = Convert.ToInt32(reader["Stavka_Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Stavka_Artikal_idArtikal"]);                    
                    string kolicina = reader["kolicina"].ToString();
                   
                    

                    StorniranRacun element = new StorniranRacun(idStorniranRacun,idRacun,idArtikal,kolicina);

                    KolekcijaStorniranRacun.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja storniranih racuna iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStorniranRacun;
        }

    }
}
