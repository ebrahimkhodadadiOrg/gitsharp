using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserContorlTest
{
    public partial class AdvancedTextbox : TextBox
    {
        public event EventHandler Textchanged;

        /// <summary>
        /// The text to show in the textbox when there is no text inside it.
        /// </summary>
        public string DefaultTextToShow
        {
            set
            {
                DefaultText = value;
            }
            get
            {
                return DefaultText;
            }
        }

        private string DefaultText;
        private bool HasDefaultText;
        public AdvancedTextbox()
        {
            InitializeComponent();
        }

        public AdvancedTextbox(string defaulttext)
        {
            InitializeComponent();
            this.DefaultText = defaulttext;
            EnterDefaultTextState();
        }

        private void EnterDefaultTextState()
        {
            HasDefaultText = true;
            this.textBox1.Text = "[" + this.DefaultText + "]";
            this.textBox1.ForeColor = Color.Gray;
        }

        private void LeaveDefaultTextState()
        {
            this.textBox1.Text = "";
            HasDefaultText = false;
            this.textBox1.ForeColor = Color.Black;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (HasDefaultText)
            {
                LeaveDefaultTextState();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                EnterDefaultTextState();
            }
        }



    }
}
