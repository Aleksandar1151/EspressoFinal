using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspressoFinal.Data
{
    public class KliknutaStavka
    {            
        public int idArtikal { get;set;}   
        public string naziv { get;set;}       
        public double cijena { get;set;}
        public int kolicina { get;set;}
        //public string ispisKolicina { get;set;}

        public KliknutaStavka()
        {

        }

        public KliknutaStavka(int id, string naziv, int kolicina, double cijena)
        {
            this.idArtikal = id;
            this.naziv = naziv;               
            this.cijena = cijena;
            this.kolicina = kolicina;
           // this.ispisKolicina = "x"+kolicina;

        }
    }
}
