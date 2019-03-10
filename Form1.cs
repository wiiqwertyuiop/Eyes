using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * TODO:
 * Optimize and clean up code
 * 
 * Add switch between normal eye movement, independent, and same
 * 
 */

namespace Eyes
{

    public partial class Form1 : Form
    {
        const float XSCALE = 0.75f;
        const float YSCALE = 0.62f;

        int mouseX = 0;
        int mouseY = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black);

            float FormWidth = this.ClientSize.Width * 0.99f;
            float FormHeight = this.ClientSize.Height * 0.99f;

            float CanvasWidth = FormWidth * XSCALE; // Canvas width is the form width scaled down
            float CanvasHeight = FormHeight * YSCALE; // Y scale

            CanvasStartXPos = FormWidth * (1 - XSCALE); // canvas zero X pos
            CanvasMiddle = FormWidth * (2*XSCALE - 1); // Middle of canvas x-wise
            sizeX = CanvasMiddle / 2.5f;
            
            CanvasStartYPos = FormHeight * (1 - YSCALE); // Y
            sizeY = CanvasHeight - CanvasStartYPos;

            // Draw eyes
            g.DrawEllipse(myPen, CanvasStartXPos, CanvasStartYPos, sizeX, sizeY);
            g.DrawEllipse(myPen, CanvasWidth-sizeX, CanvasStartYPos, sizeX, sizeY);

            // Draw eyeballs
            if (this.normalToolStripMenuItem.Checked) EnterNormal(ref g);
            else if (this.independentToolStripMenuItem.Checked) EnterIndependent(ref g);
            else EnterFixed(ref g);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            this.Invalidate();
        }

        // Menu items
        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.normalToolStripMenuItem.Checked = true;
            this.independentToolStripMenuItem.Checked = false;
            this.fixedToolStripMenuItem.Checked = false;
        }

        private void independentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.normalToolStripMenuItem.Checked = false;
            this.independentToolStripMenuItem.Checked = true;
            this.fixedToolStripMenuItem.Checked = false;
        }

        private void fixedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.independentToolStripMenuItem.Checked = false;
            this.normalToolStripMenuItem.Checked = false;
            this.fixedToolStripMenuItem.Checked = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop) System.Windows.Forms.Application.Exit();
            else System.Environment.Exit(1);
        }
    }
}
