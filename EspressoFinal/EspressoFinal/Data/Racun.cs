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
        public int idRacun { get;set;}
        public string datum { get;set;}
        public string iznos { get;set;}
        public int idNalog { get;set;}

       
        public Racun()
        {            
            this.datum = DateTime.Today.ToString("dd-MM-yyyy");;           
        }

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

        public void Sacuvaj()
        {
            Database.InitializeDB();

            try
            {
                String query = string.Format("INSERT INTO racun" +
                    "(datum, Nalog_idNalog" +
                    ") VALUES " +
                    "('{0}', '{1}')"
                    , datum, 1);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                cmd.ExecuteNonQuery();

                idRacun = (int)cmd.LastInsertedId;
                Database.dbConn.Close();               
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa racuna u bazu.\nRazlog: " + ex.Message); }
        }

        public void Azuriraj()
        {
            try
            {
                String query = string.Format("UPDATE racun SET " +
               "iznos='{0}', WHERE (`idRacun` = '{1}')"
               , iznos, idRacun);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                Database.dbConn.Open();
                cmd.ExecuteNonQuery();
                Database.dbConn.Close();               
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom mijenjanja artikla u bazi.\nRazlog: " + ex.Message); }
        }
    }
}
