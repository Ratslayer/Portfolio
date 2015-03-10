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
    public class RsSpecial : RsActor
    {
        public RsSpecial(RsActor.Desc desc, RsSocket owner)
            : base(desc)
        {
            RsMaterialDesc mdesc = new RsMaterialDesc();
            mdesc.Color = new Vector4(); // FIXME: Special building color to be loaded;
            mdesc.DiffuseMapName = "";   // FIXME: Special building diffuse map name
            mdesc.NormalMapName = "";    // FIXME: Special building map name
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
