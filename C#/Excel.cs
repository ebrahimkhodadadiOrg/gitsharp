void SaveExcellFile(string path)
        {
            StiReport report = new StiReport();
            report.ScriptLanguage = StiReportLanguageType.CSharp;

            //Add data to datastore
            System.Data.DataTable dt = _listReports.ToDataTable("MyList");
            report.RegData("MyList", dt);

            //Fill dictionary
            report.Dictionary.Synchronize();
            StiPage page = report.Pages.Items[0];
            //Create HeaderBand
            page.RightToLeft = true;


            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Name = "HeaderBand";
            page.Components.Add(headerBand);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "MyList";
            //dataBand.Height = 50f;
            dataBand.RightToLeft = true;

            dataBand.Name = "DataBand";
            dataBand.RightToLeft = true;
            page.Components.Add(dataBand);

            StiDataSource dataSource = report.Dictionary.DataSources[0];

            //Create texts
            Double pos = 0;
            page.Width = 500;
            page.Height = 1500;
            Double columnWidth = StiAlignValue.AlignToMinGrid(page.Width / dataSource.Columns.Count, 0.1, true);
            columnWidth = 5;
            int nameIndex = 1;

            foreach (StiDataColumn column in dataSource.Columns)
            {
                if (!gv.Columns.Select(x => x.Name).Contains(column.Name))
                {
                    continue;
                }
                columnWidth = gv.Columns[column.Name].Width / 25;
                //Create text on header
                StiText headerText = new StiText(new RectangleD(pos, 0, columnWidth, 1.5f));

                headerText.Text.Value = gv.Columns[column.Name].HeaderText;
                headerText.HorAlignment = StiTextHorAlignment.Center;
                headerText.Name = "HeaderText" + nameIndex.ToString();
                headerText.Brush = new StiSolidBrush(Color.MediumSeaGreen);
                headerText.Border.Side = StiBorderSides.All;
                headerText.Font = new System.Drawing.Font("B Nazanin", 14, FontStyle.Bold);
                headerText.VertAlignment = StiVertAlignment.Center;
                headerBand.Components.Add(headerText);

                //Create text on Data Band
                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 1f));
                dataText.Text.Value = "{MyList." + column.Name + "}";
                dataText.Name = "DataText" + nameIndex.ToString();
                dataText.HorAlignment = StiTextHorAlignment.Center;
                dataText.Border.Side = StiBorderSides.All;
                dataText.VertAlignment = StiVertAlignment.Center;
                dataText.Font = new System.Drawing.Font("B Nazanin", 12);
                dataBand.Components.Add(dataText);
                pos += columnWidth;
                nameIndex++;

            }

            StiExcelExportService excelService = new StiExcelExportService();
            report.Render(false);
            excelService.Export(report, path);

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gv.RowCount < 0)
                {
                    Class.Message.ShowInfo("هیچ اطلاعاتی موجود نمی باشد .");
                    return;
                }

                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "مسیر ذخیره فایل اکسل را انتخاب کنید";

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    SaveExcellFile(fd.SelectedPath + "\\Report.xls");
                    Class.Message.ShowInfo("خروجی اکسل ذخیره شد .");
                }


            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, Name);
                Class.Message.ShowError("فایل اکسل باز می باشد .");
            }

        }
    }