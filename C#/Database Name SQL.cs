var connectionS =
                System.Configuration.ConfigurationManager.
                ConnectionStrings["Rangarang_Offset.Properties.Settings.OffsetConnectionString"].ConnectionString;
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionS);
                string database = builder.InitialCatalog;
