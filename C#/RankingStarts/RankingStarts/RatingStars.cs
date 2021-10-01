using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RankingStarts
{
    public partial class RatingStars : UserControl
    {
        public event EventHandler RateChanged;
        public int Rate
        {
            get
            {
                int number = 0;

                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(PictureBox))
                    {
                        PictureBox p = (PictureBox)item;

                        if (Convert.ToInt32(p.Tag) == 1)
                        {
                            number++;
                        }
                    }
                }

                return number;
            }
        }
        public RatingStars()
        {
            InitializeComponent();

            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(PictureBox))
                {
                    PictureBox p = (PictureBox)item;

                    p.Image = imageList1.Images[0];
                }
            }
        }

        private void RatingStars_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox ClickedPicturebox = (PictureBox)sender;

            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() == typeof(PictureBox))
                {
                    PictureBox p = (PictureBox)this.Controls[i];

                    if (p.Name.CompareTo(ClickedPicturebox.Name) == -1 || p.Name.CompareTo(ClickedPicturebox.Name) == 0)
                    {
                        p.Image = imageList1.Images[1];

                        p.Tag = 1;
                    }
                    else
                    {
                        p.Image = imageList1.Images[0];

                        p.Tag = 0;
                    }
                }
            }

            RateChanged(this, e);
        }
    }
}
