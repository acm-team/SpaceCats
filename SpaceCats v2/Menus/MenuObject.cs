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
    public class MenuObject:GameObject
    {
        //********************************************
        // Constants
        //********************************************
        private const float FADE_SPEED = 0.1f;    // fade speed = 100%/1 sec = 100/1000 = 0.1

        //********************************************
        // Fields
        //********************************************
        private MenuObject z_menuItemAbove, z_menuItemBelow;
        private MenuObject z_menuItemLeft, z_menuItemRight;
        private string z_command;
        private Vector2 z_originalPosition;  // for moving the object back on screen after it has been moved off
        private bool z_isHiding, z_isUnhiding;
        private bool z_selected;
        private string z_tag;
        protected int z_notSelFirstFrame, z_notSelLastFrame, z_selFirstFrame, z_selLastFrame;
        private float z_shakeTimer;
        private bool z_locked;
        private MenuObject z_imageObject;

        //********************************************
        // Public Properties
        //********************************************
        public MenuObject MenuItemAbove
        {
            get { return z_menuItemAbove; }
            set { z_menuItemAbove = value; }
        }
        public MenuObject MenuItemBelow
        {
            get { return z_menuItemBelow; }
            set { z_menuItemBelow = value; }
        }
        public MenuObject MenuItemLeft
        {
            get { return z_menuItemLeft; }
            set { z_menuItemLeft = value; }
        }
        public MenuObject MenuItemRight
        {
            get { return z_menuItemRight; }
            set { z_menuItemRight = value; }
        }
        public string Command
        {
            get { return z_command; }
            set { z_command = value; }
        }
        public string Tag
        {
            get { return z_tag; }
            set { z_tag = value; }
        }
        public virtual bool Selected
        {
            get { return z_selected; }
            set 
            { 
                z_selected = value;
                if (value)
                {
                    FirstFrameToDraw = z_selFirstFrame;
                    LastFrameToDraw = z_selLastFrame;
                }
                else
                {
                    FirstFrameToDraw = z_notSelFirstFrame;
                    LastFrameToDraw = z_notSelLastFrame;
                }
            }
        }
        public bool IsMoving
        {
            get { return (Velocity!=Vector2.Zero); }
        }
        public bool Locked
        {
            get { return z_locked; }
            set 
            {
                if (z_imageObject != null)
                {
                    // this will change the image drawn to the locked image
                    //   represented by the selected and not selected images respectively
                    z_imageObject.Selected = value;
                }
                z_locked = value; 
            }
        }
        public MenuObject ImageObject
        {
            get { return z_imageObject; }
            set { z_imageObject = value; }
        }

        //********************************************
        // Constructors
        //********************************************
        public MenuObject(Main game, Texture2D texture, Vector2 position, string tag)
            : base(game, texture)
        {
            Tag = tag;
            Layer = LayerConstants.MenuLayer;
            z_originalPosition = Position = position;
            MenuItemAbove = MenuItemBelow = MenuItemRight = MenuItemLeft = null;
            this.Velocity = Vector2.Zero;
            this.MaxSpeed = 10;
            z_selected = false;
            Visible = false;
            SpriteAlpha = 0.0f;
        }

        //********************************************
        // Methods
        //********************************************
        public void Hide()
        {
            z_isHiding = true;
            z_isUnhiding = false;
        }

        public void Show()
        {
            Visible = true;
            z_isHiding = false;
            z_isUnhiding = true;
        }

        public void HideNow()
        {
            z_isHiding = z_isUnhiding = false;
            Visible = false;
            SpriteAlpha = 0.0f;
        }

        public void ShowNow()
        {
            z_isHiding = z_isUnhiding = false;
            Visible = true;
            SpriteAlpha = 100.0f;
        }

        public void SetMenuPointers(MenuObject above, MenuObject below, MenuObject left, MenuObject right, string command)
        {
            z_menuItemAbove = above;
            z_menuItemBelow = below;
            z_menuItemLeft = left;
            z_menuItemRight = right;
            z_command = command;
        }

        public override void Update(GameTime gameTime)
        {
            if (MovingTo != Vector2.Zero)
            {
                if (Vector2.Distance(Position, MovingTo) < Speed*gameTime.ElapsedGameTime.Milliseconds)
                {
                    Position = MovingTo;
                    Velocity = Vector2.Zero;
                }
                base.Update(gameTime);
            }
            // the following is to shake a menu object that is either locked...
            if (z_shakeTimer > 0.0f)
            {
                z_shakeTimer -= 1.0f;
                if (z_shakeTimer < 0.0f)
                    z_shakeTimer = 0.0f;
                DrawRotation = MathHelper.ToRadians((float)(6 * Math.Sin(z_shakeTimer)));
            }
        }

        public void Shake()
        {
            if (z_imageObject == null)
                z_shakeTimer = MathHelper.Pi * 4;
            else
                z_imageObject.Shake();
        }

        public void SetFrames(int notSelFirstFrame, int notSelLastFrame, int selFirstFrame, int selLastFrame)
        {
            z_notSelFirstFrame = notSelFirstFrame;
            z_notSelLastFrame = notSelLastFrame;
            z_selFirstFrame = selFirstFrame;
            z_selLastFrame = selLastFrame;
            FirstFrameToDraw = (z_selected ? z_selFirstFrame : z_notSelFirstFrame);
            LastFrameToDraw = (z_selected ? z_notSelFirstFrame : z_notSelLastFrame);
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (Locked)
            {
                SpriteColor = Color.Lerp(Color.Black, Color.White, 0.35f);
            }
            if (z_isHiding)
            {
                SpriteAlpha -= FADE_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                if (SpriteAlpha == 0.0f)
                {
                    z_isHiding = false;
                    Visible = false;
                }
            }
            if (z_isUnhiding)
            {
                SpriteAlpha += FADE_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                if (SpriteAlpha == 100.0f)
                    z_isUnhiding = false;
            }
            base.Draw(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
            z_isHiding = z_isUnhiding = false;
            SpriteAlpha = 0.0f;
            Visible = false;
            MoveTo(Vector2.Zero, 0.0f);
        }
    }
}
