
	public Load()
	{
        var test = nameDatabase.nametbl.OrderBy(a => a.Name).Select(a => new { ID = a.ID, a.Name }).ToList();
        test.Insert(0, new { ID = 0, Name = "همه" });

        ((GridViewComboBoxColumn)gridViewPaperList.Columns["NameColumn"]).DisplayMember = "Name";
        ((GridViewComboBoxColumn)gridViewPaperList.Columns["NameColumn"]).ValueMember = "ID";
        ((GridViewComboBoxColumn)gridViewPaperList.Columns["NameColumn"]).DataSource = material;
        //((GridViewComboBoxColumn)gridViewPaperList.Columns["NameColumn"]).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
        ((GridViewComboBoxColumn)gridViewPaperList.Columns["NameColumn"]).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
}
}
