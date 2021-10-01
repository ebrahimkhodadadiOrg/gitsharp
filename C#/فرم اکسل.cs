using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop;
using Telerik.WinControls.UI;
using Rangarang_Offset.DataModel;
using System.Text;
using System.Globalization;

namespace NameNameSpace
{
    public partial class FrmImportFromExcel
    {
        public FrmImportFromExcel()
        {
            InitializeComponent();
        }

        #region OPEN WORKBOOK VARIABLES
        private static object vk_missing = System.Reflection.Missing.Value;
        private object vk_update_links = 0;
        private object vk_read_only = false;
        private object vk_format = 1;
        private object vk_password = vk_missing;
        private object vk_write_res_password = vk_missing;
        private object vk_ignore_read_only_recommend = true;
        private object vk_origin = vk_missing;
        private object vk_delimiter = vk_missing;
        private object vk_editable = false;
        private object vk_notify = false;
        private object vk_converter = vk_missing;
        private object vk_add_to_mru = false;
        private object vk_local = false;
        private object vk_corrupt_load = false;
        #endregion

        #region CLOSE WORKBOOK VARIABLES
        private object vk_save_changes = false;
        private object vk_route_workbook = false;
        #endregion

        #region function
        private string Get_Excel_Column_Name(int ColIndex)
        {
            string col = "";

            if (ColIndex <= 26)
                col = Convert.ToChar(64 + ColIndex).ToString();
            else
            {
                col = "A" + Convert.ToChar(64 + (ColIndex - 26));
            }
            return col;

        }
        private long FindMaxCode()
        {
            try
            {
                var Variable = NameDataBase.NameTbl.Where(a => a.code.Length != 0).Select(a => a.code).ToList().Select(a => Convert.ToInt64(a)).ToList();
                if (Variable.Any())
                    return GiftCard.Max() + 1;
                return 1;
            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, Name);
                return 0;
            }
        }
        private string Set_String_Size(object NewValue, int size)
        {
            if (NewValue == null)
                return "";
            string str = NewValue.ToString();
            if (str.Length > size)
                return str.Substring(0, size);
            else
                return str;
        }
        #endregion

        #region btn
        private void tsbclose_Click(object sender, EventArgs e)
        {
            Close();
        }
        //دکمه اضافه کردن excel
        private void tsbImport_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            OpenFileDialog op = new OpenFileDialog();
            string path = "";
            string strSource = "";

            if (op.ShowDialog() == DialogResult.OK)
            {
                strSource = op.FileName;
                path = op.FileName;
            }
            else
                return;

            try
            {
                System.Windows.Forms.Application.DoEvents();

                System.Diagnostics.Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                foreach (System.Diagnostics.Process p in pProcess)
                {
                    p.Kill();
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("En"));

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(strSource, vk_update_links, vk_read_only, vk_format, vk_password, vk_write_res_password, vk_ignore_read_only_recommend, vk_origin, vk_delimiter, vk_editable, vk_notify, vk_converter, vk_add_to_mru, vk_local, vk_corrupt_load);
                Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Worksheet)(excelBook.Worksheets[1]);

                Microsoft.Office.Interop.Excel.Range r;
                string strCell = "";

                gv.Rows.Clear();

                int rowIndex = -1;

                for (int i = 0; i < excelWorksheet.UsedRange.Rows.Count - 1; i++)
                {
                    if (i % 10 == 0)
                        System.Windows.Forms.Application.DoEvents();

                    strCell = Get_Excel_Column_Name(1) + (i + 2).ToString();
                    r = excelWorksheet.get_Range(strCell, strCell);


                    //gv.Rows.Add();
                    #region
                    //اضافه کردن سطر ها به excel
                    GridViewDataRowInfo dataRowInfo = new GridViewDataRowInfo(gv.MasterView);
                    dataRowInfo.Cells[0].Value = null;
                    //dataRowInfo.Cells[1].Value = null;
                    //dataRowInfo.Cells[2].Value = null;

                    gv.Rows.Add(dataRowInfo);
                    #endregion
                    rowIndex++;
                    gv.Rows[rowIndex].Cells[0].Value = (rowIndex + 1).ToString();

                    for (int j = 1; j < 4; j++)
                    {
                        strCell = Get_Excel_Column_Name(j) + (i + 2).ToString();
                        r = excelWorksheet.get_Range(strCell, strCell);

                        if (r.Value2 == null)
                            gv.Rows[rowIndex].Cells[j - 1].Value = "";
                        else
                            gv.Rows[rowIndex].Cells[j - 1].Value = r.Value2.ToString();

                    }

                }

                excelBook.Close(vk_save_changes, strSource, vk_route_workbook);

                System.Windows.Forms.Application.DoEvents();

                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                foreach (System.Diagnostics.Process p in pProcess)
                {
                    p.Kill();
                }


            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, Name);
                return;
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (gv.RowCount != 0)
            {
                try
                {
                    waitingControl1.Visible = true;
                    System.Windows.Forms.Application.DoEvents();
                    TblChoosenName Variable;
                    for (int i = 0; i < gv.RowCount; i++)
                    {
                        using (var transaction = new System.Transactions.TransactionScope())
                        {
                            try
                            {
                                StringBuilder sb = new StringBuilder();
                                Variable = new tblCCGiftCard();

                                ////چک کد
                                //if (gv.Rows[i].Cells[0].Value.ToString() != null || gv.Rows[i].Cells[0].Value.ToString() != "")
                                //{
                                //    int code;
                                //    if (Int32.TryParse(gv.Rows[i].Cells[0].Value.ToString(), out code))
                                //    {
                                //        if (NameDataBase.NameTbl.ToList().Any(a => a.Codeint == code))
                                //        {
                                //            sb.AppendLine("کد وارد شده یکی از سطر ها تکراری است لطفا بعدا آن را تصحیح کنید.");
                                //            Class.Message.ShowError(sb.ToString());

                                //            break;
                                //        }
                                //        else
                                //        {
                                //            giftCard.Codeint = code;
                                //            giftCard.code = null;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if (NameDataBase.NameTbl.ToList().Any(a => a.code == Set_String_Size(gv.Rows[i].Cells[0].Value, 255)))
                                //        {
                                //            sb.AppendLine("کد وارد شده یکی از سطر ها تکراری است لطفا بعدا آن را تصحیح کنید.");
                                //            Class.Message.ShowError(sb.ToString());

                                //            break;
                                //        }
                                //        else
                                //        {
                                //            Variable.code = Set_String_Size(gv.Rows[i].Cells[0].Value, 255);
                                //            Variable.Codeint = null;
                                //        }
                                //    }
                                //}
                                //

                                //چک تاریخ
                                //DateTime dt;
                                //string format = "yyyy-dd-MM";

                                //if (gv.Rows[i].Cells[2].Value.ToString() != null && gv.Rows[i].Cells[2].Value.ToString() != "")
                                //{
                                //    if (DateTime.TryParse(gv.Rows[i].Cells["Exptime"].Value.ToString(), out dt))
                                //    {
                                //        Variable.Exptime = Set_String_Size(gv.Rows[i].Cells["Exptime"].Value, 255);
                                //    }
                                //    else
                                //    {
                                //        sb.AppendLine("تاریخ وارد شده یکی از سطر ها اشتباه است لطفا بعدا آن را تصحیح کنید.");
                                //        Class.Message.ShowError(sb.ToString());
                                //        Variable.Exptime = null;
                                //    }
                                //}
                                //

                                Variable.Credit = Convert.ToDouble(gv.Rows[i].Cells["Credit"].Value);
                                Variable.CardStatus = "فعال";

                                NameDataBase.NameTbl.AddObject(giftCard);
                                Context.SaveChanges();
                                transaction.Complete();
                            }
                            catch (Exception) { transaction.Dispose(); }
                        }
                    }
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    clsGlobal.MainForm.CloseForm(this);
                }
                catch (Exception ex)
                {
                    clsGlobal.ErrorHandling(ex, Name);
                }
                finally
                {
                    waitingControl1.Visible = false;
                }
            }
        }

        //دریافت نمونه فایل
        private void tsbSample_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("En"));

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Add("");
                Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Worksheet)(excelBook.Worksheets[1]);
                Microsoft.Office.Interop.Excel.Range r;
                string strCell = "";
                for (int j = 1; j < 4/*تعداد سطر ها مثال 4*/; j++)
                {
                    strCell = Get_Excel_Column_Name(j) + (1).ToString();
                    r = excelWorksheet.get_Range(strCell, strCell);
                    switch (j)
                    {
                        case 1:
                            r.Value = "کد";
                            break;
                        case 2:
                            r.Value = "اعتبار";
                            break;
                        case 3:
                            r.Value = "زمان انقضا";
                            break;
                        case 4:
                        default:
                            break;
                    }

                }
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Excel 2003 File Format  |*.xls | Excel 2007 File Format | *.xlsx";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    excelBook.Close(true, save.FileName, vk_route_workbook);
                    MessageBox.Show("فایل با موفقیت ذخیره شد");
                    System.Diagnostics.Process.Start(save.FileName);
                }

            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, Name);
            }

        }
        #endregion

    }
}