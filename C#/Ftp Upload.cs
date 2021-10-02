 _ftpAddress = "ftp://111.11.11.11/";
                    _ftpUsername = "testuser";
                    _ftpPassword = "testpass";

private bool FtpCreateFolder(string ftpAddress)
        {
            try
            {
                WebRequest ftpRequest = WebRequest.Create(_ftpAddress + ftpAddress);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpRequest.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
                ftpRequest.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                response.Close();
                return false;
            }

public void UploadFile(string localFileAddress, string webAddress, string webFileName)
        {
            FtpCreateFolder(webAddress);
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpAddress + webAddress + webFileName);
            //request.Method = WebRequestMethods.Ftp.UploadFile;
            //request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
            //byte[] fileContents = File.ReadAllBytes(localFileAddress);
            //request.ContentLength = fileContents.Length;
            //request.KeepAlive = false;
            //Stream requestStream = request.GetRequestStream();
            //requestStream.Write(fileContents, 0, fileContents.Length);
            //requestStream.Close();
            //webFileName
            var request = (FtpWebRequest)WebRequest.Create(_ftpAddress + webAddress + webFileName + ".zip");
            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = false;
            request.UsePassive = true;

            //Convert To The Zip
            using (Stream stream = File.Open(localFileAddress + ".zip", FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update, false, null))
                {
                    using (ZipArchiveEntry entry = archive.CreateEntry(localFileAddress))
                    {
                        var entryStream = entry.Open();
                        FileStream fsa = new FileStream(localFileAddress, FileMode.Open);
                        fsa.CopyTo(entryStream);
                        entryStream.Flush();

                        fsa.Close();
                        fsa.Dispose();
                    }
                }
            }

            var fileInf = new FileInfo(localFileAddress + ".zip");
            request.ContentLength = fileInf.Length;
            int buffLength = 2048;//2kb
            byte[] buff = new byte[buffLength];
            var fs = fileInf.OpenRead();
            var allLength = fs.Length;
            var strm = request.GetRequestStream();
            var contentLen = fs.Read(buff, 0, buffLength);
            while (contentLen != 0)
            {
                strm.Write(buff, 0, contentLen);
                contentLen = fs.Read(buff, 0, buffLength);
                WorkPercent = Convert.ToInt32(fs.Position / (double)allLength * 100);
            }
            strm.Close();
            fs.Close();

            File.Delete(localFileAddress + ".zip");