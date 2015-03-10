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
    class RsInput
    {
        static private MouseState CurrentMouseState, LastMouseState;
        static private KeyboardState CurrentKeyState, LastKeyState;
        static private GamePadState CurrentPadState, LastPadState;
        static private Vector2 deltaMouse, lastPos, curPos;
        static private Vector2 leftStickPos, rightStickPos;
        private const float ZOOM_STEP = 1.0f;
        public static RsKeyBindings KeyBindings { get; private set; }
        //debug data
        public bool bDrawScene = true;
        public RsInput()
        {   
        }
        public static void Init()
        {
            deltaMouse = new Vector2();
            lastPos = new Vector2();
            curPos = new Vector2();
            CurrentKeyState = LastKeyState = Keyboard.GetState();
            CurrentMouseState = LastMouseState = CurrentMouseState = Mouse.GetState();
            CurrentPadState = LastPadState = GamePad.GetState(PlayerIndex.One);
            KeyBindings = (RsKeyBindings)RedStream.Content.GetObjectAttributes("KeyBindings");
            leftStickPos = new Vector2();
            rightStickPos = new Vector2();
        }
        public static void UpdateInput()
        {
            LastKeyState = CurrentKeyState;
            LastMouseState = CurrentMouseState;
            LastPadState = CurrentPadState;
            CurrentKeyState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            CurrentPadState = GamePad.GetState(PlayerIndex.One);

            leftStickPos.X = CurrentPadState.ThumbSticks.Left.X;
            leftStickPos.Y = CurrentPadState.ThumbSticks.Left.Y;
            rightStickPos.X = CurrentPadState.ThumbSticks.Right.X;
            rightStickPos.Y = CurrentPadState.ThumbSticks.Right.Y;

            lastPos = curPos;
            curPos = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            deltaMouse = curPos - lastPos;
        }
        public void ProcessInput()
        {
            ProcessKeyboard();
            ProcessMouseMove();
#if XBOX
            if(RsScene.gameRunning == 0)
                ProcessGamePadMove();
#endif
            ProcessMouseClick();
        }

        public void ProcessGamePadMove()
        {
            if (Down(Buttons.RightThumbstickDown) || Down(Buttons.RightThumbstickUp) ||
                Down(Buttons.RightThumbstickLeft) || Down(Buttons.RightThumbstickRight))
            {
                RedStream.Scene.Camera.Orbit(rightStickPos.X, rightStickPos.Y);
            }
            if (Down(Buttons.RightShoulder))
            {
                RedStream.Scene.Camera.Zoom = RedStream.Scene.Camera.Zoom + ZOOM_STEP;
            }
            else if (Down(Buttons.LeftShoulder))
            {
                RedStream.Scene.Camera.Zoom = RedStream.Scene.Camera.Zoom - ZOOM_STEP;
            }
        }
        public void ProcessMouseMove()
        {
            if ((ButtonState)typeof(MouseState).GetProperty(KeyBindings.CameraMove).GetValue(CurrentMouseState, null) == ButtonState.Pressed)
            {
                if (deltaMouse != Vector2.Zero)
                {
                    RedStream.Scene.Camera.Orbit(deltaMouse.X, deltaMouse.Y);
                }
            }

            if ((ButtonState)typeof(MouseState).GetProperty(KeyBindings.CameraZoom).GetValue(CurrentMouseState, null) == ButtonState.Pressed)
            {
                if (deltaMouse != Vector2.Zero)
                {
                    RedStream.Scene.Camera.Zoom = RedStream.Scene.Camera.Zoom + (float)deltaMouse.Y;
                }
            }
        }
        public void ProcessMouseClick()
        {

        }
        public void ProcessKeyboard()
        {
            if (Down(KeyBindings.ExitKey) || Down(Buttons.Back))
                RedStream.Game.Exit();
            
            if (Down(KeyBindings.SetWireframe))
            {
                RedStream.Graphics.mode = RsGraphics.Mode.Wireframe;
                RedStream.Graphics.FinalMap = RedStream.Graphics.DiffuseMap;
            }
            /*if (Tapped(Keys.W))
            {
                RsGameplayState state = (RsGameplayState) RsStateManager.Top();
                RsStateManager.Push(new RsBloomTransitionState(state, 1.0f, 1f, 0.7f, 5f));
                RsStateManager.Push(new RsBloomTransitionState(state, 10.0f, 0.5f, 0.0f, 1f));
            }*/
            if (Tapped(KeyBindings.VisualizeBounds))
                RedStream.Graphics.bVisualizeBounds = !RedStream.Graphics.bVisualizeBounds;
            
            if (Down(KeyBindings.IncreaseBloomFactor))
                RedStream.Graphics.BloomFactor += 1.0f / 60.0f;
            
            if (Down(KeyBindings.DecreaseBloomFactor))
                RedStream.Graphics.BloomFactor -= 1.0f / 60.0f;
            
            if (Down(KeyBindings.SetDiffuseMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.DiffuseMap;
                RedStream.Graphics.mode = RsGraphics.Mode.Color;
            }
            //if (Tapped(Keys.S))
            //    RsGameInfo.Gyroscope.AddRing();
            if (Down(KeyBindings.SetLightMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.LightMap[1];
                RedStream.Graphics.mode = RsGraphics.Mode.Lit;
            }

            if (Down(KeyBindings.SetSpecularMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.SpecularMap[1];
                RedStream.Graphics.mode = RsGraphics.Mode.Lit;
            }

            if (Down(KeyBindings.SetDeferredMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.DeferredMap;
                RedStream.Graphics.mode = RsGraphics.Mode.Lit;
            }

            if (Down(KeyBindings.SetLightPassedMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.LightPassedMap;
                RedStream.Graphics.mode = RsGraphics.Mode.Full;
            }

            if (Down(KeyBindings.SetDownsampledMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.DownsampledMap[3];
                RedStream.Graphics.mode = RsGraphics.Mode.Full;
            }

            if (Down(KeyBindings.SetBlurredMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.BlurredMap[1];
                RedStream.Graphics.mode = RsGraphics.Mode.Full;
            }

            if (Down(KeyBindings.SetBloomedMap))
            {
                RedStream.Graphics.FinalMap = RedStream.Graphics.BloomedMap;
                RedStream.Graphics.mode = RsGraphics.Mode.Full;
            }            
            if (Tapped(KeyBindings.DrawScene))
                bDrawScene = !bDrawScene;

            //if (Tapped(Keys.M))
           //     RsStateManager.Pop();
            

        }
        public static bool Down(Keys key)
        {
            return CurrentKeyState.IsKeyDown(key);
        }
        public static bool Down(Buttons button)
        {
            return CurrentPadState.IsButtonDown(button);
        }
        public static bool Tapped(Keys key)
        {
            return CurrentKeyState.IsKeyDown(key) && !LastKeyState.IsKeyDown(key);
        }
        public static bool Tapped(Buttons button)
        {
            return CurrentPadState.IsButtonDown(button) && !LastPadState.IsButtonDown(button);
        }

        public static Vector2 LeftStick()
        {
            return leftStickPos;
        }
        public static Vector2 RightStick()
        {
            return rightStickPos;
        }
    }
}
