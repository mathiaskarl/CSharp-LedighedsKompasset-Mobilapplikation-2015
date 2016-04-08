using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DBMS;
using LedighedsApp.View.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LedighedsApp.View.UserControls
{
    public sealed partial class TutorialUc : UserControl
    {
        private List<Tutorial> _tutorials;
        private List<GuiAnimation> _sizeAnimations;
        private List<GuiAnimation> _positionAnimations;
        private Tutorial _currentTutorial;
        private int _currentStep = 1;
        private double _desiredWidth = 300;
        public double _desiredHeight = 0;
        private bool _isOpen = false;

        private DispatcherTimer _timer;

        public TutorialUc(List<Tutorial> tutorials)
        {
            this.InitializeComponent();
            TopBorder.Background = new SolidColorBrush(HexToColor.GetColor("778899"));
            Container.BorderBrush = new SolidColorBrush(HexToColor.GetColor("778899"));
            Width = 300;
            Height = 0;
            
            _sizeAnimations = new List<GuiAnimation>();
            _positionAnimations = new List<GuiAnimation>();
            _tutorials = new List<Tutorial>();
            _tutorials = tutorials;

            SetContent();
            SetButtons();
        }

        public void OnBootTrigger()
        {
            if (_currentTutorial != null)
                if (!_currentTutorial.HasBeenSeen && User.Instance.Settings.Tutorial)
                {
                    Trigger();
                    _currentTutorial.HasBeenSeen = true;
                    Conn.Update(_currentTutorial);
                }
        }

        public void Trigger()
        {
            if (!_isOpen && _currentTutorial != null)
            {
                GuiAnimation sizeAnimation = new GuiAnimation(new Point(0, 0));
                sizeAnimation.AddStep(_desiredWidth, _desiredHeight, 20);
                _sizeAnimations.Add(sizeAnimation);

                GuiAnimation positionAnimation = new GuiAnimation(new Point(300, _desiredHeight));
                positionAnimation.AddStep(0, 0, 20);
                _positionAnimations.Add(positionAnimation);

                if (!_timer.IsEnabled)
                {
                    _timer.Start();    
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _isOpen = false;
            GuiAnimation sizeAnimation = new GuiAnimation(new Point(_desiredWidth-2, _desiredHeight-2));
            sizeAnimation.AddStep(0, 0, 20);
            _sizeAnimations.Add(sizeAnimation);

            GuiAnimation positionAnimation = new GuiAnimation(new Point(0, 0));
            positionAnimation.AddStep(300, _desiredHeight, 20);
            _positionAnimations.Add(positionAnimation);

            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        private void SetButtons()
        {
            BackwardButton.IsEnabled = _currentStep > 1;
            ForwardButton.IsEnabled = _currentStep < _tutorials.Count;
        }

        private void SetContent()
        {
            foreach (Tutorial t in _tutorials.Where(t => t.StepNum == _currentStep))
            {
                _currentTutorial = t;
            }
            if (_currentTutorial == null) return;
            TextBlock.Text = _currentTutorial.Text;
            TextBlock.Measure(new Size());
            TextBlock.Arrange(new Rect());
            StepsTextBlock.Text = _currentTutorial.StepNum + "/" + _tutorials.Count;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            _currentStep++;
            SetContent();
            SetButtons();
            
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            _currentStep--;
            SetContent();
            SetButtons();
        }

        

        private void InitTimer()
        {
            _timer = new DispatcherTimer(){Interval = new TimeSpan(0,0,0,0,10)};
            _timer.Tick += _timer_Tick;
        }

        void _timer_Tick(object sender, object e)
        {
            int count = 0;
            foreach (Point p in from animation in _sizeAnimations where !animation.IsDone select animation.AnimatedPoint())
            {
                VerifyAndSetDimensions(p);
                count++;
            }

            foreach (Point p in from animation in _positionAnimations where !animation.IsDone select animation.AnimatedPoint())
            {
                Margin = new Thickness(p.X,-_desiredHeight,0,0);
                count++;
            }

            if (count == 0)
            {
                _timer.Stop();
            }
        }

        private void VerifyAndSetDimensions(Point size)
        {
            if (size.X > _desiredWidth-2 && size.Y > _desiredHeight-2 &&!_isOpen)
            {
                _isOpen = true;
            }

            if (_isOpen)
            {    
                Width = _desiredWidth;
                Height = _desiredHeight;
                
            }
            else
            {
                Width = size.X;
                Height = size.Y;
            }
                
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _desiredHeight = 90 + (int)TextBlock.ActualHeight;
            InitTimer();
            OnBootTrigger();
        }

        private void TextBlock_LayoutUpdated(object sender, object e)
        {
            if (_isOpen)
            {
                _desiredHeight = 90 + (int)TextBlock.ActualHeight;
                Width = _desiredWidth;
                Height = _desiredHeight;
                Margin = new Thickness(Margin.Left, -_desiredHeight, 0, 0);
            }
            
        }
    }
}
