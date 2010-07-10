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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        //*********************************************
        // Constants
        //*********************************************
        private const float DESIGNED_WIDTH = 1280;
        private const float DESIGNED_HEIGHT = 720;
        private const float DESIGN_ASPECT_RATIO = 1.777778f;

        //*********************************************
        // Fields
        //*********************************************
        public Rectangle WorldRect = new Rectangle(0, 0, (int)DESIGNED_WIDTH, (int)DESIGNED_HEIGHT);
        private GraphicsDeviceManager z_graphics;
        private SpriteBatch z_spriteBatch;
        private StageManager z_stageManager;
        private GameStateManager z_gameStateManager;
        private MenuManager z_menuManager;
        private MissionManager z_missionManager;
        private InputManager z_inputManager;
        private AudioManager z_audioManager;
        private Matrix z_scaleMatrix;
        private float z_scaleFactor, z_aspectRatio;
        private float z_offsetY;
        private bool z_fullScreen;
        private Rectangle z_viewport;
        private Texture2D z_blackDot;
        private bool[] z_missionLocked;
        private PlayerShip z_player1;

        //*********************************************
        // Public properties
        //*********************************************
        public Rectangle ViewPort
        {
            get { return z_viewport; }
            set
            {
                z_graphics.PreferredBackBufferWidth = value.Width;
                z_graphics.PreferredBackBufferHeight = value.Height;
                z_graphics.ApplyChanges();
                z_viewport = value;
                z_aspectRatio = (float)value.Width / (float)value.Height;
                z_scaleFactor = value.Width / Main.DESIGNED_WIDTH;
                z_scaleMatrix = Matrix.CreateScale(z_scaleFactor, z_scaleFactor, 1);
                if (Math.Abs(DESIGN_ASPECT_RATIO - z_aspectRatio) > 0.1)
                {
                    z_viewport.Height = (int)(value.Width/DESIGN_ASPECT_RATIO);
                    z_offsetY = (value.Height - z_viewport.Height) / 2;
                    z_scaleMatrix.Translation = new Vector3(0,z_offsetY,0);
                }
                else
                {
                    z_offsetY = 0;
                    z_scaleMatrix.Translation = Vector3.Zero;
                }
            }
        }
        public float ScaleFactor
        { get { return z_scaleFactor; } }
        public bool FullScreen
        {
            get { return z_fullScreen; }
            set
            {
                z_fullScreen = value;
                z_graphics.IsFullScreen = value;
                z_graphics.ApplyChanges();
            }
        }
        public SpriteBatch SpriteBatch
        {
            get { return z_spriteBatch; }
        }
        public GraphicsDeviceManager Graphics
        {
            get { return z_graphics; }
        }
        public StageManager StageManager
        { get { return z_stageManager; } }
        public GameStateManager GameStateManager
        { get { return z_gameStateManager; } }
        public MenuManager MenuManager
        { get { return z_menuManager; } }
        public AudioManager AudioManager
        { get { return z_audioManager; } }
        public MissionManager MissionManager
        { get { return z_missionManager; } }
        public InputManager InputManager
        { get { return z_inputManager; } }
        public PlayerShip Player1
        {
            get { return z_player1; }
            set { z_player1 = value; }
        }

        public Main()
        {
            z_graphics = new GraphicsDeviceManager(this);
            ViewPort = new Rectangle(0, 0, 1280, 720);
            Content.RootDirectory = "Content";
            z_fullScreen = false;
//            this.IsFixedTimeStep = false;

            z_stageManager = new StageManager(this);
            z_gameStateManager = new GameStateManager(this);
            z_menuManager = new MenuManager(this);
            z_missionManager = new MissionManager(this);
            z_inputManager = new InputManager(this);
            z_audioManager = new AudioManager(this);
            z_missionLocked = new bool[6];

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // lock all missions but the first
            for (int i = 2; i < 6; i++)
                LockMission(i);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            z_spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            ObjectFactory.Initialize();
            ObjectFactory.AddObjectType(GameObject.ObjectTypeID, new GameObject(this, Content.Load<Texture2D>("Images\\ship1")), true, 10);
            ObjectFactory.AddObjectType(PlayerShip.ObjectTypeID, new PlayerShip(this), true, 5);
            z_gameStateManager.LoadContent();
            z_stageManager.LoadContent();
            z_missionManager.LoadContent();
            z_menuManager.LoadContent();
            z_audioManager.LoadContent();
            z_blackDot = Content.Load<Texture2D>("Images\\Black Dot");

            Player1 = new PlayerShip(this); // create this here because it loads resources
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // update the input mnanager first, since most other managers rely on it
            z_inputManager.Update(gameTime);

            // next update the gameState manager
            // this will update the menu manager, mission manager, etc as required
            z_gameStateManager.Update(gameTime);

            // update the stage manager last
            z_stageManager.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //z_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, z_scaleMatrix);
            z_spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // only draw the stage manager... it handles ALL drawing
            z_stageManager.Draw(gameTime);

            if (z_offsetY != 0)
            {
                z_spriteBatch.Draw(z_blackDot, new Rectangle(0, 0, (int)ViewPort.Width,(int) z_offsetY), Color.White);
                z_spriteBatch.Draw(z_blackDot, new Rectangle(0,(int)(ViewPort.Height + z_offsetY + 1), (int)ViewPort.Width, (int)z_offsetY), Color.White);
            }

            z_spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void StartNewGame()
        {
            z_player1.Reset();
            // if/when we support multiple players, reset those players here as well.
        }

        public Rectangle WorldToScreen(Rectangle r)
        {
            return new Rectangle((int)(r.X * z_scaleFactor),(int)(r.Y * z_scaleFactor + z_offsetY), (int)(r.Width * z_scaleFactor), (int)(r.Height * z_scaleFactor));
        }

        public Vector2 WorldToScreen(Vector2 p)
        {
            return new Vector2(p.X * z_scaleFactor, p.Y * z_scaleFactor + z_offsetY);
        }

        public bool IsMissionLocked(int missionNum)
        {
            return z_missionLocked[missionNum];
        }

        public void UnlockMission(int missionNum)
        {
            z_missionLocked[missionNum] = false;
        }

        public void LockMission(int missionNum)
        {
            z_missionLocked[missionNum] = true;
        }
        
    
    }
}
