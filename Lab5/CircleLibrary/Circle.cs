using System;
using System.Collections.Generic;
using System.Drawing;

namespace CircleLibrary
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public class DirectionChangedEventArgs : EventArgs
    {
        public Direction Direction { get; }
        public DirectionChangedEventArgs(Direction direction)
        {
            Direction = direction;
        }
    }

    public class EdgeReachedEventArgs : EventArgs
    {
        public string Edge { get; }
        public EdgeReachedEventArgs(string edge)
        {
            Edge = edge;
        }
    }

    public delegate void DirectionChangedHandler(object sender, DirectionChangedEventArgs e);
    public delegate void EdgeReachedHandler(object sender, EdgeReachedEventArgs e);

    public class Circle
    {
        public event DirectionChangedHandler DirectionChanged;
        public event EdgeReachedHandler EdgeReached;

        private Point position;
        private readonly int radius;
        private Color color;
        private Direction currentDirection;
        private Size formSize;

        public Circle(Point startPosition, int radius, Color color, Size formSize)
        {
            position = startPosition;
            this.radius = radius;
            this.color = color;
            currentDirection = Direction.None;
            this.formSize = formSize;
        }

        public void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, position.X - radius, position.Y - radius, radius * 2, radius * 2);
            }
        }

        public void Move(Direction newDirection, int speed)
        {
            if (newDirection != currentDirection && newDirection != Direction.None)
            {
                currentDirection = newDirection;
                DirectionChanged?.Invoke(this, new DirectionChangedEventArgs(newDirection));
            }

            Point newPosition = position;

            switch (newDirection)
            {
                case Direction.Up:
                    newPosition.Y -= speed;
                    break;
                case Direction.Down:
                    newPosition.Y += speed;
                    break;
                case Direction.Left:
                    newPosition.X -= speed;
                    break;
                case Direction.Right:
                    newPosition.X += speed;
                    break;
            }

            bool edgeHit = false;
            var edges = new List<string>();

            if (newPosition.X - radius <= 0)
            {
                edgeHit = true;
                edges.Add("Left");
                newPosition.X = radius;
            }
            else if (newPosition.X + radius >= formSize.Width)
            {
                edgeHit = true;
                edges.Add("Right");
                newPosition.X = formSize.Width - radius;
            }

            if (newPosition.Y - radius <= 0)
            {
                edgeHit = true;
                edges.Add("Top");
                newPosition.Y = radius;
            }
            else if (newPosition.Y + radius >= formSize.Height)
            {
                edgeHit = true;
                edges.Add("Bottom");
                newPosition.Y = formSize.Height - radius;
            }

            position = newPosition;

            if (edgeHit)
            {
                EdgeReached?.Invoke(this, new EdgeReachedEventArgs(string.Join(", ", edges)));
            }
        }

        public Point Position
        {
            get => position;
            set => position = value;
        }

        public int Radius => radius;
        public Color Color => color;
        public Direction CurrentDirection => currentDirection;
        public Size FormSize => formSize;

        public void UpdateFormSize(Size newSize)
        {
            if (newSize.Width <= 0 || newSize.Height <= 0) throw new ArgumentException("Размер должен быть положительным", nameof(newSize));
            formSize = newSize;
        }
    }
}