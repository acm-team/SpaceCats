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
    class StarField:GameObject
    {
        private SpriteFont z_font;

        public StarField(Main game)
            : base(game, game.Content.Load<Texture2D>("Textures\\Starscape4"))
        {
            Layer = LayerConstants.BackgroundLayer;
            Width = game.WorldRect.Width;
            Height = game.WorldRect.Height;
            Speed = 25.0f/1000;
            Direction = VectorHelper.AngleToVector(MathHelper.ToRadians(90));
            Position = Vector2.Zero;
            z_font = game.Content.Load<SpriteFont>("Fonts\\LivesFont");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (X > TheGame.WorldRect.Width)
                X -= TheGame.WorldRect.Width;
            else if (X < 0)
                X += TheGame.WorldRect.Width;
            if (Y > TheGame.WorldRect.Height)
                Y -= TheGame.WorldRect.Height;
            else if (Y < 0)
                Y += TheGame.WorldRect.Height;
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle drawRect = TheGame.WorldRect;

            drawRect.X = (int)Position.X;
            drawRect.Y = (int)Position.Y;
            TheGame.SpriteBatch.Draw(Sprite, TheGame.WorldToScreen(drawRect), Color.White);
            drawRect.X -= TheGame.WorldRect.Width;
            TheGame.SpriteBatch.Draw(Sprite, TheGame.WorldToScreen(drawRect), Color.White);
            drawRect.Y -= TheGame.WorldRect.Height;
            TheGame.SpriteBatch.Draw(Sprite, TheGame.WorldToScreen(drawRect), Color.White);
            drawRect.X += TheGame.WorldRect.Width;
            TheGame.SpriteBatch.Draw(Sprite, TheGame.WorldToScreen(drawRect), Color.White);
            
            // base.Draw(gameTime);

            if (gameTime.ElapsedGameTime.Milliseconds != 0)
//                Main.SpriteBatch.DrawString(z_font, String.Format("{0}", 1000.0f / gameTime.ElapsedGameTime.Milliseconds), new Vector2(Main.WorldRect.Width / 2, 10), Color.White);
                TheGame.SpriteBatch.DrawString(z_font, String.Format("{0}", (int)(1000 / gameTime.ElapsedGameTime.Milliseconds)), TheGame.WorldToScreen(new Vector2(TheGame.WorldRect.Width / 2, 10)), Color.White);
        }
    }
}
