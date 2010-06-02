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
    class MainMenu : Menu
    {
        //********************************************
        // Fields
        //********************************************
        private MenuObject z_title;
        private MenuObject z_missions, z_ship, z_store;
        private MenuObject z_achievements, z_option, z_back;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public MainMenu(Main game)
            : base(game, "Main")
        {
        }

        //********************************************
        // Methods
        //********************************************
        public override void LoadContent()
        {
            z_title = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main Menu Logo"), new Vector2(640, 120), "");
            z_missions = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main_Missions"), new Vector2(380, 405), "Missions");
            z_ship = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main_Ship"), new Vector2(640, 405), "Ship");
            z_store = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main_Store"), new Vector2(900, 405), "Store");
            z_option = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main_Options"), new Vector2(380, 620), "Options");
            z_achievements = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Main_Achievements"), new Vector2(640, 620), "Achievements");
            z_back = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Back"), new Vector2(900, 620), "Back");
            z_ship.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Ship_Image"), new Vector2(640, 320), "");
            z_store.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MainMenus\\Store_Image"), new Vector2(900, 320), "");
            z_back.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Back Image"), new Vector2(900, 525), "");


            z_missions.SetMenuPointers(null, z_option, null, z_ship, "GoToMenu Mission");
            z_ship.SetMenuPointers(null, z_achievements, z_missions, z_store, "");
            z_store.SetMenuPointers(null, z_back, z_ship, z_option, "");
            z_achievements.SetMenuPointers(z_ship, null, z_option, z_back, "");
            z_option.SetMenuPointers(z_missions, null, z_store, z_achievements, "");
            z_back.SetMenuPointers(z_store, null, z_achievements, null, "Back");

            z_missions.SpriteRects.Add(new Rectangle(0, 0, 165, 29));
            z_missions.SpriteRects.Add(new Rectangle(0, 31, 165, 41));
            z_missions.SetFrames(0, 0, 1, 1);
            z_ship.SpriteRects.Add(new Rectangle(0, 0, 83, 37));
            z_ship.SpriteRects.Add(new Rectangle(0, 39, 83, 49));
            z_ship.SetFrames(0, 0, 1, 1);
            z_store.SpriteRects.Add(new Rectangle(0, 0, 109, 28));
            z_store.SpriteRects.Add(new Rectangle(0, 30, 109, 42));
            z_store.SetFrames(0, 0, 1, 1);
            z_option.SpriteRects.Add(new Rectangle(0, 0, 140, 35));
            z_option.SpriteRects.Add(new Rectangle(0, 37, 140, 49));
            z_option.SetFrames(0, 0, 1, 1);
            z_achievements.SpriteRects.Add(new Rectangle(0, 0, 256, 30));
            z_achievements.SpriteRects.Add(new Rectangle(0, 33, 256, 42));
            z_achievements.SetFrames(0, 0, 1, 1);
            z_back.SpriteRects.Add(new Rectangle(0, 0, 96, 30));
            z_back.SpriteRects.Add(new Rectangle(0, 31, 96, 46));
            z_back.SetFrames(0, 0, 1, 1);
            z_ship.ImageObject.SpriteRects.Add(new Rectangle(0, 0, 74, 59));
            z_ship.ImageObject.SpriteRects.Add(new Rectangle(0, 65, 74, 95));
            z_ship.ImageObject.SetFrames(0, 0, 1, 1);
            z_store.ImageObject.SpriteRects.Add(new Rectangle(0, 0, 92, 84));
            z_store.ImageObject.SpriteRects.Add(new Rectangle(8, 92, 75, 92));
            z_store.ImageObject.SetFrames(0, 0, 1, 1);


            Select(z_missions);

            Objects.Add(z_title);
            Objects.Add(z_missions);
            Objects.Add(z_ship);
            Objects.Add(z_store);
            Objects.Add(z_achievements);
            Objects.Add(z_option);
            Objects.Add(z_back);
            Objects.Add(z_ship.ImageObject);
            Objects.Add(z_store.ImageObject);
            Objects.Add(z_back.ImageObject);

            // Temporary
            z_ship.Locked = true;
            z_option.Locked = true;
            z_achievements.Locked = true;
            z_store.Locked = true;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
