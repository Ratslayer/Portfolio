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
namespace RedStream
{
    public class RsInstructionsState : RsState
    {
        RsIHud Hud = new RsInstructionsHud();
        public override void Draw()
        {
            RedStream.Graphics.Render();
            Hud.Draw(RedStream.Graphics.batch);
        }
        public override void Input(GameTime time)
        {
            Hud.ProcessInput();
            base.Input(time);
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
        }
        public override void EnterState()
        {
            base.EnterState();
        }
    }
}
