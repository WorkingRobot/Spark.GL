using System;
using System.Collections.Generic;

namespace Spark.GL
{
    internal class GameObject
    {
        public string Name;
        public bool Active;
        public Transform transform {get; private set;}
        private List<IComponent> components;

        public void AddComponent(IComponent component)
        {
            if (GetComponent(component.GetType()) == null)
                components.Add(component);
            else
                throw new ArgumentException(String.Format("{0} already exists in {1}", component.GetType().FullName, Name));
        }
        public IComponent GetComponent(Type componentType)
        {
            foreach (IComponent component in components)
            {
                if (componentType == component.GetType())
                {
                    return component;
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
        }
    }
}