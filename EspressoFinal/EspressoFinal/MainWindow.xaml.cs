using EspressoFinal.Forms.Login;
using EspressoFinal.Forms.MainPage;
using EspressoFinal.Forms.Tabs;
using System;
using System.Collections.Generic;
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

namespace EspressoFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Login LoginPage = new Login();
        public static MainPage MainPage = new MainPage();
        public static ProdajaPage prodajaPage = new ProdajaPage();
        public static StornirajPage stornirajPage = new StornirajPage();
        public static OtpisPage otpisPage = new OtpisPage();
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(otpisPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
