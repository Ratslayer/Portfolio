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
    public class RsSelectionState : RsMainRenderState
    {
        
        public RsSelectionState()
            : base()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        private IEnumerable<DescDataPair<RsTowerData>> TowerData;
        private SpriteFont font;
        private SmoothVectorComponent CameraPos, CameraUp;
        private int iSocket = 0, iRing = 0, iTowerData = 0;
        private bool PlanetFull = false;
        private RsSocket currentSocket;
        private Texture2D circle;
        public override void EnterState()
        {
            circle = RedStream.Content.GetTexture("CircleLogoNoWords");
            RsGameInfo.Gyroscope.Reset();
            RsGameInfo.updateTowerList();
            CameraPos = new SmoothVectorComponent(RedStream.Scene.Camera.Pos, new Vector3(0, 100, 0), 2, 0);
            CameraUp = new SmoothVectorComponent(RedStream.Scene.Camera.Up, new Vector3(0, 0, -1), 2, 0);
            /*if (RsGameInfo.TowerData[0].Data.TowerType == RsTowerData.RsTowerType.Defensive)
                iRing = 0;
            else iRing = 1;*/
            iRing = 0;
            SetDeffensiveTowerData();
            UpdateRings();
            UpdateSocket(0, iRing);
            base.EnterState();
        }
        private void SetDeffensiveTowerData()
        {
            TowerData = from data in RsGameInfo.TowerData
                        where data.Data.TowerType == RsTowerData.RsTowerType.Defensive
                        select data;
            iTowerData = 0;
        }
        private void SetOffensiveTowerData()
        {
            TowerData = from data in RsGameInfo.TowerData
                        where data.Data.TowerType == RsTowerData.RsTowerType.Offensive
                        select data;
            iTowerData = 0;
        }
        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = RedStream.Graphics.batch;
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (iRing != 0)
                batch.Draw(circle, RsState.Project(currentSocket.LocalPos), null, Color.Red, 0, new Vector2(circle.Width, circle.Height) * 0.5f, 0.2f, SpriteEffects.None, 0);
            Vector2 pos = new Vector2(20);
            batch.DrawString(font, "Moneyz: " + RsGameInfo.Money, pos, Color.White);
            batch.DrawString(font, "Towers", pos += new Vector2(0,20), Color.White);
            batch.DrawString(font, "Cost", pos + new Vector2(225, 0), Color.White);
            for (int i = 0; i < TowerData.Count(); i++)
            {
                DescDataPair<RsTowerData> towerData = TowerData.ElementAt(i);
                Color color;
                if (i == iTowerData)
                    color = Color.Red;
                else color = Color.White;
                batch.DrawString(font, towerData.Data.Name, pos+=new Vector2(0, 15), color);
                batch.DrawString(font, ""+towerData.Data.Cost, pos+new Vector2(225, 0), color);
            }
            RsTowerData Building = TowerData.ElementAt(iTowerData).Data;
            Vector2 wepPos = new Vector2(20, 320);
            RsUpgradeData upgrade = (RsUpgradeData)RedStream.Content.GetObjectAttributes("\\Upgrades\\" + Building.InitialUpgrade);
            RsWeaponData weapon = (RsWeaponData)RedStream.Content.GetObjectAttributes("\\Weapons\\" + upgrade.Weapon);
            batch.DrawString(font, ":" + weapon.Name, wepPos, Color.Wheat); wepPos.Y += 20;
            batch.DrawString(font, ":========", wepPos, Color.Wheat); wepPos.Y += 20;
            batch.DrawString(font, ":Weapon>" + weapon.Description, wepPos, Color.Wheat); wepPos.Y += 20;
            batch.DrawString(font, ":Damage>" + weapon.DamageDealt, wepPos, Color.Wheat); wepPos.Y += 20;
            batch.DrawString(font, ":RateOfFire>" + weapon.RateOfFire, wepPos, Color.Wheat); wepPos.Y += 20;
            batch.DrawString(font, ":Range>" + weapon.Range, wepPos, Color.Wheat); wepPos.Y += 20;

            if (currentSocket != null)
            {
                RsTower Selected = currentSocket.Building;
                if (Selected == null)
                {
                    batch.DrawString(font, "<empty socket>", new Vector2(RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width / 2 - 60, RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height - 30), Color.GreenYellow);
                }
                else
                {
                    batch.DrawString(font, "<" + Selected.Weapon.Attributes.Name + ">", new Vector2(RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width / 2 - 60, RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height - 30), Color.Red);
                }
            }
            else batch.DrawString(font, "<Planet Full>", new Vector2(RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width / 2 - 60, RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height - 30), Color.Red);
            batch.End();
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
            RedStream.Scene.Camera.Pos = CameraPos.V;
            RedStream.Scene.Camera.Up = CameraUp.V;
        }
        public override void Input(Microsoft.Xna.Framework.GameTime time)
        {
            // FIXME: Alex will improve RsInput for code reuse 
#if XBOX
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
            //if (RsInput.Tapped(true, RsInput.ThumbDirection.Down))
            if (RsInput.Tapped(Buttons.LeftThumbstickDown))
                NextRing();
            if (RsInput.Tapped(Buttons.LeftThumbstickUp))
                PreviousRing();
            if (iRing != 3)
            {
                if (RsInput.Tapped(Buttons.LeftThumbstickLeft) || RsInput.Tapped(Buttons.DPadLeft))
                    NextSocket();
                if (RsInput.Tapped(Buttons.LeftThumbstickRight) || RsInput.Tapped(Buttons.DPadRight))
                    PreviousSocket();
            }
            if (RsInput.Tapped(Buttons.A))
                ToggleBuilding();
            if (RsInput.Tapped(Buttons.Start))
                RsStateManager.Push(new RsGameplayState());
            if (RsInput.Tapped(Buttons.DPadDown))
                NextBuilding();
            if (RsInput.Tapped(Buttons.DPadUp))
                PreviousBuilding();
#else

            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            if (RsInput.Tapped(Keys.Down))
                NextRing();
            if (RsInput.Tapped(Keys.Up))
                PreviousRing();
            if (iRing != 0)
            {
                if (RsInput.Tapped(Keys.Left))
                    NextSocket();
                if (RsInput.Tapped(Keys.Right))
                    PreviousSocket();
            }
            if (RsInput.Tapped(Keys.Space))
                ToggleBuilding();
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsGameplayState());
            if (RsInput.Tapped(Keys.OemPeriod))
                NextBuilding();
            if (RsInput.Tapped(Keys.OemComma))
                PreviousBuilding();
#endif
            base.Input(time);
        }
        private void NextRing()
        {
            /* If the currently selected tower is a defensive building, only the planet can be selected */
            //if (TowerData.ElementAt(iTowerData).Data.TowerType == RsTowerData.RsTowerType.Defensive) 
            //    return;

            /* And if this is an offensive tower, only allow rings 0, 1, 2 */
            int prevRing = iRing;
            iRing = RsUtil.Mod(++iRing, RsGameInfo.Gyroscope.iLastRing+1);
            UpdateRings(); // FIXME: Alex will make iRing a property so this can happen automagically */
            UpdateSocket(prevRing, iRing); // FIXME: Alex will make iRing a property so this can happen automagically */
        }
        private void PreviousRing()
        {
            /* If the currently selected tower is a defensive building, only the planet can be selected */
            if (RsGameInfo.TowerData[iTowerData].Data.TowerType == RsTowerData.RsTowerType.Defensive) return;

            int prevRing = iRing;
            iRing = RsUtil.Mod(--iRing, RsGameInfo.Gyroscope.iLastRing + 1);
            UpdateRings(); // FIXME: Alex will make iRing a property so this can happen automagically */
            UpdateSocket(prevRing, iRing); // FIXME: Alex will make iRing a property so this can happen automagically */
        }
        private void UpdateRings()
        {
            RsGyroscope gs = RsGameInfo.Gyroscope;
            for (int i = 1; i < 4; i++)
            {
                if (gs.Rings[i-1] != null)
                {
                    if (i == iRing)
                        gs.Rings[i-1].Material.Colored = true;
                    else gs.Rings[i-1].Material.Colored = false;
                }
            }
            if (iRing == 0)
                RsGameInfo.Planet.Material.Colored = true;
            else RsGameInfo.Planet.Material.Colored = false;
            for (int i = 0; i < 4; i++)
            {
                List<RsSocket> sockets = GetSockets(i);
                if (sockets != null)
                {
                    bool Colored = i == iRing;
                    foreach (RsSocket socket in sockets)
                        if (socket.Building != null)
                            socket.Building.Material.Colored = Colored;
                }
            }
        }
        private void UpdateSocket(int iFrom, int iTo)
        {
            List<RsSocket> lFrom = GetSockets(iFrom), lTo = GetSockets(iTo);
            //if just switched to a gyro reset the socket
            if (iFrom == 0)
            {
                iSocket = 0;
                SetOffensiveTowerData();
                currentSocket = lTo[iSocket];
            }
            //if just switched to a planet get the first socket available
            if (iTo == 0)
            {
                SetDeffensiveTowerData();
                for (int i = 0; i < lTo.Count; i++)
                {
                    currentSocket = lTo[i];
                    iSocket = i;
                    if (currentSocket.Building == null)
                    {
                        PlanetFull = false;
                        return;
                    }
                }
                //if none exist declare the planet full
                PlanetFull = true;
                currentSocket = null;
            }
            //if swapped from ring to ring
            else
            {
                //get the closest socket on the other ring
                iSocket = (int)(((float)iSocket / (float)lFrom.Count) * lTo.Count);
                currentSocket = lTo[iSocket];
            }
        }
        private List<RsSocket> GetSockets(int ir)
        {
            if (ir >= 1 && ir < RsGameInfo.Gyroscope.iLastRing+1)
            {
                RsGyro gyro = RsGameInfo.Gyroscope.Rings[ir-1];
                if (gyro != null)
                    return gyro.Sockets;
                else return null;
            }
            else if (ir == 0)
                return RsGameInfo.Planet.Sockets;
            else return null;
        }
        private void NextSocket()
        {
            List<RsSocket> sockets = GetSockets(iRing);
            if (iRing == 0)
            {
                for (int i = 0; i < sockets.Count; i++)
                {
                    currentSocket = sockets[i];
                    iSocket = i;
                    if (currentSocket.Building == null)
                    {
                        PlanetFull = false;
                        return;
                    }
                }
                //if none exist declare the planet full
                PlanetFull = true;
                currentSocket = null;
            }
            else
            {
                iSocket=RsUtil.Mod(++iSocket, sockets.Count);
                currentSocket = sockets[iSocket];
            }
        }
        private void PreviousSocket()
        {
            if (iRing != 0)
            {
                List<RsSocket> sockets = GetSockets(iRing);
                iSocket = RsUtil.Mod(--iSocket, sockets.Count);
                currentSocket = sockets[iSocket];
            }
        }
        private void ToggleBuilding()
        {
            if (iRing == 0 && PlanetFull)
                return;
            if (currentSocket.Building == null)
            {
                DescDataPair<RsTowerData> towerData = TowerData.ElementAt(iTowerData);
                if (RsGameInfo.Money >= towerData.Data.Cost)
                {
                    currentSocket.AttachBuilding(new RsTower(towerData.Desc, currentSocket, towerData.Data));
                    RsGameInfo.Money -= towerData.Data.Cost;
                }
                if (iRing == 0)
                    NextSocket();
            }
            else
            {
                RsTower building = ((RsTower)currentSocket.Building);
                RsGameInfo.Money += building.Attributes.Cost;
                building.Die();
                currentSocket.AttachBuilding(null);
            }
        }
        private void NextBuilding()
        {
            /* Change to a new tower */
            iTowerData = RsUtil.Mod(++iTowerData, TowerData.Count());

            // FIXME: This whole thing will go in a iTowerdata property
            /* If the currently selected tower is a defensive building, only the planet can be selected */
            /*if (RsGameInfo.TowerData[iTowerData].Data.TowerType == RsTowerData.RsTowerType.Defensive)
            {
                int prevRing = iRing;
                iRing = 0; // FIXME: Alex will put a constant here
                UpdateRings(); // FIXME: Alex will make iRing a property so this can happen automagically 
                UpdateSocket(prevRing, iRing); // FIXME: Alex will make iRing a property so this can happen automagically 
            }
            else if (iRing == 0)
            {
                iRing = RsGameInfo.Gyroscope.iLastRing;
                UpdateRings();
                UpdateSocket(0, iRing);
            }*/
        }
        private void PreviousBuilding()
        {
            iTowerData = RsUtil.Mod(--iTowerData, TowerData.Count());

            // FIXME: This whole thing will go in a iTowerdata property
            /* If the currently selected tower is a defensive building, only the planet can be selected */
            /*if (RsGameInfo.TowerData[iTowerData].Data.TowerType == RsTowerData.RsTowerType.Defensive)
            {
                int prevRing = iRing;
                iRing = 0; // FIXME: Alex will put a constant here
                UpdateRings(); // FIXME: Alex will make iRing a property so this can happen automagically 
                UpdateSocket(prevRing, iRing); // FIXME: Alex will make iRing a property so this can happen automagically 
            }
            else if (iRing == 0)
            {
                iRing = RsGameInfo.Gyroscope.iLastRing;
                UpdateRings();
                UpdateSocket(0, iRing);
            }*/
        }
    }
}
