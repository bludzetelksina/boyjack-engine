using System.Drawing;

namespace BoyJackEngine.BJG
{
    public class Sprite
    {
        public string Name { get; set; }
        public Image Image { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float ScaleX { get; set; } = 1.0f;
        public float ScaleY { get; set; } = 1.0f;
        public float Rotation { get; set; } = 0.0f;
        public float Alpha { get; set; } = 1.0f;
        public bool Visible { get; set; } = true;
        public int Layer { get; set; } = 0;

        public Sprite(string name, Image image)
        {
            Name = name;
            Image = image;
        }

        public RectangleF GetBounds()
        {
            float width = Image.Width * ScaleX;
            float height = Image.Height * ScaleY;
            return new RectangleF(X, Y, width, height);
        }

        public bool CheckCollision(Sprite other)
        {
            return GetBounds().IntersectsWith(other.GetBounds());
        }
    }
}