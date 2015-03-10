using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsDamageableActor : RsActor
    {
        public RsDamageableActor(RsActor.Desc desc, RsDamageableData data)
            : base(desc)
        {
            MaxHealth = data.Health;
            Health = data.Health;
        }
        public virtual void TakeDamage(float damage)
        {
            Health = Math.Max(Health - damage, 0);
        }
        public float MaxHealth, Health;
    }
}
