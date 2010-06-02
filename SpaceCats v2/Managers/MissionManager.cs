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
    public class MissionManager
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;
        private bool z_isShowing;
        private int z_loadedMission;

        //********************************************
        // Public Properties
        //********************************************
        public bool IsShowing
        { get { return z_isShowing; } }            

        //********************************************
        // Constructors
        //********************************************
        public MissionManager(Main game)
        {
            z_game = game;
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
        }

        public void Reset()
        {

        }

        public void LoadContent()
        {
            // place holder, all content actually gets loaded at the time a mission is selected
        }

        public void LoadMission(int mission)
        {

        }

        public void Show()
        {
            // clear the stage
            z_game.StageManager.Reset();
            // start the correct song playing
            // z_game.AudioManager.Play(SongList.Theme);
            // set a flag showing that the menu system is now showing
            z_isShowing = true;
        }

        public void Hide()
        {
            z_isShowing = false;
        }

        //********************************************
        // Static Methods
        //********************************************

    }
}
