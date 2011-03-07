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
    public class PlayerShip:GameObject
    {
        //**************************
        // Private Fields
        //**************************
        private int z_health;
        private int z_lives;
        private int z_score;
        private int z_shotsTaken;
        private int z_hits;
        private int z_xLimit;
        private int z_yLimit;

        //**************************
        // Public Properties
        //**************************
        public const int ObjectTypeID = 2;
        public int Health
        {
            get { return z_health; }
            set { z_health = value; }
        }
        public int Lives
        {
            get { return z_lives; }
            set { z_lives = value; }
        }
        public int Score
        {
            get { return z_score; }
            set { z_score = value; }
        }
        public int ShotsTaken
        {
            get { return z_shotsTaken; }
            set { z_shotsTaken = value; }
        }
        public int Hits
        {
            get { return z_hits; }
            set { z_hits = value; }
        }
        public float HitPercentage
        {
            get { return (float)z_hits / (float)z_shotsTaken; }
        }


        //**************************
        // Constructors
        //**************************
        public PlayerShip(Main game)
            : base(game, game.Content.Load<Texture2D>("Images\\ship1"))
        {
            z_typeID = PlayerShip.ObjectTypeID;
            Health = 100;
            Lives = 3;
            Score = 0;
            ShotsTaken = 0;
            Hits = 0;
            Position = new Vector2(640, 650);
            SpriteOrientation = 3f * MathHelper.PiOver2;
            Layer = LayerConstants.ObjectLayer;
            Direction = -Vector2.UnitY;
            DrawRotation = VectorHelper.VectorToAngle(Direction);
            MaxSpeed = 0.8f;
            z_xLimit = game.ViewPort.Width-25;
            z_yLimit = game.ViewPort.Height-30;
        }

        public PlayerShip(PlayerShip p)
            : base (p)
        {
            z_typeID = PlayerShip.ObjectTypeID;
            z_health = p.z_health;
            z_lives = p.z_lives;
            z_score = p.z_score;
            z_shotsTaken = p.z_shotsTaken;
            z_hits = p.z_hits;
        }

        //**************************
        // Methods
        //**************************
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (X < 25)
            {
                X = 25;
                Velocity = new Vector2(0, Velocity.Y);
            }
            if (X > z_xLimit)
            {
                X = z_xLimit;
                Velocity = new Vector2(0, Velocity.Y);
            }
            if (Y < 30)
            {
                Y = 30;
                Velocity = new Vector2(Velocity.X, 0);
            }
            if (Y > z_yLimit)
            {
                Y = z_yLimit;
                Velocity = new Vector2(Velocity.X, 0);
            }
        }

        public override GameObject Clone()
        {
            return new PlayerShip(this);
        }

        public override void Reset()
        {
            base.Reset();
            Health = 100;
            Lives = 3;
            Score = 0;
            ShotsTaken = 0;
            Hits = 0;
            Position = new Vector2(640, 650);
            Direction = -Vector2.UnitY;
            DrawRotation = VectorHelper.VectorToAngle(Direction);
            Speed = 0;
        }
    }
}
