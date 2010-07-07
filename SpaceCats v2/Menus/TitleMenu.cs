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
    class TitleMenu:Menu
    {
        //********************************************
        // Fields
        //********************************************
        private MenuObject z_title;
        private MenuObject z_start;
        private MenuObject z_options;
        private MenuObject z_exit;
        private MenuObject z_selector;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public TitleMenu(Main game)
            : base(game, "Title")
        {
        }

        //********************************************
        // Methods
        //********************************************
        public override void LoadContent()
        {
            z_title = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\TitleMenus\\spacecats_wip"), new Vector2(640, 170), "");
            z_start = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\TitleMenus\\Title_Start"), new Vector2(640, 400), "Start");
            z_options = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\TitleMenus\\Title_Options"), new Vector2(640, 480), "Options");
            z_exit = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\TitleMenus\\Title_Exit"), new Vector2(640, 560), "Exit");
            z_selector = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\TitleMenus\\Selection Arrow"), Vector2.Zero, null);

            z_start.SetMenuPointers(null, z_options, null, null, "GoToMenu Main");
            z_options.SetMenuPointers(z_start, z_exit, null, null, "Options");
            z_exit.SetMenuPointers(z_options, null, null, null, "Exit");

            z_selector.Position = new Vector2(z_start.Position.X - z_start.Width / 2 - z_selector.Width / 2 - 15, z_start.Position.Y);
            Select(z_start);

            Objects.Add(z_title);
            Objects.Add(z_start);
            Objects.Add(z_options);
            Objects.Add(z_exit);
            Objects.Add(z_selector);

            // Temporary
            z_options.Locked = true;
        }

        public override void Select(MenuObject select)
        {
            if (select == null)
                return;
            base.Select(select);
            Vector2 newPos = new Vector2(select.Position.X - select.Width / 2 - z_selector.Width / 2 - 15, select.Position.Y);
            if (z_selector.Position != newPos)
                z_selector.MoveTo(newPos, 600.0f / 1000);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Reset()
        {
        }

    }
}
