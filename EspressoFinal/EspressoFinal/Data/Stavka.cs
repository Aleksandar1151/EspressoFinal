﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace EspressoFinal.Data
{
    public class Stavka
    {
        public int idRacun  { get;set;}
        public int idArtikal { get;set;}
        public int kolicina { get;set;}
        public double cijena { get;set;}

        public string naziv { get;set;}
        public int idStorniranRacun { get;set;}


        public Stavka()
        {
            idRacun = 0;
            idArtikal = 0;
            kolicina = 0;
        }

        public Stavka(int idArtikal, int kolicina)
        {           
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
        }

        public Stavka(int idRacun, int idArtikal, string naziv, double cijena, int kolicina)
        {
            this.idRacun = idRacun;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.naziv = naziv;
            
        }

        public Stavka(int idRacun, int idArtikal, string naziv, double cijena, int kolicina, int idStorniranRacun )
        {
            this.idRacun = idRacun;
            this.idArtikal = idArtikal;
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.naziv = naziv;
            this.idStorniranRacun = idStorniranRacun;
        }

        public Stavka(Stavka x)
        {
            idRacun = x.idRacun;
            idArtikal = x.idArtikal;
            kolicina = x.kolicina;
            cijena = x.cijena;
            naziv = x.naziv;
        }

        public static ObservableCollection<Stavka> Ucitaj()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM stavka;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();
                   
                    

                    Stavka element = new Stavka(idRacun,idArtikal,naziv,cijena,kolicina);

                    KolekcijaStavka.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }

        public static ObservableCollection<Stavka> UcitajStavkeRacuna(int id)
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();
                
            try
            {
                String query = string.Format("SELECT * FROM stavka WHERE `Racun_idRacun` = {0}" , id);
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();                

                while (reader.Read())
                {
                    
                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    
                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();
                   
                    

                    Stavka element = new Stavka(idRacun,idArtikal,naziv,cijena,kolicina);

                    KolekcijaStavka.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!\nRazlog: " + ex.Message); }


             return KolekcijaStavka;
        }

        public static void Sacuvaj(List<Stavka> ListStavka)
        {
            Database.InitializeDB();

            try
            {
                foreach(Stavka stavka in ListStavka)
                {
                    

                    //Čuvanje u bazi Stavke
                    String query = string.Format("INSERT INTO stavka SET " +
                        "Racun_idRacun = (SELECT idracun FROM racun WHERE idracun = '{0}')," +
                        "Artikal_idArtikal = (SELECT idartikal FROM artikal where idartikal = '{1}'), " +
                        "cijena = '{2}', kolicina = '{3}', naziv = '{4}'" , stavka.idRacun, stavka.idArtikal, stavka.cijena, stavka.kolicina, stavka.naziv);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                }


                           
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa racuna u bazu.\nRazlog: " + ex.Message); }
        }

        public static void SacuvajStorniran(List<Stavka> ListStavka)
        {
            Database.InitializeDB();

            try
            {
                foreach(Stavka stavka in ListStavka)
                {
                    //Čuvanje u bazi Stavke
                    String query = string.Format("INSERT INTO stavka SET " +
                        "Racun_idRacun = (SELECT idracun FROM racun WHERE idracun = '{0}')," +
                        "Artikal_idArtikal = (SELECT idartikal FROM artikal where idartikal = '{1}'), " +
                        "StorniranRacun_idStorniranRacun = (SELECT idStorniranRacun FROM storniranracun where idStorniranRacun = '{2}'), " +
                        "cijena = '{3}', kolicina = '{4}', naziv = '{5}'" , stavka.idRacun, stavka.idArtikal,stavka.idStorniranRacun, stavka.cijena, stavka.kolicina, stavka.naziv);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                }
    
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa racuna u bazu.\nRazlog: " + ex.Message); }
        }
    
        
        
            public static ObservableCollection<Stavka> UcitajMjesecneStavkeBezStorniranih()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "select racun.datum, stavka.Racun_idRacun, stavka.Artikal_idArtikal, stavka.naziv, stavka.kolicina, stavka.cijena from stavka inner join racun where racun.idRacun = stavka.Racun_idRacun and stavka.StorniranRacun_idStorniranRacun is null";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                string danasnjiDatum = DateTime.Today.ToString("dd-MM-yyyy");
                string danasnjiMjesec = danasnjiDatum.Split('-')[1];

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    String datum = reader["datum"].ToString();

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();

                    if (datum.Split('-')[1].Equals(danasnjiMjesec))
                    {

                        Stavka element = new Stavka(idRacun, idArtikal, naziv, cijena, kolicina);

                        KolekcijaStavka.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }


        public static ObservableCollection<Stavka> UcitajMjesecneStavkeSaStorniranim()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "select racun.datum, stavka.Racun_idRacun, stavka.Artikal_idArtikal, stavka.naziv, stavka.kolicina, stavka.cijena, stavka.StorniranRacun_idStorniranRacun from stavka inner join racun where racun.idRacun = stavka.Racun_idRacun and stavka.StorniranRacun_idStorniranRacun is not null";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                string danasnjiDatum = DateTime.Today.ToString("dd-MM-yyyy");
                string danasnjiMjesec = danasnjiDatum.Split('-')[1];

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    String datum = reader["datum"].ToString();

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    int idStorniran = Convert.ToInt32(reader["StorniranRacun_idStorniranRacun"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();

                    if (datum.Split('-')[1].Equals(danasnjiMjesec))
                    {

                        Stavka element = new Stavka(idRacun, idArtikal, naziv, cijena, kolicina, idStorniran);

                        KolekcijaStavka.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }

        public static ObservableCollection<Stavka> UcitajGodisnjeStavkeBezStorniranih()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "select racun.datum, stavka.Racun_idRacun, stavka.Artikal_idArtikal, stavka.naziv, stavka.kolicina, stavka.cijena from stavka inner join racun where racun.idRacun = stavka.Racun_idRacun and stavka.StorniranRacun_idStorniranRacun is null";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                string danasnjiDatum = DateTime.Today.ToString("dd-MM-yyyy");
                string trenutnaGodina = danasnjiDatum.Split('-')[2];

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    String datum = reader["datum"].ToString();

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();

                    if (datum.Split('-')[2].Equals(trenutnaGodina))
                    {

                        Stavka element = new Stavka(idRacun, idArtikal, naziv, cijena, kolicina);

                        KolekcijaStavka.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }


        public static ObservableCollection<Stavka> UcitajGodisnjeStavkeSaStorniranim()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            try
            {
                String query = "select racun.datum, stavka.Racun_idRacun, stavka.naziv, stavka.Artikal_idArtikal, stavka.kolicina, stavka.cijena, stavka.StorniranRacun_idStorniranRacun from stavka inner join racun where racun.idRacun = stavka.Racun_idRacun and stavka.StorniranRacun_idStorniranRacun is not null";

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                string danasnjiDatum = DateTime.Today.ToString("dd-MM-yyyy");
                string trenutnaGodina = danasnjiDatum.Split('-')[2];

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);
                    String datum = reader["datum"].ToString();

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    int idStorniran = Convert.ToInt32(reader["StorniranRacun_idStorniranRacun"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();

                    if (datum.Split('-')[2].Equals(trenutnaGodina))
                    {

                        Stavka element = new Stavka(idRacun, idArtikal, naziv, cijena, kolicina, idStorniran);

                        KolekcijaStavka.Add(element);
                    }

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }

        public static ObservableCollection<Stavka> UcitajDnevneStavkeBezStorniranih()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();

            string datum = DateTime.Today.ToString("dd-MM-yyyy");
            try
            {
                String query = String.Format("select stavka.Racun_idRacun, stavka.Artikal_idArtikal, stavka.naziv, stavka.kolicina, stavka.cijena from stavka inner join racun where racun.idRacun = stavka.Racun_idRacun and stavka.StorniranRacun_idStorniranRacun is null and racun.datum = '{0}'", datum);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

             

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();



                        Stavka element = new Stavka(idRacun, idArtikal, naziv, cijena, kolicina);

                        KolekcijaStavka.Add(element);


                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }

        public static ObservableCollection<Stavka> UcitajStavkeRacunaSaStorniranimStavkama()
        {
            ObservableCollection<Stavka> KolekcijaStavka = new ObservableCollection<Stavka>();
            Database.InitializeDB();
            string datum = DateTime.Today.ToString("dd-MM-yyyy"); ;

            try
            {
                String query = string.Format("select stavka.StorniranRacun_idStorniranRacun, stavka.Racun_idRacun, stavka.naziv, stavka.cijena, stavka.kolicina from stavka  inner join racun where stavka.Racun_idRacun = racun.idRacun AND racun.datum='{0}'  AND stavka.StorniranRacun_idStorniranRacun is not null", datum);

                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                Database.dbConn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int idRacun = Convert.ToInt32(reader["Racun_idRacun"]);
                    //int idArtikal = Convert.ToInt32(reader["Artikal_idArtikal"]);

                    int kolicina = Convert.ToInt32(reader["kolicina"]);
                    double cijena = Convert.ToDouble(reader["cijena"]);
                    string naziv = reader["naziv"].ToString();
                    int idStorniranRacun = Convert.ToInt32(reader["StorniranRacun_idStorniranRacun"]);



                    Stavka element = new Stavka(idRacun, 0, naziv, cijena, kolicina, idStorniranRacun);

                    KolekcijaStavka.Add(element);

                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja stavke iz baze!\nRazlog: " + ex.Message); }

            return KolekcijaStavka;
        }
    
    }
}
