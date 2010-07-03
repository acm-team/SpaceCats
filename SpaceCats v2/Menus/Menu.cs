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
    public class Menu
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;
        private List<MenuObject> z_objects;
        private MenuObject z_selected;
        private string z_tag;
        private bool z_isActive;
        private bool z_isTransitioning;

        //********************************************
        // Public Properties
        //********************************************
        public string Tag
        {
            get { return z_tag; }
            set { z_tag = value; }
        }
        public List<MenuObject> Objects
        {
            get { return z_objects; }
        }
        public MenuObject Selected
        {
            get { return z_selected; }
            set 
            {
                if (z_selected != null)
                    z_selected.Selected = false;
                z_selected = value;
                z_selected.Selected = true;
            }
        }
        public Main TheGame
        { get { return z_game; } }
        public bool isActive
        {
            get { return z_isActive; }
            set { z_isActive = value; }
        }
        public bool isTransitioning
        {
            get { return z_isTransitioning; }
            set { z_isTransitioning = value; }
        }

        //********************************************
        // Constructors
        //********************************************
        public Menu(Main game, string tag)
        {
            z_game = game;
            z_objects = new List<MenuObject>();
            Tag = tag;
        }

        //********************************************
        // Methods
        //********************************************
        public virtual void LoadContent()
        {
            // placeholder
        }

        public virtual void Update(GameTime gameTime)
        {
            // placeholder
        }

        public virtual void Show()
        {
            foreach (MenuObject obj in Objects)
                obj.Show();
        }

        public virtual void Hide()
        {
            foreach (MenuObject obj in Objects)
                obj.Hide();
        }

        public virtual void HideNow()
        {
            foreach (MenuObject obj in Objects)
                obj.HideNow();
        }

        public virtual void ShowNow()
        {
            foreach (MenuObject obj in Objects)
                obj.ShowNow();
        }

        public virtual void Draw(GameTime gameTime)
        {
            // placeholder
        }

        public virtual void SelectByTag(string tag)
        {
            foreach (MenuObject obj in Objects)
            {
                if (obj.Tag.CompareTo(tag) == 0)
                {
                    Select(obj);
                    return;
                }
            }
        }

        public virtual void Select(MenuObject select)
        {
            if (select != null)
                Selected = select;
        }

        public virtual void Reset()
        {
            foreach (MenuObject obj in z_objects)
                obj.Reset();
        }
    }
}
