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
    class Enemy:GameObject
    {
        private int z_health;
        private int z_damage;
        private int z_invincible;

        public int Health
        {
            get { return z_health; }
            set 
            { 
                if (value<=0)
                {
                    RemoveMe = true;
                }
                else
                    z_health = value; 
            }
        }

        public Enemy(Main game, Texture2D sprite)
            : base(game, sprite) { }

        public Enemy(Enemy enemy)
            : base(enemy) { }
            
    }
}
