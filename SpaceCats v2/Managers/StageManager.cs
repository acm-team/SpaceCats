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
        private Stage z_stage;
        private Main z_game;
        private bool z_isUpdating;
        private Stack<GameObject> z_adds, z_removes;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public StageManager(Main game)
        {
            z_game = game;
            z_stage = new Stage();
            z_isUpdating = false;
            z_adds = new Stack<GameObject>(5);
            z_removes = new Stack<GameObject>(10);
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
            z_isUpdating = true;

            // update all items on the stage
            foreach (GameObject obj in z_stage)
            {
                obj.Update(gametime);
                if (obj.RemoveMe)
                    z_removes.Push(obj);
            }

            // remove any that were flagged for removal
            while(z_removes.Count>0)
            {
                // if the object came from a pool, return it to its pool
                if (z_removes.Peek() is IPoolableGameObject)
                    ((IPoolableGameObject)z_removes.Peek()).ReturnToPool();
                // remove the object from the stage
                z_stage.Remove(z_removes.Pop());
            }

            // add any objects that were added while updating
            while(z_adds.Count>0)
            {
                z_stage.Add(z_adds.Pop());
            }

            z_isUpdating = false;
        }

        public void Draw(GameTime gametime)
        {
            // draw the objects on the stage
            foreach (GameObject obj in z_stage)
                obj.Draw(gametime);
        }

        // Insert a new object into the main object list at the correct position based on layer
        public void AddObject(GameObject obj)
        {
            // if we are in the update loop, we cannot modify the stage structure,
            // as it would invalidate the enumerator
            if (z_isUpdating)
                z_adds.Push(obj);
            else
                z_stage.Add(obj);
        }

        // remove an object from the main object list
        public void RemoveObject(GameObject obj)
        {
            // if we are in the update loop, we cannot modify the stage structure,
            // as it would invalidate the enumerator
            if (z_isUpdating)
                z_removes.Push(obj);
            else
                z_stage.Remove(obj);
        }

        public void Reset()
        {
            z_stage.Clear();
        }

        public void LoadContent()
        {
        }

        //********************************************
        // Static Methods
        //********************************************

    }


}
