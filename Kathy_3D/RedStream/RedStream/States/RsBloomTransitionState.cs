using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsBloomTransitionState : RsDeferredRenderState
    {
        public RsBloomTransitionState(RsState renderState, float? desiredBloom, float bloomSpeed, float? desiredLightCutoff, float lightCutoffSpeed)
            : base(renderState)
        {
            this.desiredBloom = desiredBloom;
            this.desiredLightCutoff = desiredLightCutoff;
            this.bloomSpeed = bloomSpeed;
            this.lightCutoffSpeed = lightCutoffSpeed;
        }
        private SmoothFloatComponent smoothBloom = null, smoothLightCutoff = null;
        private float? desiredBloom, desiredLightCutoff;
        private float bloomSpeed, lightCutoffSpeed;
        public override void EnterState()
        {
            base.EnterState();
            if (desiredBloom != null)
                smoothBloom = new SmoothFloatComponent(RedStream.Graphics.BloomFactor, (float)desiredBloom, bloomSpeed, 0);
            if (desiredLightCutoff != null)
                smoothLightCutoff = new SmoothFloatComponent(RedStream.Graphics.LightCutoff, (float)desiredLightCutoff, lightCutoffSpeed, 0);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (smoothBloom != null)
                RedStream.Graphics.BloomFactor = smoothBloom.F;
            if (smoothLightCutoff != null)
                RedStream.Graphics.LightCutoff = smoothLightCutoff.F;
            if ((smoothBloom == null || !smoothBloom.bActive)
                && (smoothLightCutoff == null || !smoothLightCutoff.bActive))
                RsStateManager.Pop();
            
        }
        public override void ExitState()
        {
            if (smoothBloom != null)
                smoothBloom.Delete();
            if (smoothLightCutoff != null)
                smoothLightCutoff.Delete();
            base.ExitState();
        }
    }
}
