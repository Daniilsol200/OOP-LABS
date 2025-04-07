using System;
using System.Drawing;
using System.Windows.Forms;
using static Circle;

// Перечисление для направлений движения
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

// Класс делегата для события смены направления
public delegate void DirectionChangedEventHandler(object sender, DirectionChangedEventArgs e);

// Класс аргументов события
public class DirectionChangedEventArgs : EventArgs
{
    public Direction NewDirection { get; private set; }
    public Direction OldDirection { get; private set; }

    public DirectionChangedEventArgs(Direction newDirection, Direction oldDirection)
    {
        NewDirection = newDirection;
        OldDirection = oldDirection;
    }
}

// Основной класс Круг
public class Circle
{
    // Событие смены направления
    public event DirectionChangedEventHandler DirectionChanged;

    // Свойства круга
    private int x;
    private int y;
    private int radius;
    private Color color;
    private Direction currentDirection;
    private int speed = 5;

    public Circle(int x, int y, int radius, Color color)
    {
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.color = color;
        this.currentDirection = Direction.None;
    }

    // Метод отрисовки круга
    public void Draw(Graphics g)
    {
        using (SolidBrush brush = new SolidBrush(color))
        {
            g.FillEllipse(brush, x - radius, y - radius, radius * 2, radius * 2);
        }
    }

    // Обработка нажатия клавиш
    public void Move(KeyEventArgs e)
    {
        Direction oldDirection = currentDirection;
        Direction newDirection = currentDirection;

        switch (e.KeyCode)
        {
            case Keys.Up:
                newDirection = Direction.Up;
                y -= speed;
                break;
            case Keys.Down:
                newDirection = Direction.Down;
                y += speed;
                break;
            case Keys.Left:
                newDirection = Direction.Left;
                x -= speed;
                break;
            case Keys.Right:
                newDirection = Direction.Right;
                x += speed;
                break;
        }

        // Если направление изменилось, вызываем событие
        if (newDirection != oldDirection)
        {
            currentDirection = newDirection;
            OnDirectionChanged(oldDirection, newDirection);
        }
    }

    // Метод вызова события
    protected virtual void OnDirectionChanged(Direction oldDirection, Direction newDirection)
    {
        DirectionChanged?.Invoke(this, new DirectionChangedEventArgs(newDirection, oldDirection));
    }

    // Пример формы для тестирования
    public class CircleForm : Form
    {
        private Circle circle;

        public CircleForm()
        {
            this.Width = 400;
            this.Height = 400;
            this.DoubleBuffered = true;

            circle = new Circle(200, 200, 20, Color.Red);
            circle.DirectionChanged += Circle_DirectionChanged;
        }

        private void Circle_DirectionChanged(object sender, DirectionChangedEventArgs e)
        {
            Console.WriteLine($"Направление изменилось с {e.OldDirection} на {e.NewDirection}");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            circle.Draw(e.Graphics);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            circle.Move(e);
            Invalidate(); // Перерисовка формы
        }
    }
}

// Точка входа программы
class Program
{
    static void Main()
    {
        Application.Run(new CircleForm());
    }
}