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
        public int idNalog  {get;set;}
        public string ime {get;set;}
        public string lozinka  {get;set;}
        public string privilegije  {get;set;}

        public Nalog(){}


        public Nalog(string ime, string lozinka, int privilegije)
        {
            
            this.ime = ime;
            this.lozinka = lozinka;
            if(privilegije == 0)
                this.privilegije = "nema";
            else this.privilegije = "ima";
            
            
        }
        public Nalog(int idNalog, string ime, string lozinka, int privilegije)
        {
            this.idNalog = idNalog;
            this.ime = ime;
            this.lozinka = lozinka;
            if(privilegije == 0)
                this.privilegije = "nema";
            else this.privilegije = "ima";
        }

        #region Baza podataka

        public static ObservableCollection<Nalog> Ucitaj()
        {
            ObservableCollection<Nalog> KolekcijaNalog = new ObservableCollection<Nalog>();
            Database.InitializeDB();

            try
            {
                String query = "SELECT * FROM nalog;";
                
                MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);
                
                
                Database.dbConn.Open();
                
                MySqlDataReader reader = cmd.ExecuteReader();
                

                while (reader.Read())
                {
                    
                    int idNalog = Convert.ToInt32(reader["idNalog"]);
                    string ime = reader["ime"].ToString();
                    string lozinka = reader["lozinka"].ToString();
                    int privilegije = Convert.ToInt32(reader["privilegije"]);
                    Nalog element = new Nalog(idNalog, ime, lozinka, privilegije);
                    KolekcijaNalog.Add(element);
                   
                }
                Database.dbConn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom preuzimanja naloga iz baze!!!!!\nRazlog: " + ex.Message); }

            return KolekcijaNalog;
        }

        public void Sacuvaj()
        {
            Database.InitializeDB();
            try
            {
                int priv = 0;
                if(privilegije == "ima") priv = 1;

                    String query = string.Format("INSERT INTO nalog SET " +
                        "ime = '{0}', lozinka = '{1}', privilegije = '{2}'" , ime, lozinka, priv );

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                  
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa naloga u bazu.\nRazlog: " + ex.Message); }
        }

        public void Azuriraj()
        {
            Database.InitializeDB();
            try
            {
                int priv = 0;
                if(privilegije == "ima") 
                        priv = 1; 
                    

                    String query = string.Format("UPDATE nalog SET " +
               "privilegije='{0}' WHERE idNalog = '{1}'"
               , priv, idNalog);

                    MySqlCommand cmd = new MySqlCommand(query, Database.dbConn);

                    Database.dbConn.Open();
                    cmd.ExecuteNonQuery();                
                    Database.dbConn.Close();  
                  
            }
            catch (Exception ex) { MessageBox.Show("Greška prilikom unosa naloga u bazu.\nRazlog: " + ex.Message); }
        }

        #endregion

    }
}
