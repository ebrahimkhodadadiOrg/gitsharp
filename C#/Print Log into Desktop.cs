string er = ex.Message + "\n" + "Trace: "
                    + "\n" + (ex.StackTrace.Split('\n').Length > 0 ? ex.StackTrace.Split('\n').Last() : ex.StackTrace)
                    + (ex.InnerException != null ? "\n" + "Inner Message: " + ex.InnerException.Message + "\n" + "Inner Trace: " +
                    (ex.InnerException.StackTrace.Split('\n').Length > 0 ? ex.InnerException.StackTrace.Split('\n').Last() : ex.InnerException.StackTrace) : "");
string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Log.txt";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                string dateTime = PersianDateTime.Now.ToString();
                string[] dt = dateTime.Split('/');
                string exeFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = exeFolder + string.Format("\\Log\\ExceptionLog@ {0}-{1}-{2}.txt", dt[0], dt[1], dt[2]);
                var streamWriter = new StreamWriter(filePath, true);
                streamWriter.WriteLine("[Log Exception]" + streamWriter.NewLine + "Date: " + dateTime + streamWriter.NewLine + "Time: " +
                    DateTime.Now.ToLongTimeString() + streamWriter.NewLine + "Occured in Form: " + formName + streamWriter.NewLine + "Message: " +
                    streamWriter.NewLine + "   " + ex.Message + streamWriter.NewLine + "Trace: " + streamWriter.NewLine + (ex.StackTrace.Split('\n').Length > 0 ? ex.StackTrace.Split('\n').Last() : ex.StackTrace) +
                    (ex.InnerException != null ? streamWriter.NewLine + "Inner Message: " + streamWriter.NewLine + "   " + ex.InnerException.Message +
                                                 streamWriter.NewLine + "Inner Trace: " + streamWriter.NewLine + (ex.InnerException.StackTrace.Split('\n').Length > 0 ? ex.InnerException.StackTrace.Split('\n').Last() : ex.InnerException.StackTrace) : "")
                    + streamWriter.NewLine + "--------------------------------------------------------------------------------");
                streamWriter.Close();