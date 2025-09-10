using System;

namespace BoyJackEngine.BJGSH
{
    public abstract class Scene
    {
        public string Name { get; protected set; }
        public bool IsActive { get; set; }

        public Scene(string name)
        {
            Name = name;
            IsActive = false;
        }

        public virtual void Initialize() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Draw() { }
        public virtual void OnEnter() { IsActive = true; }
        public virtual void OnExit() { IsActive = false; }
        public virtual void Cleanup() { }
    }
}