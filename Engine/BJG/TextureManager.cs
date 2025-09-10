using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BoyJackEngine.BJG
{
    public class TextureManager
    {
        private Dictionary<string, Image> _textures;

        public TextureManager()
        {
            _textures = new Dictionary<string, Image>();
        }

        public bool LoadTexture(string name, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Image image = Image.FromFile(filePath);
                    _textures[name] = image;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public Image GetTexture(string name)
        {
            return _textures.ContainsKey(name) ? _textures[name] : null;
        }

        public bool HasTexture(string name)
        {
            return _textures.ContainsKey(name);
        }

        public void UnloadTexture(string name)
        {
            if (_textures.ContainsKey(name))
            {
                _textures[name].Dispose();
                _textures.Remove(name);
            }
        }

        public void UnloadAll()
        {
            foreach (var texture in _textures.Values)
            {
                texture.Dispose();
            }
            _textures.Clear();
        }
    }
}