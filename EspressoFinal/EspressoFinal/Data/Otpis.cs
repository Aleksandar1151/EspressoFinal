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
        public string kolicina;
        public int idNalog;
       

        public Otpis()
        {}

        public Otpis(int idOtpis,int idArtikal , string kolicina, int idNalog)
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
                     string kolicina = reader["kolicina"].ToString();
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    

                    Otpis element = new Otpis(idOtpis,idArtikal,kolicina, idNalog );

                    KolekcijaOtpis.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja otpisa iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaOtpis;
        }



    }

}              
