using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.DataModel;

namespace LedighedsApp.View.Animation
{
    class WaterSystem
    {
        private List<WaterLine> _water;
        private int _nextWaterHeight;
        private double _step = 0;
        private double _radius = 2.0;
        private bool _waterHeightChanged = false;
        private bool _waterIsMoving = false;

        public int CurrentWaterHeight;
        public int Offset = 0;

        public WaterSystem()
        {
            _water = new List<WaterLine>();
        }

        public void Update()
        {
            for (int i = 0; i < _water.Count; i++)
            {
                _water[i].Line.Y2 = CalculateVerticalPosition(i);
            }

            _step += 0.5;

            if (!_waterHeightChanged) return;

            _waterIsMoving = true;

            if (CurrentWaterHeight > _nextWaterHeight && CurrentWaterHeight > _nextWaterHeight+1)
            {
                CurrentWaterHeight-=2;
            }
            else if (CurrentWaterHeight < _nextWaterHeight && CurrentWaterHeight < _nextWaterHeight + 1)
            {
                CurrentWaterHeight+=2;
            }
            else
            {
                _waterHeightChanged = false;
                _waterIsMoving = false;
            }
        }

        public void SetWaterHeight(int height)
        {
            _waterHeightChanged = true;
            _nextWaterHeight = height;
        }

        private double CalculateVerticalPosition(double internalOffset)
        {
            if (_waterIsMoving && _radius < 5.0)
            {
                _radius += 0.001;
            }
            else if (!_waterIsMoving && _radius > 2.0)
            {
                _radius -= 0.001;
            }
            return Math.Sin(_step + (internalOffset / 5)+Offset) * _radius + CurrentWaterHeight;
        }

        public void InitWater(Canvas canvas)
        {
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 17, 71, 117), Offset = 0.0 });
            myBrush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 43, 116, 178), Offset = 0.5 });
            myBrush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 65, 154, 228), Offset = 1.0 });

            
            for (int i = 0; i <= (int) canvas.ActualWidth + 1; i += 10)
            {
                    _water.Add(new WaterLine(new Point(i, canvas.ActualHeight),
                    new Point(i, canvas.ActualHeight - CurrentWaterHeight), myBrush));
            }
           
            

            foreach (WaterLine w in _water)
            {
                canvas.Children.Add(w.Line);
            }
        }
    }
}
