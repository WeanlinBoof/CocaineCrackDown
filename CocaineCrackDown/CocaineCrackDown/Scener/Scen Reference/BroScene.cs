using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.System;
using CocaineCrackDown.System.SpelLäggeHanterare;
using CocaineCrackDown.Verktyg;


using FredflixAndChell.Shared.Maps;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;
using System.Collections.Generic;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace FredflixAndChell.Shared.Scenes
{
    public class BroScene : Scene
    {
        private readonly GameSettings SpelInstälningar;
        private ScreenSpaceRenderer SkärmRendrare;
        private ReflectionRenderer ReflektionsRendrare;
        private RenderLayerRenderer LjusRendrare;

        public CinematicLetterboxPostProcessor LetterBox { get; private set; }

        public BroScene(GameSettings gameSettings)
        {
            SpelInstälningar = gameSettings;
            Setup();
        }

        public override void Initialize()
        {
            Screen.IsFullscreen = true;
            //AssetLoader.LoadBroScene(content);
            SetDesignResolution(SkärmBredd, SkärmHöjd, SceneResolutionPolicy.ShowAllPixelPerfect);

            SetupRenderering();
        }

        public virtual void Setup()
        {
            InitializePlayerScores();
            //Fixa Så Att Karta() har alla saker som behövs från samt till Map()
            //fixa så att karta inte är konstant samma
            Karta karta = AddEntity(new Karta("testnr1"));
            karta.Setup(SpelInstälningar.Karta);
            LjusRendrare.RenderTargetClearColor = karta.AmbientLightingColor;
            AddSceneComponent(new SmoothCamera(ReflektionsRendrare.Camera));
            AddSceneComponent(new SpelSystem(SpelInstälningar, karta));
            AddSceneComponent(new HUD());
            ///////////////FIxa kanske
            //AddSceneComponent(new ControllerSystem());
            //////////////////
            // TODO turn back on for sweet details. Sweetails.
            AddEntity(new DebugHud());
        }

        private void InitializePlayerScores()
        {
            if (KontextHanterare.PlayerMetadata == null)
            {
                KontextHanterare.PlayerMetadata = new List<PlayerMetadata>();
            }
        }

        public virtual void OnGameHandlerAdded(IGameModeHandler gameModeHandler)
        {
        }


        public override void Unload()
        {
            base.Unload();
        }

        #region Rendering Setup
        private void SetupRenderering()
        {
            Camera.SetMinimumZoom(4);
            Camera.SetMaximumZoom(6);
            Camera.SetZoom(4);

            // Render reflective surfaces
            ReflektionsRendrare = ReflectionRenderer.CreateAndSetupScene(this, -1,
                new int[] { Lager.Player, Lager.Bullet, Lager.Interactables });

            Material M = new ReflectionMaterial(ReflektionsRendrare);

            // Rendering all layers but lights and screenspace
            RenderLayerExcludeRenderer renderLayerExcludeRenderer = AddRenderer(new RenderLayerExcludeRenderer(0,
                Lager.Lights, Lager.Lights2, Lager.HUD));
            renderLayerExcludeRenderer.RenderTargetClearColor = new Color(0, 0, 0);

            // Rendering lights
            LjusRendrare = AddRenderer(new RenderLayerRenderer(1,
                Lager.Lights, Lager.Lights2));
            LjusRendrare.RenderTexture = new RenderTexture();
            LjusRendrare.RenderTargetClearColor = new Color(80, 80, 80, 255);

            // Postprocessor effects for lighting
            var spriteLightPostProcessor = AddPostProcessor(new SpriteLightPostProcessor(2, LjusRendrare.RenderTexture));

            // Render screenspace
            SkärmRendrare = new ScreenSpaceRenderer(100, Lager.HUD);
            SkärmRendrare.ShouldDebugRender = false;
            AddRenderer(SkärmRendrare);

            // Letterbox effect when a winner is determined
            LetterBox = AddPostProcessor(new CinematicLetterboxPostProcessor(3));
        }
        #endregion

        public override void Update()
        {
            base.Update();
        }
    }
}
