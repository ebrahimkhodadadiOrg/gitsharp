using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsSecondTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool ISMouseDown = false;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            //g.Clear(pictureBox1.BackColor);
            //g.FillEllipse(Brushes.Khaki, new Rectangle(e.X - 25, e.Y - 25, 50, 50));

            if (ISMouseDown)
            {
                g.FillEllipse(Brushes.Red, new Rectangle(e.X - 2, e.Y - 2, 4, 4));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ISMouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            ISMouseDown = false;
        }
    }
}
