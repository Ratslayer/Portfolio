using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsWeaponNode :RsActor
    {
        TimeSpan timer = TimeSpan.Zero;
        int Jumps;
        RsActor Holder;
        RsWeapon Originator;
        RsActor.Desc desc;
       // RsActor NextHolder { get; set; }
        public RsWeaponNode(RsActor.Desc desc, RsActor holder, RsWeapon originator, int jumps)
            : base(desc)
        {
            Originator = originator;
            Holder = holder;
            Pos = Holder.Pos;
            Radius = 50;
            Jumps = jumps;
            this.desc = desc;
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Pos = Holder.Pos;
            timer -= gameTime.ElapsedGameTime;

            if (timer <= TimeSpan.Zero)
            {
                Die();
            }
            base.Update(gameTime);
        }

        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is RsEnemy && obj != Holder)
            {
                Originator.Beam(this, (RsActor)obj);
                if (Jumps > 0) new RsWeaponNode(desc, (RsActor)obj, Originator, --Jumps);
                Die();
            
            }
        }
    }
}
