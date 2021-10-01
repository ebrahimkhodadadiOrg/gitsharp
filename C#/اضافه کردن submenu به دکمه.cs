foreach (var item in n)
            {
                var item = new ToolStripMenuItem
                {
                    Name = btn_.Name + "_" + ExampleID,
                    Text = stimul.Name
                };
                item.Click += btn_Click;
                bt_Print.DropDownItems.Add(item);
            }
            btn_.DefaultItem = btn_.DropDownItems[0];