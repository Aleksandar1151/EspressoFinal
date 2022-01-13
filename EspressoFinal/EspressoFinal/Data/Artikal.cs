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
    public class Artikal
    {
        public int idArtikal;
        public string naziv;
        public string cijena;
        public string kolicina;
        public int kategorija;

        public Artikal(){}

        public Artikal(int idArtikal, string naziv, string cijena, string kolicina, int kategorija)
        {
            this.idArtikal = idArtikal;
            this.naziv = naziv;
            this.cijena = cijena;
            this.kolicina = kolicina;
            this.kategorija = kategorija;
        }

        #region Baza podataka

        public static ObservableCollection<Artikal> Ucitaj()
        {
            ObservableCollection<Artikal> KolekcijaArtikal = new ObservableCollection<Artikal>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM artikal;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();
                

                while (reader.Read())
                {
                    
                    int idArtikal = Convert.ToInt32(reader["idArtikal"]);
                    string naziv = reader["naziv"].ToString();
                    string cijena = reader["cijena"].ToString();
                    string kolicina = reader["kolicina"].ToString();
                    int kategorija = Convert.ToInt32(reader["Kategorija_idKategorija"]);
                    Artikal element = new Artikal(idArtikal, naziv, cijena, kolicina,kategorija);
                    KolekcijaArtikal.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja artikala iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaArtikal;
        }



        #endregion


    }
}
