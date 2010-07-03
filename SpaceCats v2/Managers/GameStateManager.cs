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
    public enum GameState
    {
        InMenuSystem,
        GamePlaying,
        GamePaused,
        GameOver,
        QuitGame
    }

    public class GameStateManager
    {
        //********************************************
        // Fields
        //********************************************
        private GameState z_gameState;
        private Main z_game;

        //********************************************
        // Public Properties
        //********************************************
        public GameState GameState
        { 
            get { return z_gameState; }
            set { z_gameState = value; }
        }

        //********************************************
        // Constructors
        //********************************************
        public GameStateManager(Main game)
        {
            z_game = game;
            z_gameState = GameState.InMenuSystem;
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
            if (z_game.InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (z_game.InputManager.KeyPressed(Keys.D1) && z_game.ViewPort.Width != 1280)
                {
                    z_game.ViewPort = new Rectangle(0, 0, 1280, 720);
                }
                else if (z_game.InputManager.KeyPressed(Keys.D8) && z_game.ViewPort.Width != 800)
                {
                    z_game.ViewPort = new Rectangle(0, 0, 800, 600);
                }
                else if (z_game.InputManager.KeyPressed(Keys.D6) && z_game.ViewPort.Width != 640)
                {
                    z_game.ViewPort = new Rectangle(0, 0, 640, 480);
                }
                else if (z_game.InputManager.KeyPressed(Keys.D9) && z_game.ViewPort.Width != 1920)
                {
                    z_game.ViewPort = new Rectangle(0, 0, 1920, 1080);
                }
                else if (z_game.InputManager.KeyPressed(Keys.F))
                {
                    z_game.FullScreen = !z_game.FullScreen;
                }
                else if (z_game.InputManager.KeyPressed(Keys.D0))
                {
                    if (z_game.InputManager.IsKeyDown(Keys.LeftShift) || z_game.InputManager.IsKeyDown(Keys.RightShift))
                    {
                        z_game.AudioManager.SoundOn = !z_game.AudioManager.SoundOn;
                    }
                    else
                    {
                        z_game.AudioManager.MusicOn = !z_game.AudioManager.MusicOn;
                    }
                }
                else if (z_game.InputManager.KeyPressed(Keys.OemMinus))
                {
                    if (z_game.InputManager.IsKeyDown(Keys.LeftShift) || z_game.InputManager.IsKeyDown(Keys.RightShift))
                    {
                        z_game.AudioManager.MusicVolume -= 0.05f;
                    }
                    else
                    {
                        z_game.AudioManager.MasterVolume -= 0.05f;
                    }
                }
                else if (z_game.InputManager.KeyPressed(Keys.OemPlus))
                {
                    if (z_game.InputManager.IsKeyDown(Keys.LeftShift) || z_game.InputManager.IsKeyDown(Keys.RightShift))
                    {
                        z_game.AudioManager.MusicVolume += 0.05f;
                    }
                    else
                    {
                        z_game.AudioManager.MasterVolume += 0.05f;
                    }
                }
            }
            switch (z_gameState)
            {
                case GameState.InMenuSystem:
                    if (!z_game.MenuManager.IsShowing)
                        z_game.MenuManager.Show();
                    z_game.MenuManager.Update(gametime);
                    break;
                case GameState.GamePlaying:
                    if (!z_game.MissionManager.IsShowing)
                        z_game.MissionManager.Show();
                    z_game.MissionManager.Update(gametime);
                    break;
                case GameState.GameOver:
                    break;
                case GameState.QuitGame:
                    z_game.AudioManager.Play(SongList.None);
                    z_game.Exit();
                    break;
                default:
                    break;
            }

        }

        public void Reset()
        {
            z_gameState = GameState.InMenuSystem;
        }

        public void LoadContent()
        {
        }


        //********************************************
        // Static Methods
        //********************************************

    }
}
