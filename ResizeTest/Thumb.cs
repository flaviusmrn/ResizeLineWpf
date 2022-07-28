using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ResizeTest
{
    public class Thumb
    {
        private Point _initialPositionThumb;
        private Point _finalPositionThumb;
        private UserControl userControl;
        private readonly Canvas _canvas;
        private readonly Grid _grid;
        private Point _origin;
        private readonly Action<Point> thumbMovedCallback;
        private readonly Grid _thumb;

        public event EventHandler<EventArgs> PositionChanged;

        public Thumb(UserControl userControl, Canvas canvas, Grid grid, Point origin, Action<Point> thumbMovedCallback)
        {
            this.userControl = userControl;
            this._canvas = canvas;
            this._grid = grid;
            this._origin = origin;
            this.thumbMovedCallback = thumbMovedCallback;
            _thumb = new Grid
            {
                Width = 10,
                Height = 10,
                Background = Brushes.Red,
            };

            _thumb.MouseLeftButtonDown += Thumb_DragStarted;
            _thumb.MouseMove += Thumb_DragMove;
            _thumb.MouseLeftButtonUp += Thumb_DragCompleted;
            _thumb.RenderTransform = new TranslateTransform(_origin.X - _canvas.ActualWidth / 2, _origin.Y - _canvas.ActualHeight / 2);

            _grid.Children.Add(_thumb);
        }

        private void Thumb_DragStarted(object sender, MouseButtonEventArgs e)
        {
            var thumb = sender as Grid;
            if (thumb != null)
            {
                thumb.CaptureMouse();
                _initialPositionThumb = e.GetPosition(userControl);
            }
        }

        private void Thumb_DragMove(object sender, MouseEventArgs e)
        {
            var thumb = sender as Grid;
            if (thumb != null)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed &&
                thumb.IsMouseCaptured)
                {
                    _finalPositionThumb = e.GetPosition(userControl);
                    thumb.RenderTransform = new TranslateTransform(_finalPositionThumb.X - _canvas.ActualWidth / 2, _finalPositionThumb.Y - _canvas.ActualHeight / 2);
                    thumbMovedCallback?.Invoke(_finalPositionThumb);
                }
            }
        }

        private void Thumb_DragCompleted(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid thumb)
            {
                thumb.ReleaseMouseCapture();
            }
        }

        internal void Scale(double xScaling, double yScaling)
        {
            _origin = new Point(_origin.X * xScaling, _origin.Y * yScaling);
            _thumb.RenderTransform = new TranslateTransform(_origin.X - _canvas.ActualWidth / 2, _origin.Y  - _canvas.ActualHeight / 2);
        }
    }
}
