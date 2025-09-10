using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BoyJackEngine.BJGSH
{
    public class GameLoop
    {
        private System.Windows.Forms.Timer _gameTimer;
        private Stopwatch _stopwatch;
        private float _targetFPS;
        private float _deltaTime;
        private long _lastFrameTime;

        public float DeltaTime => _deltaTime;
        public float FPS => 1.0f / _deltaTime;
        public bool IsRunning { get; private set; }

        public event Action<float> UpdateEvent;
        public event Action DrawEvent;

        public GameLoop(float targetFPS = 60.0f)
        {
            _targetFPS = targetFPS;
            _stopwatch = new Stopwatch();
            
            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = (int)(1000.0f / _targetFPS);
            _gameTimer.Tick += OnGameTick;
        }

        private void OnGameTick(object sender, EventArgs e)
        {
            if (!IsRunning) return;

            long currentTime = _stopwatch.ElapsedMilliseconds;
            _deltaTime = (currentTime - _lastFrameTime) / 1000.0f;
            _lastFrameTime = currentTime;

            // Cap delta time to prevent spiral of death
            if (_deltaTime > 1.0f / 30.0f)
                _deltaTime = 1.0f / 30.0f;

            UpdateEvent?.Invoke(_deltaTime);
            DrawEvent?.Invoke();
        }

        public void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            _stopwatch.Start();
            _lastFrameTime = 0;
            _gameTimer.Start();
        }

        public void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;
            _gameTimer.Stop();
            _stopwatch.Stop();
        }

        public void SetTargetFPS(float fps)
        {
            _targetFPS = fps;
            _gameTimer.Interval = (int)(1000.0f / _targetFPS);
        }

        public void Dispose()
        {
            Stop();
            _gameTimer?.Dispose();
        }
    }
}