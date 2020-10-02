using System;
using System.Collections.Generic;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.Scener;
using CocaineCrackDown.System.SpelLäggeHanterare;
using CocaineCrackDown.Verktyg;
using CocaineCrackDown.Verktyg.Events;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Systems;
using Nez.Tweens;

namespace CocaineCrackDown.System {
    public class SpelSystem : SceneComponent {
        private const float TransitionDelay = 4f;
        private const float LetterBoxSize = 140f;

        private SpelStatus SpeletsStatus;
        private GrundScen grundScen;
        private SmoothCamera Kamera;
        private Emitter<SpelEvents , SpelEventParameter> Sändare;
        private Cooldown ÖverGångsFördröjning;

        public SpelStatus SpelStatus => SpeletsStatus;
        public List<Spelare> Spelarna { get; }
        public Karta Karta { get; }
        public GameSettings Inställningar { get; }
        public IGameModeHandler SpelLäggeHanterare { get; private set; }

        public SpelSystem(GameSettings gameSettings , Karta karta) { 
            Karta = karta;
            Spelarna = new List<Spelare>();
            ÖverGångsFördröjning = new Cooldown(TransitionDelay , true);
            Inställningar = gameSettings;
            Sändare = new Emitter<SpelEvents , SpelEventParameter>();
        }
        public override void OnEnabled() {
            grundScen = Scene as GrundScen;
            Kamera = Scene.GetSceneComponent<SmoothCamera>();
            SetupGameModeHandler();
        }

        private void SetupGameModeHandler() {
            switch(Inställningar.SpelLäge) {
                case SpelLägen.HUB:
                    SpelLäggeHanterare = new HubHandler(this);
                    break;
                case SpelLägen.STANDARD:
                    SpelLäggeHanterare = new HubHandler(this);
                    break;
                case SpelLägen.BRUHMODE:
                    SpelLäggeHanterare = new HubHandler(this);
                    break;
                case SpelLägen.EASY:
                    SpelLäggeHanterare = new HubHandler(this);
                    break;
                case SpelLägen.HARD:
                    SpelLäggeHanterare = new HubHandler(this);
                    break;
            }
            (Scene as GrundScen)?.OnGameHandlerAdded(SpelLäggeHanterare);
            SpelLäggeHanterare.Setup(Inställningar);
        }

        public void RegisterPlayer(Spelare spelare) {
            Spelarna.Add(spelare);
            var playerMeta = KontextHanterare.PlayerMetadataByIndex(spelare.SpelarIndex);
            if(playerMeta == null) {
                playerMeta = new PlayerMetadata {
                    SpelarIndex = spelare.SpelarIndex
                };
                KontextHanterare.PlayerMetadata.Add(playerMeta);
            }
        }

        public override void Update() {
            switch(SpeletsStatus) {
                case SpelStatus.Starting:
                    break;
                case SpelStatus.Started:
                    break;
                case SpelStatus.Ending:
                    Time.TimeScale *= 0.9975f;
                    Kamera.BaseZoom *= 1.0025f;
                    grundScen.LetterBox.letterboxSize = 0.99f * grundScen.LetterBox.letterboxSize + 0.01f * LetterBoxSize;
                    ÖverGångsFördröjning.Update();
                    if(ÖverGångsFördröjning.IsReady()) {
                        PrepareForNewRound();
                        if(SpelLäggeHanterare.WeHaveAWinner()) {
                            EndGame();
                        }
                        else {
                            StartNextRound();
                        }
                    }
                    break;
                case SpelStatus.Ended:
                    break;
            }
        }

        private void EndGame() {
            KontextHanterare.PlayerMetadata.ForEach(p => p.Poäng = 0);
            Core.StartSceneTransition(new CrossFadeTransition(() => new NätLobby()));
        }

        public void StartRound() {
            SpeletsStatus = SpelStatus.Started;
        }

        private void PrepareForNewRound() {
            SpeletsStatus = SpelStatus.Ended;
            Time.TimeScale = 1.0f;
            //TODO: This might be a source for bugs later on ...
            TweenManager.StopAllTweens();
        }

        public void StartNextRound() {
            Core.StartSceneTransition(new CrossFadeTransition(
                () => new GrundScen(KontextHanterare.GameSettings)));
        }

        public void EndRound() {
            SpeletsStatus = SpelStatus.Ending;
            Time.TimeScale = 0.8f;
            ÖverGångsFördröjning.Start();
            VignettePostProcessor sepia = grundScen.AddPostProcessor(new VignettePostProcessor(5));
            sepia.Effect = new SepiaEffect();
            Kamera.SetWinMode(true);
            if(SpelLäggeHanterare.WeHaveAWinner()) {
                grundScen.LetterBox.color = Color.White;
            }
        }

        public void Subscribe(SpelEvents gameEvent , Action<SpelEventParameter> handler) {
            Sändare.AddObserver(gameEvent , handler);
        }

        public void Publish(SpelEvents gameEvent , SpelEventParameter parameters) {
            Sändare.Emit(gameEvent , parameters);
        }
    }

}
