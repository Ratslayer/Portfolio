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
    class RsGraphics
    {
        #region PublicVars
        public enum Stage
        {
            Diffuse,
            Shadows,
            Lighting
        };
        public enum Mode
        {
            Color,
            Lit,
            Full,
            Wireframe
        };
        public GraphicsDeviceManager graphics { get; private set; }
        public SpriteBatch batch { get; private set; }
        public Stage stage { get; private set; }
        public Mode mode { get; set; }
        public RsCamera MainCamera { get; private set; }
        public RsLight Light { get; private set; }
        public int ObjectsDrawn=0;
        public bool bVisualizeBounds=false;
        public float FramesPerSecond = 0;

        #region Targets
        //2D targets
        public RenderTarget2D ShadowMap { get; private set; }
        public RenderTarget2D DiffuseMap { get; private set; } 
        public RenderTarget2D DeferredMap { get; private set; }
        public RenderTarget2D BloomedMap { get; private set; }
        public RenderTarget2D LightPassedMap { get; private set; }
        public RenderTarget2D DisplacementMap { get; private set; }
        public RenderTarget2D FinalMap, LastBoundMap=null;
        //2D target arrays
        public RenderTarget2D[] LightMap { get; private set; }
        public RenderTarget2D[] SpecularMap { get; private set; }
        public RenderTarget2D[] DownsampledMap { get; private set; }
        public RenderTarget2D[] BlurredMap { get; private set; }
        //Cube targets
        public RenderTargetCube EnvMap { get; private set; }
        public RenderTargetCube CubeShadowMap { get; private set; }
        #endregion

        public float BloomFactor, LightCutoff;
        public Vector2 Dimensions 
        { 
            get 
            { 
                return new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height); 
            } 
            private set{}
        }
        #endregion

        #region PrivateVars
        private class FaceParams
        {
            public FaceParams(CubeMapFace face, Vector3 dir, Vector3 up)
            {
                this.face = face;
                this.dir = dir;
                this.up = up;
            }
            public CubeMapFace face;
            public Vector3 dir, up;
        };
        private Effect DiffuseEffect, ShadowEffect, LightEffect, BlendingEffect, BlurEffect, EnvEffect;
        private Vector2 TargetWidth;
        private Texture2D pixel;
        private IEnumerable<RsActor> Actors;
        #endregion

        #region PublicMethods
        public RsGraphics(GraphicsDeviceManager graphics, SpriteBatch batch)
        {
            this.graphics = graphics;
            this.batch = batch;
            pixel = RedStream.Content.GetTexture("Pixel");
            MainCamera = null;
            TargetWidth = new Vector2(512, 512);
            mode = Mode.Full;
            BloomFactor = 1.0f;
            LightCutoff = 0.7f;
            Init();
        }
        public void Render()
        {
            ObjectsDrawn = 0;
            Actors = from actor in RedStream.Game.Components
                     where actor is RsActor && ((RsActor)actor).Visible
                     select (RsActor)actor;
            DrawDiffuse();
            if (mode == Mode.Lit || mode == Mode.Full)
            {
                RenderLighting();
                ApplyLighting();
            }
            if (mode == Mode.Full)
            {
                PostProcess();
            }
            AddTextures(null, FinalMap, null, new Vector2(1, 0));
        }
        public void BuildDisplacementMap()
        {
            List<Kathy.Pit> pits = Kathy.GameInfo.Pits;
            Vector2 dimensions = new Vector2(DisplacementMap.Width, DisplacementMap.Height);
            Vector2 field = new Vector2(Kathy.GameInfo.fieldSize.X, Kathy.GameInfo.fieldSize.Z);
            Bind(DisplacementMap);
            Clear(Color.Black);
            batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            foreach (Kathy.Pit pit in pits)
            {
                Vector2 pos = (new Vector2(pit.box.Min.X, pit.box.Min.Z) + field / 2) / field * dimensions;
                Vector2 size = new Vector2(pit.box.Max.X - pit.box.Min.X, pit.box.Max.Z - pit.box.Min.Z) / field * dimensions;
                //Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
                batch.Draw(pixel, pos, null, Color.White, 0, new Vector2(), size, SpriteEffects.None, 0);
            }
            batch.End();
            Unbind();
            DiffuseEffect.Parameters["DisplacementTexture"].SetValue(DisplacementMap);
            LightEffect.Parameters["DisplacementTexture"].SetValue(DisplacementMap);
            ShadowEffect.Parameters["DisplacementTexture"].SetValue(DisplacementMap);
            graphics.GraphicsDevice.Textures[1] = DisplacementMap;
        }
        public void BeginPostProcess()
        {
            Bind(DeferredMap);
        }
        public void EndPostProcess()
        {
            PostProcess();
            AddTextures(null, FinalMap, null, new Vector2(1, 0));
        }
        #endregion

        #region PrivateMethods
        private void Init()
        {
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            InitShaders();
            InitTargets();

            RsSprites.init();
        }
        private void InitShaders()
        {
            DiffuseEffect = LoadShader("Basic");
            ShadowEffect = LoadShader("ShadowMapping");
            LightEffect = LoadShader("LightMapping");
            BlendingEffect = LoadShader("Blending");
            BlurEffect = LoadShader("Blur");
            EnvEffect = LoadShader("EnvMapping");
        }
        private void InitTargets()
        {
            ShadowMap = CreateTarget(TargetWidth, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            CubeShadowMap = CreateTargetCube(TargetWidth, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);

            DiffuseMap = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            DeferredMap = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            LightMap = new RenderTarget2D[3];
            SpecularMap = new RenderTarget2D[3];
            for (int i = 0; i < 3; i++)
            {
                LightMap[i] = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
                SpecularMap[i] = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            }
            DownsampledMap = new RenderTarget2D[4];
            for (int i = 0, invScale = 2; i < 4; i++, invScale *= 2)
                DownsampledMap[i] = CreateTarget(Dimensions / (float)invScale, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BlurredMap = new RenderTarget2D[2];
            for (int i = 0; i < 2; i++)
                BlurredMap[i] = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            LightPassedMap = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BloomedMap = CreateTarget(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            EnvMap = CreateTargetCube(Dimensions, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            DisplacementMap = CreateTarget(new Vector2(200, 200), SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            FinalMap = BloomedMap;
        }
        private RenderTarget2D CreateTarget(Vector2 dimensions, SurfaceFormat colorFormat, DepthFormat depthFormat)
        {
            return new RenderTarget2D(graphics.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y, true, colorFormat, depthFormat, 1, RenderTargetUsage.PreserveContents);
        }
        private RenderTargetCube CreateTargetCube(Vector2 dimensions, SurfaceFormat colorFormat, DepthFormat depthFormat)
        {
            return new RenderTargetCube(graphics.GraphicsDevice, (int)dimensions.X, true, colorFormat, depthFormat);
        }
        private void Bind(RenderTarget2D target)
        {
            graphics.GraphicsDevice.SetRenderTarget(target);
            LastBoundMap=target;
        }
        private void Bind(RenderTargetCube target, CubeMapFace face)
        {
            graphics.GraphicsDevice.SetRenderTarget(target, face);
        }
        private void Unbind()
        {
            graphics.GraphicsDevice.SetRenderTarget(null);
        }
        private void Bind(RenderTargetBinding[] targets)
        {
            graphics.GraphicsDevice.SetRenderTargets(targets);
        }
        public void Clear(Color color)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, color, 1.0f, 1);
        }
        private void ClearDepth()
        {
            graphics.GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 1);
        }
        private void ClearTarget(RenderTarget2D target, Color color)
        {
            Bind(target);
            Clear(color);
        }
        private void ClearTargetDepth(RenderTarget2D target)
        {
            Bind(target);
            ClearDepth();
        }
        private Effect LoadShader(string shaderName)
        {
            return RedStream.Content.GetEffect(shaderName);
        }
        private void RenderToCubeMap(RenderTargetCube map, Vector3 pos, IEnumerable<RsActor> actors, Effect effect, bool bLoadDiffuse)
        {
            RsCamera tempCam = MainCamera;
            List<FaceParams> list = new List<FaceParams>();

            list.Add(new FaceParams(CubeMapFace.NegativeX, Vector3.Left, Vector3.Up));
            list.Add(new FaceParams(CubeMapFace.NegativeY, Vector3.Down, Vector3.Forward));
            list.Add(new FaceParams(CubeMapFace.NegativeZ, Vector3.Backward, Vector3.Up));
            list.Add(new FaceParams(CubeMapFace.PositiveX, Vector3.Right, Vector3.Up));
            list.Add(new FaceParams(CubeMapFace.PositiveY, Vector3.Up, Vector3.Backward));
            list.Add(new FaceParams(CubeMapFace.PositiveZ, Vector3.Forward, Vector3.Up));

            RsCamera.Desc cdesc = new RsCamera.Desc();
            cdesc.Pos = pos;
            cdesc.FieldOfView = 90;
            foreach (FaceParams param in list)
            {
                Bind(map, param.face);
                Clear(Color.Black);
                cdesc.At = cdesc.Pos + param.dir;
                cdesc.Up = param.up;
                MainCamera = new RsCamera(cdesc);
                DrawActors(effect, actors, bLoadDiffuse, false, false, false);
                Unbind();
            }
            MainCamera = tempCam;
        }
        private void CreateEnvMap(RsActor actor)
        {
            IEnumerable<RsActor> actors = from Actor in Actors
                                          where Actor != actor && Actor.Material.Colored && Actor.Material.DiffuseFactor == 1.0f
                                          select Actor;
            RenderToCubeMap(EnvMap, actor.Pos, actors, DiffuseEffect, true);
            Bind(DiffuseMap);
        }
        private void DrawParticles()
        {
            IEnumerable<ParticleSystem> particles =
                from particle in RedStream.Game.Components
                where particle is ParticleSystem
                select (ParticleSystem)particle;
            foreach (ParticleSystem particle in particles)
                particle.Draw();
        }
        private void RenderLighting()
        {
            ClearTarget(LightMap[0], Color.Black);
            ClearTarget(SpecularMap[0], Color.Black);
            bool firstTime = true;
            IEnumerable<RsLight> lights =
                from component
                in RedStream.Game.Components
                where component is RsLight
                select (RsLight)component;
            foreach (RsLight light in lights)
            {
                CycleLightMap();
                Light = light;
                if (light.LightType == RsLightDesc.LightType.Spot)
                {
                    ShadowEffect.CurrentTechnique = ShadowEffect.Techniques["T_Spot"];
                    CreateShadowMap();
                    Bind((RenderTarget2D)null);
                    LightEffect.CurrentTechnique = LightEffect.Techniques["T_PCFSpot"];
                    //graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                    //graphics.GraphicsDevice.Textures[0] = ShadowMap;
                    //LightEffect.Parameters["ShadowTexture"].SetValue(RedStream.Graphics.ShadowMap);                    
                }
                else if (light.LightType == RsLightDesc.LightType.Point)
                {
                    ShadowEffect.CurrentTechnique = ShadowEffect.Techniques["T_Cube"];
                    CreateCubeShadowMap();
                    Bind((RenderTarget2D)null);
                    LightEffect.CurrentTechnique = LightEffect.Techniques["T_PCFPoint"];
                    ///graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                    //graphics.GraphicsDevice.Textures[0] = CubeShadowMap;
                    //LightEffect.Parameters["CubeShadowTexture"].SetValue(RedStream.Graphics.CubeShadowMap);
                }
                MainCamera = RedStream.Scene.Camera;
                LoadCamera(LightEffect);
                LoadLight(light);
                GenerateLight(firstTime);
                BlendLighting();
                firstTime = false;
            }
        }
        private void CycleLightMap()
        {
            RsUtil.Swap(ref LightMap[0], ref LightMap[1]);
            RsUtil.Swap(ref SpecularMap[0], ref SpecularMap[1]);
        }
        private void CreateShadowMap()
        {
            stage = Stage.Shadows;
            Bind(ShadowMap);
            Clear(Color.Black);
            MainCamera = Light.GetCamera(0);
            LoadCamera(ShadowEffect);
            IEnumerable<RsActor> actors =
                from actor in Actors
                where actor.Material.CastsShadows
                select actor;
            DrawActors(ShadowEffect, actors, false, false, false, false);
        }
        private void CreateCubeShadowMap()
        {
            IEnumerable<RsActor> actors =
                from actor in Actors
                where actor.Material.CastsShadows
                select actor;
            RenderToCubeMap(CubeShadowMap, Light.Pos, actors, ShadowEffect, false);
        }
        private void GenerateLight(bool firstTime)
        {
            if(firstTime)
                ClearTarget(LightMap[0], Color.White);
            else ClearTarget(LightMap[0], Color.Black);
            ClearTarget(SpecularMap[0], Color.Black);
            //bind targets
            RenderTargetBinding[] targets = new RenderTargetBinding[2];
            targets[0] = LightMap[0];
            targets[1] = SpecularMap[0];
            Bind(targets);
            //prepare the shaders
            DiffuseEffect.Parameters["MaterialColor"].SetValue(new Vector4(1));
            DiffuseEffect.CurrentTechnique = DiffuseEffect.Techniques["T_Color"];
            //get light values
            IEnumerable<RsActor> actors = from actor in Actors
                                          where actor.Material.Lit || actor.Material.Emissive
                                          select actor;
            DrawActors(LightEffect, actors, false, true, true, false);
        }
        private void LoadLight(RsLight light)
        {
            RsCamera lightCamera = RedStream.Graphics.Light.GetCamera(0);
            LightEffect.Parameters["LightView"].SetValue(lightCamera.View);
            LightEffect.Parameters["LightProj"].SetValue(lightCamera.Proj);
            LightEffect.Parameters["LightPos"].SetValue(lightCamera.Pos);
            //light intensity
            LightEffect.Parameters["LightColor"].SetValue(light.Color);
            LightEffect.Parameters["LightAttenuation"].SetValue(light.Attenuation);
            LightEffect.Parameters["LightRadius"].SetValue(light.Radius);

            if (light.LightType == RsLightDesc.LightType.Spot)
            {
                LightEffect.Parameters["ShadowTexture"].SetValue(ShadowMap);
                //graphics.GraphicsDevice.Textures[0] = ShadowMap;
            }
            else if (light.LightType == RsLightDesc.LightType.Point)
            {
                LightEffect.Parameters["CubeShadowTexture"].SetValue(CubeShadowMap);
                //graphics.GraphicsDevice.Textures[0] = CubeShadowMap;
            }
        }
        private void BlendLighting()
        {
            AddTextures(LightMap[2], LightMap[1], LightMap[0], new Vector2(1, 1));
            AddTextures(SpecularMap[2], SpecularMap[1], SpecularMap[0], new Vector2(1,1));
            RsUtil.Swap(ref LightMap[0], ref LightMap[2]);
            RsUtil.Swap(ref SpecularMap[0], ref SpecularMap[2]);
        }
        private void ApplyLighting()
        {
            Unbind();
            BlendingEffect.Parameters["Texture2"].SetValue(LightMap[0]);
            BlendingEffect.Parameters["Texture3"].SetValue(SpecularMap[0]);
            DrawFullscreen(DeferredMap, DiffuseMap, BlendingEffect, "LightBlend");
        }
        private void PostProcess()
        {
            LightPass();
            Downsample();
            Blur();
            Bloom();
        }
        private void LightPass()
        {
            BlurEffect.Parameters["LightCutoff"].SetValue(LightCutoff);
            DrawFullscreen(LightPassedMap, DeferredMap, BlurEffect, "LightPass");
        }
        private void Downsample()
        {
            Texture2D src = LightPassedMap;
            for (int i = 0; i < 4; i++)
            {
                Vector2 texel = new Vector2(1.0f / (float)src.Width, 1.0f / (float)src.Height);
                BlurEffect.Parameters["TexelSize"].SetValue(texel);
                DrawFullscreen(DownsampledMap[i], src, BlurEffect, "Downsample");
                src = DownsampledMap[i];
            }
        }
        private void Blur()
        {
           BlurEffect.Parameters["BlurDir"].SetValue(new Vector2(.5f / (float) DownsampledMap[3].Width, 0));
           DrawFullscreen(BlurredMap[0], DownsampledMap[3], BlurEffect, "GaussianBlur");
           Unbind();
           BlurEffect.Parameters["BlurDir"].SetValue(new Vector2(0, 8 / (float) BlurredMap[0].Height));
           DrawFullscreen(BlurredMap[1], BlurredMap[0], BlurEffect, "GaussianBlur");
           Unbind();
        }
        private void Bloom()
        {
            Unbind();
            AddTextures(BloomedMap, DeferredMap, BlurredMap[1], new Vector2(1, BloomFactor));
        }
        private void AddTextures(RenderTarget2D dest, Texture2D texture_1, Texture2D texture_2, Vector2 blendFactor)
        {
            Unbind();
            BlendingEffect.Parameters["BlendFactor"].SetValue(blendFactor);
            if(texture_2!=null)
                BlendingEffect.Parameters["Texture2"].SetValue(texture_2);
            DrawFullscreen(dest, texture_1, BlendingEffect, "Additive");
        }
        private void MultiplyTextures(RenderTarget2D dest, Texture2D texture_1, Texture2D texture_2)
        {
            Unbind();
            if (texture_2 != null)
                BlendingEffect.Parameters["Texture2"].SetValue(texture_2);
            DrawFullscreen(dest, texture_1, BlendingEffect, "Multiplicative");
        }
        private void DrawDiffuse()
        {
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Bind(DiffuseMap);
            Clear(Color.Black);
            MainCamera = RedStream.Scene.Camera;
            //render diffuse
            IEnumerable<RsActor> actors =
                from actor in Actors
                where actor.Material.Colored && actor.Material.DiffuseFactor == 1.0f
                select actor;
            DrawActors(DiffuseEffect, actors, true, false, false, false);
            //render fresnel
            actors =
                from actor in Actors
                where actor.Material.Colored && actor.Material.DiffuseFactor != 1.0f
                select actor;
            DrawActors(EnvEffect, actors, false, true, false, true);
            //render particles
            DrawParticles();
            //render bounds
            if (bVisualizeBounds)
                DebugDraw(Actors);
        }
        private void DrawActors(Effect effect, IEnumerable<RsActor> actors, bool bLoadDiffuse, bool bLoadNormal, bool bLoadSpecular, bool bLoadEnv)
        {
            MainCamera.UpdateFrustum();
            LoadCamera(effect);
            Draw(effect, actors, bLoadDiffuse, bLoadNormal, bLoadSpecular, bLoadEnv);
        }
        private void Draw(Effect effect, IEnumerable<RsActor> actors, bool bLoadDiffuse, bool bLoadNormal, bool bLoadSpecular, bool bLoadEnv)
        {
            foreach (RsActor actor in actors)
            {
                if (RedStream.Scene.Camera.CanSee(actor))
                {
                    DrawActor(effect, actor, bLoadDiffuse, bLoadNormal, bLoadSpecular, bLoadEnv);
                    ObjectsDrawn++;
                }
            }
        }
        private void DebugDraw(IEnumerable<RsActor> actors)
        {
            DiffuseEffect.CurrentTechnique = DiffuseEffect.Techniques["T_Wireframe"];
            LoadCamera(DiffuseEffect);
            foreach (RsActor actor in actors)
            {
                if (MainCamera.Frustum.Intersects(actor.BoundingSphereOnlyForAsenicsUse))
                {
                    BoundingSphere sphere = actor.BoundingSphereOnlyForAsenicsUse;
                    //shader.DebugDrawSphere(sphere.Center, sphere.Radius, new Vector4(0, 1, 0, 1));
                    RedStream.Graphics.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    Matrix world = Matrix.CreateScale(sphere.Radius)
                                 * Matrix.CreateTranslation(sphere.Center);
                    DiffuseEffect.Parameters["World"].SetValue(world);
                    //LoadCamera(RedStream.Graphics.MainCamera);
                    DiffuseEffect.Parameters["MaterialColor"].SetValue(new Vector4(0, 1, 0, 1));
                    Model model = RedStream.Content.GetModel("Sphere");
                    foreach (EffectPass pass in DiffuseEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (ModelMesh mesh in model.Meshes)
                        {
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                part.Effect = DiffuseEffect;
                            }
                            mesh.Draw();
                        }
                    }
                }
            }
        }
        private void DrawFullscreen(RenderTarget2D target, Texture2D texture, Effect effect, string techniqueName)
        {
            Bind(target);
            Clear(Color.Black);
            DrawFullscreenQuad(target, texture, effect, techniqueName);
        }
        public void DrawFullscreenQuad(Texture2D texture, Vector4 color)
        {
            DrawFullscreenQuad(LastBoundMap, texture, null, "", new Color(color), BlendState.NonPremultiplied);
        }
        public void DrawFullscreenQuad(RenderTarget2D target, Texture2D texture, Effect effect, string techniqueName)
        {
            DrawFullscreenQuad(target, texture, effect, techniqueName, Color.White, BlendState.Opaque);
        }
        public void DrawFullscreenQuad(RenderTarget2D target, Texture2D texture, Effect effect, string techniqueName, Color color, BlendState blendState)
        {
            //scale the texture so it fits the target
            Vector2 scale=new Vector2(1);
            if (effect == null)
            {
                if (target != null)
                    scale = new Vector2((float)target.Width / (float)texture.Width, (float)target.Height / (float)texture.Height);
                else scale = new Vector2((float)Dimensions.X / (float)texture.Width, (float)Dimensions.Y / (float)texture.Height);
            }
            else
            {
                effect.Parameters["Scale"].SetValue(new Vector2(texture.Width, texture.Height));
                effect.CurrentTechnique = effect.Techniques["T_" + techniqueName];
            }
            SamplerState samplerState;
            if (texture.Format == SurfaceFormat.Single)
                samplerState = SamplerState.PointWrap;
            else samplerState = SamplerState.LinearWrap;
            //draw texture
            batch.Begin(SpriteSortMode.BackToFront, blendState, samplerState, DepthStencilState.Default, RasterizerState.CullNone, effect);
            batch.Draw(texture, new Vector2(), null, color, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            batch.End();
        }
        protected void LoadCamera(Effect effect)
        {
            if (MainCamera != null)
            {
                effect.Parameters["View"].SetValue(MainCamera.View);
                effect.Parameters["Proj"].SetValue(MainCamera.Proj);
                effect.Parameters["EyePos"].SetValue(MainCamera.Pos);
                if (RedStream.Graphics.stage == RsGraphics.Stage.Lighting)
                {
                    RsCamera lightCamera = RedStream.Graphics.Light.GetCamera(0);
                    effect.Parameters["LightView"].SetValue(lightCamera.View);
                    effect.Parameters["LightProj"].SetValue(lightCamera.Proj);
                    effect.Parameters["LightPos"].SetValue(lightCamera.Pos);
                }
            }
        }
        protected void ChooseTechnique(Effect effect, RsActor actor)
        {
            if (effect == DiffuseEffect)
            {
                if (actor.Material.DiffuseMap != null)
                {
                    if (actor is Kathy.Terrain)
                    {
                        effect.Parameters["displacementDepth"].SetValue(Kathy.GameInfo.pitDepth);
                        effect.CurrentTechnique = effect.Techniques["T_DiffuseHighlightDisp"];
                    }
                    else effect.CurrentTechnique = effect.Techniques["T_DiffuseHighlight"];
                }
                else effect.CurrentTechnique = effect.Techniques["T_Color"];
            }
            if (effect == LightEffect)
            {
                if (Light.LightType == RsLightDesc.LightType.Point)
                {
                    if (actor is Kathy.Terrain)
                    {
                        effect.Parameters["displacementDepth"].SetValue(Kathy.GameInfo.pitDepth);
                        effect.CurrentTechnique = effect.Techniques["T_PCFPointDisp"];
                    }
                    else effect.CurrentTechnique = effect.Techniques["T_PCFPoint"];
                }
                else effect.CurrentTechnique = effect.Techniques["T_PCFSpot"];
            }
            if (effect == ShadowEffect)
            {
                if (Light.LightType == RsLightDesc.LightType.Point)
                {
                    if (actor is Kathy.Terrain)
                    {
                        effect.Parameters["displacementDepth"].SetValue(Kathy.GameInfo.pitDepth);
                        effect.CurrentTechnique = effect.Techniques["T_CubeHeight"];
                    }
                    else effect.CurrentTechnique = effect.Techniques["T_Cube"];
                }
                else effect.CurrentTechnique = effect.Techniques["T_Spot"];
            }
        }
        protected void LoadMaterial(Effect effect, RsMaterial material, bool bLoadDiffuse, bool bLoadNormal, bool bLoadSpecular)
        {
            if (bLoadDiffuse)
            {
                effect.Parameters["MaterialColor"].SetValue(material.Color);
                if (material.DiffuseMap != null)
                {
                    effect.Parameters["DiffuseTexture"].SetValue(material.DiffuseMap);
                    effect.CurrentTechnique = effect.Techniques["T_DiffuseHighlight"];
                }
                else effect.CurrentTechnique = effect.Techniques["T_Color"];
            }
            if (bLoadNormal)
            {
                effect.Parameters["Bump"].SetValue(material.Bumpiness);
                if (material.NormalMap != null)
                    effect.Parameters["NormalTexture"].SetValue(material.NormalMap);
            }
            if (bLoadSpecular)
            {
                effect.Parameters["Shininess"].SetValue(material.Shininess);
                effect.Parameters["Specular"].SetValue(material.Specular);
                if (material.SpecularMap != null)
                    effect.Parameters["SpecularTexture"].SetValue(material.SpecularMap);
            }
        }
        protected void LoadEnvMapping(Effect effect, RsMaterial material)
        {
            //env mapping params
            effect.Parameters["DiffuseFactor"].SetValue(material.DiffuseFactor);
            effect.Parameters["FresnelBias"].SetValue(material.FresnelBias);
            effect.Parameters["FresnelScale"].SetValue(material.FresnelScale);
            effect.Parameters["FresnelPower"].SetValue(material.FresnelPower);
            effect.Parameters["EtaRatio"].SetValue(material.Refractivity);
            //env map
            effect.Parameters["EnvTexture"].SetValue(RedStream.Graphics.EnvMap);
            //diffuse map
            effect.Parameters["MaterialColor"].SetValue(material.Color);
            if (material.DiffuseMap != null)
                effect.Parameters["DiffuseTexture"].SetValue(material.DiffuseMap);
            effect.CurrentTechnique = effect.Techniques["T_Fresnel"];
        }
        protected void LoadActor(Effect effect, RsActor actor)
        {
            Matrix world = Matrix.CreateScale(actor.Scale)
                         * Matrix.CreateFromQuaternion(actor.Orientation)
                         * Matrix.CreateTranslation(actor.Pos);
            effect.Parameters["World"].SetValue(world);
        }
        protected void DrawActor(Effect effect, RsActor actor, bool bLoadDiffuse, bool bLoadNormal, bool bLoadSpecular, bool bLoadEnv)
        {
            if (effect == LightEffect && actor.Material.Emissive)
            {
                DrawActor(DiffuseEffect, actor, true, false, false, false);
                return;
            }
            LoadActor(effect, actor);
            LoadMaterial(effect, actor.Material, bLoadDiffuse, bLoadNormal, bLoadSpecular);
            if (bLoadEnv)
            {
                CreateEnvMap(actor);
                LoadEnvMapping(effect, actor.Material);
            }
            if (actor.Material.Cull)
                RedStream.Graphics.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            else RedStream.Graphics.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            ChooseTechnique(effect, actor);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ModelMesh mesh in actor.Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                    }
                    mesh.Draw();
                }
            }
        }
        #endregion
    }
}
