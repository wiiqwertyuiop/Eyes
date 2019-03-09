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
 * Make eye movement better
 * Optimize (math might be able to be made simpler)
 * 
 */

namespace Eyes
{

    public partial class Form1 : Form
    {

        int mouseX = 0;
        int mouseY = 0;

        float x; // aka canvas zero X pos
        float sizeX; // x + sizeX = end of outer eye

        float y;
        float sizeY;

        public Form1()
        {
            InitializeComponent();
            this.Width = 400;
            this.Height = 400;

            this.DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black);
            Brush myBrush = new SolidBrush(Color.Black);

            float FormWidth = this.ClientSize.Width * 0.99f;
            float FormHeight = this.ClientSize.Height * 0.99f;

            float CanvasWidth = FormWidth * 0.75f; // X scale
            x = FormWidth - CanvasWidth; // aka canvas zero X pos
            float CanvasMiddle = CanvasWidth - x;
            sizeX = CanvasMiddle / 2.5f;

            float CanvasHeight = FormHeight * 0.62f; // Y scale
            y = FormHeight - CanvasHeight;
            sizeY = CanvasHeight - y;

            // Draw eyes
            g.DrawEllipse(myPen, x, y, sizeX, sizeY);
            g.DrawEllipse(myPen, CanvasMiddle+(x-sizeX), y, sizeX, sizeY);

            // Draw eyeballs
            PointF EyeBall = DrawEyeBall();
            g.FillEllipse(myBrush, EyeBall.X, EyeBall.Y, sizeX / 4, sizeY / 4); // Left

            EyeBall = DrawEyeBall(CanvasMiddle - sizeX);
            g.FillEllipse(myBrush, EyeBall.X, EyeBall.Y, sizeX / 4, sizeY / 4); // Right

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

        private PointF DrawEyeBall(float which = 0)
        {
            // Get middle of eye balls
            float EyeMidX = (x + (x + sizeX)) / 2 - (sizeX / 8) + which;
            float EyeMidY = (y + (y + sizeY)) / 2 - (sizeY / 8);

            // Distance from middle of eyeball to edge of eye
            float xBase = (x + sizeX + which) - EyeMidX - sizeX / 4;
            float yBase = (y + sizeY) - EyeMidY - sizeY / 4;

            // Mouse location -> grid coordinate with the middle of the eyeball being 0
            double mouseGraphPosX = mouseX - EyeMidX;
            double mouseGraphPosY = mouseY - EyeMidY;

            // Get hypotenuse based on mouse position (this is so the eyeball will move closer to the center as we get closer)
            double hypotenuse = Math.Sqrt(mouseGraphPosX * mouseGraphPosX + mouseGraphPosY * mouseGraphPosY);
            // Get sin and cos (this determines the X and Y pos of the eyeball obv)
            double xchange = mouseGraphPosX / hypotenuse;
            double ychange = mouseGraphPosY / hypotenuse;

            // Scale xBase
            if (hypotenuse / x < 1) xBase *= (float)hypotenuse / x;
            if (hypotenuse / y < 1) yBase *= (float)hypotenuse / y;

            /*if (hypotenuse / x < 1)
            {
                xBase -= xBase * ((float)hypotenuse / x);
            } else
            {
                xBase = 0;
            }*/

            // Move eyeball from middle to where it should go based on mouse position
            EyeMidX += xBase * (float)xchange;
            EyeMidY += yBase * (float)ychange;

            return new PointF(EyeMidX, EyeMidY);
        }
    }
}
