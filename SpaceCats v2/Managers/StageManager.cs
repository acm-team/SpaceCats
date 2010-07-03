using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceCats_v2
{
    public class StageManager
    {
        //********************************************
        // Fields
        //********************************************
        private List<GameObject> z_objects;
        private Main z_game;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public StageManager(Main game)
        {
            z_game = game;
            z_objects = new List<GameObject>();
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
            int i;
            for (i = 0; i < z_objects.Count; i++)
            {
                z_objects[i].Update(gametime);
                if (z_objects[i].RemoveMe)
                {
                    // if the object came from a pool, return it to its pool
                    if (z_objects[i] is IPoolableGameObject)
                        ((IPoolableGameObject)z_objects[i]).ReturnToPool();
                    // remove the object from the list and decrement the index
                    z_objects.RemoveAt(i--);
                }
            }
        }

        public void Draw(GameTime gametime)
        {
            int i;
            // draw the objects from back to front;
            for (i = z_objects.Count - 1; i >= 0; i--)
                z_objects[i].Draw(gametime);
        }

        // Insert a new object into the main object list at the correct position based on layer
        public void AddObject(GameObject obj)
        {
            int i;
            // objects get inserted into the foreground of the layer specified
            for (i = 0; i < z_objects.Count; i++)
            {
                if (obj.Layer <= z_objects[i].Layer)
                    break;
            }
            z_objects.Insert(i, obj);
        }

        // remove an object from the main object list
        public void RemoveObject(GameObject obj)
        {
            z_objects.Remove(obj);
        }

        public void Reset()
        {
            z_objects.Clear();
        }

        public void LoadContent()
        {
        }

        //********************************************
        // Static Methods
        //********************************************

    }


}
