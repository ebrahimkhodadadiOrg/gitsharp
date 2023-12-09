    public void DownloadDriver()
    {
        var current = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe");

        try
        {
            // check if exist
            if (File.Exists(current))
                return;

            // Synchronous
            AnsiConsole.Status()
                .Start("Downloading Chrome driver...", ctx =>
                {

                    string zipPath = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver_win32.zip");

                    // download
                    string path = "https://chromedriver.storage.googleapis.com/94.0.4606.41/chromedriver_win32.zip";
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(path, zipPath);
                        client.Dispose();
                    }

                    // Update the status and spinner
                    ctx.Status("Start Unzip chrome driver...");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    // unzip
                    string extractPath = Directory.GetCurrentDirectory();
                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

                    // remove zip
                    File.Delete(zipPath);
                });
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"* [red][[ERROR while Downloading chrome driver]] " +
                $"download chrome driver manually and move to {current} [/]");
            AnsiConsole.WriteException(ex);
        }
    }