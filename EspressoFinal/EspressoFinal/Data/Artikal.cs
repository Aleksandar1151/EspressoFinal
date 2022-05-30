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
        public string naziv { get;set;}   
        public double cijena { get;set;}   
        public int kolicina { get;set;}   
        public int kategorija { get;set;}   

        public Artikal(){}


        public Artikal(string naziv, double cijena, int kolicina, int kategorija)
        {
            
            this.naziv = naziv;
            this.cijena = cijena;
            this.kolicina = kolicina;
            this.kategorija = kategorija;
        }
        public Artikal(int idArtikal, string naziv, double cijena, int kolicina, int kategorija)
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
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    int kolicina = Convert.ToInt32( reader["kolicina"]);
                    int kategorija = Convert.ToInt32(reader["Kategorija_idKategorija"]);
                    Artikal element = new Artikal(idArtikal, naziv, cijena, kolicina,kategorija);

                    
                    KolekcijaArtikal.Add(element);
                    
                    
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja artikala iz baze.\nRazlog: " + ex.Message); }

            KolekcijaArtikal  = new ObservableCollection<Artikal>(KolekcijaArtikal.OrderBy(i => i.kategorija));

            return KolekcijaArtikal;
        }

        public static void Azuriraj(ObservableCollection<Artikal> KolekcijaArtikal)
        {
            try
            {

                foreach(Artikal artikal in KolekcijaArtikal)
                {
                    String query = string.Format("UPDATE artikal SET " +
                   "kolicina='{0}' WHERE idArtikal = '{1}'"
                   ,artikal.kolicina, artikal.idArtikal);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();
                    Database.dbConn.Close();       
                }


                        
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom mijenjanja artikla u bazi.\nRazlog: " + ex.Message); }
        }

        public static void Dodaj(Artikal noviArtikal)
        {
            try
            {
                 String query = string.Format("INSERT INTO artikal SET " +
                        "Kategorija_idKategorija = (SELECT idkategorija FROM kategorija WHERE idkategorija = '{0}')," +                        
                        " naziv = '{1}', cijena = '{2}', kolicina = '{3}'" , noviArtikal.kategorija, noviArtikal.naziv, noviArtikal.cijena, noviArtikal.kolicina);

                    

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();
                    Database.dbConn.Close();       
              


                        
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom dodavanja artikla u bazu.\nRazlog: " + ex.Message); }
        }




        #endregion


    }
}
