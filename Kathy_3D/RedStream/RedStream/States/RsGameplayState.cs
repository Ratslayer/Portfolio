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
    public class RsGameplayState : RsMainRenderState
    {
        RsIHud Hud = new RsGameplayHud();
        public RsGameplayState()
        {
            pixel = RedStream.Content.GetTexture("pixel");
        }
        public override void Draw()
        {
            base.Draw();
            Hud.Draw(RedStream.Graphics.batch);
            Rectangle viewportRect = new Rectangle(0, 0,
            RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
            RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
            DrawHealthBar(RedStream.Graphics.batch, new Vector2(viewportRect.Width/2-250, 0), new Vector2(500, 50));
        }
        public override void Input(GameTime time)
        {
            Hud.ProcessInput();
#if XBOX
            if (RsInput.Tapped(Buttons.LeftTrigger))
                RsGameInfo.Gyroscope.SelectNextRing();

            if (RsInput.Tapped(Buttons.RightTrigger))
                RsGameInfo.Gyroscope.SelectPreviousRing();

            if (RsInput.Down(Buttons.LeftThumbstickUp))
                RsGameInfo.Gyroscope.AlignRing(7.5f * RsInput.LeftStick().Y);

            if (RsInput.Down(Buttons.LeftThumbstickDown))
                RsGameInfo.Gyroscope.AlignRing(7.5f * RsInput.LeftStick().Y);
            if (RsInput.Down(Buttons.RightStick))
            {
                if (RsInput.Tapped(Buttons.A))
                {
                    RsStateManager.Push(new RsEndState());
                }
                if (RsInput.Tapped(Buttons.X))
                {
                    RsGameInfo.ShipsLeft = 0;
                    RsShipFactory.NextLevel();
                }
                if (RsInput.Tapped(Buttons.B))
                {
                    RsStateManager.Push(new RsGameOverState());
                }
            }
            if (RsInput.Tapped(Buttons.A))
                RsGameInfo.Gyroscope.ToggleMode();
            if (RsInput.Tapped(Buttons.Start))
                RsStateManager.Push(new RsPausedState());
            if (RsInput.Tapped(Buttons.Y))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(Buttons.LeftThumbstickLeft))
                RsGameInfo.Gyroscope.RotateRing(10 * RsInput.LeftStick().X);
            if (RsInput.Down(Buttons.LeftThumbstickRight))
                RsGameInfo.Gyroscope.RotateRing(10 * RsInput.LeftStick().X);
#else

            if (RsInput.Tapped(RsInput.KeyBindings.SelectNextRing))
                RsGameInfo.Gyroscope.SelectNextRing();

            if (RsInput.Tapped(RsInput.KeyBindings.SelectPreviousRing))
                RsGameInfo.Gyroscope.SelectPreviousRing();

            if (RsInput.Down(RsInput.KeyBindings.RotateRingRight))
                RsGameInfo.Gyroscope.AlignRing(3.0f);

            else if (RsInput.Down(RsInput.KeyBindings.RotateRingLeft))
                RsGameInfo.Gyroscope.AlignRing(-3.0f);

            if (RsInput.Tapped(RsInput.KeyBindings.ToggleGyro))
                RsGameInfo.Gyroscope.ToggleMode();
            if (RsInput.Tapped(Keys.P))
                RsStateManager.Push(new RsPausedState());
            if (RsInput.Tapped(Keys.H))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(Keys.A))
                RsGameInfo.Gyroscope.RotateRing(10);
            else if (RsInput.Down(Keys.D))
                RsGameInfo.Gyroscope.RotateRing(-10);
            if (RsInput.Down(Keys.K))
            {
                RsGameInfo.ShipsLeft = 0;
                RsShipFactory.NextLevel();
            }
            if (RsInput.Down(Keys.G))
            {
                RsStateManager.Push(new RsGameOverState());
            }
            if (RsInput.Tapped(Keys.U))
            {
                RsStateManager.Push(new RsEndState());
            }
#endif
            base.Input(time);
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
            RsPlanet planet = RsGameInfo.Planet;
            SmoothHealth.F = planet.Health;
            SmoothShield.F = planet.Shield.Health;

            if (planet.Health <= 0)
            {
                RsStateManager.Push(new RsGameOverState());
            }
            RsShipFactory.Update(time);
            RsPhysics.CollideObjects((float)time.ElapsedGameTime.TotalSeconds);
        }
        public override void EnterState()
        {
            if (RsGameInfo.iWave == 1) RsShipFactory.LoadWave(RsGameInfo.iWave);
            if (MediaPlayer.Queue.ActiveSong != RsGameInfo.ActionTheme)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(RsGameInfo.ActionTheme);
            }
            RsPlanet planet = RsGameInfo.Planet;
            SmoothHealth = new SmoothFloatComponent(planet.Health, planet.MaxHealth, 3, 0);
            SmoothShield = new SmoothFloatComponent(planet.Shield.Health, planet.Shield.Health, 3, 0);
            for (int i = 0; i < 3; i++)
            {
                RsGyro ring = RsGameInfo.Gyroscope.Rings[i];
                if (ring != null)
                {
                    ring.Material.Colored = true;
                    foreach (RsSocket socket in ring.Sockets)
                        if (socket.Building != null)
                            socket.Building.Material.Colored = true;
                }
            }
            planet.Material.Colored = true;
            foreach (RsSocket socket in planet.Sockets)
                if (socket.Building != null)
                    socket.Building.Material.Colored = true;
            base.EnterState();
        }
        public override void ExitState()
        {
            SmoothHealth.Delete();
            SmoothShield.Delete();
            base.ExitState();
        }
        private Texture2D pixel;
        private SmoothFloatComponent SmoothHealth, SmoothShield;
        public void DrawHealthBar(SpriteBatch batch, Vector2 pos, Vector2 size)
        {
            RsPlanet planet = RsGameInfo.Planet;
            Vector4 healthColor = new Vector4(1 - SmoothHealth.F / planet.MaxHealth, SmoothHealth.F / planet.MaxHealth, 0, 1) * 0.5f;
            healthColor.W = 1;
            float offset = 1 - Math.Max(healthColor.X, healthColor.Y);
            healthColor += new Vector4(offset, offset, 0, 0);
            float healthLength = (size.X - 6) * SmoothHealth.F / planet.MaxHealth, shieldLength = (size.X - 6) * SmoothShield.F / planet.Shield.MaxHealth;
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            DrawRectangle(batch, pos, size, new Vector4(1, 1, 1, 1));
            DrawRectangle(batch, pos + new Vector2(1), size - new Vector2(2), new Vector4(0, 0, 0, 1));
            DrawRectangle(batch, pos + new Vector2(2), size - new Vector2(4), new Vector4(1, 1, 1, 1));
            DrawRectangle(batch, pos + new Vector2(3), new Vector2(shieldLength, (size.Y - 6) / 2), new Vector4(0, 0, .5f, 1));
            DrawRectangle(batch, pos + new Vector2(3) + new Vector2(0, (size.Y - 6) / 2), new Vector2(healthLength, (size.Y - 6) / 2), healthColor);
            batch.End();
        }
        private void DrawRectangle(SpriteBatch batch, Vector2 pos, Vector2 size, Vector4 color)
        {
            batch.Draw(pixel, pos, null, new Color(color), 0, new Vector2(), size, SpriteEffects.None, 0.5f);
        }
    }
}
