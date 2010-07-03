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
        public const String Jesse = "a massive homo";
    }

    public class Layer
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
    }

    public class Stage
    {
        private Layer[] z_layers;

        public Stage()
        {
            z_layers = new Layer[LayerConstants.NumLayers];
            for (int i = 0; i < LayerConstants.NumLayers; i++)
            {
                layer = new Layer();
            }
        }

        public void Clear()
        {
            foreach (Layer l in z_layers)
            {
                l.Clear();
            }
        }
    }
}
