using System.Drawing;
using CircleLibrary;
using NUnit.Framework;

namespace CircleLibrary.Tests
{
    [TestFixture]
    public class CircleTests
    {
        private Circle circle;
        private bool directionChangedCalled;
        private Direction lastDirection;
        private bool edgeReachedCalled;
        private string lastEdge;

        [SetUp]
        public void SetUp()
        {
            circle = new Circle(new Point(100, 100), 20, Color.Red, new Size(800, 600));
            directionChangedCalled = false;
            edgeReachedCalled = false;
            lastDirection = Direction.None;
            lastEdge = null;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            circle.DirectionChanged += (sender, e) =>
            {
                directionChangedCalled = true;
                lastDirection = e.Direction;
            };
            circle.EdgeReached += (sender, e) =>
            {
                edgeReachedCalled = true;
                lastEdge = e.Edge;
            };
        }

        [Test]
        public void Constructor_InitializesCorrectly()
        {
            Point expectedPosition = new Point(100, 100);
            int expectedRadius = 20;
            Color expectedColor = Color.Red;
            Size expectedFormSize = new Size(800, 600);

            Assert.AreEqual(expectedPosition, circle.Position);
            Assert.AreEqual(expectedRadius, circle.GetType().GetField("radius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(circle));
            Assert.AreEqual(expectedColor, circle.GetType().GetField("color", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(circle));
            Assert.AreEqual(expectedFormSize, circle.GetType().GetField("formSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(circle));
            Assert.AreEqual(Direction.None, circle.GetType().GetField("currentDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(circle));
        }

        [Test]
        public void Move_Up_UpdatesPositionCorrectly()
        {
            Point expectedPosition = new Point(100, 95);

            circle.Move(Direction.Up, 5);

            Assert.AreEqual(expectedPosition, circle.Position);
            Assert.IsTrue(directionChangedCalled, "DirectionChanged should be triggered");
            Assert.AreEqual(Direction.Up, lastDirection);
            Assert.IsFalse(edgeReachedCalled, "EdgeReached should not be triggered");
        }

        [Test]
        public void Move_Down_UpdatesPositionCorrectly()
        {
            Point expectedPosition = new Point(100, 105);

            circle.Move(Direction.Down, 5);

            Assert.AreEqual(expectedPosition, circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.AreEqual(Direction.Down, lastDirection);
            Assert.IsFalse(edgeReachedCalled);
        }

        [Test]
        public void Move_Left_UpdatesPositionCorrectly()
        {
            Point expectedPosition = new Point(95, 100);

            circle.Move(Direction.Left, 5);

            Assert.AreEqual(expectedPosition, circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.AreEqual(Direction.Left, lastDirection);
            Assert.IsFalse(edgeReachedCalled);
        }

        [Test]
        public void Move_Right_UpdatesPositionCorrectly()
        {
            Point expectedPosition = new Point(105, 100);

            circle.Move(Direction.Right, 5);

            Assert.AreEqual(expectedPosition, circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.AreEqual(Direction.Right, lastDirection);
            Assert.IsFalse(edgeReachedCalled);
        }

        [Test]
        public void Move_SameDirection_DoesNotTriggerDirectionChanged()
        {
            circle.Move(Direction.Up, 5);
            directionChangedCalled = false;

            circle.Move(Direction.Up, 5);

            Assert.IsFalse(directionChangedCalled, "DirectionChanged should not be triggered for same direction");
            Assert.AreEqual(new Point(100, 90), circle.Position);
        }

        [Test]
        public void Move_ToLeftEdge_TriggersEdgeReached()
        {
            circle = new Circle(new Point(25, 100), 20, Color.Red, new Size(800, 600));
            SubscribeToEvents();

            circle.Move(Direction.Left, 5);

            Assert.AreEqual(new Point(20, 100), circle.Position, "Position should be adjusted to left edge");
            Assert.IsTrue(directionChangedCalled, "DirectionChanged should be triggered");
            Assert.IsTrue(edgeReachedCalled, "EdgeReached should be triggered");
            Assert.That(lastEdge, Does.Contain("Left"), "Edge should include Left");
        }

        [Test]
        public void Move_ToRightEdge_TriggersEdgeReached()
        {
            circle = new Circle(new Point(775, 100), 20, Color.Red, new Size(800, 600));
            SubscribeToEvents();

            circle.Move(Direction.Right, 5);

            Assert.AreEqual(new Point(780, 100), circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.IsTrue(edgeReachedCalled);
            Assert.That(lastEdge, Does.Contain("Right"));
        }

        [Test]
        public void Move_ToTopEdge_TriggersEdgeReached()
        {
            circle = new Circle(new Point(100, 25), 20, Color.Red, new Size(800, 600));
            SubscribeToEvents();

            circle.Move(Direction.Up, 5);

            Assert.AreEqual(new Point(100, 20), circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.IsTrue(edgeReachedCalled);
            Assert.That(lastEdge, Does.Contain("Top"));
        }

        [Test]
        public void Move_ToBottomEdge_TriggersEdgeReached()
        {
            circle = new Circle(new Point(100, 575), 20, Color.Red, new Size(800, 600));
            SubscribeToEvents();

            circle.Move(Direction.Down, 5);

            Assert.AreEqual(new Point(100, 580), circle.Position);
            Assert.IsTrue(directionChangedCalled);
            Assert.IsTrue(edgeReachedCalled);
            Assert.That(lastEdge, Does.Contain("Bottom"));
        }

        [Test]
        public void UpdateFormSize_UpdatesFormSizeCorrectly()
        {
            Size newSize = new Size(1000, 800);

            circle.UpdateFormSize(newSize);

            Assert.AreEqual(newSize, circle.GetType().GetField("formSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(circle));
        }
    }
}