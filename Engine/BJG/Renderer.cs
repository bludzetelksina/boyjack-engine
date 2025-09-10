using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace BoyJackEngine.BJG
{
    public class Renderer
    {
        private Control _renderTarget;
        private List<Sprite> _sprites;
        private Graphics _graphics;
        private Bitmap _backBuffer;

        public Renderer(Control renderTarget)
        {
            _renderTarget = renderTarget;
            _sprites = new List<Sprite>();
            
            // Create back buffer for double buffering
            _backBuffer = new Bitmap(_renderTarget.Width, _renderTarget.Height);
            _graphics = Graphics.FromImage(_backBuffer);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        public void AddSprite(Sprite sprite)
        {
            _sprites.Add(sprite);
        }

        public void RemoveSprite(Sprite sprite)
        {
            _sprites.Remove(sprite);
        }

        public void ClearSprites()
        {
            _sprites.Clear();
        }

        public void Render()
        {
            // Clear the back buffer
            _graphics.Clear(Color.Black);

            // Sort sprites by layer for proper drawing order
            var sortedSprites = _sprites.Where(s => s.Visible).OrderBy(s => s.Layer);

            foreach (var sprite in sortedSprites)
            {
                DrawSprite(sprite);
            }

            // Copy back buffer to front buffer
            using (Graphics frontBuffer = _renderTarget.CreateGraphics())
            {
                frontBuffer.DrawImage(_backBuffer, 0, 0);
            }
        }

        private void DrawSprite(Sprite sprite)
        {
            if (sprite.Image == null) return;

            // Save the current graphics state
            GraphicsState state = _graphics.Save();

            // Apply transformations
            _graphics.TranslateTransform(sprite.X + (sprite.Image.Width * sprite.ScaleX) / 2, 
                                       sprite.Y + (sprite.Image.Height * sprite.ScaleY) / 2);
            _graphics.RotateTransform(sprite.Rotation);
            _graphics.ScaleTransform(sprite.ScaleX, sprite.ScaleY);

            // Apply alpha blending if needed
            if (sprite.Alpha < 1.0f)
            {
                ColorMatrix colorMatrix = new ColorMatrix();
                colorMatrix.Matrix33 = sprite.Alpha;
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);

                Rectangle destRect = new Rectangle(
                    -(int)(sprite.Image.Width / 2),
                    -(int)(sprite.Image.Height / 2),
                    sprite.Image.Width,
                    sprite.Image.Height
                );

                _graphics.DrawImage(sprite.Image, destRect, 0, 0, sprite.Image.Width, sprite.Image.Height, GraphicsUnit.Pixel, imageAttributes);
            }
            else
            {
                _graphics.DrawImage(sprite.Image, 
                    -(int)(sprite.Image.Width / 2), 
                    -(int)(sprite.Image.Height / 2));
            }

            // Restore the graphics state
            _graphics.Restore(state);
        }

        public void DrawText(string text, float x, float y, Color color, Font? font = null)
        {
            if (font == null)
                font = SystemFonts.DefaultFont;

            using (Brush brush = new SolidBrush(color))
            {
                _graphics.DrawString(text, font, brush, x, y);
            }
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color, bool filled = false)
        {
            Rectangle rect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            
            if (filled)
            {
                using (Brush brush = new SolidBrush(color))
                {
                    _graphics.FillRectangle(brush, rect);
                }
            }
            else
            {
                using (Pen pen = new Pen(color))
                {
                    _graphics.DrawRectangle(pen, rect);
                }
            }
        }

        public void Resize(int width, int height)
        {
            _backBuffer?.Dispose();
            _graphics?.Dispose();
            
            _backBuffer = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_backBuffer);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        public void Dispose()
        {
            _backBuffer?.Dispose();
            _graphics?.Dispose();
        }
    }
}