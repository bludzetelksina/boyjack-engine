using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BoyJackEngine.BJGSH
{
    public class InputManager
    {
        private HashSet<Keys> _keysDown;
        private HashSet<Keys> _keysPressed;
        private HashSet<Keys> _keysReleased;
        private Point _mousePosition;
        private HashSet<MouseButtons> _mouseButtonsDown;
        private HashSet<MouseButtons> _mouseButtonsPressed;
        private HashSet<MouseButtons> _mouseButtonsReleased;

        public Point MousePosition => _mousePosition;

        public InputManager()
        {
            _keysDown = new HashSet<Keys>();
            _keysPressed = new HashSet<Keys>();
            _keysReleased = new HashSet<Keys>();
            _mouseButtonsDown = new HashSet<MouseButtons>();
            _mouseButtonsPressed = new HashSet<MouseButtons>();
            _mouseButtonsReleased = new HashSet<MouseButtons>();
        }

        public void Update()
        {
            // Clear pressed and released states for next frame
            _keysPressed.Clear();
            _keysReleased.Clear();
            _mouseButtonsPressed.Clear();
            _mouseButtonsReleased.Clear();
        }

        public void OnKeyDown(Keys key)
        {
            if (!_keysDown.Contains(key))
            {
                _keysPressed.Add(key);
            }
            _keysDown.Add(key);
        }

        public void OnKeyUp(Keys key)
        {
            _keysDown.Remove(key);
            _keysReleased.Add(key);
        }

        public void OnMouseDown(MouseButtons button, Point position)
        {
            _mousePosition = position;
            if (!_mouseButtonsDown.Contains(button))
            {
                _mouseButtonsPressed.Add(button);
            }
            _mouseButtonsDown.Add(button);
        }

        public void OnMouseUp(MouseButtons button, Point position)
        {
            _mousePosition = position;
            _mouseButtonsDown.Remove(button);
            _mouseButtonsReleased.Add(button);
        }

        public void OnMouseMove(Point position)
        {
            _mousePosition = position;
        }

        // Key state queries
        public bool IsKeyDown(Keys key) => _keysDown.Contains(key);
        public bool IsKeyPressed(Keys key) => _keysPressed.Contains(key);
        public bool IsKeyReleased(Keys key) => _keysReleased.Contains(key);

        // String-based key queries for script system
        public bool IsKeyDown(string keyName) => IsKeyDown(ParseKey(keyName));
        public bool IsKeyPressed(string keyName) => IsKeyPressed(ParseKey(keyName));
        public bool IsKeyReleased(string keyName) => IsKeyReleased(ParseKey(keyName));

        // Mouse state queries
        public bool IsMouseButtonDown(MouseButtons button) => _mouseButtonsDown.Contains(button);
        public bool IsMouseButtonPressed(MouseButtons button) => _mouseButtonsPressed.Contains(button);
        public bool IsMouseButtonReleased(MouseButtons button) => _mouseButtonsReleased.Contains(button);

        private Keys ParseKey(string keyName)
        {
            return keyName.ToUpper() switch
            {
                "LEFT" => Keys.Left,
                "RIGHT" => Keys.Right,
                "UP" => Keys.Up,
                "DOWN" => Keys.Down,
                "SPACE" => Keys.Space,
                "ENTER" => Keys.Enter,
                "ESCAPE" => Keys.Escape,
                "A" => Keys.A,
                "D" => Keys.D,
                "S" => Keys.S,
                "W" => Keys.W,
                _ => Enum.TryParse<Keys>(keyName, true, out Keys key) ? key : Keys.None
            };
        }
    }
}