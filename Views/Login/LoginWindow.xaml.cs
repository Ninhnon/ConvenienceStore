using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ConvenienceStore.Views.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Loginwindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                imagerotator();
            }));
        }


        int i = 2;

        private void imagerotator()

        {

            Storyboard myStoryboard2 = new Storyboard();

            myStoryboard2.SpeedRatio = 5;

            var fadein = new DoubleAnimation()

            {

                From = 1,

                To = 1,

                Duration = TimeSpan.FromSeconds(2),

            };

            Storyboard.SetTarget(fadein, imgframe);

            Storyboard.SetTargetProperty(fadein, new PropertyPath(ImageBrush.OpacityProperty));

            var sb = new Storyboard();

            sb.Children.Add(fadein);

            sb.Completed += new EventHandler(sb_Completed0);

            sb.Begin();

        }
        private void sb_Completed0(object sender, EventArgs e)

        {

            Storyboard myStoryboard2 = new Storyboard();

            myStoryboard2.SpeedRatio = 5;

            var fadein = new DoubleAnimation()

            {

                From = 1,

                To = 1,

                Duration = TimeSpan.FromSeconds(0.5),

            };

            Storyboard.SetTarget(fadein, imgframe);

            Storyboard.SetTargetProperty(fadein, new PropertyPath(ImageBrush.OpacityProperty));

            var sb = new Storyboard();

            sb.Children.Add(fadein);

            sb.Completed += new EventHandler(sb_Completed);

            sb.Begin();

        }
        private void sb_Completed(object sender, EventArgs e)
        {
            string strUri2 = String.Format("\\Resources\\Images\\cute\\{0}.png", i.ToString());
            Uri relativeUri = new Uri(strUri2, UriKind.Relative);
            //Uri relativeUri = new Uri(strUri2);
            i++;

            if (i > 8) i = 1;
            imgframe.Source = new BitmapImage(relativeUri);

            Storyboard myStoryboard2 = new Storyboard();

            myStoryboard2.SpeedRatio = 5;

            var fadein = new DoubleAnimation()

            {

                From = 1,

                To = 1,

                Duration = TimeSpan.FromSeconds(.5),

            };

            Storyboard.SetTarget(fadein, imgframe);

            Storyboard.SetTargetProperty(fadein, new PropertyPath(ImageBrush.OpacityProperty));

            var sb = new Storyboard();

            sb.Children.Add(fadein);

            sb.Begin();

            imagerotator();

        }
    }
}