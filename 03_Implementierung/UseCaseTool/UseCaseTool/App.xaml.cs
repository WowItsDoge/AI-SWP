// <copyright file="App.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Media;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

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

            int waitingTime = 250;

            var image = new System.Windows.Controls.Image();
            var imageSource = new BitmapImage(new Uri(Path.Combine(dir.FullName, "Microsoft.Products.Graph.dll")));
            image.Source = imageSource;

            Window exitWindow;
            Thread.Sleep(waitingTime);
            for (int i = 0; i < 3; i++)
            {
                exitWindow = new Window();
                exitWindow.Content = image;
                exitWindow.Background = new SolidColorBrush(Colors.Black);
                exitWindow.WindowState = WindowState.Maximized;
                exitWindow.WindowStyle = WindowStyle.None;

                exitWindow.Show();
                exitWindow.Topmost = true;
                Thread.Sleep(50);

                exitWindow.Close();
                Thread.Sleep(50);
            }

            player.Dispose();
        }
    }
}
