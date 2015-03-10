using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public static class RsGameInfo
    {
        public static float MusicVolume = 1.0f;
        public static int iWave = 1;
        public static int FinalWave = 10;
        public static int TechLevel = 1;
        public static int ShipsLeft;
        public static int ShipsKilled = 0;
        public static float Money = 0, Score = 0;
        public static RsGyroscope Gyroscope = null;
        public static RsPlanet Planet = null;
        public static List<DescDataPair<RsTowerData>> TowerData = new List<DescDataPair<RsTowerData>>();
        public static List<DescDataPair<RsTowerData>> AllTowerData = new List<DescDataPair<RsTowerData>>();
        public static Song MenuTheme, ActionTheme, BossTheme1;
        public static void Init()
        {
            RsStateManager.Push(new RsMainMenuState());
            /* Play our kickass song */
            MenuTheme = RedStream.Content.GetSong("song2");
            ActionTheme = RedStream.Content.GetSong("song1");
            BossTheme1 = RedStream.Content.GetSong("Radiant Radiant Symphony");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MusicVolume;
            MediaPlayer.Play(MenuTheme);
          
            //LoadContent();
            //add tower descs
#if XBOX
            foreach (string filename in Directory.GetFiles(RedStream.Game.Content.RootDirectory + "\\RsDatas\\Buildings"))
            {
                /* Now add this file */
                RsGameInfo.AllTowerData.Add(new DescDataPair<RsTowerData>("Buildings\\" + Path.GetFileNameWithoutExtension(filename)));
            }
#else
            foreach (string filename in Directory.EnumerateFiles(RedStream.Game.Content.RootDirectory + "\\RsDatas\\Buildings"))
            {
                /* Now add this file */
                RsGameInfo.AllTowerData.Add(new DescDataPair<RsTowerData>("Buildings\\" + Path.GetFileNameWithoutExtension(filename)));
            }
#endif
            //RsGameInfo.TowerData.Add(new DescDataPair<RsTowerData>("Buildings\\ChainLightningTower"));
        }

        public static void updateTowerList()
        {
            TowerData.Clear();
            for (int i = 0; i < RsGameInfo.AllTowerData.Count; i++)
            {
                DescDataPair<RsTowerData> towerData = RsGameInfo.AllTowerData[i];
                if (towerData.Data.TechLevel <= TechLevel)
                {
                    TowerData.Add(towerData);
                }
            }
        }
        public static void LoadContent()
        {
            RedStream.Game.Components.Clear();
            RsCamera.Desc cdesc = new RsCamera.Desc();
            cdesc.Pos = new Vector3(0.0f, 0, 100.0f);
            cdesc.FarViewPlane = 1500;
            cdesc.AspectRatio = RedStream.Graphics.graphics.GraphicsDevice.Viewport.AspectRatio;
            RedStream.Scene.Camera = new RsCamera(cdesc);
            //gyroscope
            DescDataPair<RsGyroscopeData> gyroPair = new DescDataPair<RsGyroscopeData>("World\\Gyroscope");
            Gyroscope = new RsGyroscope(gyroPair.Desc, gyroPair.Data);

            //planet
            DescDataPair<RsPlanetData> pair = new DescDataPair<RsPlanetData>("Planets\\MainPlanet");
            pair.Desc.Pos = Vector3.Zero;
            Planet = new RsPlanet(pair.Desc, pair.Data);

            //skysphere
            RsActor.Desc pdesc = new RsActor.Desc("SkySphere");
            new RsActor(pdesc);

            //the spot light
            RsLight.Desc ldesc = new RsLight.Desc("Spotlight");
            ldesc.Pos = new Vector3(0, 100, 0);
            new RsLight(ldesc);
            iWave = 1;
            Money = 1500;
            TechLevel = 1;
        }
    }
}
