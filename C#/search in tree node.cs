private void txtSearch_TextChanging(object sender, Telerik.WinControls.TextChangingEventArgs e)
{
    //Search in tree view 
    if (txtSearch.Text.Trim() != "")
        SearchRecursive(treeView1.Nodes, txtSearch.Text);
    else
    {
        foreach (TreeNode node in treeView1.Nodes)
            node.BackColor = Color.White;
    }
}

private bool SearchRecursive(IEnumerable nodes, string searchFor)
{
    foreach (TreeNode node in nodes)
    {
        if (node.Text.ToUpper().Contains(searchFor) || node.Text.ToLower().Contains(searchFor))
        {
            treeView1.SelectedNode = node;
            node.BackColor = Color.Yellow;
        }
        else
        {
            node.BackColor = Color.White;
        }

        if (SearchRecursive(node.Nodes, searchFor))
            return true;
    }
    return false;
}