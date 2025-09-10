using System;
using System.Collections.Generic;
using BoyJackEngine.BJS;

namespace BoyJackEngine
{
    public class ConsoleBJSBuiltins : BJSBuiltins
    {
        private ConsoleRenderer _renderer;
        private ConsoleInputManager _inputManager;

        public ConsoleBJSBuiltins(ConsoleRenderer renderer, ConsoleInputManager inputManager) 
            : base(null, null, null, null, null)
        {
            _renderer = renderer;
            _inputManager = inputManager;
        }

        public new bool HasFunction(string name)
        {
            return name switch
            {
                "draw_sprite" or "draw_text" or "draw_rectangle" or
                "key_pressed" or "key_down" or
                "set_window_title" => true,
                _ => false
            };
        }

        public new object CallFunction(string name, params object[] args)
        {
            return name switch
            {
                "draw_sprite" => DrawSprite(args),
                "draw_text" => DrawText(args),
                "draw_rectangle" => DrawRectangle(args),
                "key_pressed" => KeyPressed(args),
                "key_down" => KeyDown(args),
                "set_window_title" => SetWindowTitle(args),
                _ => null
            };
        }

        private object DrawSprite(object[] args)
        {
            if (args.Length >= 3)
            {
                string sprite = args[0].ToString();
                int x = Convert.ToInt32(args[1]);
                int y = Convert.ToInt32(args[2]);
                
                ConsoleColor color = ConsoleColor.White;
                if (args.Length >= 4)
                {
                    color = ParseColor(args[3].ToString());
                }
                
                char spriteChar = sprite.Length > 0 ? sprite[0] : '@';
                _renderer.DrawSprite(spriteChar, x, y, color);
            }
            return null;
        }

        private object DrawText(object[] args)
        {
            if (args.Length >= 3)
            {
                string text = args[0].ToString();
                int x = Convert.ToInt32(args[1]);
                int y = Convert.ToInt32(args[2]);
                
                ConsoleColor color = ConsoleColor.White;
                if (args.Length >= 4)
                {
                    color = ParseColor(args[3].ToString());
                }
                
                _renderer.DrawText(text, x, y, color);
            }
            return null;
        }

        private object DrawRectangle(object[] args)
        {
            if (args.Length >= 4)
            {
                int x = Convert.ToInt32(args[0]);
                int y = Convert.ToInt32(args[1]);
                int width = Convert.ToInt32(args[2]);
                int height = Convert.ToInt32(args[3]);
                
                ConsoleColor color = ConsoleColor.White;
                char fill = '#';
                
                if (args.Length >= 5)
                {
                    color = ParseColor(args[4].ToString());
                }
                
                _renderer.DrawRectangle(x, y, width, height, fill, color);
            }
            return null;
        }

        private object KeyPressed(object[] args)
        {
            if (args.Length >= 1)
            {
                string key = args[0].ToString();
                return _inputManager.IsKeyPressed(key);
            }
            return false;
        }

        private object KeyDown(object[] args)
        {
            if (args.Length >= 1)
            {
                string key = args[0].ToString();
                return _inputManager.IsKeyDown(key);
            }
            return false;
        }

        private object SetWindowTitle(object[] args)
        {
            if (args.Length >= 1)
            {
                string title = args[0].ToString();
                Console.Title = title;
                return true;
            }
            return false;
        }

        private ConsoleColor ParseColor(string colorName)
        {
            return colorName.ToLower() switch
            {
                "white" => ConsoleColor.White,
                "black" => ConsoleColor.Black,
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                "blue" => ConsoleColor.Blue,
                "yellow" => ConsoleColor.Yellow,
                "cyan" => ConsoleColor.Cyan,
                "magenta" => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };
        }
    }
}