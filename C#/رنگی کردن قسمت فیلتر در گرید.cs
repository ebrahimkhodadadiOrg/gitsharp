private void GridViewEX_ViewRowFormatting(object sender, RowFormattingEventArgs e)
{
    if (this.FilterDescriptors.ToList().Count == 0)
        checkRowHeader = false;
    else
        checkRowHeader = true;

    //change filter mode color
    if (checkRowHeader)
    {
        if (e.RowElement is GridFilterRowElement)
        {
            e.RowElement.DrawFill = true;
            e.RowElement.BackColor = Color.Gray;
        }
    }

    if (!checkRowHeader)
    {
        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
        e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
        e.RowElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
        e.RowElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
    }
}