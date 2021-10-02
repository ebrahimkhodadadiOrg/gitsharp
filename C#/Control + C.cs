if (!e.Data.GetDataPresent(typeof(MyDataType)))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }