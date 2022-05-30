using EspressoFinal.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EspressoFinal.Forms.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static ObservableCollection<Nalog> KolekcijaNaloga { get;set;}
        public static int IDNalog = Properties.Settings.Default.Nalog;
        public static string Nalog_Naziv = "";
        public static string NalogPrivilegije = "";
        public LoginWindow()
        {
            InitializeComponent();
            KolekcijaNaloga = Nalog.Ucitaj();       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {             
            LoginFunction();            
        }

        private void PressedEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) LoginFunction();
        }

        private void LoginFunction()
        {
            int index = KolekcijaNaloga.ToList().FindIndex(item => item.ime == ImeBox.Text);
             
            if(index != -1)
            {
                if(KolekcijaNaloga[index].lozinka == LozinkaBox.Password)
                {
                    IDNalog = KolekcijaNaloga[index].idNalog;
                    
                    Nalog_Naziv = KolekcijaNaloga[index].ime;
                    NalogPrivilegije = KolekcijaNaloga[index].privilegije;

                    Properties.Settings.Default.Nalog = IDNalog;
                    Properties.Settings.Default.Nalog_Naziv = Nalog_Naziv;

                    Properties.Settings.Default.Save();

                    Window mainWindow = new EspressoFinal.MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Greška prilikom prijavljivanja.");


            }
            else
            {
                MessageBox.Show("Greška prilikom prijavljivanja.");
            }

        }
    }
}
