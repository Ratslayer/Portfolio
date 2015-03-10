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
using System.Collections;
namespace WindowsGame3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private List<GameObject> objects, deadObjects, newObjects;
        GraphicsDeviceManager graphics;
        public RenderManager rMan;
        public static Random random;
        public bool bPaused = false, bGameOver=false;
        public Input input;
        public int width
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Width;
            }
            private set { }
        }
        public int height
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Height;
            }
            private set { }

        }
        public Stack<GameState> stateStack;
        protected Song music;
        public AudioEngine audio;
        public WaveBank waveBank;
        public SoundBank soundBank;
        public Level level;
        public Game1()
        {
            stateStack = new Stack<GameState>();
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 900;
            
            input = new Input();
            Content.RootDirectory = "Content";
        } 
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            objects=new List<GameObject>();
            deadObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            random = new Random();
            rMan = new RenderManager(graphics, objects, width, height);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            rMan.colorEffect = Content.Load<Effect>("ColorEffect");
            rMan.bloomEffect = Content.Load<Effect>("BloomEffect");
            rMan.downsampleEffect = Content.Load<Effect>("DownSampleEffect");
            rMan.gaussianBlurEffect = Content.Load<Effect>("GaussianBlurEffect");
            rMan.waveEffect = Content.Load<Effect>("WaveEffect");
            music = Content.Load<Song>("Meltdown");
            audio = new AudioEngine(@"Content\sound.xgs");
            waveBank = new WaveBank(audio, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(audio, @"Content\Sound Bank.xsb");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(music);
            GameContent.LoadContent();

            level = new Level(this);
            //door = new door();
            PushState(new MainMenuState(new BackGroundState()));
            //Reset();
        }
        
        
        public static float GetRandom(float max, float min)
        {
            float domain = max - min;
            float d = (float)Game1.random.NextDouble() * domain + min;
            return d;
        }
        
        public void Add(GameObject obj)
        {
            newObjects.Add(obj);
        }
        public List<GameObject> GetAll()
        {
            return objects;
        }
        public void ClearAll()
        {
            objects.Clear();
        }
        public void AddNew()
        {
            objects.AddRange(newObjects);
            newObjects.Clear();
        }
        public void RemoveDead()
        {
            foreach (GameObject obj in objects)
                if (obj.bDead)
                    deadObjects.Add(obj);
            foreach (GameObject obj in deadObjects)
                objects.Remove(obj);
            deadObjects.Clear();
        }
        public void Remove(GameObject obj)
        {
            deadObjects.Add(obj);
        }
        public static Vector2 GetRandom(Vector2 min, Vector2 max)
        {
            Vector2 v;
            v.X = GetRandom(max.X, min.X);
            v.Y = GetRandom(max.Y, min.Y);
            return v;
        }
        public static Vector2 GetRotation(float angle)
        {
            angle *= MathHelper.Pi / 180.0f;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public void PushState(GameState state)
        {
            state.EnterState();
            stateStack.Push(state);
        }
        public GameState PopState()
        {
            GameState state = stateStack.Pop();
            state.ExitState();
            return state;
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stateStack.Peek().ProcessInput(gameTime);
            stateStack.Peek().Update(time);
            base.Update(gameTime);
        }
        public void CheckCollisions()
        {
            if (Program.game.bPaused)
                return;
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject obj_1=(GameObject)objects[i];
                if(!(obj_1 is ParticleEmitter))
                    for (int j = i + 1; j < objects.Count; j++)
                    {
                        GameObject obj_2=(GameObject)objects[j];
                        if (!(obj_2 is ParticleEmitter)
                            &&obj_1.Collides(obj_2))
                        {
                            obj_1.OnCollision(obj_2);
                            obj_2.OnCollision(obj_1);
                        }
                    }
                Vector2 newPos = Vector2.Clamp(obj_1.pos, obj_1.Bounds / 2+new Vector2(0,70), new Vector2(width, height) - obj_1.Bounds / 2);
                if (obj_1.pos != newPos)
                {
                    Vector2 normal = new Vector2();
                    if (obj_1.pos.X > newPos.X)
                        normal.X = -1.0f;
                    else if (obj_1.pos.X < newPos.X)
                        normal.X = 1.0f;
                    if (obj_1.pos.Y > newPos.Y)
                        normal.Y = -1.0f;
                    else if (obj_1.pos.Y < newPos.Y)
                        normal.Y = 1.0f;
                    obj_1.OnWallCollision(normal);
                    obj_1.pos = newPos;
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            stateStack.Peek().Draw();
            //rMan.Draw();
            base.Draw(gameTime);
        }
    }
}
