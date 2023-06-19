//https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/

var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);


        public string GenerateBodyFromHtml(string htmlFileName, params object[] parameters)
        {
            string htmlBody;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", $"{htmlFileName}.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                htmlBody = SourceReader.ReadToEnd();
            }

            return string.Format(htmlBody, parameters);
        }