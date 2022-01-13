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
    public class Kategorija
    {
        public int idKategorija;
        public string naziv;
        public string boja;

        public Kategorija(){}

        public Kategorija(int idKategorija, string naziv, string boja)
        {
            this.idKategorija = idKategorija;
            this.naziv = naziv;
            this.boja = boja;
        }

        #region Baza podataka

        public static ObservableCollection<Kategorija> Ucitaj()
        {
            ObservableCollection<Kategorija> KolekcijaKategorija = new ObservableCollection<Kategorija>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM kategorija;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idKategorija = Convert.ToInt32(reader["idKategorija"]);
                    string naziv = reader["naziv"].ToString();
                    string boja = reader["boja"].ToString();
                    

                    Kategorija element = new Kategorija(idKategorija, naziv, boja);

                    KolekcijaKategorija.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja kategorija iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaKategorija;
        }

        #endregion
    }
}
