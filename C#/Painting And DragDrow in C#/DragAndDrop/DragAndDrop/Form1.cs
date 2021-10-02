using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragAndDrop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).DoDragDrop(((PictureBox)sender), DragDropEffects.All);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox p = (PictureBox)e.Data.GetData(typeof(PictureBox));
            //p.Location = this.PointToClient(new Point(e.X, e.Y));
            //this.BackgroundImageLayout = ImageLayout.Stretch;
            //this.BackgroundImage = p.Image;
            PictureBox p1 = new PictureBox();
            p1.Size = p.Size;
            p1.Location = this.PointToClient(new Point(e.X, e.Y));
            p1.Image = p.Image;
            p1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(p1);
        }
    }
}
