using System;
using System.Collections.Generic;
using System.Threading;

namespace BoyJackEngine
{
    public class ConsoleRenderer
    {
        private int _width;
        private int _height;
        private char[,] _buffer;
        private ConsoleColor[,] _colorBuffer;

        public ConsoleRenderer(int width = 80, int height = 25)
        {
            _width = width;
            _height = height;
            _buffer = new char[_height, _width];
            _colorBuffer = new ConsoleColor[_height, _width];
            
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }

        public void Clear()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _buffer[y, x] = ' ';
                    _colorBuffer[y, x] = ConsoleColor.Black;
                }
            }
        }

        public void DrawText(string text, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            for (int i = 0; i < text.Length && x + i < _width; i++)
            {
                if (y >= 0 && y < _height && x + i >= 0)
                {
                    _buffer[y, x + i] = text[i];
                    _colorBuffer[y, x + i] = color;
                }
            }
        }

        public void DrawSprite(char sprite, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _buffer[y, x] = sprite;
                _colorBuffer[y, x] = color;
            }
        }

        public void DrawRectangle(int x, int y, int width, int height, char fill = '#', ConsoleColor color = ConsoleColor.White)
        {
            for (int dy = 0; dy < height; dy++)
            {
                for (int dx = 0; dx < width; dx++)
                {
                    int px = x + dx;
                    int py = y + dy;
                    if (px >= 0 && px < _width && py >= 0 && py < _height)
                    {
                        _buffer[py, px] = fill;
                        _colorBuffer[py, px] = color;
                    }
                }
            }
        }

        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Console.ForegroundColor = _colorBuffer[y, x];
                    Console.Write(_buffer[y, x]);
                }
                if (y < _height - 1) Console.WriteLine();
            }
            
            Console.ResetColor();
        }
    }
}