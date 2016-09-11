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
using System.Windows.Shapes;

namespace CaptchaProcess
{
    /// <summary>
    /// SPsetting.xaml 的交互逻辑
    /// </summary>
    public partial class SPsetting : Window
    {
        public SPsetting()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.m_SauvolaWidth = int.Parse(widthValue.Text);

            MainWindow.m_SauvolaFactor = float.Parse(factorValue.Text);

            MainWindow.m_SpWindow.Hide();
        }


    }
}
