using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplePaintingApp
{
    public partial class Form1 : Form
    {
        Bitmap CurrentBitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox2.BackColor = colorDialog1.Color;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(CurrentBitmap, new Point(0, 0));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                printDialog1.Document.Print();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentBitmap = new Bitmap(panel1.Width, panel1.Height);
            Graphics g = Graphics.FromImage(CurrentBitmap);

            g.Clear(Color.White);
            panel1.BackgroundImage = CurrentBitmap;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(CurrentBitmap);
            g.Clear(Color.White);
            panel1.BackgroundImage = CurrentBitmap;
            panel1.Refresh();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop("1", DragDropEffects.All);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop("2", DragDropEffects.All);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop("3", DragDropEffects.All);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop("4", DragDropEffects.All);
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop(textBox1.Text, DragDropEffects.All);
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            Graphics g = Graphics.FromImage(CurrentBitmap);
            Point p;

            switch (e.Data.GetData(typeof(string)).ToString())
            {
                case "1":
                    p = panel1.PointToClient(new Point(e.X - 10, e.Y - 10));
                    g.DrawEllipse(new Pen(pictureBox2.BackColor), new Rectangle(p.X,p.Y, 20, 20));
                    break;
                case "2":
                    p = panel1.PointToClient(new Point(e.X - 10, e.Y - 10));
                    g.DrawRectangle(new Pen(pictureBox2.BackColor), new Rectangle(p.X,p.Y, 20, 20));
                    break;
                case "3":
                    p = panel1.PointToClient(new Point(e.X - 10, e.Y - 10));
                    g.FillEllipse(new SolidBrush(pictureBox2.BackColor), new Rectangle(p.X, p.Y, 20, 20));
                    break;
                case "4":
                    p = panel1.PointToClient(new Point(e.X - 10, e.Y - 10));
                    g.FillRectangle(new SolidBrush(pictureBox2.BackColor), new Rectangle(p.X, p.Y, 20, 20));
                    break;
                default:
                    p = panel1.PointToClient(new Point(e.X - 10, e.Y - 10));
                    g.DrawString(e.Data.GetData(typeof(string)).ToString(), this.Font, new SolidBrush(pictureBox2.BackColor), p);
                    break;
            }

            panel1.BackgroundImage = CurrentBitmap;
            panel1.Refresh();
        }
    }
}
