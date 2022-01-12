using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EspressoFinal.Data
{
    public class Nalog
    {
        public int idNalog;
        public string ime;
        public string lozinka;
        public bool privilegije;

        public Nalog(){}

        public Nalog(int idNalog, string ime, string lozinka, bool privilegije)
        {
            this.idNalog = idNalog;
            this.ime = ime;
            this.lozinka = lozinka;
            this.privilegije = privilegije;
        }

        #region Baza podataka

        public static ObservableCollection<Nalog> Ucitaj()
        {
            ObservableCollection<Nalog> KolekcijaNaloga = new ObservableCollection<Nalog>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM nalog;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();
                

                while (reader.Read())
                {
                    Console.WriteLine("2");
                    int idNalog = Convert.ToInt32(reader["idNalog"]);
                    string ime = reader["ime"].ToString();
                    string lozinka = reader["lozinka"].ToString();
                    bool privilegije = Convert.ToBoolean(reader["privilegije"]);
                    Nalog user = new Nalog(idNalog, ime, lozinka, privilegije);
                    KolekcijaNaloga.Add(user);
                    Console.WriteLine("3");
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja naloga iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaNaloga;
        }

        #endregion

    }
}
