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
using System.Collections;
namespace WindowsGame3
{
    /// <summary>
    /// Class that represents a damageable game object.
    /// </summary>
    public class Damageable : GameObject
    {
        public float Health
        {
            get
            {
                return _health;
            }
            private set
            {
                 _health = MathHelper.Clamp(value, 0, maxHealth);
            }
        }
        public enum HitType
        {
            Enemy,
            Player
        };
        public HitType Type
        {
            get;
            private set;
        }
        private float _health;
        private float maxHealth;
        /// <summary>
        /// The time for which the enemy will be pushed back.
        /// </summary>
        private float PushedBackTime = 0;
        public void PushBack(Vector2 vel)
        {
            this.vel = vel;
            PushedBackTime = .3f;
        }
        public bool IsPushedBack()
        {
            return PushedBackTime > 0f;
        }
        public Damageable(Texture2D texture, float maxHealth, HitType type)
            : base(texture)
        {
            this.maxHealth = maxHealth;
            _health = maxHealth;
            Type = type;
        }
        public void RestoreHealth()
        {
            Health = maxHealth;
        }
        public void SetHealth(float Health)
        {
            this.Health = Health;
        }
        public virtual void TakeDamage(float damage)
        {
            Health = _health - damage;
        }
        public void Heal(float damage)
        {
            Health = _health + damage;
        }
        public override void Update(float time)
        {
            base.Update(time);
            PushedBackTime -= time;
            if (Health <= 0.0f)
            {
                Die();
            }
        }
    }
}
