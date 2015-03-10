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
    /// Bloomed transition state that sets the bloom factor to a specific value.
    /// </summary>
    public class TransitionState : DeferredRenderState
    {
        public TransitionState(float desiredFactor, float factorSpeed, GameState renderState)
            : base(renderState)
        {
            this.factorSpeed = factorSpeed;
            this.desiredFactor = desiredFactor;
        }
        /// <summary>
        /// Desired bloom factor that this state has to get to.
        /// </summary>
        private float desiredFactor;
        /// <summary>
        /// Speed at which the bloom factor changes
        /// </summary>
        private float factorSpeed;
        public override void Update(float gameTime)
        {
            RenderManager rman = Program.game.rMan;
            //update bloom factor if needed
            if (rman.bloomFactor != desiredFactor)
            {
                float dir = Math.Sign(desiredFactor - rman.bloomFactor);
                rman.bloomFactor += dir * gameTime * factorSpeed;
                if (Math.Abs(rman.bloomFactor - desiredFactor) < 1)
                    rman.bloomFactor = desiredFactor;

            }
            else Program.game.PopState();
            base.Update(gameTime);
        }
    }
}
