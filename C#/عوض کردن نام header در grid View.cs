private void gv_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.Row is GridViewTableHeaderRowInfo)
            {
                gv.Controls.Add(new TextBox() 
                {
                    Width = e.Row.Cells[e.ColumnIndex].ColumnInfo.Width,
                    Location = GetRadCellLocation(e.Row.Cells[e.ColumnIndex]),
                    TextAlign = System.Windows.Forms.HorizontalAlignment.Center,
                    Multiline = true,
                    Height = GetRadCellHeight(e.Row.Cells[e.ColumnIndex])
                });

            }
        }

        public Point GetRadCellLocation(GridViewCellInfo gridViewCellInfo)
        {
            GridCellElement cellElement = gv.TableElement.GetCellElement(gridViewCellInfo.RowInfo, gridViewCellInfo.ColumnInfo);
            return cellElement.ControlBoundingRectangle.Location;
        }
        public int GetRadCellHeight(GridViewCellInfo gridViewCellInfo)
        {
            GridCellElement cellElement = gv.TableElement.GetCellElement(gridViewCellInfo.RowInfo, gridViewCellInfo.ColumnInfo);
            return cellElement.ControlBoundingRectangle.Height;
        }