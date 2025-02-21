using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dalamud.Updater
{
    public partial class FormXL : Form
    {
        public FormXL()
        {
            InitializeComponent();
        }

        private void FormXL_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void FormXL_Paint(object sender, PaintEventArgs e)
        {
            // Define the gradient colors
            var color1 = Color.FromArgb(19, 47, 70);
            var color2 = Color.FromArgb(55, 141, 255);
            // new color "#132f46"


            // Create a LinearGradientBrush
            using var brush = new LinearGradientBrush(this.ClientRectangle, color1, color2, 30F);
            // Fill the form's background with the gradient
            //e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://aonyx.ffxiv.wang/?form=du");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
