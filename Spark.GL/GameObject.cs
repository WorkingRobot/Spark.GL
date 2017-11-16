using System;
using System.Collections.Generic;

namespace Spark.GL
{
    public class GameObject
    {
        public string Name;
        public bool Active;
        public Transform transform;
        internal Window window;
        internal List<Component> components;

        public T AddComponent<T>() where T : Component, new()
        {
            if (GetComponent<T>() == null)
            {
                T comp = new T();
                comp.gameObject = this;
                comp.window = window;
                components.Add(comp);
                return comp;
            }
            throw new ArgumentException(String.Format("{0} already exists in {1}", typeof(T).FullName, Name));
        }
        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (typeof(T) == component.GetType())
                {
                    return (T)component;
                }
            }
            return null;
        }

        internal GameObject parent;
        public void SetParent(GameObject newParent)
        {
            parent.children.Remove(this);
            newParent.children.Add(this);
            parent = newParent;
        }
        public GameObject GetParent()
        {
            return parent;
        }

        internal List<GameObject> children;
        

        public GameObject(string name)
        {
            Name = name;
            transform = new Transform();
            components = new List<Component>();
            Active = true;
            children = new List<GameObject>();
            parent = null;
            Window.window.AddObject(this);
        }
    }
}