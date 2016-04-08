using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using LedighedsApp.Model.DataModel.Enum;

namespace LedighedsApp.View.Animation
{
    public class GuiAnimation
    {
        private StackPanel _stackpanel;
        private Queue<AnimationStep> _steps = new Queue<AnimationStep>();
        private AnimationStep _currentStep = null;
        private AnimationType _type;
        private Point _initialPoint;
        private int _tick = 0;

        public bool IsDone { get; private set; }



        public GuiAnimation(StackPanel stackPanel, AnimationType type)
        {
            _stackpanel = stackPanel;
            _type = type;
            IsDone = false;
            _initialPoint = new Point(Canvas.GetLeft(stackPanel), Canvas.GetTop(stackPanel));
        }

        public GuiAnimation(Point initalPoint)
        {
            
            IsDone = false;
            _initialPoint = initalPoint;
        }

        public void AddStep(double x, double y, int duration)
        {
            _steps.Enqueue(new AnimationStep(x,y,duration));
        }

        public void AddPause(int duration)
        {
            _steps.Enqueue(new AnimationStep(duration));
        }

        public void Animate()
        {
            if (_currentStep == null&&_steps.Count>0)
            {
                _currentStep = _steps.Dequeue();
            }

            if (_tick == _currentStep.DurationInTicks && _steps.Count > 0)
            {
                _currentStep = _steps.Dequeue();
                _tick = 0;
            }
            else if (_tick == _currentStep.DurationInTicks && _steps.Count == 0)
            {
                IsDone = true;
            }

            if (_tick < _currentStep.DurationInTicks)
            {
                if (!_currentStep.IsPause)
                {
                    switch (_type)
                    {
                        case AnimationType.Position:
                            PositionAnimation();
                            break;

                        case AnimationType.Size:
                            SizeAnimation();
                            break;
                    }  
                }
                _tick++;
            }   
        }

        public Point AnimatedPoint()
        {
            Point point = new Point();

            if (_currentStep == null && _steps.Count > 0)
            {
                _currentStep = _steps.Dequeue();
            }

            if (_tick == _currentStep.DurationInTicks && _steps.Count > 0)
            {
                _currentStep = _steps.Dequeue();
                _tick = 0;
            }
            else if (_tick == _currentStep.DurationInTicks && _steps.Count == 0)
            {
                IsDone = true;
            }

            if (_tick < _currentStep.DurationInTicks)
            {
                if (!_currentStep.IsPause)
                {
                    point = PointAnimation();
                }
                _tick++;
            }
            return point;
        }

        public void Reset()
        {
            _tick = 0;
            IsDone = false;
        }

        private void SizeAnimation()
        {
            _stackpanel.Width = Lerp(_stackpanel.Width, _currentStep.X, 0.5);
            _stackpanel.Height = Lerp(_stackpanel.Height, _currentStep.Y, 0.5);
        }

        private Point PointAnimation()
        {
            _initialPoint.X = Lerp(_initialPoint.X, _currentStep.X, 0.5);
            _initialPoint.Y = Lerp(_initialPoint.Y, _currentStep.Y, 0.5);
            return new Point(_initialPoint.X,_initialPoint.Y);
        }

        private void PositionAnimation()
        {
            Canvas.SetLeft(_stackpanel, Lerp(Canvas.GetLeft(_stackpanel), _currentStep.X, 0.5));
            Canvas.SetTop(_stackpanel, Lerp(Canvas.GetTop(_stackpanel), _currentStep.Y, 0.5));
        }

        private double Lerp(double value1, double value2, double t)
        {
            return (1 - t) * value1 + t * value2;
        }




    }
}
