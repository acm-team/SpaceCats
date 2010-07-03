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
    public static class VectorHelper
    {
        public static float VectorToAngle(Vector2 vec)
        {
            float retval =  (float)Math.Atan2(vec.Y, vec.X);
            return retval;
        }

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }

    public static class LayerConstants
    {
        public const float DialogLayer = 0.0f;
        public const float MenuLayer = 0.0f;
        public const float AchievementLayer = 0.0f;
        public const float ScoreLayer = 1.0f;
        public const float FloatingTextLayer = 2.0f;
        public const float ExplodLayer = 3.0f;
        public const float ObjectLayer = 4.0f;
        public const float BackgroundLayer = 5.0f;
    }
    
    public class GameObject
    {
        //*******************************
        //* Field Variables
        //*******************************
        private Main z_game;
        private int z_id;
        private Vector2 z_position, z_direction;
        private float z_layer;
        private float z_speed, z_maxSpeed;
        private float z_acceleration;
        private float z_accelerateTo;
        private Texture2D z_sprite;
        private List<Rectangle> z_spriteRects;
        private int z_firstFrameToDraw, z_lastFrameToDraw, z_currentFrame;
        private int z_animationTimer, z_animationDelay;
        private bool z_visible;
        private float z_width, z_height, z_scaleX, z_scaleY;
        private float z_drawRotation, z_spriteOrientation;
        private float z_maxTurnRate;
        private int z_spriteWidth, z_spriteHeight;
        private Color z_spriteColor;
        private Vector2 z_SpriteCenter;
        private bool z_removeMe;
        private Vector2 z_moveTo;
        private float z_spriteAlpha;
        private GameObject z_parent;
        private List<GameObject> z_children;

        //*******************************
        // Public Properties
        //*******************************
        public Main TheGame
        {
            get { return z_game; }
        }
        public int ID
        {
            get { return z_id; }
            set { z_id = value; }
        }
        public Vector2 Position
        {
            get { return z_position; }
            set { z_position = value; }
        }
        public float X
        {
            get { return z_position.X; }
            set { z_position.X = value; }
        }
        public float Y
        {
            get { return z_position.Y; }
            set { z_position.Y = value; }
        }
        public Vector2 Velocity
        {
            get { return z_speed * z_direction; }
            set
            { 
                z_direction = value;
                // do not normalize if the length is zero, results are undefined
                if (value.Length()!=0)
                    z_direction.Normalize();
                z_speed = value.Length();
            }
        }
        public float Speed
        {
            get { return z_speed; }
            set { z_speed = value; }
        }
        public float MaxSpeed
        {
            get { return z_maxSpeed; }
            set { z_maxSpeed = value; }
        }
        public Vector2 Direction
        {
            get { return z_direction; }
            set 
            {
                z_direction = value;
                z_direction.Normalize(); // ensure that the direction is a unit vector
            }
        }
        public float Layer
        {
            get { return z_layer; }
            set { z_layer = value; }
        }
        public float SubLayer
        {
            get { return z_layer - (float)Math.Floor(z_layer); }
        }
        public bool Visible
        {
            get { return z_visible; }
            set { z_visible = value; }
        }
        public bool RemoveMe
        {
            get { return z_removeMe; }
            set { z_removeMe = value; }
        }
        public Vector2 MovingTo
        { get { return z_moveTo; } }
        public float MaxTurnRate
        {
            get { return z_maxTurnRate; }
            set { z_maxTurnRate = value; }
        }

        // the rotation angle to draw the object in radians
        public float DrawRotation
        {
            get { return z_drawRotation; }
            set { z_drawRotation = value; }
        }
        
        // the actual image which consists of one or more animation cels
        public Texture2D Sprite
        {
            get { return z_sprite; }
            set
            {
                z_sprite = value;
                if (value!=null)
                {
                    z_width = value.Width;
                    z_height = value.Height;
                    z_scaleX = z_scaleY = 1.0f;
                    z_firstFrameToDraw = z_lastFrameToDraw = z_currentFrame = 0;
                    z_SpriteCenter = new Vector2(z_width / 2, z_height / 2);
                    z_spriteColor = Color.White;
                }
            }
        }

        // the actual size of the current animation cel
        public Rectangle SpriteRect
        {
            get 
            { 
                if (z_spriteRects.Count()==0)
                    return new Rectangle(0,0,z_sprite.Width, z_sprite.Height);
                else 
                    return z_spriteRects[z_currentFrame];
            }
        }
        
        // the drawing rectangle to draw the object in world coords
        public Rectangle DrawRect
        {
            get { return new Rectangle((int)z_position.X, (int)z_position.Y, (int)(SpriteWidth * z_scaleX * (1 - SubLayer)), (int)(SpriteHeight * z_scaleY * (1 - SubLayer))); }
        }

        // used for handling animation
        public int AnimationTimer
        {
            get { return z_animationTimer; }
            set
            {
                z_animationTimer = value;
                if (z_animationTimer >= z_animationDelay)
                {
                    z_animationTimer = 0;
                    CurrentFrame++;
                }
            }
        }
        public int AnimationDelay
        {
            get { return z_animationDelay; }
            set { z_animationDelay = value; }
        }
        
        // return the actual size of the current animation cel in world coords
        public float SpriteWidth
        {
            get
            {
                if (z_spriteRects.Count() == 0)
                    return z_sprite.Width;
                else
                    return z_spriteRects[z_currentFrame].Width;
            }
        }
        public float SpriteHeight
        {
            get
            {
                if (z_spriteRects.Count() == 0)
                    return z_sprite.Height;
                else
                    return z_spriteRects[z_currentFrame].Height;
            }
        }
        public float SpriteOrientation
        {
            get { return z_spriteOrientation; }
            set { z_spriteOrientation = value; }
        }
        
        // the "desired" Width and Height actual values may vary due to animation cel sizes
        // when setting these values, it sets a scalefactor based on the largest animation cel
        // in the current range
        public float Width
        {
            get { return z_width; }
            set 
            {
                int spriteWidth = 0;
                if (z_spriteRects.Count == 0)
                    spriteWidth = Sprite.Width;
                else
                {
                    // we are going to set the scale factor using the widest sprite in the 
                    // current set of frames
                    for (int i = z_firstFrameToDraw; i <= z_lastFrameToDraw; i++)
                    {
                        if (z_spriteRects[i].Width > spriteWidth)
                            spriteWidth = z_spriteRects[i].Width;
                    }
                }
                z_width = value;
                z_scaleX = value / spriteWidth;
            }
        }
        public float Height
        {
            get { return z_height; }
            set 
            {
                int spriteHeight = 0;
                if (z_spriteRects.Count == 0)
                    spriteHeight = Sprite.Height;
                else
                {
                    // we are going to set the scale factor using the widest sprite in the 
                    // current set of frames
                    for (int i = z_firstFrameToDraw; i <= z_lastFrameToDraw; i++)
                    {
                        if (z_spriteRects[i].Height > spriteHeight)
                            spriteHeight = z_spriteRects[i].Height;
                    }
                }
                z_height = value;
                z_scaleY = value / spriteHeight;
            }
        }

        // a list of the rectangles corresponding to all the animation cels within the main sprite
        public List<Rectangle> SpriteRects
        {
            get { return z_spriteRects; }
        }

        // these define the range of cels to use for drawing an animation
        public int FirstFrameToDraw
        {
            get { return z_firstFrameToDraw; }
            set
            {
                z_firstFrameToDraw = value;
                CurrentFrame = z_firstFrameToDraw;
            }
        }
        public int LastFrameToDraw
        {
            get { return z_lastFrameToDraw; }
            set
            {
                z_lastFrameToDraw = value;
                CurrentFrame = z_firstFrameToDraw;
            }
        }
        public int CurrentFrame
        {
            get { return z_currentFrame; }
            set
            {
                // changing the current animation sprite will also readjust the sprite center (for drawing)
                z_currentFrame = (value > z_lastFrameToDraw ? z_firstFrameToDraw : value);
                z_SpriteCenter = new Vector2(SpriteWidth / 2, SpriteHeight / 2);
            }
        }

        // the color to use when drawing the sprite
        public Color SpriteColor
        {
            get { return z_spriteColor; }
            set { z_spriteColor = value; }
        }
        public float SpriteAlpha
        {
            get { return z_spriteAlpha; }
            set
            {
                if (value > 100.0f)
                    z_spriteAlpha = 100.0f;
                else if (value < 0.0f)
                    z_spriteAlpha = 0.0f;
                else
                    z_spriteAlpha = value;
                z_spriteColor.A = (byte)(z_spriteAlpha / 100.0f * 255);

            }
        }

        // Parent/child relationships
        public GameObject Parent
        {
            get { return z_parent; }
            set { z_parent = value; }
        }
        public List<GameObject> Children
        {
            get { return z_children; }
            set { z_children = value; }
        }

        //********************************
        // Constructors
        //********************************
        public GameObject()
        {
        }

        public GameObject(Main game, Texture2D sprite)
        {
            z_game = game;
            z_spriteRects = new List<Rectangle>();
            // the spriteRect array will likely be maintained as a static variable by derived classes to save memory.
            
            Sprite = sprite; // this automatically sets all sprite related parameters, such as width, center, scale, etc

            z_animationDelay = 100;
            z_animationTimer = 0;
            z_position = Vector2.Zero;
            z_direction = Vector2.Zero;
            z_speed = 0.0f;
            z_removeMe = false;
            z_spriteColor = Color.White;
            z_spriteAlpha = 100.0f;
            z_visible = true;
            z_drawRotation = 0.0f;
            z_maxTurnRate = MathHelper.Pi / 3000f;
            z_children = new List<GameObject>();
        }

        //********************************
        // Methods
        //********************************
        public virtual void Update(GameTime gameTime)
        {
            // modify speed with acceleration
            if (z_acceleration != 0f)
            {
                if (Math.Abs(z_speed - z_accelerateTo) <= Math.Abs(z_acceleration * gameTime.ElapsedGameTime.Milliseconds))
                {
                    z_speed = z_accelerateTo;
                    z_acceleration = 0f;
                }
                else
                    z_speed += z_acceleration * gameTime.ElapsedGameTime.Milliseconds;
            }
            // apply velocity to position
            Position += Velocity * gameTime.ElapsedGameTime.Milliseconds;
            foreach (GameObject child in z_children)
                child.Position += Velocity * gameTime.ElapsedGameTime.Milliseconds;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (z_visible)
            {
                // adjust the animation timer, which will automatically change the current sprite frame if needed
                AnimationTimer += gameTime.ElapsedGameTime.Milliseconds;
                // draw the current sprite
                // DrawRect returns the current draw rectangle, scaling for the desired spritesize
                // 
                z_game.SpriteBatch.Draw(z_sprite, z_game.WorldToScreen(DrawRect), SpriteRect, z_spriteColor,
                    z_drawRotation - z_spriteOrientation, z_SpriteCenter, SpriteEffects.None, 0);
            }
        }

        public virtual void MoveTo(Vector2 moveto, float speed)
        {
            // pass a speed of zero to continue moving at the current speed
            if (speed != 0.0f)
                z_speed = speed;
            z_moveTo = moveto;
            Direction = moveto - z_position;
        }

        public void TurnToward(float newDirection, GameTime gameTime)
        {
            float curDirection = VectorHelper.VectorToAngle(Direction);
            float multiplier = 1.0f;
           
            newDirection %= MathHelper.TwoPi; 

            // if the difference in direction is less than the max turn rate, jump to new direction
            if (Math.Abs(newDirection - curDirection) < z_maxTurnRate * gameTime.ElapsedGameTime.Milliseconds)
            {
                Direction = VectorHelper.AngleToVector(newDirection);
                return;
            }

            // if the difference in direction is more than half a circle, turn the other direction
            if (Math.Abs(newDirection - curDirection) > MathHelper.Pi)
                multiplier = -1.0f;
            if (newDirection > curDirection)
            {
                if (newDirection - curDirection < z_maxTurnRate * gameTime.ElapsedGameTime.Milliseconds)
                    Direction = VectorHelper.AngleToVector(newDirection);
                else
                    Direction = VectorHelper.AngleToVector(curDirection + z_maxTurnRate * gameTime.ElapsedGameTime.Milliseconds * multiplier);
            }
            else
            {
                if (curDirection - newDirection < z_maxTurnRate * gameTime.ElapsedGameTime.Milliseconds)
                    Direction = VectorHelper.AngleToVector(newDirection);
                else
                    Direction = VectorHelper.AngleToVector(curDirection - z_maxTurnRate * gameTime.ElapsedGameTime.Milliseconds * multiplier);
            }
        }

        public void AddChild(GameObject child)
        {
            child.Parent = this;
            z_children.Add(child);
        }

        public void AccelerateTo(float newSpeed, float accelTime)
        {
            z_accelerateTo = newSpeed;
            if (accelTime == 0f)
                z_acceleration = (newSpeed - z_speed) / 1000f;
            else
                z_acceleration = (newSpeed - z_speed) / accelTime;
        }

        public virtual void Reset()
        {
            z_animationTimer = 0;
            z_position = z_moveTo = Vector2.Zero;
            z_direction = Vector2.UnitX;
            z_accelerateTo = z_acceleration = z_speed = 0.0f;
            z_removeMe = false;
            z_visible = true;
            z_drawRotation = 0;
        }
    }
}
