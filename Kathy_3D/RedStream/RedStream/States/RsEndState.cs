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
    public class RsEndState : RsMainRenderState
    {

        public RsEndState()
            : base()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        private SpriteFont font;
        private SmoothVectorComponent CameraPos, CameraUp;
        private Texture2D winnerIsYou;
        public SmoothFloatComponent SmoothAlpha;
        public override void EnterState()
        {

            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.1f, 0);
            RsGameInfo.Gyroscope.Reset();
            CameraPos = new SmoothVectorComponent(RedStream.Scene.Camera.Pos, new Vector3(0, 100, 0), 2, 0);
            CameraUp = new SmoothVectorComponent(RedStream.Scene.Camera.Up, new Vector3(0, 0, -1), 2, 0);
            winnerIsYou = RedStream.Content.GetTexture("a_winner_is_you");
            base.EnterState();
        }
        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = RedStream.Graphics.batch;
            RedStream.Graphics.DrawFullscreenQuad(winnerIsYou, new Vector4(1, 1, 1, SmoothAlpha.F));
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Vector2 pos = new Vector2(20);
            batch.DrawString(font, "A winner is you!", pos, Color.White);
            batch.DrawString(font, "Final Score:" + RsGameInfo.Score, pos + new Vector2(0, 40), Color.White);

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
#if XBOX
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();

            if (RsInput.Tapped(Buttons.Start) || RsInput.Tapped(Buttons.A))
                RsStateManager.Push(new RsOutroState());
#else
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();

            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsOutroState());
#endif
            base.Input(time);
        }




    }
}
