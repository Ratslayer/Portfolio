using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using StillDesign.PhysX;
namespace RedStream
{
    class RedStream
    {
        /// <summary>
        /// Instance of RsGraphics that is responsible for rendering the scene.
        /// </summary>
        public static RsGraphics Graphics { get; private set; }
        /// <summary>
        /// Instance of RsContent that is responsible for creation, distribution and management of the content in the game
        /// </summary>
        public static RsContent Content { get; private set; }
        /// <summary>
        /// Instance of RsGame that keeps the game loop running and calls the Update functions in GameObjects
        /// </summary>
        public static RsGame Game { get; private set; }
        /// <summary>
        /// Instance of RsScene that keeps all information that is relevant to the scene.
        /// </summary>
        public static RsScene Scene { get; private set; }
        /// <summary>
        /// Instance of RsInput that responds to player's input.
        /// </summary>
        public static RsInput Input { get; private set; }
        //function to be called inside Game's LoadContent override
        public static void Init(RsGame game, GraphicsDeviceManager graphics)
        {
            RedStream.Game = game;
            SpriteBatch batch = new SpriteBatch(game.GraphicsDevice);
            Content = new RsContent();
            RsInput.Init();
            Graphics = new RsGraphics(graphics, batch);
            Input = new RsInput();
            Scene = new RsScene();
            Scene.Init();
        }      
    }
}
