namespace Spark.GL
{
    public class Component
    {
        public GameObject gameObject {get; internal set;}
        public virtual void Awake() { }
        public virtual void Load() { }
        public virtual void Update() { }
        public virtual void Render() { }
        public virtual void Exit() { }
        //public virtual void OnEnable() { }
        //public virtual void OnDisable() { }
    }
}