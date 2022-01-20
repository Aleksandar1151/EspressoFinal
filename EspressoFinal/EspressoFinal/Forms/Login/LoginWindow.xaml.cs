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
        public static string NalogPrivilegije = "";
        public LoginWindow()
        {
            InitializeComponent();
            KolekcijaNaloga = Nalog.Ucitaj();       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             int index = KolekcijaNaloga.ToList().FindIndex(item => item.ime == ImeBox.Text);
             
            if(index != -1)
            {
                if(KolekcijaNaloga[index].lozinka == LozinkaBox.Password)
                {
                    IDNalog = KolekcijaNaloga[index].idNalog;
                    NalogPrivilegije = KolekcijaNaloga[index].privilegije;

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
