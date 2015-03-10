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
    /// State during which all pits grow to their intended sizes, one by one.
    /// </summary>
    public class BubblingState : DeferredRenderState
    {
        public BubblingState(float bubblingSpeed, GameState renderState)
            :base(renderState)
        {
            this.bubblingSpeed = bubblingSpeed;
        }
        /// <summary>
        /// The speed at which the pits grow.
        /// </summary>
        private float bubblingSpeed;
        public override void Update(float gameTime)
        {
            List<GameObject> objects = Program.game.GetAll();
            //get the next pit in line
            foreach(GameObject obj in objects)
                if(obj is Pit)
                {
                    float speed;
                    Pit pit = (Pit)obj;
                    //determine the direction of growth
                    if (bubblingSpeed < 0)
                    {
                        pit.desiredRadius = 0;
                        speed = -bubblingSpeed;
                    }
                    else speed = bubblingSpeed;
                    //if the pit is not at needed size, change it and return
                    if (pit.Radius != pit.desiredRadius)
                    {
                        float dir = Math.Sign(pit.desiredRadius - pit.Radius);
                        pit.Radius += dir * gameTime * speed;
                        if (Math.Abs(pit.Radius - pit.desiredRadius) < speed / 100)
                            pit.Radius = pit.desiredRadius;
                        return;
                    } 
                    //else continue;
                }
            Program.game.PopState();
            base.Update(gameTime);
        }
    }
}
