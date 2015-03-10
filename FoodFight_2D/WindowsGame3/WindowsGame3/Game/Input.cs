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
    public class Input
    {
        private KeyboardState CurKeys, PrevKeys;
        private MouseState CurMouse, PrevMouse;
        public Input()
        {
            UpdateStates();
        }
        public void ProcessInput(GameTime time)
        {
            UpdateStates();
            ProcessKeys(time);
            ProcessMouse(time);
        }
        private void ProcessKeys(GameTime time)
        {
            Game1 game=Program.game;
            RenderManager rman=game.rMan;
            if (CurKeys.IsKeyDown(Keys.Escape))
                game.Exit();
            if (CurKeys.IsKeyDown(Keys.OemPlus))
                rman.bloomFactor += 0.1f;
            if (CurKeys.IsKeyDown(Keys.OemMinus))
                rman.bloomFactor -= 0.1f;
            if (CurKeys.IsKeyDown(Keys.Space))
                rman.finalTarget = rman.bloomTarget[1];
            if (CurKeys.IsKeyUp(Keys.LeftControl))
                rman.finalTarget = rman.bloomTarget[0];
            if (CurKeys.IsKeyDown(Keys.Space))
                Level.Player.Shoot(GetInputDir());
            else Level.Player.bShooting = false;
            if (CurKeys.IsKeyDown(Keys.Tab))
                game.bPaused = true;
            if(!Level.Player.IsPushedBack())
                Level.Player.vel = GetInputDir() * Level.Player.movSpeed;
        }
        private void ProcessMouse(GameTime time)
        {
        }
        private void UpdateStates()
        {
            PrevKeys = CurKeys;
            PrevMouse=CurMouse;
            CurKeys = Keyboard.GetState();
            CurMouse = Mouse.GetState();
        }
        private Vector2 GetInputDir()
        {
            Vector2 dir=new Vector2();
            if (CurKeys.IsKeyDown(Keys.Left))
                dir.X = -1.0f;
            if (CurKeys.IsKeyDown(Keys.Right))
                dir.X = 1.0f;
            if (CurKeys.IsKeyDown(Keys.Up))
                dir.Y = -1.0f;
            if (CurKeys.IsKeyDown(Keys.Down))
                dir.Y = 1.0f;
            return dir;
        }
    }
}
