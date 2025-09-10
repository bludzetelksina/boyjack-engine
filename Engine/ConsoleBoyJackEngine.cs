using System;
using System.IO;
using System.Threading;
using BoyJackEngine.BJS;

namespace BoyJackEngine
{
    public class ConsoleBoyJackEngine
    {
        private ConsoleRenderer _renderer;
        private ConsoleInputManager _inputManager;
        private BJSExecutor _scriptExecutor;
        private ConsoleBJSBuiltins _builtins;
        private BJSScript _mainScript;
        private bool _running;
        private Thread _gameThread;

        public void Initialize(string title = "BoyJack Engine Console", int width = 80, int height = 25)
        {
            Console.Title = title;
            _renderer = new ConsoleRenderer(width, height);
            _inputManager = new ConsoleInputManager();
            
            _builtins = new ConsoleBJSBuiltins(_renderer, _inputManager);
            _scriptExecutor = new BJSExecutor(_builtins);
            
            Console.WriteLine("BoyJack Engine Console Version Initialized!");
            Console.WriteLine("Loading...");
            Thread.Sleep(1000);
        }

        public void LoadScript(string scriptPath)
        {
            try
            {
                if (File.Exists(scriptPath))
                {
                    string scriptContent = File.ReadAllText(scriptPath);
                    var parser = new BJSParser();
                    _mainScript = parser.Parse(scriptContent);
                    _scriptExecutor.ExecuteScript(_mainScript);
                    Console.WriteLine($"Script loaded: {scriptPath}");
                }
                else
                {
                    Console.WriteLine($"Script file not found: {scriptPath}");
                    // Create a simple default script
                    CreateDefaultScript();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading script: {ex.Message}");
                CreateDefaultScript();
            }
        }

        private void CreateDefaultScript()
        {
            string defaultScript = @"
// Default BoyJack Engine Demo
scene = ""Game""
player.x = 10
player.y = 10

on start:
    set_window_title(""BoyJack Console Demo"")

on update:
    if key_pressed(""RIGHT""):
        player.x += 1
    if key_pressed(""LEFT""):
        player.x -= 1
    if key_pressed(""DOWN""):
        player.y += 1
    if key_pressed(""UP""):
        player.y -= 1

on draw:
    draw_text(""BoyJack Engine Console Demo"", 1, 1, ""white"")
    draw_text(""Use ARROW KEYS to move the [@] character"", 1, 2, ""white"")
    draw_text(""Press ESC to quit"", 1, 3, ""white"")
    draw_sprite(""@"", player.x, player.y, ""yellow"")
    draw_text(""Position: "" + player.x + "","" + player.y, 1, 5, ""green"")
";

            var parser = new BJSParser();
            _mainScript = parser.Parse(defaultScript);
            _scriptExecutor.ExecuteScript(_mainScript);
        }

        public void Run()
        {
            _running = true;
            _gameThread = new Thread(GameLoop);
            _gameThread.Start();
            
            if (_mainScript != null)
            {
                _scriptExecutor.ExecuteEvent("start");
            }
            
            _gameThread.Join();
        }

        private void GameLoop()
        {
            while (_running)
            {
                _inputManager.Update();
                
                // Check for ESC to quit
                if (_inputManager.IsKeyPressed("ESCAPE"))
                {
                    _running = false;
                    break;
                }
                
                if (_mainScript != null)
                {
                    _scriptExecutor.ExecuteEvent("update");
                }
                
                _renderer.Clear();
                
                if (_mainScript != null)
                {
                    _scriptExecutor.ExecuteEvent("draw");
                }
                
                _renderer.Render();
                
                Thread.Sleep(16); // ~60 FPS
            }
        }

        public void Stop()
        {
            _running = false;
            _inputManager?.Stop();
        }
    }
}