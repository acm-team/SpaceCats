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
        private int z_missionTime;

        //********************************************
        // Public Properties
        //********************************************
        public bool IsShowing
        { get { return z_isShowing; } }
        public int MissionTime
        { get { return z_missionTime; } }

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
            z_missionTime += gametime.ElapsedGameTime.Milliseconds;
            if (z_game.InputManager.KeyPressed(GameControls.Menu))
            {
                Hide();
                z_game.GameStateManager.GameState = GameState.InMenuSystem;
            }
            z_game.PlayerManager.Update(gametime);
        }

        public void Reset()
        {

        }

        public void LoadContent()
        {
            // place holder, all content actually gets loaded at the time a mission is selected
        }

        public void StartMission(int mission)
        {
            // This essentially starts a brandnew mission...
            switch (mission)
            {
                case 1:
                    //z_game.AudioManager.Play(SongList.Mission1);
                    z_game.StageManager.AddObject(new StarField(z_game));
                    break;
                case 2:
                    //z_game.AudioManager.Play(SongList.Mission2);
                    break;
                case 3:
                    //z_game.AudioManager.Play(SongList.Mission3);
                    break;
                case 4:
                    //z_game.AudioManager.Play(SongList.Mission4);
                    break;
                case 5:
                    //z_game.AudioManager.Play(SongList.Mission5);
                    break;
                default:
                    break;
            }
            z_missionTime = 0;
            z_game.StageManager.AddObject(z_game.Player1);
        }

        public void Show()
        {
            // start the correct song playing
            // z_game.AudioManager.Play(SongList.Theme);
            // set a flag showing that the menu system is now showing
            z_isShowing = true;
        }

        public void Hide()
        {
            z_game.StageManager.RemoveObject(z_game.Player1);
            z_isShowing = false;
        }

        //********************************************
        // Static Methods
        //********************************************

    }
}
