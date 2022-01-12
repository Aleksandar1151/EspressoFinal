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
        public static SkladistePage skladistePage = new SkladistePage();
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(prodajaPage);
        }

        private void Button_Transition(object sender, RoutedEventArgs e)
        {
            var pressedButton = (Button)sender;
            int index = int.Parse(pressedButton.Uid);

            string lightColor = "#DBEBC0";
            string darkColor = "#383D3B";

            BrushConverter bc = new BrushConverter(); 
            
            ChangeButtonColors(ButtonTab1,darkColor,lightColor);
            ChangeButtonColors(ButtonTab2,darkColor,lightColor);
            ChangeButtonColors(ButtonTab3,darkColor,lightColor);
            ChangeButtonColors(ButtonTab4,darkColor,lightColor);
            ChangeButtonColors(ButtonTab5,darkColor,lightColor);

            MainGrid.Children.Clear();
            switch (index)
            {
                 case 1:
                    {
                        
                        MainGrid.Children.Add(prodajaPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 2:
                    {
                        
                        MainGrid.Children.Add(stornirajPage);
                       ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                    case 3:
                    {
                       
                        MainGrid.Children.Add(otpisPage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                     case 4:
                    {
                       
                        MainGrid.Children.Add(skladistePage);
                        ChangeButtonColors(pressedButton,lightColor,darkColor);
                        break;
                    }
                     case 5:
                    {
                       
                       // MainGrid.Children.Add();
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
