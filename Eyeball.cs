using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Eyes
{
    partial class Form1
    {
        float CanvasStartXPos; // aka canvas zero X pos
        float sizeX; // x + sizeX = end of outer eye

        float CanvasStartYPos;
        float sizeY;

        float CanvasMiddle; // Middle of canvas (x-wise only)

        private Brush myBrush = new SolidBrush(Color.Black);

        // Different tracking methods
        void EnterFixed(ref Graphics g) // One eyeball controls both eyes
        {
            float EyeMidX = CanvasStartXPos + (3 * sizeX / 8); // Get the X pos that is in the middle of the eye

            // EyeMidX determines which eye is being calculated for
            // in this case we want the left eyeball to handle movement for both eyes
            PointF loc = DrawEyeBall(EyeMidX);

            // loc.X contains the amount to change X (done this way for use in further calculations in other routines)
            loc.X += EyeMidX;

            // loc.Y is already ready for use
            g.FillEllipse(myBrush, loc.X, loc.Y, sizeX / 4, sizeY / 4); // Left
            g.FillEllipse(myBrush, loc.X + (CanvasMiddle - sizeX), loc.Y, sizeX / 4, sizeY / 4); // Right
        }

        void EnterIndependent(ref Graphics g) // Each eye moves independently
        {
            float EyeMidX = CanvasStartXPos + (3 * sizeX / 8); // Get the X pos that is in the middle of the eye
            float DistanceToRightEye = CanvasMiddle - sizeX; // This is the distance to the right eye

            // Calculate position for the left eye
            PointF loc = DrawEyeBall(EyeMidX);
            g.FillEllipse(myBrush, loc.X+EyeMidX, loc.Y, sizeX / 4, sizeY / 4); // Left

            // Now we calculate the right eye's own positions
            EyeMidX += DistanceToRightEye;

            // Calculate position for the right eye
            loc = DrawEyeBall(EyeMidX);
            g.FillEllipse(myBrush, loc.X+EyeMidX, loc.Y, sizeX / 4, sizeY / 4); // Right
        }

        void EnterNormal(ref Graphics g) // In the middle the eyes act independently, otherwise whichever eye the cursor is closer to has control                       
        {
            // Do calculations for each eyeball
            float EyeMidX_l = CanvasStartXPos + (3 * sizeX / 8);
            float DistanceToRightEye = CanvasMiddle - sizeX; // This is the distance to the right eye
            float EyeMidX_r = DistanceToRightEye + EyeMidX_l;

            // this moves the pos of the spot where they switch control so it looks more natural
            float waitDist = (CanvasMiddle - CanvasStartXPos - sizeX) / 2;

            PointF loc;
            // Depending on which half of the canvas we are on, give the closest eyeball to the cursor control
            if (mouseX > CanvasMiddle - waitDist) loc = DrawEyeBall(EyeMidX_r);
            else loc = DrawEyeBall(EyeMidX_l);

            // If we are in the middle of the screen and the right/left eye has control, flip our X-pos so we look towards the middle
            if (mouseX < EyeMidX_r && mouseX > CanvasMiddle-waitDist) EyeMidX_l += loc.X * -1;
            else EyeMidX_l += loc.X; // Add the distance to change, to the left eyeballs position

            g.FillEllipse(myBrush, EyeMidX_l, loc.Y, sizeX / 4, sizeY / 4); // Left            

            if (mouseX > EyeMidX_l && mouseX < CanvasMiddle-waitDist) EyeMidX_r += loc.X * -1;
            else EyeMidX_r += loc.X;

            g.FillEllipse(myBrush, EyeMidX_r, loc.Y, sizeX / 4, sizeY / 4); // Right
        }

        private PointF DrawEyeBall(float EyeMidX)
        {
            float xBase = 3 * sizeX / 8; // Distance from middle of eyeball to edge of eye (x-wise)

            float EyeMidY = CanvasStartYPos + (3 * sizeY / 8); // Get the Y position that is the middle of the eye
            float yBase = 3 * sizeY / 8; // Get Y distance from middle to the edge of the eye

            // Mouse location -> grid coordinate with the middle of the eyeball being 0
            double mouseGraphPosX = mouseX - EyeMidX;
            double mouseGraphPosY = mouseY - EyeMidY;

            // Get hypotenuse based on mouse position (this is so the eyeball will move closer to the center as we get closer)
            double hypotenuse = Math.Sqrt(mouseGraphPosX * mouseGraphPosX + mouseGraphPosY * mouseGraphPosY);
            // Get sin and cos (this determines the X and Y pos of the eyeball obv)
            double xchange = mouseGraphPosX / hypotenuse;
            double ychange = mouseGraphPosY / hypotenuse;

            // Scale the postion it takes to get to the edge of the eye
            if (hypotenuse / CanvasStartXPos < 1) xBase *= (float)hypotenuse / CanvasStartXPos;
            if (hypotenuse / CanvasStartYPos < 1) yBase *= (float)hypotenuse / CanvasStartYPos;

            // Move eyeball from middle to where it should go based on mouse position
            EyeMidY += yBase * (float)ychange;

            // For X just return the distance to change
            return new PointF(xBase * (float)xchange, EyeMidY);
        }
    }
}
