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
    class MissionMenu : Menu
    {
        //********************************************
        // Fields
        //********************************************
        private MenuObject z_title, z_mission1, z_mission2, z_mission3, z_mission4, z_mission5, z_back;

        //********************************************
        // Public Properties
        //********************************************

        //********************************************
        // Constructors
        //********************************************
        public MissionMenu(Main game)
            : base(game, "Mission")
        {
        }

        //********************************************
        // Methods
        //********************************************
        public override void Show()
        {
            z_mission1.Locked = TheGame.IsMissionLocked(1);
            z_mission2.Locked = TheGame.IsMissionLocked(2);
            z_mission3.Locked = TheGame.IsMissionLocked(3);
            z_mission4.Locked = TheGame.IsMissionLocked(4);
            z_mission5.Locked = TheGame.IsMissionLocked(5);
            base.Show();
        }
        
        public override void LoadContent()
        {
            z_title = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission Logo"), new Vector2(640, 120), "");
            z_mission1 = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission 1"), new Vector2(430, 405), "Mission 1");
            z_mission2 = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission 2"), new Vector2(640, 405), "Mission 2");
            z_mission3 = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission 3"), new Vector2(850, 405), "Mission 3");
            z_mission4 = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission 4"), new Vector2(430, 620), "Mission 4");
            z_mission5 = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Mission 5"), new Vector2(640, 620), "Mission 5");
            z_back = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Back"), new Vector2(850, 620), "Back");
            z_mission1.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Lock"), new Vector2(430, 325), "");
            z_mission2.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Lock"), new Vector2(640, 325), "");
            z_mission3.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Lock"), new Vector2(850, 325), "");
            z_mission4.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Lock"), new Vector2(430, 530), "");
            z_mission5.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Lock"), new Vector2(640, 530), "");
            z_back.ImageObject = new MenuObject(TheGame, TheGame.Content.Load<Texture2D>("Menus\\MissionMenus\\Back Image"), new Vector2(850, 530), "");

            z_mission1.SetMenuPointers(null, z_mission4, null, z_mission2, "StartMission 1");
            z_mission2.SetMenuPointers(null, z_mission5, z_mission1, z_mission3, "StartMission 2");
            z_mission3.SetMenuPointers(null, z_back, z_mission2, z_mission4, "StartMission 3");
            z_mission4.SetMenuPointers(z_mission1, null, z_mission3, z_mission5, "StartMission 4");
            z_mission5.SetMenuPointers(z_mission2, null, z_mission4, z_back, "StartMission 5");
            z_back.SetMenuPointers(z_mission3, null, z_mission5, null, "Back");

            z_mission1.SpriteRects.Add(new Rectangle(0, 1, 175, 28));
            z_mission1.SpriteRects.Add(new Rectangle(0, 30, 175, 44));
            z_mission1.SetFrames(0, 0, 1, 1);
            z_mission2.SpriteRects.Add(new Rectangle(0, 0, 179, 28));
            z_mission2.SpriteRects.Add(new Rectangle(0, 29, 179, 44));
            z_mission2.SetFrames(0, 0, 1, 1);
            z_mission3.SpriteRects.Add(new Rectangle(0, 0, 179, 28));
            z_mission3.SpriteRects.Add(new Rectangle(0, 29, 179, 44));
            z_mission3.SetFrames(0, 0, 1, 1);
            z_mission4.SpriteRects.Add(new Rectangle(0, 1, 179, 28));
            z_mission4.SpriteRects.Add(new Rectangle(0, 30, 179, 44));
            z_mission4.SetFrames(0, 0, 1, 1);
            z_mission5.SpriteRects.Add(new Rectangle(0, 1, 179, 28));
            z_mission5.SpriteRects.Add(new Rectangle(0, 30, 179, 44));
            z_mission5.SetFrames(0, 0, 1, 1);
            z_back.SpriteRects.Add(new Rectangle(0, 0, 96, 30));
            z_back.SpriteRects.Add(new Rectangle(0, 31, 96, 46));
            z_back.SetFrames(0, 0, 1, 1);
            z_mission1.ImageObject.SpriteRects.Add(new Rectangle(0, 0, 1, 1));
            z_mission1.ImageObject.SpriteRects.Add(new Rectangle(0, 0, 75, 92));
            z_mission1.ImageObject.SetFrames(0, 0, 1, 1);
            z_mission2.ImageObject.SetFrames(0, 0, 0, 0);
            z_mission3.ImageObject.SetFrames(0, 0, 0, 0);
            z_mission4.ImageObject.SetFrames(0, 0, 0, 0);
            z_mission5.ImageObject.SetFrames(0, 0, 0, 0);
            z_back.ImageObject.SetFrames(0, 0, 0, 0);

            Select(z_mission1);

            Objects.Add(z_title);
            Objects.Add(z_mission1);
            Objects.Add(z_mission2);
            Objects.Add(z_mission3);
            Objects.Add(z_mission4);
            Objects.Add(z_mission5);
            Objects.Add(z_back);
            Objects.Add(z_mission1.ImageObject);
            Objects.Add(z_mission2.ImageObject);
            Objects.Add(z_mission3.ImageObject);
            Objects.Add(z_mission4.ImageObject);
            Objects.Add(z_mission5.ImageObject);
            Objects.Add(z_back.ImageObject);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
