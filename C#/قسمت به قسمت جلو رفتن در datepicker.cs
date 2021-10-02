private void FaDatePicker_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Text))
            {
                this.SelectionStart = 0;
                this.SelectionLength = 4;
                counter = 0;
            }
        }

        private void FaDatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            counter += 1;
            if (counter == 5)
            {
                this.SelectionStart = 5;
                this.SelectionLength = 2;
            }
            if (counter == 7)
            {
                this.SelectionStart = 8;
                this.SelectionLength = 2;
            }
            if (counter == 8)
            {
                SendKeys.Send("{TAB}");
            }
        }