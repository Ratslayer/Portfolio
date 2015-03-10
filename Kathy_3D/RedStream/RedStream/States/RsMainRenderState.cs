using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsMainRenderState : RsState
    {
        public override void Draw()
        {
            RedStream.Graphics.Render();
        }
    }
}
