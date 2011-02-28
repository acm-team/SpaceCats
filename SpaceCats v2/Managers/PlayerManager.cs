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
    public class PlayerManager
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public PlayerManager(Main game)
        {
            z_game = game;
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
            if (z_game.InputManager.IsKeyDown(GameControls.Up))
                z_game.Player1.Velocity -= (Vector2.UnitY)*.1f;
            if (z_game.InputManager.IsKeyDown(GameControls.Down))
                z_game.Player1.Velocity += (Vector2.UnitY)*.1f;
            if (z_game.InputManager.IsKeyDown(GameControls.Right))
                z_game.Player1.Velocity += (Vector2.UnitX)*.1f;
            if (z_game.InputManager.IsKeyDown(GameControls.Left))
                z_game.Player1.Velocity -= (Vector2.UnitX)*.1f;
        }

        public void Reset()
        {
        }

        //********************************************
        // Static Methods
        //********************************************

    }
}
