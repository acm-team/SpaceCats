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
    public class GameControl
    {
        //*******************************
        //* Field Variables
        //*******************************
        private Keys[] z_keys;
        private Buttons[] z_buttons;

        //*******************************
        //* Public Properties
        //*******************************
        public Keys Key
        {
            get { return z_keys[0]; }
            set { z_keys[0] = value; }
        }
        public Buttons Button
        {
            get { return z_buttons[0]; }
            set { z_buttons[0] = value; }
        }

        //*******************************
        //* Constants
        //*******************************

        //*******************************
        //* Constructors
        //*******************************
        private GameControl()
        {
        }

        public GameControl(Keys key)
        {
            z_keys = new Keys[1];
            z_keys[0] = key;
        }

        public GameControl(Keys[] keys)
        {
            z_keys = keys;
        }

        public bool Pressed(KeyboardState curState, KeyboardState prevState)
        {
            // returns true if any of the keys in the list was just pressed
            foreach (Keys key in z_keys)
            {
                if (curState.IsKeyDown(key) && prevState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool Pressed(GamePadState curState, GamePadState prevState)
        {
            // returns true if any of the buttons in the list was just pressed
            foreach (Buttons button in z_buttons)
            {
                if (curState.IsButtonDown(button) && prevState.IsButtonUp(button))
                    return true;
            }
            return false;
        }

        public bool Released(KeyboardState curState, KeyboardState prevState)
        {
            // returns true if any of the keys in the list was just released
            foreach (Keys key in z_keys)
            {
                if (curState.IsKeyUp(key) && prevState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool Released(GamePadState curState, GamePadState prevState)
        {
            // returns true if any of the buttons in the list was just released
            foreach (Buttons button in z_buttons)
            {
                if (curState.IsButtonUp(button) && prevState.IsButtonDown(button))
                    return true;
            }
            return false;
        }

        public bool IsDown(KeyboardState state)
        {
            // returns true if any of the keys in the list are down, false otherwise
            foreach (Keys key in z_keys)
            {
                if (state.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool IsDown(GamePadState state)
        {
            // returns true if any of the buttons in the list are pressed, false otherwise
            foreach (Buttons button in z_buttons)
            {
                if (state.IsButtonDown(button))
                    return true;
            }
            return false;
        }
    }
}
