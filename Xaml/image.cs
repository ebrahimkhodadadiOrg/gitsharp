BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri((Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + Environment.UserName + ".bmp");
                logo.EndInit();
                windowsImage.Source = logo;