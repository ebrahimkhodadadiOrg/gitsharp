Stack<TreeNode> Nodes = new Stack<TreeNode>();
            TreeNode Root = new TreeNode("E:\\PDF",1,1);
            Nodes.Push(Root);

            while (Nodes.Count>0)
            {
                TreeNode t = Nodes.Pop();
                foreach (string item in Directory.GetDirectories(t.Text))
                {
                    TreeNode tmp = new TreeNode(item, 1, 1);
                    t.Nodes.Add(tmp);
                    Nodes.Push(tmp);
                }
            }

            treeView1.Nodes.Add(Root);
