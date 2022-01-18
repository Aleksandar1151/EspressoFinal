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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EspressoFinal.Forms.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public static ObservableCollection<Nalog> KolekcijaNaloga { get;set;}
        public static int IDNalog = Properties.Settings.Default.Nalog;
        public Login()
        {
            InitializeComponent();
            KolekcijaNaloga = Nalog.Ucitaj();        
            
            //Properties.Settings.Default.Nalog;
            //Properties.Settings.Default.Save();
        }

        private void Prijava_Click(object sender, RoutedEventArgs e)
        {
            

            Window login = new EspressoFinal.MainWindow();
            login.Show();
        }
    }
}
