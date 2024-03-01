using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MemoriaJatek {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window {
        string hatter;
        BitmapImage biHatter;
        List<Image> imKepek;
        List<BitmapImage> biKepek;
        DispatcherTimer dt;
        int kattintas;
        int talalat;
        Image kattintottKep1;
        Image kattintottKep2;
        public MainWindow() {

            jatekIndul();

        }

        private void jatekIndul() {
            InitializeComponent();

            kattintas = 0;
            talalat = 0;
            hatter = "rozsa.jpg";
            imKepek = new List<Image>() { im11, im12, im13, im14, im21, im22, im23, im24 };
            biKepek = new List<BitmapImage>();

            dt = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3), IsEnabled = false };
            dt.Tick += dt_Tick;

            kepBeolvasas();
            kepMegjelenites();
            dt.Start();
        }

        private void dt_Tick(object sender, EventArgs e) {
            kezdes();
            dt.Stop();
        }

        private void kepBeolvasas() {
            List<string> kepNevek = new List<string>() { "kepek/tulipan.jpg", "kepek/margareta.png", "kepek/nefelejcs.png", "kepek/szegfu.png" };
            HashSet<int> kartyak = new HashSet<int>();
            int counter = 0;
            Random rnd = new Random();

            do {
                if(kartyak.Add(rnd.Next(imKepek.Count)))
                    counter++;
            } while(counter < imKepek.Count);

            try {
                biHatter = new BitmapImage(new Uri(@"kepek/" + hatter, UriKind.Relative));

                foreach(int i in kartyak) {
                    biKepek.Add(new BitmapImage(new Uri(@kepNevek[i / 2], UriKind.Relative)));
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void kepMegjelenites() {


            for(int i = 0;i < imKepek.Count;i++) {
                imKepek[i].Source = biKepek[i];
                imKepek[i].IsEnabled = false;
            }
        }

        private void kezdes() {
            foreach(Image ik in imKepek) {
                ik.Source = biHatter;
                ik.IsEnabled = true;
            }
        }

        private void btnBezár_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void im_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            kattintas++;
            if(kattintas == 1) {
                kattintottKep1 = (Image)sender;
                for(int i = 0;i < imKepek.Count;i++) {
                    if(imKepek[i] == kattintottKep1) {
                        kattintottKep1.IsEnabled = false;
                        kattintottKep1.Source = biKepek[i];
                    }
                }
            }
            if(kattintas == 2 && (Image)sender != kattintottKep1) {
                kattintottKep2 = (Image)sender;
                for(int i = 0;i < imKepek.Count;i++) {
                    if(imKepek[i] == kattintottKep2) {
                        kattintottKep2.IsEnabled = false;
                        kattintottKep2.Source = biKepek[i];
                    }
                }
                if(kattintottKep1.Source.ToString() == kattintottKep2.Source.ToString()) {
                    talalat++;
                    if(talalat == imKepek.Count / 2) {
                        MessageBoxResult mess = MessageBox.Show("Nyertél!\nJátszol még egyet?", "Gratulálok", MessageBoxButton.YesNo);
                        switch(mess) {
                            case MessageBoxResult.Yes:
                                jatekIndul();

                                break;
                            case MessageBoxResult.No:
                                Application.Current.Shutdown();
                                break;
                        }
                    } else
                        kattintas = 0;
                } else {
                    MessageBox.Show("Nem talált!");
                    kattintottKep1.Source = biHatter;
                    kattintottKep2.Source = biHatter;
                    kattintottKep1.IsEnabled = true;
                    kattintottKep2.IsEnabled = true;
                    kattintas = 0;
                }
            }


        }
    }
}