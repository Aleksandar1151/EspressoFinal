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
        
        public static MainPage MainPage = new MainPage();
       
        public MainWindow()
        {
            InitializeComponent();


            if(LoginWindow.NalogPrivilegije == "nema")
            {
                ButtonTab3.IsEnabled = false;
                ButtonTab4.IsEnabled = false;
            }


            MainGrid.Children.Clear();
            ProdajaPage prodajaPage = new ProdajaPage();
            //Login LoginPage = new Login();
            MainGrid.Children.Add(prodajaPage);
        }

        private void Button_Transition(object sender, RoutedEventArgs e)
        {
            var pressedButton = (Button)sender;
            int index = int.Parse(pressedButton.Uid);

            string lightColor = "#CAD2C5";
            string darkColor = "#52796F";

            BrushConverter bc = new BrushConverter(); 
            
            ChangeButtonColors(ButtonTab1,darkColor,lightColor);
            ChangeButtonColors(ButtonTab2,darkColor,lightColor);
            ChangeButtonColors(ButtonTab3,darkColor,lightColor);
            ChangeButtonColors(ButtonTab4,darkColor,lightColor);
           // ChangeButtonColors(ButtonTab5,darkColor,lightColor);

            MainGrid.Children.Clear();
            switch (index)
            {
                 case 1:
                    {
                        ProdajaPage prodajaPage = new ProdajaPage();
                        MainGrid.Children.Add(prodajaPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 2:
                    {
                        StornirajPage stornirajPage = new StornirajPage();
                        MainGrid.Children.Add(stornirajPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 3:
                    {
                       
                        SkladistePage skladistePage = new SkladistePage();
                        MainGrid.Children.Add(skladistePage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 4:
                    {
                        NalogPage naloziPage = new NalogPage();
                        MainGrid.Children.Add(naloziPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    
            }

        }

        private void ChangeButtonColors(Button button, string foreColor, string backColor)
        {
            BrushConverter bc = new BrushConverter(); 
            button.Background = (Brush)bc.ConvertFrom(backColor); 
            button.Foreground = (Brush)bc.ConvertFrom(foreColor); 
            

        }
    }
}
