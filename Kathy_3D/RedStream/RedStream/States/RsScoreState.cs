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
    public class RsScoreState : RsMainRenderState
    {
        int factories;
        int gain;
        public RsScoreState()
            : base()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        private SpriteFont font;
        private SmoothVectorComponent CameraPos, CameraUp;
        private static int prevTechLevel = 0;
        private static int prevRingCount = 1;
        private bool newTechLevel = false, newRingCount = false;
        private Color rageColorGreen = new Color(1, 1, 1),  rageColorRed = new Color(1, 1, 1);
        public override void EnterState()
        {
            newTechLevel = (prevTechLevel != RsGameInfo.TechLevel);
            prevTechLevel = RsGameInfo.TechLevel;

            newRingCount = (prevRingCount != RsGameInfo.Gyroscope.iLastRing);
            prevRingCount = RsGameInfo.Gyroscope.iLastRing;
  
            RsGameInfo.Gyroscope.Reset();
            CameraPos = new SmoothVectorComponent(RedStream.Scene.Camera.Pos, new Vector3(0, 100, 0), 2, 0);
            CameraUp = new SmoothVectorComponent(RedStream.Scene.Camera.Up, new Vector3(0, 0, -1), 2, 0);

            base.EnterState();
        }


        public int GetNumFactories()
        {
            IEnumerable<RsSocket> sockets = from socket in RsGameInfo.Planet.Sockets
                                            where socket.Building != null && socket.Building.Weapon.Attributes.Name == "Factory"
                                            select socket;
            return sockets.Count();
        }

        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = RedStream.Graphics.batch;
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Vector2 pos = new Vector2(20);
            
            batch.DrawString(font,"Ships Killed:" + RsGameInfo.ShipsKilled , pos , Color.White);
            factories = GetNumFactories();
            batch.DrawString(font, "Bonus from Factories(" + factories + "): +%" + factories*10, pos + new Vector2(0, 20), Color.White);
            gain= 100 * (RsGameInfo.ShipsKilled + (int)((float)(RsGameInfo.ShipsKilled) * (float)((float)factories/10f)));
                
            batch.DrawString(font, "Money earned " + gain, pos += new Vector2(0, 40), Color.White);

           
            batch.DrawString(font, "Score:" + RsGameInfo.Score, pos + new Vector2(0, 60), Color.White);
            Vector2 curPos = new Vector2(20, RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height - 30);

            batch.DrawString(font, "TechLevel:" + RsGameInfo.TechLevel, pos + new Vector2(0, 100), Color.White);


            

            if (newTechLevel)
            {
                
                
                for (int i = RsGameInfo.AllTowerData.Count-1; i >= 0; i--)
                {
                    DescDataPair<RsTowerData> towerData = RsGameInfo.AllTowerData[i];
                    if (towerData.Data.TechLevel == prevTechLevel)
                    {
                        batch.DrawString(font, "You've researched a new tower: " + towerData.Data.Name + "!", curPos, rageColorGreen);
                        curPos.Y -= 20;
                    }
                }
                batch.DrawString(font, "You've reached tech level " + RsGameInfo.TechLevel + "!", curPos, rageColorGreen);
                curPos.Y -= 20;
            }
            if (newRingCount)
            {
                batch.DrawString(font, "Your engineers built a new ring!", curPos, rageColorRed);
                curPos.Y -= 20;
            }

            batch.End();
        }
        double angle;
        public override void Update(GameTime time)
        {
            base.Update(time);
            RedStream.Scene.Camera.Pos = CameraPos.V;
            RedStream.Scene.Camera.Up = CameraUp.V;
            angle += ((float)time.ElapsedGameTime.Milliseconds/500f);

            rageColorGreen = new Color(0, 1 - MathHelper.Clamp((float)(Math.Cos(angle)), 0.0f, 0.7f), 0);
            rageColorRed = new Color( 1 - MathHelper.Clamp((float)(Math.Cos(angle)), 0.0f, 0.7f), 0, 0);
        }
        public override void Input(Microsoft.Xna.Framework.GameTime time)
        {
#if XBOX
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();

            if (RsInput.Tapped(Buttons.Start) || RsInput.Tapped(Buttons.A))
            {
                RsGameInfo.ShipsKilled = 0;
                RsGameInfo.Money += gain;
                RsStateManager.Push(new RsSelectionState());
            }
#else
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();

            if (RsInput.Tapped(Keys.Enter))
            {
                RsGameInfo.ShipsKilled = 0;
                RsGameInfo.Money += gain;
                RsStateManager.Push(new RsSelectionState());
            }
#endif
            base.Input(time);
        }




    }
}
