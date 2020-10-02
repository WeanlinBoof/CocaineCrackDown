using System;
using System.Linq;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Entiteter.Events;
using CocaineCrackDown.Entiteter.Gestalter;
using CocaineCrackDown.Verktyg;
using CocaineCrackDown.Verktyg.Events;

using Nez;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.System.SpelLäggeHanterare {
    public class HubHandler : GameModeHandler {
        private GameSettings NästaSpelInställningar;

        private int readyPlayers = 0;
        private bool starting;

        public HubHandler(SpelSystem gameSystem) : base(gameSystem) {
            SetGameSettings(KontextHanterare.GameSettings ?? GameSettings.Default);
        }

        public override void Setup(GameSettings settings) {
            base.Setup(settings);
            GameSystem.Subscribe(SpelEvents.GlobalMapEvent , HandleMapEvent);
        }

        public void SetGameSettings(GameSettings gameSettings) {
            NästaSpelInställningar = gameSettings;

            ToggleGameMode(gameSettings.SpelLäge);
            ToggleMap(gameSettings.Karta);
            ToggleTeamMode(gameSettings.CoopLäge);
        }
        private void HandleMapEvent(SpelEventParameter parameters) {
            GlobalKartEventParameter mapEventParameters = (GlobalKartEventParameter)parameters;
            if(mapEventParameters == null) {
                return;
            }

            switch(mapEventParameters.KartEvent.EventKey) {
                case Strings.EventReady:
                    object state = mapEventParameters.KartEvent.Parameters[0];
                    bool entered = (string)state == Strings.CollisionMapEventEnter;
                    HandleReadiness(entered);
                    break;
                case Strings.TiledMapGameModeKey:
                    ToggleGameMode(NästaSpelInställningar.SpelLäge);
                    break;
                case Strings.TiledMapCharacterSelectKey:
                    Spelare characterPlayer = mapEventParameters.KartEvent.Parameters[0] as Spelare;
                    SelectCharacter(characterPlayer);
                    break;
                case Strings.TiledMapMapKey:
                    ToggleMap();
                    break;
            }
        }
       private void ToggleGameMode(SpelLägen gameMode) {
            // Cannot select hub
            if(gameMode == SpelLägen.HUB) {
                return;
            }
            NästaSpelInställningar.SpelLäge = gameMode;
            NotifyGameSettingsChange(Strings.TiledMapGameModeDisplayKey , gameMode.ToString());
        }
        private void ToggleMap(string nextMap = null) {
            if(nextMap == null) {
                return;
            }
            NästaSpelInställningar.Karta = nextMap;
            NotifyGameSettingsChange(Strings.TiledMapMapDisplayKey , nextMap);
        }

        private void ToggleTeamMode(CoopLägen teamMode) {
            var nextTeamMode = teamMode;
            NästaSpelInställningar.CoopLäge = nextTeamMode;
            NotifyGameSettingsChange(Strings.TiledMapTeamsDisplayKey , nextTeamMode.ToString());
        }

        private void SelectCharacter(Spelare player) {
            var currentCharacter = player.Parameters.CharacterName;
            GestaltParameter nextCharacter = Gestalter.GetNextAfter(currentCharacter);
            player.SetParameters(nextCharacter);
        }

        private void NotifyGameSettingsChange(string key , string karta) {
            GameSystem.Karta.EmitMapEvent(new KartEvent {
                EventKey = key ,
                Parameters = new string[] { $"{karta}" }
            });
        }




        private void HandleReadiness(bool playerBecameReady) {
            readyPlayers += playerBecameReady ? 1 : -1;

            var readyZoneColor = readyPlayers == 0 ? "standby" :
                readyPlayers < GameSystem.Spelarna.Count ? "partial_ready"
                : "ready";

            GameSystem.Karta.EmitMapEvent(new KartEvent {
                EventKey = "ready_zone" ,
                Parameters = new object[] { readyZoneColor }
            });

            if(readyPlayers == GameSystem.Spelarna.Count && !starting) {
                starting = true;
                Countdown(3);
            }
        }

        private void Countdown(int seconds) {
            Console.WriteLine($"Starting in {seconds} seconds");
            if(readyPlayers == GameSystem.Spelarna.Count) {
                if(seconds == 0) {
                    StartMatch();
                }
                else {
                    Core.Schedule(1 , _ => Countdown(seconds - 1));
                }
            }
            else {
                starting = false;
            }
        }

        private void StartMatch() {
            KontextHanterare.GameSettings = NästaSpelInställningar;
            GameSystem.StartNextRound();
        }


        public override bool WeHaveAWinner() {
            return false;
        }
    }
}
