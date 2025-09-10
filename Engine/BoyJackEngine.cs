using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BoyJackEngine.BJG;
using BoyJackEngine.BJGSH;
using BoyJackEngine.BJS;

namespace BoyJackEngine
{
    public class BoyJackEngine
    {
        private WindowFormHost _window;
        private GameLoop _gameLoop;
        private TextureManager _textureManager;
        private Renderer _renderer;
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private BJSExecutor _scriptExecutor;
        private BJSBuiltins _builtins;
        private BJSScript _mainScript;
        private bool _isPaused;

        public bool IsRunning => _gameLoop?.IsRunning ?? false;
        public bool IsPaused => _isPaused;

        public BoyJackEngine()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public void Initialize(string title = "BoyJack Engine", int width = 800, int height = 600)
        {
            // Create main window
            _window = new WindowFormHost(title, width, height);
            
            // Connect pause/resume functionality
            _window.OnTogglePauseRequested += TogglePause;
            
            // Get components from window
            _renderer = _window.Renderer;
            _inputManager = _window.InputManager;
            
            // Initialize other systems
            _textureManager = new TextureManager();
            _sceneManager = new SceneManager();
            
            // Initialize scripting system
            _builtins = new BJSBuiltins(_textureManager, _renderer, _inputManager, _sceneManager, _window);
            _scriptExecutor = new BJSExecutor(_builtins);
            
            // Initialize game loop
            _gameLoop = new GameLoop(60.0f);
            _gameLoop.UpdateEvent += OnUpdate;
            _gameLoop.DrawEvent += OnDraw;
            
            // Initialize scene manager
            _sceneManager.Initialize();
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
                }
                else
                {
                    Console.WriteLine($"Script file not found: {scriptPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading script: {ex.Message}");
            }
        }

        public void Run()
        {
            if (_mainScript != null)
            {
                _scriptExecutor.ExecuteEvent("start");
            }
            
            _gameLoop.Start();
            Application.Run(_window);
        }

        public void Stop()
        {
            _gameLoop?.Stop();
            _window?.Close();
        }

        public void Pause()
        {
            _isPaused = true;
            _window.UpdateGameState(_isPaused);
        }

        public void Resume()
        {
            _isPaused = false;
            _window.UpdateGameState(_isPaused);
        }

        public void TogglePause()
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }

        private void OnUpdate(float deltaTime)
        {
            _inputManager.Update();
            
            // Check for pause key (F1)
            if (_inputManager.IsKeyPressed(System.Windows.Forms.Keys.F1))
            {
                TogglePause();
            }
            
            // Only update game logic if not paused
            if (!_isPaused)
            {
                _sceneManager.Update(deltaTime);
                
                if (_mainScript != null)
                {
                    _scriptExecutor.ExecuteEvent("update");
                }
            }
        }

        private void OnDraw()
        {
            _renderer.ClearSprites();
            
            _sceneManager.Draw();
            
            if (_mainScript != null)
            {
                _scriptExecutor.ExecuteEvent("draw");
            }
            
            // Show pause overlay if paused
            if (_isPaused)
            {
                _renderer.DrawRectangle(0, 0, _window.Width, _window.Height, Color.FromArgb(128, 0, 0, 0), true);
                _renderer.DrawText("GAME PAUSED", _window.Width / 2 - 50, _window.Height / 2 - 10, Color.White, new Font("Arial", 16, FontStyle.Bold));
                _renderer.DrawText("Press F1 to Resume", _window.Width / 2 - 70, _window.Height / 2 + 20, Color.Yellow);
            }
            
            _renderer.Render();
        }

        public void Dispose()
        {
            _gameLoop?.Dispose();
            _textureManager?.UnloadAll();
            _renderer?.Dispose();
            _window?.Dispose();
        }
    }
}