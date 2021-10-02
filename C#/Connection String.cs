if (txtIP.Text.Trim().Length == 0)
            {
                Class.Message.ShowError("IP را وارد نمایید.");
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("ServerIP");
            config.AppSettings.Settings.Remove("ConnectionString");
            config.AppSettings.Settings.Add("ServerIP", txtIP.TextValue.Trim());
            //config.AppSettings.Settings.Add("ConnectionString", "Data Source="
            //    + txtIP.TextValue.Trim() + "\\SQL2008R2;Initial Catalog=Offset;User ID=sa;Password=a123A;MultipleActiveResultSets=True");
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["Rangarang_Offset.Properties.Settings.OffsetConnectionString"].ConnectionString = "Data Source="
                + txtIP.TextValue.Trim() + "\\SQL2008R2;Initial Catalog=Offset;User ID=sa;password=a123A;";
            var connectionStringsSectionEntity = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSectionEntity.ConnectionStrings["orderEntity"].ConnectionString = "metadata=res://*/DataModel.edmOrder.csdl|res://*/DataModel.edmOrder.ssdl|res://*/DataModel.edmOrder.msl;provider=System.Data.SqlClient;provider connection string=" + '\u0022' + "Data Source=" + txtIP.TextValue.Trim() + "\\SQL2008R2;Initial Catalog=Offset;User ID=sa;Password=a123A;MultipleActiveResultSets=True" + '\u0022';
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
            ConfigurationManager.RefreshSection("appSettings");
            clsGlobal.ServerIP = txtIP.TextValue.Trim();
            DialogResult = DialogResult.OK;
            Class.Message.ShowInfo("اطلاعات با موفقیت ذخیره گردید. لطفا نرم افزار را مجددا اجرا کنید.");
            Close();