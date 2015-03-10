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
    public abstract class RsState
    {
        public bool bInitialized = false;
        public virtual void EnterState()
        {
            bInitialized = true;
        }
        public virtual void ExitState()
        {
        }
        public virtual void Input(GameTime time)
        {
        }
        public abstract void Draw();
        public virtual void Update(GameTime time)
        {
            if (!bInitialized)
                EnterState();
            bHasLoadedMatrices = false;
        }
        //helper function that converts 3D coordinates to 2D
        public static Vector2 Project(Vector3 v)
        {
            if (!bHasLoadedMatrices)
            {
                LoadMatrices();
                bHasLoadedMatrices = true;
            }
            Vector3 newV = Viewport.Project(v, Proj, View, World);
            return new Vector2(newV.X, newV.Y);
        }
        #region Private
        private static Matrix View, Proj, World;
        private static Viewport Viewport;
        private static bool bHasLoadedMatrices = false;
        private static void LoadMatrices()
        {
            Viewport = RedStream.Graphics.graphics.GraphicsDevice.Viewport;
            View = RedStream.Scene.Camera.View;
            Proj = RedStream.Scene.Camera.Proj;
            World = Matrix.Identity;
        }
        #endregion
    }
}
