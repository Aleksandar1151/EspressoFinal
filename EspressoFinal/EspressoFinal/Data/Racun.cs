using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using EspressoFinal.Forms.Login;

namespace EspressoFinal.Data
{
    public class Racun
    {
        public int idRacun { get;set;}
        public string datum { get;set;}
        public double iznos { get;set;}
        public int idNalog { get;set;}

       
        public Racun()
        {            
            this.datum = DateTime.Today.ToString("dd-MM-yyyy");;           
        }

        public Racun(int idRacun, string datum, double iznos, int idNalog)
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
                    double iznos = Convert.ToDouble(reader["iznos"]);
                    int idNalog = Convert.ToInt32(reader["Nalog_idNalog"]);                    

                    Racun element = new Racun(idRacun,datum,iznos, idNalog );

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
                String query = string.Format("INSERT INTO racun SET " +
                    "datum = '{0}', Nalog_idNalog = (SELECT idNalog FROM nalog WHERE idNalog = '{1}')" ,datum, LoginWindow.IDNalog);

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
               "iznos='{0}' WHERE idRacun = '{1}'"
               , iznos, idRacun);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                Database.dbConn.Open();
                cmd.ExecuteNonQuery();
                Database.dbConn.Close();               
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom mijenjanja racuna u bazi.\nRazlog: " + ex.Message); }
        }

        public void PretraziRacun(int id)
        {
            try
            {
                String query = string.Format("SELECT * from racun where `idracun` = {0}" , id);

                 MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();   

                if(reader.Read())
                {
                     idRacun = Convert.ToInt32(reader["idRacun"]);                   
                     datum = reader["datum"].ToString();
                    // iznos = Convert.ToDouble(reader["iznos"]);
                     idNalog = Convert.ToInt32(reader["Nalog_idNalog"]);    
                }

                          


                Database.dbConn.Close();               
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom ucitavanja racuna iz baze.\nRazlog: " + ex.Message); }
        }
    
        
        
         public static ObservableCollection<Racun> UcitajStornirane()
        {
            ObservableCollection<Racun> KolekcijaRacun = new ObservableCollection<Racun>();
            Database.InitializeDB();
            string DanasnjiDatum = DateTime.Today.ToString("dd-MM-yyyy"); ;
            try
            {
                String query = String.Format("SELECT * FROM racun WHERE datum = '{0}'", DanasnjiDatum);
                Console.WriteLine(query);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int idRacun = Convert.ToInt32(reader["idRacun"]);
                    string datum = reader["datum"].ToString();
                    //double iznos = Convert.ToDouble(reader["iznos"]);
                    int idNalog = Convert.ToInt32(reader["Nalog_idNalog"]);

                    Racun element = new Racun(idRacun, datum, 0, idNalog);

                    KolekcijaRacun.Add(element);
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja racuna iz baze1!!!!!\nRazlog: " + ex.Message); }
            Console.WriteLine(KolekcijaRacun.Count);
            return KolekcijaRacun;
        }
        
    }
}
