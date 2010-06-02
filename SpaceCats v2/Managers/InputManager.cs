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
    public class GameControls
    {
        public static GameControl Up = new GameControl(new Keys[] { Keys.W, Keys.Up });
        public static GameControl Down = new GameControl(new Keys[] { Keys.S, Keys.Down });
        public static GameControl Left = new GameControl(new Keys[] { Keys.A, Keys.Left });
        public static GameControl Right = new GameControl(new Keys[] { Keys.D, Keys.Right });
        public static GameControl Enter = new GameControl(new Keys[] { Keys.Space, Keys.Enter});
        public static GameControl Fire = new GameControl(Keys.Space);
        public static GameControl Back = new GameControl(Keys.Escape);
        public static GameControl Menu = new GameControl(Keys.Escape);
        public static GameControl Pause = new GameControl(Keys.Escape);
    }

    public class InputManager
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;
        private KeyboardState z_prevKeyState, z_curKeyState;
        private GamePadState z_prevPadState, z_curPadState;

        //********************************************
        // Public Properties
        //********************************************
        public KeyboardState CurrentKeyState
        { get { return z_curKeyState; } }
        public KeyboardState PreviousKeyState
        { get { return z_prevKeyState; } }
        public GamePadState CurrentPadState
        { get { return z_curPadState; } }
        public GamePadState PreviousPadState
        { get { return z_prevPadState; } }

        //********************************************
        // Constructors
        //********************************************
        public InputManager(Main game)
        {
            z_game = game;
            z_prevKeyState = z_curKeyState = new KeyboardState();
            z_prevPadState = z_curPadState = new GamePadState();
            SetDefaultControls();
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
            z_prevKeyState = z_curKeyState;
            z_curKeyState = Keyboard.GetState();
            z_prevPadState = z_curPadState;
            z_curPadState = GamePad.GetState(PlayerIndex.One);
        }

        public void SetDefaultControls()
        {
        }

        public void Reset()
        {
            z_prevKeyState = z_curKeyState = new KeyboardState();
            z_prevPadState = z_curPadState = new GamePadState();
        }

        public bool KeyPressed(Keys key)
        {
            return (z_curKeyState.IsKeyDown(key) && z_prevKeyState.IsKeyUp(key));
        }

        public bool KeyPressed(GameControl control)
        {
            return control.Pressed(z_curKeyState, z_prevKeyState);
        }

        public bool KeyReleased(Keys key)
        {
            return (z_curKeyState.IsKeyUp(key) && z_prevKeyState.IsKeyDown(key));
        }

        public bool KeyReleased(GameControl control)
        {
            return control.Released(z_curKeyState, z_prevKeyState);
        }

        public bool IsKeyDown(Keys key)
        {
            return z_curKeyState.IsKeyDown(key);
        }

        public bool IsKeyDown(GameControl control)
        {
            return control.IsDown(z_curKeyState);
        }
    }
}
