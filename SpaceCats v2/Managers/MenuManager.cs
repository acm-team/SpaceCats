﻿using System;
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
    public class MenuManager
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;
        private List<GameObject> z_objects;
        private List<Menu> z_menus;
        private Stack<Menu> z_menuStack;
        private StarField z_starField;
        private bool z_isShowing;
        private PlayerShip z_bgPlayer, z_bgPlayer2;
        private bool z_transitioning = false;
        private int z_transitionstep;
        private Menu z_transitionto;
        private Menu z_transitionfrom;
        private TimeSpan z_transitiontime;

        //********************************************
        // Public Properties
        //********************************************
        public bool IsShowing
        { get { return z_isShowing; } }
        public bool IsTransitioning
        {
            get { return z_transitioning; }
            set { z_transitioning = value; }
        }
        public Menu CurrentMenu
        {
            get
            {
                if (z_menuStack.Count > 0)
                    return z_menuStack.Peek();
                else
                    return null;
            }
            set
            {
                if (value == null)
                    z_menuStack.Pop();
                else
                    z_menuStack.Push(value);
            }
        }
        public MenuObject CurrentMenuItem
        {
            get
            {
                if (CurrentMenu != null)
                    return CurrentMenu.Selected;
                else
                    return null;
            }
        }

        //********************************************
        // Constructors
        //********************************************
        public MenuManager(Main game)
        {
            z_game = game;
            z_objects = new List<GameObject>();
            z_menus = new List<Menu>();
            z_menus.Add(new TitleMenu(z_game));
            z_menus.Add(new MainMenu(z_game));
            z_menus.Add(new MissionMenu(z_game));
            z_menuStack = new Stack<Menu>();
            z_isShowing = false;
        }

        //********************************************
        // Methods
        //********************************************
        
        public void Update(GameTime gameTime)
        {
            Vector2 finish, temp;
            float orbitRadius1 = 200, orbitRadius2 = 100;
            float theta1, theta2;

            if (z_transitioning)
            {
                if (z_transitionstep == 1)
                {
                    CurrentMenu.Hide();
                    z_transitiontime = TimeSpan.Zero;
                    z_transitionstep = 2;
                }
                else if (z_transitionstep == 2)
                {
                    z_transitiontime += gameTime.ElapsedGameTime;

                    //pauses inbetween menu transitions for x milliseconds
                    if (z_transitiontime > TimeSpan.FromMilliseconds(300))
                    {
                        z_transitionstep = 3;
                    }
                }
                else if (z_transitionstep == 3)
                {
                    if (!CurrentMenu.isTransitioning)
                    {
                        CurrentMenu = z_transitionto;
                        z_transitionto = CurrentMenu;
                        CurrentMenu.Show();
                        z_transitionstep = 4;
                    }
                }
                else if (z_transitionstep == 4)
                {
                    if (!CurrentMenu.isTransitioning)
                    {
                        CurrentMenu.isActive = true;
                        z_transitionto = null;
                        z_transitionfrom = null;
                        z_transitioning = false;
                        z_transitionstep = 0;
                    }
                }
            }

            //only accept user input if the current menu is active
            if (CurrentMenu.isActive)
            {
                if (z_game.InputManager.KeyPressed(GameControls.Down) && !z_game.InputManager.IsKeyDown(GameControls.Up))
                    CurrentMenu.Select(CurrentMenu.Selected.MenuItemBelow);
                else if (z_game.InputManager.KeyPressed(GameControls.Up) && !z_game.InputManager.IsKeyDown(GameControls.Down))
                    CurrentMenu.Select(CurrentMenu.Selected.MenuItemAbove);
                else if (z_game.InputManager.KeyPressed(GameControls.Left) && !z_game.InputManager.IsKeyDown(GameControls.Right))
                    CurrentMenu.Select(CurrentMenu.Selected.MenuItemLeft);
                else if (z_game.InputManager.KeyPressed(GameControls.Right) && !z_game.InputManager.IsKeyDown(GameControls.Left))
                    CurrentMenu.Select(CurrentMenu.Selected.MenuItemRight);
                else if (z_game.InputManager.KeyPressed(GameControls.Back))
                {
                    if (z_menuStack.Count > 1)
                    {
                        TransitionTo(null);
                        //CurrentMenu.Hide();
                        //CurrentMenu = null;  // this sets CurrentMenu to the previous menu
                        //CurrentMenu.Show();
                    }
                    else
                    {
                        z_game.GameStateManager.GameState = GameState.QuitGame;
                    }
                }
                else if (z_game.InputManager.KeyPressed(GameControls.Enter))
                {
                    if (CurrentMenuItem.Locked)
                    {
                        CurrentMenuItem.Shake();
                    }
                    else
                    {
                        String[] commandWords = CurrentMenuItem.Command.Split(' ');
                        if (commandWords[0].CompareTo("GoToMenu") == 0)
                        {
                            TransitionTo(GetMenuByTag(commandWords[1]));
                            //CurrentMenu.Hide();
                            //CurrentMenu = GetMenuByTag(commandWords[1]);
                            //CurrentMenu.Show();
                        }
                        else if (commandWords[0].CompareTo("Back") == 0)
                        {
                            TransitionTo(null);
                            //CurrentMenu.Hide();
                            //CurrentMenu = null; // this sets CurrentMenu to the previous menu
                            //CurrentMenu.Show();
                        }
                        else if (commandWords[0].CompareTo("Exit") == 0)
                        {
                            z_game.GameStateManager.GameState = GameState.QuitGame;
                        }
                        else if (commandWords[0].CompareTo("StartMission") == 0)
                        {
                            z_game.StartNewGame();
                            HideNow();
                            z_game.MissionManager.StartMission(Int32.Parse(commandWords[1]));
                            z_game.GameStateManager.GameState = GameState.GamePlaying;
                        }
                        else
                        {
                            CurrentMenuItem.Shake();
                        }
                    }
                }
            }

            temp = z_bgPlayer.Position - CurrentMenuItem.Position;
            theta1 = VectorHelper.VectorToAngle(temp);
            if (temp.Length() > orbitRadius1 + z_bgPlayer.Speed)
                theta1 -= (float)Math.Acos(orbitRadius1 / temp.Length());
            theta2 = z_bgPlayer.Speed * gameTime.ElapsedGameTime.Milliseconds / orbitRadius1;
            finish = CurrentMenuItem.Position + orbitRadius1 * VectorHelper.AngleToVector(theta1 - theta2);
            z_bgPlayer.TurnToward(VectorHelper.VectorToAngle(finish - z_bgPlayer.Position), gameTime);
            z_bgPlayer.DrawRotation = VectorHelper.VectorToAngle(z_bgPlayer.Direction);


            temp = z_bgPlayer2.Position - z_bgPlayer.Position;
            theta1 = VectorHelper.VectorToAngle(temp);
            if (temp.Length() > orbitRadius2 + z_bgPlayer2.Speed)
                theta1 += (float)Math.Acos(orbitRadius2 / temp.Length());
            else
            {
                if (z_bgPlayer2.Parent == null)
                {
                    z_bgPlayer.AddChild(z_bgPlayer2);
                    z_bgPlayer2.AccelerateTo(250f / 1000f, 5000);
                }
            }
            theta2 = z_bgPlayer2.Speed * gameTime.ElapsedGameTime.Milliseconds / orbitRadius2;
            finish = z_bgPlayer.Position + orbitRadius2 * VectorHelper.AngleToVector(theta1 + theta2);
            z_bgPlayer2.TurnToward(VectorHelper.VectorToAngle(finish - z_bgPlayer2.Position), gameTime);
            z_bgPlayer2.DrawRotation = VectorHelper.VectorToAngle(z_bgPlayer2.Direction);

            
            foreach (Menu menu in z_menus)
                menu.Update(gameTime);
        }

        public void TransitionTo(Menu transitiontarget)
        {
            z_transitioning = true;
            z_transitionstep = 1;
            z_transitionfrom = CurrentMenu;
            z_transitionto = transitiontarget;
            CurrentMenu.isActive = false;
        }

        public void Show()
        {
            // start the correct song playing
            z_game.AudioManager.Play(SongList.Theme);

            // add all the menu objects into the stage
            foreach (GameObject obj in z_objects)
                z_game.StageManager.AddObject(obj);
            CurrentMenu.Show();
            // set a flag showing that the menu system is now showing
            z_isShowing = true;
        }

        public void Hide()
        {
            z_isShowing = false;
            foreach (GameObject obj in z_objects)
            {
                z_game.StageManager.RemoveObject(obj);
            }
        }

        public void HideNow()
        {
            z_isShowing = false;
            foreach (GameObject obj in z_objects)
            {
                if (obj is MenuObject)
                    ((MenuObject)obj).HideNow();
                z_game.StageManager.RemoveObject(obj);
            }
        }

        public void Reset()
        {
            foreach (Menu menu in z_menus)
                menu.Reset();
            z_menuStack.Clear();
            CurrentMenu = z_menus[0];
           
            z_isShowing = false;
        }

        public void LoadContent()
        {
            // create a starfield for the background
            z_starField = new StarField(z_game);
            z_bgPlayer = (PlayerShip) ObjectFactory.GetObject(PlayerShip.ObjectTypeID);
            //z_bgPlayer = new PlayerShip(z_game);
            z_bgPlayer.Position = new Vector2(0, 360);
            z_bgPlayer.Direction = Vector2.UnitX;
            z_bgPlayer.Speed = 150f / 1000f;
            z_bgPlayer2 = new PlayerShip(z_game);
            z_bgPlayer2.Position = new Vector2(1280, 640);
            z_bgPlayer2.Direction = -Vector2.UnitX;
            z_bgPlayer2.Speed = 500f / 1000f;
            z_bgPlayer2.MaxTurnRate = MathHelper.Pi / 1f;
            z_bgPlayer2.Layer += 0.5f;

            // add it to the object list
            z_objects.Add(z_starField);
            z_objects.Add(z_bgPlayer);
            z_objects.Add(z_bgPlayer2);
            
            // load each menus content
            // and add it to the object list
            foreach (Menu menu in z_menus)
            {
                menu.LoadContent();
                foreach (MenuObject obj in menu.Objects)
                    z_objects.Add(obj);
            }
            
            // set the current menu
            CurrentMenu = z_menus[0];
            CurrentMenu.SelectByTag("Start");
            CurrentMenu.isActive = true;

        }
       
        public Menu GetMenuByTag(string tag)
        {
            foreach (Menu menu in z_menus)
            {
                if (menu.Tag.CompareTo(tag) == 0)
                    return menu;
            }
            return null;
        }

        //********************************************
        // Static Methods
        //********************************************

    }
}
