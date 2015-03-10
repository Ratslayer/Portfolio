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
namespace RedStream.Kathy
{
    public class GameplayState : RsMainRenderState
    {
        public GameplayState()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        bool timeLeft;
        public float timer;
        private float lightSpeed = 100, KathySpeed = 50;
        private int nPitsPerLevel = 4;
        SpriteFont font;
        public override void EnterState()
        {
            base.EnterState();
            timeLeft = true;
            RedStream.Game.Components.Clear();
            GameInfo.LoadContent();
            LoadPits();
            cameraAngle = new SmoothFloatComponent(0, 30, 1, 0);
            cameraDistance = new SmoothFloatComponent(0, 100, 1, 0);
            //GameInfo.PrizeBall.SmoothColor= new SmoothFloatComponent(1, 0, (1/60.0f), 0);
            ClampActors();
            AdjustCamera();
            UpdateLight();
            RsStateManager.Push(new RsBloomTransitionState(this, 3, 1, 0.7f, 1));
            GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(3.14f, 0, 0);
        }
        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = RedStream.Graphics.batch;
            //batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            //batch.Draw(RedStream.Graphics.DisplacementMap, new Vector2(), Color.White);
            //batch.DrawString(font, "Intersects boxes: " + bInsideBox, new Vector2(0, 400), Color.White);
            //batch.End();
            /*batch.Begin();
            batch.DrawString(font, "FPS: " + RedStream.Game.frameRate, new Vector2(), Color.White);
            batch.DrawString(font, "Time Left: " + 30*GameInfo.PrizeBall.SmoothColor.F, new Vector2(5,50), Color.White);
            batch.DrawString(font, "Controller: " + GamePad.GetState(PlayerIndex.One).IsConnected, new Vector2(5,25), Color.White);
            //batch.DrawString(font, "Objects: " + RedStream.Game.Components.Count, new Vector2(0, 15), Color.White);
            //batch.DrawString(font, "Objects Drawn: " + RedStream.Graphics.ObjectsDrawn, new Vector2(0, 30), Color.White);
            //batch.DrawString(font, "Intersects: " + bInsideBox, new Vector2(0, 45), Color.White);
            //batch.DrawString(font, "Position: " + GameInfo.Kathy.Pos, new Vector2(0, 60), Color.White);
            batch.End();*/
        }
        public override void Input(GameTime time)
        {
            float seconds = (float)time.ElapsedGameTime.TotalSeconds;
            Vector3 camDir = RedStream.Scene.Camera.At - RedStream.Scene.Camera.Pos;
            camDir.Y = 0;
            camDir.Normalize();
            Vector3 camNorm = Vector3.Cross(camDir, Vector3.Up);
            RedStream.Input.ProcessInput();
//#if XBOX
                
            if (RsInput.Down(Buttons.RightThumbstickLeft))
                GameInfo.LightBall.Pos -= camNorm * seconds * lightSpeed * -RsInput.RightStick().X;
            if (RsInput.Down(Buttons.RightThumbstickRight))
                GameInfo.LightBall.Pos += camNorm * seconds * lightSpeed * RsInput.RightStick().X;
            if (RsInput.Down(Buttons.RightThumbstickUp))
                GameInfo.LightBall.Pos += camDir * seconds * lightSpeed * RsInput.RightStick().Y;
            if (RsInput.Down(Buttons.RightThumbstickDown))
                GameInfo.LightBall.Pos -= camDir * seconds * lightSpeed * -RsInput.RightStick().Y;
            if (RsInput.Tapped(Buttons.Y))
                bFirstPerson = !bFirstPerson;
            if (RsInput.Tapped(Buttons.LeftShoulder))
                GameInfo.LightBall.Explode();
            if (GameInfo.Kathy.StunnedCounter <= 0)
            {
                if (RsInput.Tapped(Buttons.RightShoulder))
                    GameInfo.Kathy.Jump();
                if (RsInput.Down(Buttons.LeftThumbstickUp))
                    GameInfo.Kathy.Pos += camDir * seconds * KathySpeed * Math.Abs(RsInput.LeftStick().Y);

                if (RsInput.Down(Buttons.LeftThumbstickDown))
                    GameInfo.Kathy.Pos -= camDir * seconds * KathySpeed * Math.Abs(RsInput.LeftStick().Y);
                if (RsInput.Down(Buttons.LeftThumbstickRight)){
                    if(bFirstPerson)
                    {
                        GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(-1 * seconds * RsInput.LeftStick().X, 0, 0);
                    }
                    else
                    {
                        GameInfo.Kathy.Pos += camNorm * seconds * KathySpeed * Math.Abs(RsInput.LeftStick().X);
                    }
                }
                if (RsInput.Down(Buttons.LeftThumbstickLeft))
                {
                    if(bFirstPerson)
                    {
                        GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(1 * seconds * -RsInput.LeftStick().X, 0, 0);
                    }
                    else
                    {
                        GameInfo.Kathy.Pos -= camNorm * seconds * KathySpeed * Math.Abs(RsInput.LeftStick().X);
                    }
                }
                if (RsInput.Down(Buttons.LeftTrigger))
                    GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(2 * seconds, 0, 0);
                if (RsInput.Down(Buttons.RightTrigger))
                    GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(-2 * seconds, 0, 0);
            }
//#else
            if (RsInput.Down(Keys.Left))
                GameInfo.LightBall.Pos -= camNorm * seconds * lightSpeed;
            if (RsInput.Down(Keys.Right))
                GameInfo.LightBall.Pos += camNorm * seconds * lightSpeed;
            if (RsInput.Down(Keys.Up))
                GameInfo.LightBall.Pos += camDir * seconds * lightSpeed;
            if (RsInput.Down(Keys.Down))
                GameInfo.LightBall.Pos -= camDir * seconds * lightSpeed;
            if (RsInput.Tapped(Keys.LeftShift))
                bFirstPerson = !bFirstPerson;
            if (RsInput.Tapped(Keys.LeftControl))
                GameInfo.LightBall.Explode();
            if (GameInfo.Kathy.StunnedCounter <= 0)
            {
                if (RsInput.Tapped(Keys.Space))
                    GameInfo.Kathy.Jump();
                if (RsInput.Down(Keys.W))
                    GameInfo.Kathy.Pos += camDir * seconds * KathySpeed;
                if (RsInput.Down(Keys.S))
                    GameInfo.Kathy.Pos -= camDir * seconds * KathySpeed;
                if (RsInput.Down(Keys.D))
                    GameInfo.Kathy.Pos += camNorm * seconds * KathySpeed;
                if (RsInput.Down(Keys.A))
                    GameInfo.Kathy.Pos -= camNorm * seconds * KathySpeed;
                if (RsInput.Down(Keys.Q))
                    GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(2 * seconds, 0, 0);
                if (RsInput.Down(Keys.E))
                    GameInfo.Kathy.Orientation *= Quaternion.CreateFromYawPitchRoll(-2 * seconds, 0, 0);
            }
            //debug function, don't map to xbox
            if (RsInput.Tapped(Keys.Enter))
                GameInfo.LightBall.SmoothAttenuation.V = new Vector3();
//#endif
            base.Input(time);
        }
        public override void Update(GameTime time)
        {
            base.Update(time);                        
            GameInfo.PrizeBall.Material.Color = new Vector4(GameInfo.PrizeBall.SmoothColor.F, 0, 0, GameInfo.PrizeBall.SmoothColor.F);
            if (!GameInfo.PrizeBall.SmoothColor.bActive && !GameInfo.Kathy.Owned)
            {
                GameInfo.Level = 1;
                GameInfo.BeginLevel();
                RsContent.PlayCue("Scream");
                GameInfo.Kathy.Owned= true;
                timeLeft = false;
            }

            RsPhysics.CollideObjects((float)time.ElapsedGameTime.TotalSeconds);
            ClampActors();
            AdjustCamera();
            UpdateLight();
        }
        public void UpdateLight()
        {
            GameInfo.LightBall.Velocity = new Vector3();
            GameInfo.LightBall.Acceleration = new Vector3();
            //GameInfo.Light.Pos = GameInfo.LightBall.Pos;
            //GameInfo.LightSphere.Pos = GameInfo.LightBall.Pos;

            //bInsideBox = false;
            foreach (Pit pit in GameInfo.Pits)
            {
                if (pit.box.Intersects(GameInfo.LightBall.BoundingSphereOnlyForAsenicsUse))
                {
                    //bInsideBox = true;
                    if (pit.Dweller != null && pit.Dweller.Mode == Dweller.ActingMode.None)
                        pit.Dweller.JumpAndShoot();
                }
            }
        }
        private void ClampActors()
        {
            IEnumerable<Actor> actors = from comp in RedStream.Game.Components
                                        where comp is Actor
                                        select (Actor)comp;
            foreach (Actor actor in actors)
            {
                float height = actor.Pos.Y;
                Vector3 newPos;
                if (actor.curDepth == 0.0 || actor.Pos.Y > actor.Radius)
                    newPos = Clamp(actor.Pos, actor.Radius, actor.curDepth);
                else newPos = Clamp(actor.Pos, actor.Radius, actor.hitPit.box);
                if (actor is Projectile)
                {
                    if (actor.Pos.X != newPos.X || actor.Pos.Z != newPos.Z)
                        actor.Die();
                }
                else
                {
                    actor.Pos = newPos;
                    if (actor.Pos.Y > height)
                        actor.Velocity = new Vector3(actor.Velocity.X, 0, actor.Velocity.Z);
                    if(!(actor is Prize))
                        actor.Acceleration = new Vector3(0, -500.0f, 0);
                    actor.curDepth = 0;
                    actor.hitPit = null;
                }
                foreach (Pit pit in GameInfo.Pits)
                    if (pit.box.Contains(actor.Pos) == ContainmentType.Contains)
                    {
                        actor.HitPit(pit);
                        if (actor.Pos.Y == actor.Radius - GameInfo.pitDepth && !actor.bLanded)
                            actor.LandInPit(pit);
                        break;
                    }
            }
        }
        private void LoadPits()
        {
            List<Pit> pits = GameInfo.Pits;
            pits.Clear();
            int nPits = GameInfo.Level * nPitsPerLevel, nDwellers = GameInfo.Level;
            Texture2D pixel = RedStream.Content.GetTexture("pixel");
            for (int i = 0; i < nPits; i++)
            {
                while (true)
                {
                    float width = RsUtil.GetRandomFloat(10, 20);
                    Vector3 pos = GetRandomPos(width+10);
                    pos.Y = 0;
                    BoundingBox box = new BoundingBox(new Vector3(-width, -GameInfo.pitDepth, -width) + pos, new Vector3(width, GameInfo.fieldSize.Y, width) + pos);
                    bool bCollides = false;
                    foreach (Pit pit in pits)
                        if (pit.box.Intersects(box))
                            bCollides = true;
                    if (!bCollides)
                    {
                        Pit pit = new Pit();
                        pit.box = box;
                        if (i < nDwellers)
                        {
                            Dweller dw = new Dweller(new RsActor.Desc("Dweller"), pit);
                            dw.Pos = pit.Center;
                            dw.Pos.Y = dw.Radius - GameInfo.pitDepth;
                        }
                        else pit.Dweller = null;
                        pits.Add(pit);
                        break;
                    }
                }
            }
            RedStream.Graphics.BuildDisplacementMap();
        }
        private Vector3 Clamp(Vector3 pos, float radius, float depth)
        {
            Vector3 min = new Vector3(radius) - new Vector3(GameInfo.fieldSize.X / 2, depth, GameInfo.fieldSize.Z / 2);
            Vector3 max = -new Vector3(radius) + new Vector3(GameInfo.fieldSize.X / 2, GameInfo.fieldSize.Y, GameInfo.fieldSize.Z / 2);
            Vector3 v = Vector3.Clamp(pos, min, max);
            return v;
        }
        private Vector3 Clamp(Vector3 pos, float radius, BoundingBox box)
        {
            Vector3 min = box.Min + new Vector3(radius);
            Vector3 max = box.Max - new Vector3(radius);
            Vector3 v = Vector3.Clamp(pos, min, max);
            return v;
        }
        private Vector3 GetRandomPos(float radius)
        {
            Vector3 min = new Vector3(radius) - new Vector3(GameInfo.fieldSize.X / 2, 0, GameInfo.fieldSize.Z / 2);
            Vector3 max = -new Vector3(radius) + new Vector3(GameInfo.fieldSize.X / 2, 0, GameInfo.fieldSize.Z / 2);
            return RsUtil.GetRandomVector(min, max);
        }
        private bool bFirstPerson = true;
        private void AdjustCamera()
        {
            Kathy kathy = GameInfo.Kathy;
            bool visible = cameraDistance.F != 0;
            kathy.Material.Lit = visible;
            kathy.Material.Colored = visible;
            RsCamera camera = RedStream.Scene.Camera;
            Vector3 dir = kathy.Forward;
            if (bFirstPerson)
            {
                cameraAngle.SafeSet(0);
                cameraDistance.SafeSet(0);
            }
            else if (kathy.hitPit != null)
            {
                cameraAngle.SafeSet(90);
                cameraDistance.SafeSet(thirdPersonDistance);
            }
            else
            {
                cameraAngle.SafeSet(thirdPersonAngle);
                cameraDistance.SafeSet(thirdPersonDistance);
            }
            Vector3 axis = Vector3.Cross(Vector3.Up, dir);
            axis.Normalize();
            Quaternion quat = Quaternion.CreateFromAxisAngle(axis, MathHelper.ToRadians(cameraAngle.F));
            Vector3 camDir = Vector3.Transform(dir, quat);
            camera.At = kathy.Pos + dir;
            camera.Pos = kathy.Pos - camDir * cameraDistance.F;
            if (!bFirstPerson)
                camera.Pos = Clamp(camera.Pos, 0.1f, 0);
        }
        private SmoothFloatComponent cameraAngle, cameraDistance;
        private float thirdPersonAngle=15, thirdPersonDistance=150;
    }
}
