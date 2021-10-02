using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintingInCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Goldenrod, new PointF(0, 0));
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();

            if (printDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                printDialog1.Document.Print();
            }
        }
    }
}
