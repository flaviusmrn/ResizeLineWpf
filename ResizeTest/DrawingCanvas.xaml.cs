using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ResizeTest
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DrawingCanvas : UserControl
    {
        private bool _isDrawing;
        private Point _initialPosition;
        private Point _finalPosition;
        private HashSet<ResizeableLine> resizeableLines;

        public DrawingCanvas()
        {
            InitializeComponent();
            resizeableLines = new HashSet<ResizeableLine>();
            drawingCanvas.SizeChanged += DrawingCanvas_SizeChanged;
        }

        private void DrawingCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var line in resizeableLines)
            {
                var width = e.NewSize.Width;
                var height = e.NewSize.Height;
                line.ResizeFor(width, height);
            }
        }

        private void drawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawing &&
                Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _isDrawing = true;
                _initialPosition = e.GetPosition(this);
            }
        }

        private void drawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _finalPosition = e.GetPosition(this);
            }
        }

        private void drawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawing)
            {
                _isDrawing = false;
                resizeableLines.Add(new ResizeableLine(control, drawingCanvas, MainGrid, _initialPosition, _finalPosition));
            }
        }
    }
}
