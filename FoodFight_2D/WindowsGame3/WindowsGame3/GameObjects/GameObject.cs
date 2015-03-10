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
    /// This class is extended by every gameplay object that can be placed in the scene and be collided against.
    /// </summary>
    public class GameObject
    {
        #region Structs
        /// <summary>
        /// A Circle struct that is used for additional collision precision if the object we're colliding is a circle and not a box.
        /// </summary>
        private struct Circle
        {
            public Circle(float radius, Vector2 position)
            {
                Radius = radius;
                Position = position;
            }
            public float Radius;
            public Vector2 Position;
        };
        #endregion

        #region Attributes
        public Vector2 pos, vel, acc, scale;
        public Vector4 color;
        public float angle, depth, lifeTime, movSpeed, angularSpeed;
        /// <summary>
        /// If bOnTimer is true then the Object dies when LifeTime reaches 0.
        /// </summary>
        public bool bOnTimer;
        public bool bDead;
        public Texture2D texture;
        /// <summary>
        /// Param that can flip the texture horizontally if needed.
        /// </summary>
        public SpriteEffects spriteEffects;
        /// <summary>
        /// The collision Box. Setting this property will scale the texture accordingly and will disable cicle collision.
        /// </summary>
        public Vector2 Bounds { get { return new Vector2(texture.Width, texture.Height) * scale; } set { SetSize(value); bUseRadius = false; } }
        public bool bUseRadius;
        /// <summary>
        /// The collision Radius. Setting this property will not scale the texture, but will enable circle collision. 
        /// </summary>
        public float Radius { get { return _radius; } set { _radius = value; bUseRadius = true; } }
        private float _radius;
        #endregion

        #region Constructors
        public GameObject(Texture2D texture)
        {
            pos = new Vector2();
            acc = new Vector2();
            vel = new Vector2();
            scale = new Vector2(1, 1);
            color = new Vector4(1, 1, 1, 1);
            spriteEffects = SpriteEffects.None;
            this.texture = texture;
            lifeTime = 0;
            bOnTimer = false;
            depth = .5f;
        }
        #endregion

        #region Virtual Functions
        public Vector2 GetCenteredPos(Vector2 pos, Vector2 bounds, float angle)
        {
            float rads = MathHelper.ToRadians(angle);
            Matrix rotMat = Matrix.CreateRotationZ(rads);
            return pos - Vector2.Transform(bounds / 2, rotMat);
        }
        public virtual void Draw(SpriteBatch batch)
        {
            float rads = MathHelper.ToRadians(angle);
            batch.Draw(texture, GetCenteredPos(pos, Bounds, angle), null, new Color(color), rads, new Vector2(), scale, spriteEffects, depth);
        }
        public virtual void Update(float time)
        {
            //update movement vars
            vel += acc * time;
            pos += vel * time;
            angle += angularSpeed * time;
            //die if needed
            if (bOnTimer)
            {
                lifeTime -= time;
                if(lifeTime <= 0.0f)
                    Die();
            }                
            if (float.IsNaN(pos.X) || float.IsNaN(pos.Y))
                Die();
        }
        /// <summary>
        /// Calling this function will result in the removal of the GameObject from the game.
        /// </summary>
        public virtual void Die()
        {
            bDead = true;
        }
        /// <summary>
        /// Called when a GameObject collides with another GameObject.
        /// </summary>
        /// <param name="obj">GameObject that has collided against this.</param>
        public virtual void OnCollision(GameObject obj)
        {
        }
        /// <summary>
        /// Called when a GameObject collides with a screen border
        /// </summary>
        /// <param name="normal">Vector Normal to the screen border.</param>
        public virtual void OnWallCollision(Vector2 normal)
        {
        }
        #endregion

        #region Helper Functions
        #region Public Functions
        /// <summary>
        /// Accessor of the bounds for the texture.
        /// </summary>
        /// <returns>Bounding rectangle of the sprite.</returns>
        public Rectangle GetRect()
        {
            Vector2 b = Bounds / 2;
            return new Rectangle((int)(pos.X - b.X), (int)(pos.Y - b.Y), (int)(b.X * 2), (int)(b.Y * 2));
        }
        /// <summary>
        /// Allows to get a collision point between 2 GameObjects.
        /// </summary>
        /// <param name="obj">GameObject that has collided against this.</param>
        /// <returns>Collision point between the 2 GameObjects.</returns>
        public Vector2 GetCollisionPoint(GameObject obj)
        {
            if (bUseRadius)
            {
                //Circle-Circle collision
                if (obj.bUseRadius)
                {
                    Vector2 dist = obj.pos - pos;
                    dist.Normalize();
                    dist *= (pos - obj.pos).Length() / 2;
                    return pos + dist;
                }
                //Circle-Rectangle collision
                else return CircleRectanglePoint(new Circle(Radius, pos), obj.GetRect());
            }
            //Circle-Rectangle collision
            else if (obj.bUseRadius)
                return CircleRectanglePoint(new Circle(obj.Radius, obj.pos), GetRect());
            //Rectangle-Rectangle collision
            else
            {
                Rectangle cr = Rectangle.Intersect(GetRect(), obj.GetRect());
                return new Vector2(cr.Center.X, cr.Center.Y);
            }
        }
        /// <summary>
        /// Returns true if both objects intersect and false if they don't. Uses Rectangle or Circle collision. Used by game's physics function.
        /// </summary>
        /// <param name="obj">The GameObject that the collision is tested against.</param>
        /// <returns>True if both objects intersect and false if they don't.</returns>
        public bool Collides(GameObject obj)
        {
            if (bUseRadius)
            {
                //Circle-Circle collision
                if (obj.bUseRadius)
                    return (pos - obj.pos).Length() < Radius + obj.Radius;
                //Circle-Rectangle collision
                else return CircleRectangleCollides(new Circle(Radius, pos), obj.GetRect());
            }
            //Circle-Rectangle collision
            else if (obj.bUseRadius)
                return CircleRectangleCollides(new Circle(obj.Radius, obj.pos), GetRect());
            //Rectangle-Rectangle Collision
            else
            {
                Rectangle rect_1 = GetRect(), rect_2 = obj.GetRect();
                return rect_1.Intersects(rect_2);
            }
        }
        #endregion
        #region Private Functions
        /// <summary>
        /// Set image size to the desired value.
        /// </summary>
        /// <param name="size">Image size in pixels</param>
        private void SetSize(Vector2 size)
        {
            scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);
        }
        /// <summary>
        /// Tests if a Circle and a Rectangle collide.
        /// </summary>
        /// <param name="circle">The Circle that's being collided.</param>
        /// <param name="rect">The Rectangle that is being collided.</param>
        /// <returns>True if both intersect and false if they don't.</returns>
        private bool CircleRectangleCollides(Circle circle, Rectangle rect)
        {
            Vector2 point = CircleRectanglePoint(circle, rect);
            return (point - circle.Position).Length() <= circle.Radius;
        }
        /// <summary>
        /// Gets a point between a Circle and a Rectangle. Must be intersecting for this to properly work.
        /// </summary>
        /// <param name="circle">Circle that's being collided.</param>
        /// <param name="rect">Rectangle that's being collided.</param>
        /// <returns>The point of intersection between the 2 shapes.</returns>
        private Vector2 CircleRectanglePoint(Circle circle, Rectangle rect)
        {
            return Vector2.Clamp(circle.Position, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Bottom));
        }
        #endregion
        #endregion
    }
}
