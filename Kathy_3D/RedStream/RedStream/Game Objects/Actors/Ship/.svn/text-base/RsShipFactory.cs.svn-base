using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RedStream
{
    public static class RsShipFactory
    {

        static RsWaveData WaveData;
        static TimeSpan Timer = TimeSpan.Zero;
        const int shipTypes = 3;
        static int[] ships = new int[shipTypes];
        static bool BossOut;
        public static bool BossDead;

        public static void LoadWave(int waveNum)
        {
            LoadWave("Wave" + waveNum);
        }

        public static void LoadWave(string waveName)
        {
            try
            {
                WaveData = (RsWaveData)RedStream.Content.GetObjectAttributes("Waves\\"  + waveName);
            }
            catch (ContentLoadException)
            {
                RsGameInfo.iWave = 1;
                WaveData = (RsWaveData)RedStream.Content.GetObjectAttributes("Waves\\Wave1");
            }
            RsGameInfo.TechLevel = WaveData.TechLevel;
            if (RsGameInfo.Gyroscope.iLastRing < WaveData.RingCount)
            {
                RsGameInfo.Gyroscope.AddRing();
            }
            
            for (int i = 0; i < shipTypes; i++ )
            {
                RsGameInfo.ShipsLeft += WaveData.ShipCount[i];

                ships[i] = 0;
            }

            BossOut = false;

            if (WaveData.Boss == "None")
                BossDead = true;
            else
                BossDead = false;
            
        }


        public static void Update(GameTime gameTime)
        {
            
            Timer -= gameTime.ElapsedGameTime;
            if (Timer <= TimeSpan.Zero)
            {
                if (!BossOut)
                {
                    for (int i = 0; i < shipTypes; i++)
                    {
                        if (ships[i] < WaveData.ShipCount[i])
                        {
                            CreateShip("TestShip" + i);
                            ships[i]++;
                            break;
                        }

                        if (RsGameInfo.ShipsLeft <= 0)
                        {
                            RsGameInfo.ShipsLeft = 0;
                            BossOut = true;
                            if(!BossDead) CreateBoss();
                            break;
                        }
                    }
                }
                else
                {
                    if (RsGameInfo.iWave == 10)
                    {
                        CreateShip("TestShip1");
                    }

                    if (BossDead)
                    {
                        if (RsGameInfo.iWave == RsGameInfo.FinalWave)
                        {
                            RsStateManager.Push(new RsEndState());
                        }
                        else
                        {
                            NextLevel();
                        }

                    }
                        
                }
                //if (shipType0 < WaveData.ShipCount[0])
                //{
                //    CreateShip("" + (int)RsShipData.Mode.Straight);
                //    shipType0++;
                //}
                //else if (shipType1 < WaveData.ShipCount[1])
                //{
                //    CreateShip("" + (int)RsShipData.Mode.Orbit);
                //    shipType1++;
                //}

                //else if (shipType2 < WaveData.ShipCount[2])
                //{
                //    CreateShip("" + (int)RsShipData.Mode.HitAndRun);
                //    shipType2++;
                //}

                    Timer = TimeSpan.FromSeconds(WaveData.SpawnRate);
            }

        }

        private static void CreateBoss()
        {
            DescDataPair<RsShipData> pair = new DescDataPair<RsShipData>("Ships\\" + WaveData.Boss);
           // pair.Data.InitialUpgrade = WaveData.ShipUpgrade;
            
            if (RsGameInfo.iWave == 10)
            {
                pair.Desc.Scale *= 5f;
            }
            else pair.Desc.Scale *= 3f;

            RsBoss ship = new RsBoss(pair.Desc, pair.Data);
 
            float minDistance = RsGameInfo.Gyroscope.Rings[RsGameInfo.Gyroscope.iLastRing - 1].Radius + (ship.Radius*0.5f) + 5;
            float maxDistance = minDistance + 10;
            //the ships don't collide with the gyros on spawn
            ship.Pos = RsUtil.GetRandomVector(minDistance, maxDistance);
            ship.FacePoint(Vector3.Zero);
            ship.OrbitAxis = ship.Up;
            ship.SafeStraight();
            MediaPlayer.Stop();
            MediaPlayer.Play(RsGameInfo.BossTheme1);
          

        }

        public static void CreateShip(string shipName)
        {
            /* Read the graphic attributes and the ship attributes */
            DescDataPair<RsShipData> pair = new DescDataPair<RsShipData>("Ships\\" + shipName);
            pair.Data.InitialUpgrade = WaveData.ShipUpgrade;

            /* And now we can create the ship object based on the description */
            RsShip ship = new RsShip(pair.Desc, pair.Data);
            float minDistance = RsGameInfo.Gyroscope.Rings[RsGameInfo.Gyroscope.iLastRing - 1].Radius+ship.Radius+15;
            float maxDistance = minDistance + 25;
            //the ships don't collide with the gyros on spawn
            ship.Pos = RsUtil.GetRandomVector(minDistance, maxDistance);
            ship.FacePoint(Vector3.Zero);
            ship.OrbitAxis = ship.Up;
            ship.SafeStraight();
        }

        public static void NextLevel()
        {
            
            RsStateManager.Push(new RsScoreState());
            ++RsGameInfo.iWave;
            RsShipFactory.LoadWave(RsGameInfo.iWave);
        }
    }
}
