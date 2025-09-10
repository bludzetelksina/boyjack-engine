using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using BoyJackEngine.BJG;
using BoyJackEngine.BJGSH;

namespace BoyJackEngine.BJS
{
    public class BJSBuiltins
    {
        private TextureManager _textureManager;
        private Renderer _renderer;
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private WindowFormHost _windowHost;
        private Dictionary<string, SoundPlayer> _sounds;

        public BJSBuiltins(TextureManager textureManager, Renderer renderer, 
                          InputManager inputManager, SceneManager sceneManager,
                          WindowFormHost windowHost)
        {
            _textureManager = textureManager;
            _renderer = renderer;
            _inputManager = inputManager;
            _sceneManager = sceneManager;
            _windowHost = windowHost;
            _sounds = new Dictionary<string, SoundPlayer>();
        }

        public bool HasFunction(string name)
        {
            return name switch
            {
                "load_image" or "draw_sprite" or "draw_text" or "draw_rectangle" or
                "key_pressed" or "key_down" or "key_released" or
                "play_sound" or "load_sound" or
                "set_window_title" or "set_resolution" or
                "change_scene" => true,
                _ => false
            };
        }

        public object CallFunction(string name, params object[] args)
        {
            return name switch
            {
                "load_image" => LoadImage(args),
                "draw_sprite" => DrawSprite(args),
                "draw_text" => DrawText(args),
                "draw_rectangle" => DrawRectangle(args),
                "key_pressed" => KeyPressed(args),
                "key_down" => KeyDown(args),
                "key_released" => KeyReleased(args),
                "play_sound" => PlaySound(args),
                "load_sound" => LoadSound(args),
                "set_window_title" => SetWindowTitle(args),
                "set_resolution" => SetResolution(args),
                "change_scene" => ChangeScene(args),
                _ => null
            };
        }

        private object LoadImage(object[] args)
        {
            if (args.Length >= 2)
            {
                string name = args[0].ToString();
                string path = args[1].ToString();
                return _textureManager.LoadTexture(name, path);
            }
            return false;
        }

        private object DrawSprite(object[] args)
        {
            if (args.Length >= 3)
            {
                string name = args[0].ToString();
                float x = Convert.ToSingle(args[1]);
                float y = Convert.ToSingle(args[2]);
                
                var texture = _textureManager.GetTexture(name);
                if (texture != null)
                {
                    var sprite = new Sprite(name, texture)
                    {
                        X = x,
                        Y = y
                    };
                    _renderer.AddSprite(sprite);
                }
            }
            return null;
        }

        private object DrawText(object[] args)
        {
            if (args.Length >= 3)
            {
                string text = args[0].ToString();
                float x = Convert.ToSingle(args[1]);
                float y = Convert.ToSingle(args[2]);
                
                Color color = Color.White;
                if (args.Length >= 4)
                {
                    string colorName = args[3].ToString().ToLower();
                    color = colorName switch
                    {
                        "white" => Color.White,
                        "black" => Color.Black,
                        "red" => Color.Red,
                        "green" => Color.Green,
                        "blue" => Color.Blue,
                        "yellow" => Color.Yellow,
                        _ => Color.White
                    };
                }
                
                _renderer.DrawText(text, x, y, color);
            }
            return null;
        }

        private object DrawRectangle(object[] args)
        {
            if (args.Length >= 4)
            {
                float x = Convert.ToSingle(args[0]);
                float y = Convert.ToSingle(args[1]);
                float width = Convert.ToSingle(args[2]);
                float height = Convert.ToSingle(args[3]);
                
                Color color = Color.White;
                bool filled = false;
                
                if (args.Length >= 5)
                {
                    string colorName = args[4].ToString().ToLower();
                    color = colorName switch
                    {
                        "white" => Color.White,
                        "black" => Color.Black,
                        "red" => Color.Red,
                        "green" => Color.Green,
                        "blue" => Color.Blue,
                        "yellow" => Color.Yellow,
                        _ => Color.White
                    };
                }
                
                if (args.Length >= 6)
                {
                    filled = Convert.ToBoolean(args[5]);
                }
                
                _renderer.DrawRectangle(x, y, width, height, color, filled);
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

        private object KeyReleased(object[] args)
        {
            if (args.Length >= 1)
            {
                string key = args[0].ToString();
                return _inputManager.IsKeyReleased(key);
            }
            return false;
        }

        private object LoadSound(object[] args)
        {
            if (args.Length >= 2)
            {
                string name = args[0].ToString();
                string path = args[1].ToString();
                
                try
                {
                    var soundPlayer = new SoundPlayer(path);
                    soundPlayer.Load();
                    _sounds[name] = soundPlayer;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        private object PlaySound(object[] args)
        {
            if (args.Length >= 1)
            {
                string name = args[0].ToString();
                if (_sounds.ContainsKey(name))
                {
                    _sounds[name].Play();
                    return true;
                }
            }
            return false;
        }

        private object SetWindowTitle(object[] args)
        {
            if (args.Length >= 1)
            {
                string title = args[0].ToString();
                _windowHost.Text = title;
                return true;
            }
            return false;
        }

        private object SetResolution(object[] args)
        {
            if (args.Length >= 2)
            {
                int width = Convert.ToInt32(args[0]);
                int height = Convert.ToInt32(args[1]);
                _windowHost.Size = new Size(width, height);
                return true;
            }
            return false;
        }

        private object ChangeScene(object[] args)
        {
            if (args.Length >= 1)
            {
                string sceneName = args[0].ToString();
                _sceneManager.ChangeScene(sceneName);
                return true;
            }
            return false;
        }
    }
}