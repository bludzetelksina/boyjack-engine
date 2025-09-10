using System;
using System.Collections.Generic;
using System.Threading;

namespace BoyJackEngine
{
    public class ConsoleInputManager
    {
        private HashSet<ConsoleKey> _keysDown;
        private HashSet<ConsoleKey> _keysPressed;
        private Thread _inputThread;
        private bool _running;

        public ConsoleInputManager()
        {
            _keysDown = new HashSet<ConsoleKey>();
            _keysPressed = new HashSet<ConsoleKey>();
            _running = true;
            
            _inputThread = new Thread(InputLoop);
            _inputThread.Start();
        }

        private void InputLoop()
        {
            while (_running)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    var key = keyInfo.Key;
                    
                    if (!_keysDown.Contains(key))
                    {
                        _keysPressed.Add(key);
                        _keysDown.Add(key);
                    }
                }
                Thread.Sleep(16); // ~60 FPS
            }
        }

        public void Update()
        {
            _keysPressed.Clear();
        }

        public bool IsKeyDown(ConsoleKey key) => _keysDown.Contains(key);
        public bool IsKeyPressed(ConsoleKey key) => _keysPressed.Contains(key);

        public bool IsKeyDown(string keyName)
        {
            var key = ParseKey(keyName);
            return key != ConsoleKey.NoName && IsKeyDown(key);
        }

        public bool IsKeyPressed(string keyName)
        {
            var key = ParseKey(keyName);
            return key != ConsoleKey.NoName && IsKeyPressed(key);
        }

        private ConsoleKey ParseKey(string keyName)
        {
            return keyName.ToUpper() switch
            {
                "LEFT" => ConsoleKey.LeftArrow,
                "RIGHT" => ConsoleKey.RightArrow,
                "UP" => ConsoleKey.UpArrow,
                "DOWN" => ConsoleKey.DownArrow,
                "SPACE" => ConsoleKey.Spacebar,
                "ENTER" => ConsoleKey.Enter,
                "ESCAPE" => ConsoleKey.Escape,
                "A" => ConsoleKey.A,
                "D" => ConsoleKey.D,
                "S" => ConsoleKey.S,
                "W" => ConsoleKey.W,
                _ => Enum.TryParse<ConsoleKey>(keyName, true, out ConsoleKey key) ? key : ConsoleKey.NoName
            };
        }

        public void Stop()
        {
            _running = false;
            _inputThread?.Join();
        }
    }
}