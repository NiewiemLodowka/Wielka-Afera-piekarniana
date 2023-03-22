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

using System.Windows.Threading;

namespace SkokiPrzezPlotki
{
    public partial class MainWindow : Window
    {
        DispatcherTimer LicznikGry = new DispatcherTimer();

        Rect GraczHitBox;
        Rect ZiemiaHitBox;
        Rect PrzeszkodaHitBox;

        bool skok;
        int moc = 25;
        int prendkosc = 10;

        Random rnd = new Random();
        bool KoniecGry;
        double cos = 0;

        ImageBrush graczCos = new ImageBrush();
        ImageBrush przeszkodaCos = new ImageBrush();

        int[] przeszkodaMiejsce = { 320, 310, 300, 305, 315 };

        int wynik = 0;


        public MainWindow()
        {
            InitializeComponent();

            Kanvas.Focus();
            LicznikGry.Tick += SilnikGry;
            LicznikGry.Interval = TimeSpan.FromMilliseconds(20);

            RozpoczencieGry();

        }


        private void SilnikGry(object sender, EventArgs e)
        {

            if (skok == true)
            {
                prendkosc = -10;
                moc -= 1;
            }
            else
            {
                prendkosc = 15;
            }
            if (moc < 0)
            {
                skok = false;
            }
            if (Canvas.GetLeft(przeszkoda) < -50)
            {
                Canvas.SetLeft(przeszkoda, 900);

                Canvas.SetTop(przeszkoda, przeszkodaMiejsce[rnd.Next(0, przeszkodaMiejsce.Length)]);

                wynik += 1;
            }


            Canvas.SetTop(gracz, Canvas.GetTop(gracz) + prendkosc);
            Canvas.SetLeft(przeszkoda, Canvas.GetLeft(przeszkoda) - 12);

            Wynik.Content = "Score: " + wynik;

            GraczHitBox = new Rect(Canvas.GetLeft(gracz), Canvas.GetTop(gracz), gracz.Width - 20, gracz.Height);
            PrzeszkodaHitBox = new Rect(Canvas.GetLeft(przeszkoda), Canvas.GetTop(przeszkoda), przeszkoda.Width, przeszkoda.Height);
            ZiemiaHitBox = new Rect(Canvas.GetLeft(ziemia), Canvas.GetTop(ziemia), ziemia.Width, ziemia.Height);

            if(GraczHitBox.IntersectsWith(ZiemiaHitBox))
            {
                prendkosc = 0;
                Canvas.SetTop(gracz, Canvas.GetTop(ziemia) - gracz.Height);
                skok = false;
                cos += .5;
                if(cos > 8)
                {
                    cos = 1;
                }

                Bieganie(cos);
            }

            Canvas.SetLeft(background, Canvas.GetLeft(background) - 10);
            Canvas.SetLeft(backgroundv2, Canvas.GetLeft(backgroundv2) - 10);
            if (Canvas.GetLeft(background) < -1262)
            {
                Canvas.SetLeft(background, Canvas.GetLeft(backgroundv2) + backgroundv2.Width);
            }
            if (Canvas.GetLeft(background) < -1262)
            {
                Canvas.SetLeft(backgroundv2, Canvas.GetLeft(background) + background.Width);
            }


            if (GraczHitBox.IntersectsWith(PrzeszkodaHitBox))
            {
                KoniecGry = true;
                LicznikGry.Stop();
            }

            if(KoniecGry == true)
            {
                przeszkoda.Stroke = Brushes.Black;
                przeszkoda.StrokeThickness = 1;

                gracz.Stroke = Brushes.Red;
                gracz.StrokeThickness = 1;
            }
            else
            {
                gracz.StrokeThickness = 0;
                przeszkoda.StrokeThickness = 0;
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && KoniecGry == true)
            {
                RozpoczencieGry();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && skok == false && Canvas.GetTop(gracz) > 260)
            {
                skok = true;
                moc = 15;
                prendkosc = -12;

                graczCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/skok.png"));
            }
        }

        private void RozpoczencieGry()
        {
            Canvas.SetLeft(background, 0);
            Canvas.SetLeft(backgroundv2, 1262);

            Canvas.SetLeft(gracz, 110);
            Canvas.SetTop(gracz, 140);

            Canvas.SetLeft(przeszkoda, 900);
            Canvas.SetTop(przeszkoda, 300);

            Bieganie(1);

            przeszkodaCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/piec.png"));
            przeszkoda.Fill = przeszkodaCos;

            skok = false;
            KoniecGry = false;
            wynik = 0;

            Wynik.Content = "Score: " + wynik;

            LicznikGry.Start();

        }

        private void Bieganie(double i)
        {
            switch (i)
            {
                case 1:
                    graczCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/zyd1.png"));
                    break;

                case 2:
                    graczCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/zyd2.png"));
                    break;
                case 3:
                    graczCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/zyd2.png"));
                    break;
                case 4:
                    graczCos.ImageSource = new BitmapImage(new Uri("pack://application:,,,/obrazki/zyd1.png"));
                    break;

            }
            gracz.Fill = graczCos;

        }
    }
}
