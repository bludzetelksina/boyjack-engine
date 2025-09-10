using System;
using System.Drawing;
using System.Windows.Forms;
using BoyJackEngine.BJG;

namespace BoyJackEngine.BJGSH
{
    public class WindowFormHost : Form
    {
        private Panel _gamePanel;
        private Renderer _renderer;
        private InputManager _inputManager;
        private ToolStripStatusLabel _statusLabel;
        private StatusStrip _statusStrip;

        public Panel GamePanel => _gamePanel;
        public Renderer Renderer => _renderer;
        public InputManager InputManager => _inputManager;

        public WindowFormHost(string title, int width, int height)
        {
            InitializeWindow(title, width, height);
            InitializeComponents();
            SetupEventHandlers();
        }

        private void InitializeWindow(string title, int width, int height)
        {
            Text = title;
            Size = new Size(width, height);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            KeyPreview = true;
            
            // Set window icon
            try
            {
                Icon = new Icon("Assets/icons/boyjack_engine_32.ico");
            }
            catch
            {
                // Icon loading failed, continue without icon
            }
            
            // Set double buffering for smoother rendering
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                    ControlStyles.UserPaint | 
                    ControlStyles.DoubleBuffer, true);
        }

        private void InitializeComponents()
        {
            // Create status bar
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Ready");
            _statusStrip.Items.Add(_statusLabel);
            
            _gamePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };
            
            Controls.Add(_gamePanel);
            Controls.Add(_statusStrip);
            
            _renderer = new Renderer(_gamePanel);
            _inputManager = new InputManager();
        }

        private void SetupEventHandlers()
        {
            // Keyboard events
            KeyDown += (s, e) => _inputManager.OnKeyDown(e.KeyCode);
            KeyUp += (s, e) => _inputManager.OnKeyUp(e.KeyCode);

            // Mouse events for the game panel
            _gamePanel.MouseDown += (s, e) => _inputManager.OnMouseDown(e.Button, e.Location);
            _gamePanel.MouseUp += (s, e) => _inputManager.OnMouseUp(e.Button, e.Location);
            _gamePanel.MouseMove += (s, e) => _inputManager.OnMouseMove(e.Location);

            // Window resize
            Resize += OnWindowResize;
            
            // Prevent the panel from stealing focus
            _gamePanel.TabStop = false;

            // Add menu bar
            SetupMenuBar();
        }

        private void SetupMenuBar()
        {
            var menuStrip = new MenuStrip();
            
            // File menu
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => Close());
            
            // Game menu
            var gameMenu = new ToolStripMenuItem("Game");
            gameMenu.DropDownItems.Add("Pause/Resume (F1)", null, (s, e) => TogglePauseResume());
            
            // Help menu
            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("Controls", null, (s, e) => ShowControls());
            helpMenu.DropDownItems.Add("About BoyJack Engine", null, (s, e) => ShowAbout());
            
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, gameMenu, helpMenu });
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void TogglePauseResume()
        {
            // This will be called by the main engine
            OnTogglePauseRequested?.Invoke();
        }

        private void ShowControls()
        {
            MessageBox.Show("Game Controls:\n\nF1 - Pause/Resume Game\nArrow Keys - Move\nSpace - Action\nEsc - Exit", 
                           "BoyJack Engine Controls", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public event Action OnTogglePauseRequested;

        public void UpdateGameState(bool isPaused)
        {
            if (_statusLabel != null)
            {
                _statusLabel.Text = isPaused ? "Game Paused - Press F1 to Resume" : "Game Running - Press F1 to Pause";
                _statusLabel.ForeColor = isPaused ? Color.Orange : Color.Green;
            }
            
            // Update window title to show pause state
            string baseTitle = Text.Replace(" - PAUSED", "");
            Text = isPaused ? baseTitle + " - PAUSED" : baseTitle;
        }

        private void ShowAbout()
        {
            using (var aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.Resize(_gamePanel.Width, _gamePanel.Height);
            }
        }

        public void SetFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _renderer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}