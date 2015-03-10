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

namespace RedStream
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RsGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private List<GameComponent> NewObjects, DeadObjects;
        public RsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            NewObjects = new List<GameComponent>();
            DeadObjects = new List<GameComponent>();
            Content.RootDirectory = "Content";
        }
        public int frameRate = 0;
        private int frameCounter = 0;
        private float seconds = 0;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            RedStream.Init(this, graphics);
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
            RsInput.UpdateInput();
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > 1.0f)
            {
                seconds -= 1.0f;
                frameRate = frameCounter;
                frameCounter = 0;
            }
            if(RsStateManager.Top().bInitialized)
                RsStateManager.Top().Input(gameTime);
            RsStateManager.Top().Update(gameTime);
            UpdateComponents();            
            base.Update(gameTime);
        }
        private void UpdateComponents()
        {
            foreach (GameComponent obj in NewObjects)
                Components.Add(obj);
            foreach (GameComponent obj in DeadObjects)
                Components.Remove(obj);
            NewObjects.Clear();
            DeadObjects.Clear();
        }
        public void LoadComponent(GameComponent obj)
        {
            NewObjects.Add(obj);
        }
        public void DestroyComponent(GameComponent obj)
        {
            DeadObjects.Add(obj);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameCounter++;
            RedStream.Graphics.FramesPerSecond = (float)(1.0 / gameTime.ElapsedGameTime.TotalSeconds);
            if(!RsStateManager.Top().bInitialized)
                RsStateManager.Top().EnterState();
            RsStateManager.Top().Draw();
            //RedStream.Graphics.Render();
            base.Draw(gameTime);
        }
        public Vector2 GetResolution()
        {
            return new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
        }
    }
}
