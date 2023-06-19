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

public void UploadZipFile(string localFileAddress, string webAddress, string webFileName)
        {
            FtpCreateFolder(webAddress);

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
        }
        
        
public void UploadFtpFile(string folderName, string fileName)
{

    FtpWebRequest request;

    string folderName; 
    string fileName;
    string absoluteFileName = Path.GetFileName(fileName);

    request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", "127.0.0.1", folderName, absoluteFileName))) as FtpWebRequest;
    request.Method = WebRequestMethods.Ftp.UploadFile;
    request.UseBinary = 1;
    request.UsePassive = 1;
    request.KeepAlive = 1;
    request.Credentials =  new NetworkCredential(user, pass);
    request.ConnectionGroupName = "group"; 

    using (FileStream fs = File.OpenRead(fileName))
    {
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        fs.Close();
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(buffer, 0, buffer.Length);
        requestStream.Flush();
        requestStream.Close();
    }
}