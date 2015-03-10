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
    public class DescDataPair<T> where T : RsActorData
    {
        public DescDataPair(string name)
        {
            Data = (T)RedStream.Content.GetObjectAttributes(name);
            Desc = new RsActor.Desc(Data.Desc);
        }
        public T Data;
        public RsActor.Desc Desc;
    }
}
