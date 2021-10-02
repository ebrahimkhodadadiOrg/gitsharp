 var serializer = new JavaScriptSerializer();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                var encoding = new UTF8Encoding();

                var postData = encoding.GetBytes(serializer.Serialize(model));
                request.Method = "POST";
                request.ContentType = "application/json;charset=utf-8";
                //"text/plain;charset=utf-8"; //"application/x-www-form-urlencoded"; //"application/json";
                request.ContentLength = postData.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                    var response = (HttpWebResponse)request.GetResponse();
                    result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    bool res;
                    Boolean.TryParse(result, out res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, "**clsGlobal.PostModelToWeb**");
            }