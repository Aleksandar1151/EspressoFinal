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
    }
}              
