public void PrintNodesRecursive()
        {
            foreach (TreeViewItem items in treeview.Items)
            {
                tvc.Add(items);

                if (items.Items.Count > 0)
                    PrintNodesRecursive_Node(items);
            }
        }

        public void PrintNodesRecursive_Node(TreeViewItem tvsNode)
        {
            foreach (TreeViewItem items in tvsNode.Items)
            {
                tvc.Add(items);

                if (items.Items.Count > 0)
                    PrintNodesRecursive_Node(items);
            }
        }