using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ResizeTest
{
    public class ResizeableLine
    {
        private readonly Canvas _canvas;
        private readonly Grid _grid;
        private Point _initialPosition;
        private Point _finalPosition;
        private double _previousWidth;
        private double _previousHeight;
        private UserControl _userControl;

        private Line _line;
        private Thumb _thumb1;
        private Thumb _thumb2;

        public ResizeableLine(UserControl userControl, Canvas canvas, Grid grid,
            Point initialPoint, Point finalPoint)
        {
            _userControl = userControl;
            this._canvas = canvas;
            this._grid = grid;
            this._initialPosition = initialPoint;
            this._finalPosition = finalPoint;
            this._previousWidth = canvas.ActualWidth;
            this._previousHeight = canvas.ActualHeight;

            _line = new Line
            {
                X1 = _initialPosition.X,
                Y1 = _initialPosition.Y,
                X2 = _finalPosition.X,
                Y2 = _finalPosition.Y
            };

            _line.StrokeThickness = 1;
            _line.SnapsToDevicePixels = true;
            _line.Stroke = Brushes.Black;
            canvas.Children.Add(_line);
            CreateThumbs(_initialPosition, _finalPosition);
        }

        internal void ResizeFor(double width, double height)
        {
            var xScaling = width / _previousWidth;
            var yScaling = height / _previousHeight;
            _line.X1 = _line.X1 * xScaling;
            _line.X2 = _line.X2 * xScaling;
            _line.Y1 = _line.Y1 * yScaling;
            _line.Y2 = _line.Y2 * yScaling;

            _previousWidth = width;
            _previousHeight = height;

            _thumb1.Scale(xScaling, yScaling);
            _thumb2.Scale(xScaling, yScaling);
        }

        private void CreateThumbs(Point position1, Point position2)
        {
            _thumb1 = new Thumb(_userControl, _canvas, _grid, position1, (point) =>
            {
                _line.X1 = point.X;
                _line.Y1 = point.Y;
            });
            _thumb2 = new Thumb(_userControl, _canvas, _grid, position2, (point) =>
            {
                _line.X2 = point.X;
                _line.Y2 = point.Y;
            });
        }
    }
}
