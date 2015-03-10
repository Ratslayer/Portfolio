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
    public class RsShield : RsDamageableActor
    {
        public RsShield(RsActor.Desc desc, RsShieldData data, RsPlanet owner)
            : base(desc, data)
        {
            RechargeTime = data.RechargeTime;
            RechargeRate = data.RechargeRate;
            Cooldown = 0.0f;
            bActive = true;
            Owner = owner;
            DisplayedRadius = new SmoothFloatComponent(Radius, Radius, 3, 0);
            NormalRadius = Radius;
        }
        private RsPlanet Owner;
        private SmoothFloatComponent DisplayedRadius;
        private float NormalRadius;
        public override void TakeDamage(float damage)
        {
            float remainingDamage = Math.Abs(Health - damage);
            base.TakeDamage(damage);
            Cooldown = RechargeTime;
            if (Health == 0.0f)
            {
                bActive = false;
                Owner.TakeDamage(remainingDamage);
                DisplayedRadius.F = 1;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Radius = DisplayedRadius.F;
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Cooldown = Math.Max(Cooldown - seconds / RechargeTime, 0);
            if (Cooldown == 0.0f)
            {
                bActive = true;
                Health = Math.Min(Health + seconds * RechargeRate, MaxHealth);
                DisplayedRadius.F = NormalRadius;
            }
            base.Update(gameTime);
        }
        public float RechargeTime, RechargeRate, Cooldown;
        public bool bActive;
    }
}
