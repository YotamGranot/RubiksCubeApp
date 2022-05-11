using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeApp
{
    public class RubikPart
    {
        private (float, float) topLeft, topRight, bottomLeft, bottomRight;
        private Color color;

        public RubikPart((float, float) topLeft, (float, float) topRight, (float, float) bottomLeft, (float, float) bottomRight, Color color)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;
            this.color = color;

        }

        public RubikPart(RubikPart rp)
        {
            this.bottomLeft = rp.bottomLeft;
            this.bottomRight = rp.bottomRight;
            this.topLeft = rp.topLeft;
            this.topRight = rp.topRight;
            this.color = rp.color;
        }
        public void setColor(Color c)
        {
            this.color = c;
        }
        public void draw(Canvas canvas)
        {
            Paint p = new Paint();
            p.Color = color;
            p.SetStyle(Paint.Style.Fill);
            p.StrokeWidth = 30;
            Path path = new Path();
            path.MoveTo(topLeft.Item1, topLeft.Item2);
            path.LineTo(topRight.Item1, topRight.Item2);
            path.LineTo(bottomRight.Item1, bottomRight.Item2);
            path.LineTo(bottomLeft.Item1, bottomLeft.Item2);
            path.LineTo(topLeft.Item1, topLeft.Item2);
            path.Close();

            canvas.DrawPath(path, p);
            p.Color = Color.Black;
            p.SetStyle(Paint.Style.Stroke);
            canvas.DrawPath(path, p);

        }

        internal bool isIn(float x, float y)
        {
            Path path = new Path();
            path.MoveTo(topLeft.Item1, topLeft.Item2);
            path.LineTo(topRight.Item1, topRight.Item2);
            path.LineTo(bottomRight.Item1, bottomRight.Item2);
            path.LineTo(bottomLeft.Item1, bottomLeft.Item2);
            path.LineTo(topLeft.Item1, topLeft.Item2);
            path.Close();
            RectF rectF = new RectF();
            path.ComputeBounds(rectF, true);
            Region region = new Region();
            region.SetPath(path, new Region((int)rectF.Left, (int)rectF.Top, (int)rectF.Right, (int)rectF.Bottom));
            return region.Contains((int)x, (int)y);
        }
    }
}