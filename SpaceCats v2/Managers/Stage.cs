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
using System.Collections;

namespace SpaceCats_v2
{
    public static class LayerConstants
    {
        public const float BackgroundLayer = 0.0f;
        public const float ObjectLayer = 1.0f;
        public const float ExplodLayer = 2.0f;
        public const float FloatingTextLayer = 3.0f;
        public const float ScoreLayer = 4.0f;
        public const float AchievementLayer = 5.0f;
        public const float DialogLayer = 6.0f;
        public const float MenuLayer = 7.0f;
        public const int NumLayers = 8;
    }

    public class Layer : IEnumerable<GameObject>
    {
        private LinkedList<GameObject> z_objects;
        
        public Layer()
        {
            z_objects = new LinkedList<GameObject>();
        }

        public void Clear()
        {
            z_objects.Clear();
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return z_objects.GetEnumerator();
        }

        public void Add(GameObject newObject)
        {
            if (z_objects.Count==0)
                z_objects.AddFirst(newObject);
            else
            {
                // Sub-layers are .0 = foreground to .9999999 = background
                //  ( this is so that the foreground has a definite value, while we can 
                //    extend the background indefinitely. I chose this ordering because
                //    most objects will be in the very front, so we need a definitive value 
                //    for the front of the layer. )
                // They are ordered from background to foreground
                // so they are drawn in the correct order
                // hence, they must be inserted in the reverse order

                LinkedListNode<GameObject> node;
                // Working from the back...
                for (node = z_objects.Last; node != null; node=node.Previous)
                {
                    // find the first object that should be behind the new one
                    // (in most cases, it should be the first one)
                    if (node.Value.SubLayer >= newObject.SubLayer)
                    {
                        z_objects.AddAfter(node, newObject);
                        return;
                    }
                }
                // did not find any objects that should be behind this one,
                // so this must be a new sublayer at the very back
                // (worst case scenario)
                z_objects.AddFirst(newObject);
            }
        }

        public void Remove(GameObject obj)
        {
            z_objects.Remove(obj);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class StageEnumerator : IEnumerator<GameObject>
    {
        private IEnumerator z_layerEnum;
        private IEnumerator<GameObject> z_enum;

        public GameObject Current
        { get { return z_enum.Current; } }

        Object IEnumerator.Current
        { get { return z_enum.Current; } }
           
        public StageEnumerator()
        {
        }

        public StageEnumerator(Layer[] layers)
        {
            z_layerEnum = layers.GetEnumerator();
            z_layerEnum.MoveNext();
            z_enum = ((Layer)z_layerEnum.Current).GetEnumerator();
        }

        public bool MoveNext()
        {
            bool done = false;
            while (!done)
            {
                // attempt to move to the next item in the current layer
                if (z_enum.MoveNext())
                    done = true; // if successful, we're done
                else
                {
                    // if there was not another item in the current layer,
                    // attempt to move to the next layer
                    if (!z_layerEnum.MoveNext())
                    {
                        // if this was not successful, then we are at the lest element in the last layer
                        // and therefore, we are done so return false
                        return false;
                    }
                    else 
                    {
                        // otherwise, create a new enumerator for the new layer
                        z_enum = ((Layer)z_layerEnum.Current).GetEnumerator();
                    }
                }
            }
            return true;
        }

        public void Reset()
        {
            z_layerEnum.Reset(); // reset the layer enumerator
            z_layerEnum.MoveNext(); // and go to the first element in it
            z_enum = ((Layer)z_layerEnum.Current).GetEnumerator(); // create a new enumerator for the first layer
        }

        public void Dispose()
        {
            z_enum = null;
            z_layerEnum = null;
        }
    }

    public class Stage : IEnumerable<GameObject>
    {
        private Layer[] z_layers;

        public Stage()
        {
            z_layers = new Layer[LayerConstants.NumLayers];
            for (int i = 0; i < LayerConstants.NumLayers; i++)
            {
                z_layers[i] = new Layer();
            }
        }

        public void Add(GameObject obj)
        {
            z_layers[(int)obj.Layer].Add(obj);
        }

        public void Remove(GameObject obj)
        {
            z_layers[(int)obj.Layer].Remove(obj);
        }

        public IEnumerator<GameObject> GetEnumeratorForLayer(int layer)
        {
            return z_layers[layer].GetEnumerator();
        }

        public IEnumerator GetLayerEnumerator()
        {
            return z_layers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return new StageEnumerator(z_layers);
        }

        public void Clear()
        {
            foreach (Layer layer in z_layers)
            {
                layer.Clear();
            }
        }
    }
}
