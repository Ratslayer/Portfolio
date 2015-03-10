using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsDeferredRenderState : RsState
    {
        public RsDeferredRenderState(RsState renderState)
        {
            RenderState = renderState;
        }
        private RsState RenderState;
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            RenderState.Update(time);
        }
        public override void Draw()
        {
            RenderState.Draw();
        }
    }
}
