namespace Spark.GL
{
    internal class Component
    {
        public virtual GameObject gameObject {get; internal set;}
        public virtual void Awake() { }
        public virtual void Load() { }
        public virtual void Update() { }
        public virtual void Render() { }
        public virtual void Exit() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}