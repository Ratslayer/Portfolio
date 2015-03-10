using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RedStream
{
    public class RsKeyBindings : RsData
    {
        public Keys ExitKey;
        public Keys SetWireframe;
        public Keys ActivateParticles;
        public Keys VisualizeBounds;
        public Keys IncreaseBloomFactor;
        public Keys DecreaseBloomFactor;
        public Keys SetDiffuseMap;
        public Keys SetLightMap;
        public Keys SetSpecularMap;
        public Keys SetDeferredMap;
        public Keys SetLightPassedMap;
        public Keys SetDownsampledMap;
        public Keys SetBlurredMap;
        public Keys SetBloomedMap;
        public Keys SelectNextRing;
        public Keys SelectPreviousRing;
        public Keys RotateRingRight;
        public Keys RotateRingLeft;
        public Keys ToggleGyro;
        public Keys DrawScene;
        public string CameraMove;
        public string CameraZoom;
        public Keys CreateShip;
    }
}