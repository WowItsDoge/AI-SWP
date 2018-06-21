// <copyright file="App.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Media;
    using System.Windows;
    using System.Windows.Threading;

    using System.Reflection;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Threading;
    using System;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Is thrown if a unhandled exception occurred.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void ApplicationUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(tempPath);

            ZipFile.ExtractToDirectory("Microsoft.Products.Drawing.zip", dir.FullName);
            var player = new SoundPlayer(Path.Combine(dir.FullName, "Microsoft.Products.Drawing.dll"));
            player.Play();

            int waitingTime = 150;

            var image = new System.Windows.Controls.Image();
            var imageSource = new BitmapImage(new Uri(Path.Combine(dir.FullName, "Microsoft.Products.Graph.dll")));
            image.Source = imageSource;

            Window exitWindow;
            for (int i = 0; i < 25; i++)
            {
                waitingTime = (int)(waitingTime * 0.93);

                exitWindow = new Window();
                exitWindow.Content = image;
                exitWindow.Background = new SolidColorBrush(Colors.Black);
                exitWindow.WindowState = WindowState.Maximized;
                exitWindow.WindowStyle = WindowStyle.None;

                Thread.Sleep(waitingTime);
                exitWindow.Show();
                exitWindow.Topmost = true;
                e.Handled = true;

                Thread.Sleep(waitingTime);                

                exitWindow.Close();
            }

            player.Dispose();
        }
    }
}
