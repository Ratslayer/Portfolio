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
    public class GameInfo
    {
        //public static RsLight Light;
        public static RsActor Terrain;
        public static LightBall LightBall;
        public static Prize PrizeBall;
        public static Kathy Kathy;
        public static Vector3 fieldSize, lastKathyLitPos;
        public static List<Pit> Pits=new List<Pit>();
        public static float pitDepth=30;
        public static Song song;
        public static int Level, nLevels = 5;
        public static void Init()
        {
            //RsStateManager.Push(new GameplayState());
            RsStateManager.Push(new MainMenuState());
            song = RedStream.Content.GetSong("Hissen Rai");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(song);
            RsScene.gameRunning = 1;
        }
        public static void BeginLevel()
        {
            RsState renderState = RsStateManager.Pop();
            if (Level > nLevels)
            {
                RsStateManager.Push(new EndState());
                RsStateManager.Push(new RsBloomTransitionState(renderState, -7, 1, 0, 1));
            }
            else
            {
                RsStateManager.Push(new GameplayState());
                RsStateManager.Push(new RsBloomTransitionState(renderState, -7, 1, 0, 1));
            }
        }
        public static void LoadContent()
        {
            RedStream.Game.Components.Clear();
            RedStream.Game.Components.Add(RedStream.Scene.ExplosionSystem);
            RedStream.Game.Components.Add(RedStream.Scene.SmokeSystem);
            fieldSize = new Vector3(300, 100, 500);
            RsCamera.Desc cdesc = new RsCamera.Desc();
            cdesc.Pos = new Vector3(0.0f, 10, 50.0f);
            cdesc.FarViewPlane = 1500;
            cdesc.AspectRatio = RedStream.Graphics.graphics.GraphicsDevice.Viewport.AspectRatio;
            RedStream.Scene.Camera = new RsCamera(cdesc);
            RsActor.Desc adesc = new RsActor.Desc("SkySphere");
            //new RsActor(adesc);
            adesc = new RsActor.Desc("LightBall");
            adesc.Pos = new Vector3(fieldSize.X, 25, fieldSize.Z);
            LightBall = new LightBall(adesc);
            adesc.Pos = new Vector3(-fieldSize.X, 35, -fieldSize.Z);
            PrizeBall = new Prize(adesc);
            //create the terrain
            CreateTerrain();
            //create the light
            Kathy = new Kathy(new RsActor.Desc("Kathy"));
            Kathy.Pos = new Vector3(fieldSize.X - 20, 10, fieldSize.Z - 20);

        }
        public static void CreateTerrain()
        {
            //create the terrain
            RsActor.Desc adesc = new RsActor.Desc("Terrain");
            adesc.Scale = new Vector3(fieldSize.X, 1, fieldSize.Z);
            Terrain = new Terrain(adesc);
            //create the walls
            adesc.Scale = new Vector3(fieldSize.X, 1, fieldSize.Y);
            adesc.Orientation = Quaternion.CreateFromYawPitchRoll(0, (float)Math.PI / 2.0f, 0);
            adesc.Pos = new Vector3(0, fieldSize.Y / 2, -fieldSize.Z / 2);
            new RsActor(adesc);
            adesc.Orientation = Quaternion.CreateFromYawPitchRoll(0, (float)Math.PI / -2.0f, 0);
            adesc.Pos = new Vector3(0, fieldSize.Y / 2, fieldSize.Z / 2);
            new RsActor(adesc);
            adesc.Scale = new Vector3(fieldSize.Y, 1, fieldSize.Z);
            adesc.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, (float)Math.PI / -2.0f);
            adesc.Pos = new Vector3(-fieldSize.X / 2, fieldSize.Y / 2, 0);
            new RsActor(adesc);
            adesc.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 2.0f);
            adesc.Pos = new Vector3(fieldSize.X / 2, fieldSize.Y / 2, 0);
            new RsActor(adesc);
        }
    }
}
