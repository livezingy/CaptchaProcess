/* --------------------------------------------------------
 * author：livezingy
 * 
 * BLOG：http://www.livezingy.com
 * 
 * Development Environment：
 *      Visual Studio V2013
 *      
 * Version：
 *      V1.0    20160906
 *      
 * 1. load the source image
 * 2. show the result of image process
--------------------------------------------------------- */

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
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

namespace CaptchaProcess
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string m_srcImagePath = "";

        public static int m_SauvolaWidth = 100;

        public static double m_SauvolaFactor = 0.3;

        public static SPsetting m_SpWindow;

        public MainWindow()
        {
            InitializeComponent();

            m_SpWindow = new SPsetting();

            m_SpWindow.Hide();
        }

        private void load_image_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                m_srcImagePath = openFileDialog.FileName;
                BitmapImage newImg = new BitmapImage(new Uri(m_srcImagePath));
                if ((newImg.Width > 139) || (newImg.Height > 49))
                {
                    MessageBox.Show("The image is too large!");  
                }
                else
                {
                    SRC_IMAGE.Source = newImg;

                    IP_Com1_SelectionChanged_1(null, null);

                    IP_Com2_SelectionChanged_1(null, null);
                }
            }
            else
            {
                MessageBox.Show("No selected image!");  
            }
        }


        private void IP_Com1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (m_srcImagePath != "")
            {
                Bitmap processImage = SelectionChangedMethod(IP_Com1.SelectedIndex);
                PROCESS1_IMAGE.Source = BitmapToBitmapImage(processImage);
            }
        }

        private void IP_Com2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (m_srcImagePath != "")
            {
                Bitmap processImage = SelectionChangedMethod(IP_Com2.SelectedIndex);
                PROCESS2_IMAGE.Source = BitmapToBitmapImage(processImage);
            }
        }

        

        //Convert Bitmap to BitmapImage
        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private Bitmap SelectionChangedMethod(int selectedIndex)
        {
            Bitmap srcBmp = new Bitmap(m_srcImagePath);

            Byte[,] BinaryArray = new Byte[srcBmp.Height, srcBmp.Width];

            int threshold;               

            switch (selectedIndex)
            {
                case 0:
                    Byte[,] grayArraySrc = Preprocess.Preprocess.ToGrayArray(srcBmp);
                    BinaryArray = Preprocess.Preprocess.Sauvola(grayArraySrc);
                    break;

                case 1:
                    BinaryArray = Preprocess.Preprocess.ToBinaryArray(srcBmp, Preprocess.Preprocess.BinarizationMethods.Otsu, out threshold);
                    break;

                case 2:
                    BinaryArray = Preprocess.Preprocess.ToBinaryArray(srcBmp, Preprocess.Preprocess.BinarizationMethods.Iterative, out threshold);
                    break;

                case 3:
                    BinaryArray = Preprocess.Preprocess.ToGrayArray(srcBmp);
                    BinaryArray = Preprocess.Preprocess.Sauvola(BinaryArray);
                    BinaryArray = Preprocess.Preprocess.ThinPicture(BinaryArray);
                    break;

                case 4:
                    BinaryArray = CaptchaSegment.CaptchaSegment.CaptchaSegmentFun(srcBmp);
                    break;

                default: break;
            }

            Bitmap GrayBmp = Preprocess.Preprocess.BinaryArrayToBinaryBitmap(BinaryArray);

            return GrayBmp;                
        }

        private void SauvolaP_Click(object sender, RoutedEventArgs e)
        {
            m_SpWindow.Show();
        }
        
        /*
        private BitmapImage convertMatToBitmapImage(Mat matImage)
        {
           // BitmapImage bitImage;

            Image<Gray, Byte> img = matImage.ToImage<Gray, Byte>();

            Bitmap bmp = img.ToBitmap();

            return BitmapToBitmapImage(bmp);
        }
        */
           
    }
}
