using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SpaceCats_v2
{
    public struct ObjectFactoryNode
    {
        private int z_objectTypeID, z_initialSize;
        private GameObject z_default;
        private Stack<GameObject> z_pool;

        public int ObjectTypeID
        {
            get { return z_objectTypeID;}
            set { z_objectTypeID = value;}
        }
        public GameObject Default
        {
            get {return z_default;}
            set {z_default = value;}
        }
        public Stack<GameObject> Pool
        {
            get { return z_pool;}
            set { z_pool = value;}
        }
        public int InitialSize
        {
            get { return z_initialSize;}
            set { z_initialSize = value;}
        }
        
        public ObjectFactoryNode(int objectTypeID, GameObject defaultObject, bool isPooled, int initialSize)
        {
            z_objectTypeID = objectTypeID;
            z_default = defaultObject;
            z_initialSize = initialSize;
            if (isPooled)
            {
                z_pool = new Stack<GameObject>(initialSize);
            }
            else z_pool = null;
        }

        public GameObject GetObject()
        {
            if (z_pool != null)
            {
                if (z_pool.Count > 0)
                    return z_pool.Pop();
                else
                {
                    for (int i = 0; i < z_initialSize; i++)
                    {
                        z_pool.Push(z_default.Clone());
                    }
                }
            }
            return z_default.Clone();
        }

        public void ReturnToPool(GameObject o)
        {
            if (z_pool != null)
            {
                o.Reset();
                z_pool.Push(o);
            }
        }
    }

    public static class ObjectFactory
    {
        private static List<ObjectFactoryNode> z_objects;
  
        public static void Initialize()
        {
            z_objects = new List<ObjectFactoryNode>();
        }

        public static void AddObjectType(int objectTypeID, GameObject defaultObject, bool isPooled, int initialSize)
        {
            z_objects.Add(new ObjectFactoryNode(objectTypeID, defaultObject, isPooled, initialSize));
        }

        public static GameObject GetObject(int objectTypeID)
        {
            foreach(ObjectFactoryNode node in z_objects)
            {
                if (node.ObjectTypeID==objectTypeID)
                {
                    return node.GetObject();
                }
            }
            return null;
        }

        public static void ReturnObject(GameObject o)
        {
            foreach (ObjectFactoryNode node in z_objects)
            {
                if (node.ObjectTypeID == o.TypeID)
                {
                    node.ReturnToPool(o);
                    return;
                }
            }
        }
    }
}
