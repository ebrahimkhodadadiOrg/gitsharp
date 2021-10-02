using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsInCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            Graphics g = e.Graphics;

            g.Clear(Color.FromArgb(r.Next(0,256),r.Next(0,256),r.Next(0,256)));
            //g.DrawArc(Pens.Red, new Rectangle(0, 0, 50, 50), 0, 360);
            //g.DrawCurve(Pens.Red, new Point[] { new Point(5, 5), new Point(50, 84), new Point(90, 10) });
            //g.DrawEllipse(Pens.Plum, new Rectangle(5, 5, 50, 50));
            //g.DrawImage(Image.FromFile(@"C:\Users\Public\Pictures\Sample Pictures\Desert.jpg"), new Point(10, 10));
            //g.DrawLine(Pens.PowderBlue, new Point(0, 0), new Point(40, 90));
            //g.DrawRectangle(Pens.Green, new Rectangle(0, 0, 45, 45));
            //g.DrawString("Button1", new Font("Georgia", 10), Brushes.PaleVioletRed, new PointF(45, 15));

            
            //g.FillEllipse(Brushes.PaleGoldenrod, new Rectangle(0, 0, 40, 40));
        }
    }
}
