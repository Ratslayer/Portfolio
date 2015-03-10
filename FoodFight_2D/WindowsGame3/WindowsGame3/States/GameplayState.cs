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
namespace WindowsGame3
{
    /// <summary>
    /// Gameplay state that is responsible for most of game logic.
    /// </summary>
    public class GameplayState : GameState
    {
        /// <summary>
        /// Boolean that tells if the HUD should be drawn.
        /// </summary>
        private bool bDrawHUD = true;
        public override void EnterState()
        {
            Program.game.level.Reset();
            Program.game.level.InitLevel();
            base.EnterState();
        }
        public override void Draw()
        {
            RenderManager rman = Program.game.rMan;
            Game1 game = Program.game;
            SpriteFont font = GameContent.Font, bigFont=GameContent.MediumFont;
            rman.Draw();
            if (bDrawHUD)
            {
                HudBar.Draw(rman.batch);
                Level.door.DrawMsgs(rman.batch);
            }
            base.Draw();
        }
        public override void Update(float gameTime)
        {
            Game1 game = Program.game;
            List<GameObject> objects = game.GetAll();
            //update the game if not paused.
            game.AddNew();
            if(!Program.game.bPaused)
                foreach (GameObject obj in objects)
                    obj.Update(gameTime);
            game.RemoveDead();
            game.CheckCollisions();
            //Push the PausedState if the game is paused
            if (game.bPaused)
            {
                game.PushState(new PauseState(this));
                game.PushState(new TransitionState(-1, 10, this));
            }
            //Reset the level if needed
            if (game.level.bReset)
            {
                //if next level is playable, redraw the pits
                if (game.level.level <= 10)
                {
                    game.PushState(new BubblingState(300, this));
                    game.PushState(new TransitionState(1, 20, this));
                    game.PushState(new LoadLevelState(this));
                    game.PushState(new TransitionState(15, 20, this));
                    game.PushState(new BubblingState(-300, this));
                }
                //else the user has won the game. Push EndGameState
                else
                {
                    bDrawHUD = false;
                    EndGameState endGame = new EndGameState(new BackGroundState());
                    game.PushState(endGame);
                    game.PushState(new TransitionState(-.9f, 10, endGame));
                    game.PushState(new SwapBackgroundState("rainbow2", this));
                    game.PushState(new TransitionState(-20, 40, this));
                    game.PushState(new TransitionState(15, 20, this));
                }
                //if game over, push GameOverState
                if (game.bGameOver)
                {
                    game.PushState(new TransitionState(-10, 20, this));
                    game.PushState(new GameOverState(this));
                    game.PushState(new TransitionState(-.9f, 20, this));
                    game.PushState(new BubblingState(-300, this));
                }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Helper function that gets the total number of particles that the game has.
        /// Used for debugging.
        /// </summary>
        /// <returns>The total number of particles.</returns>
        private int GetNumParticles()
        {
            Game1 game = Program.game;
            List<GameObject> objects = game.GetAll();
            int nPs = 0;
            foreach (GameObject obj in objects)
                if (obj is ParticleEmitter)
                    nPs += ((ParticleEmitter)obj).GetNumParticles();
            return nPs;
        }
        public override void ProcessInput(GameTime gameTime)
        {
            Program.game.input.ProcessInput(gameTime);
            base.ProcessInput(gameTime);
        }
    }
}
