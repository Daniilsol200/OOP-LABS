using System;
using System.Drawing;
using System.Windows.Forms;
using CircleLibrary;

namespace lab5
{
    public class CircleForm : Form
    {
        private Circle circle;
        private const int SPEED = 5;

        public CircleForm()
        {
            Width = 800;
            Height = 600;
            DoubleBuffered = true;

            circle = new Circle(new Point(Width / 2, Height / 2), 20, Color.Red, ClientSize);
            circle.DirectionChanged += Circle_DirectionChanged;
            circle.EdgeReached += Circle_EdgeReached;

            KeyDown += CircleForm_KeyDown;
            Resize += CircleForm_Resize;
        }

        private void Circle_DirectionChanged(object sender, DirectionChangedEventArgs e)
        {
            MessageBox.Show($"Направление изменилось на: {e.Direction}");
        }

        private void Circle_EdgeReached(object sender, EdgeReachedEventArgs e)
        {
            MessageBox.Show($"Круг достиг края: {e.Edge}");
        }

        private void CircleForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    circle.Move(Direction.Up, SPEED);
                    break;
                case Keys.Down:
                    circle.Move(Direction.Down, SPEED);
                    break;
                case Keys.Left:
                    circle.Move(Direction.Left, SPEED);
                    break;
                case Keys.Right:
                    circle.Move(Direction.Right, SPEED);
                    break;
            }
            Invalidate();
        }

        private void CircleForm_Resize(object sender, EventArgs e)
        {
            circle.UpdateFormSize(ClientSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            circle.Draw(e.Graphics);
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new CircleForm());
        }
    }
}