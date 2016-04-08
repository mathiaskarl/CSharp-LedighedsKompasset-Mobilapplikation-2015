using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LedighedsApp.View.Animation
{
    public class WaterLine
    {
        public Line Line;

        public WaterLine(Point bottomPoint, Point topPoint, LinearGradientBrush brush)
        {
            Line = new Line
            {
                Stroke = brush,
                StrokeThickness = 11,
                X1 = bottomPoint.X,
                X2 = topPoint.X,
                Y1 = bottomPoint.Y,
                Y2 = topPoint.Y
            };
        }
    }
}
