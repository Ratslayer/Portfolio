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
    /// <summary>
    /// The class that is responsible for all the drawing done in the game.
    /// </summary>
    public class RenderManager
    {
        public RenderTarget2D colorTarget;
        public RenderTarget2D finalTarget;
        public RenderTarget2D[] bloomTarget, downsampleTarget;
        List<GameObject> objects;
        public SpriteBatch batch;
        public Effect colorEffect, downsampleEffect, bloomEffect, gaussianBlurEffect, waveEffect;
        GraphicsDeviceManager graphics;
        public int targetWidth, targetHeight;
        public float bloomFactor = 1;
        public Vector2 WaveCenter;
        public float WaveRadius, WaveWidth, WaveSpeed;
        public RenderManager(GraphicsDeviceManager graphics, List<GameObject> objects, int width, int height)
        {
            this.graphics = graphics;
            this.objects=objects;
            batch = new SpriteBatch(graphics.GraphicsDevice);
            WaveRadius = 0;
            WaveWidth = 0.1f;
            WaveCenter = new Vector2(width, height)/2;
            targetHeight = height;
            targetWidth = width;
            colorTarget = new RenderTarget2D(graphics.GraphicsDevice, width, height);
            bloomTarget = new RenderTarget2D[2];
            downsampleTarget = new RenderTarget2D[4];
            for(int i=0;i<2;i++)
                bloomTarget[i] = new RenderTarget2D(graphics.GraphicsDevice, width, height);
            for (int i = 0; i < 4; i++)
            {
                width /= 2;
                height /= 2;
                downsampleTarget[i] = new RenderTarget2D(graphics.GraphicsDevice, width, height);
            }
            finalTarget = bloomTarget[0];
        }
        public void Draw(bool bDrawObjects=true)
        {
            PreProcess(bDrawObjects);
            PostProcess();
            FinalDraw(bDrawObjects);
        }
        public void PreProcess(bool bDrawObjects)
        {
            BindTarget(colorTarget);
            BeginDraw(colorEffect);
            batch.Draw(GameContent.background, new Vector2(), Color.White);
            batch.End();
            if (bDrawObjects)
            {
                BeginDraw(colorEffect);
                foreach (GameObject obj in objects)
                    if (!(obj is Pit))
                        obj.Draw(batch);
                batch.End();
            }
            BindTarget(null);
        }
        public void PostProcess()
        {
            Downsample();
            ApplyBloom();
        }
        void Downsample()
        {
            Texture2D texture=colorTarget;
            for (int i = 0; i < 4; i++)
            {
                Vector2 texelSize = new Vector2(1.0f / downsampleTarget[i].Width, 1.0f / downsampleTarget[i].Height) * 2.0f;
                downsampleEffect.Parameters["texelSize"].SetValue(texelSize);
                RenderToTarget(downsampleTarget[i], texture, downsampleEffect, .5f);
                texture = downsampleTarget[i];
            }
        }
        void ApplyBloom()
        {
            gaussianBlurEffect.Parameters["dir"].SetValue(new Vector2(0, .5f / (float) downsampleTarget[3].Width));
            RenderToTarget(bloomTarget[0], downsampleTarget[3], gaussianBlurEffect, 16.0f);

            gaussianBlurEffect.Parameters["dir"].SetValue(new Vector2(.5f / (float)bloomTarget[0].Height, 0));
            RenderToTarget(bloomTarget[1], bloomTarget[0], gaussianBlurEffect, 1.0f);

            bloomEffect.Parameters["bloomFactor"].SetValue(bloomFactor);
            bloomEffect.Parameters["bloomTexture"].SetValue(bloomTarget[1]);
            RenderToTarget(bloomTarget[0], colorTarget, bloomEffect, 1.0f);
        }
        public void FinalDraw(bool bDrawObjects)
        {
            int curTargetId = 1, otherTargetId = 0;
            if (bDrawObjects)
            {
                waveEffect.Parameters["Resolution"].SetValue(new Vector2(targetWidth, targetHeight));
                waveEffect.Parameters["Radius"].SetValue(0);
                foreach (GameObject obj in objects)
                {
                    if (obj is Pit)
                    {
                        Pit pit = (Pit)obj;
                        waveEffect.Parameters["Width"].SetValue(pit.Radius / targetWidth);
                        waveEffect.Parameters["Position"].SetValue(pit.pos);
                        BindTarget(bloomTarget[curTargetId]);
                        BeginDraw(waveEffect);
                        batch.Draw(bloomTarget[otherTargetId], new Vector2(), Color.White);
                        batch.End();
                        curTargetId = otherTargetId;
                        otherTargetId = 1 - curTargetId;
                    }
                }
            }
            BindTarget(null);
            BeginDraw(colorEffect);
            batch.Draw(bloomTarget[otherTargetId], new Vector2(), Color.White);
            batch.End();
        }
        public void BeginDraw(Effect effect)
        {
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, effect);
        }
        public void BindTarget(RenderTarget2D target)
        {
            graphics.GraphicsDevice.SetRenderTarget(target);
            graphics.GraphicsDevice.Clear(Color.Black);
        }
        public void RenderToTarget(RenderTarget2D target, Texture2D texture, Effect effect, float scale)
        {
            BindTarget(target);
            effect.Parameters["Resolution"].SetValue(new Vector2(target.Width, target.Height));
            BeginDraw(effect);
            batch.Draw(texture, new Vector2(), null, Color.White, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            batch.End();
            BindTarget(null);
        }
    }
}
