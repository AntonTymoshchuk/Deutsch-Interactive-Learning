using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Deutsch_Interactive_Learning.FlatDesign
{
    public class Colors
    {
        private Color Color;
        private int step;

        public Color Origin
        {
            get { return Color; }
        }

        public int Step
        {
            get { return step; }
        }

        public Color Constant
        {
            get { return Color.White; }
        }

        public Color Background
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - Convert.ToInt32(step * 1.5);
                if (Red < 0)
                    Red = 0;

                Green = Color.G - Convert.ToInt32(step * 1.5);
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - Convert.ToInt32(step * 1.5);
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Color OriginButtonMouseOver
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - step;
                if (Red < 0)
                    Red = 0;

                Green = Color.G - step;
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - step;
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Color OriginButtonMouseDown
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - step * 2;
                if (Red < 0)
                    Red = 0;

                Green = Color.G - step * 2;
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - step * 2;
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Color Curtain
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - step * 3;
                if (Red < 0)
                    Red = 0;

                Green = Color.G - step * 3;
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - step * 3;
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Color CurtainButtonMouseOver
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - step * 4;
                if (Red < 0)
                    Red = 0;

                Green = Color.G - step * 4;
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - step * 4;
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Color CurtainButtonMouseDown
        {
            get
            {
                int Red, Green, Blue;

                Red = Color.R - step * 5;
                if (Red < 0)
                    Red = 0;

                Green = Color.G - step * 5;
                if (Green < 0)
                    Green = 0;

                Blue = Color.B - step * 5;
                if (Blue < 0)
                    Blue = 0;

                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public Colors(Color Color, int Step)
        {
            this.Color = Color;
            this.step = Step;
        }

        public Color GetColorByStep(int Step)
        {
            int Red, Green, Blue;

            Red = Color.R - Step;
            if (Red < 0)
                Red = 0;

            Green = Color.G - Step;
            if (Green < 0)
                Green = 0;

            Blue = Color.B - Step;
            if (Blue < 0)
                Blue = 0;

            return Color.FromArgb(Red, Green, Blue);
        }
    }
}
