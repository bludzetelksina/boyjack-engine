using System;
using System.Collections.Generic;

namespace BoyJackEngine.BJGSH
{
    public class SceneManager
    {
        private Dictionary<string, Scene> _scenes;
        private Scene _currentScene;
        private Scene _nextScene;

        public Scene CurrentScene => _currentScene;

        public SceneManager()
        {
            _scenes = new Dictionary<string, Scene>();
        }

        public void AddScene(string name, Scene scene)
        {
            _scenes[name] = scene;
        }

        public void RemoveScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                if (_currentScene == _scenes[name])
                {
                    _currentScene.OnExit();
                    _currentScene = null;
                }
                _scenes[name].Cleanup();
                _scenes.Remove(name);
            }
        }

        public void ChangeScene(string sceneName)
        {
            if (_scenes.ContainsKey(sceneName))
            {
                _nextScene = _scenes[sceneName];
            }
        }

        public void Update(float deltaTime)
        {
            // Handle scene transitions
            if (_nextScene != null)
            {
                _currentScene?.OnExit();
                _currentScene = _nextScene;
                _currentScene.OnEnter();
                _nextScene = null;
            }

            // Update current scene
            _currentScene?.Update(deltaTime);
        }

        public void Draw()
        {
            _currentScene?.Draw();
        }

        public void Initialize()
        {
            foreach (var scene in _scenes.Values)
            {
                scene.Initialize();
            }
        }

        public void Cleanup()
        {
            foreach (var scene in _scenes.Values)
            {
                scene.Cleanup();
            }
            _scenes.Clear();
            _currentScene = null;
            _nextScene = null;
        }
    }
}