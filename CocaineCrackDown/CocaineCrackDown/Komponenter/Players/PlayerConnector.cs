using CocaineCrackDown.Entiteter;
using CocaineCrackDown.System;
using CocaineCrackDown.Verktyg;


using Microsoft.Xna.Framework;

using Nez;

using System.Collections.Generic;

namespace CocaineCrackDown.Komponenter {
    public class PlayerConnector : SceneComponent {
        private List<Spelare> AnslutnaSpelare;
        private SpelSystem SpelSystem;
        private HUD Hud;
        private bool SpelareSpawnadeThisFrame;


        public PlayerConnector() {
            AnslutnaSpelare = new List<Spelare>();
        }

        public override void OnEnabled() {
            base.OnEnabled();

            SpelSystem = Scene.GetSceneComponent<SpelSystem>();
            Hud = Scene.GetSceneComponent<HUD>();
        }

        public void ControllerConnected(GamePadData gamepad) {
            int index = AnslutnaSpelare.Count + 1;
            SpawnIdlePlayer(gamepad , index);
        }

        public void SpawnDebugPlayer() {
            SpawnPlayer(null , -(AnslutnaSpelare.Count + 1));
        }

        private Player SpawnPlayer(GamePadData gamepad , int index) {

            var playerMeta = KontextHanterare.PlayerMetadataByIndex(index);
            if(playerMeta == null) {
                playerMeta = new PlayerMetadata {
                    SpelarIndex = index
                };
            }

            SpawnLocation spawnLocation = SpelSystem.Karta.GetUniqueSpawnLocation(playerMeta?.LagIndex ?? -1);

            Spelare player = Scene.AddEntity(new Spelare(playerMeta.Gestalt , (int)spawnLocation.Position.X , (int)spawnLocation.Position.Y , index));
            player.AddComponent(new PlayerController(gamepad));

            AnslutnaSpelare.Add(player);

            SpelareSpawnadeThisFrame = true;

            return player;
        }

        private void SpawnIdlePlayer(GamePadData gamepad , int index) {
            Spelare player = SpawnPlayer(gamepad , index);
            player.SpelareTillstånd = SpelarTillstånd.Idle;
        }

        public override void Update() {
            if(SpelareSpawnadeThisFrame) {
                SpelareSpawnadeThisFrame = false;
                Hud.BuildHudForAllPlayers(AnslutnaSpelare);
            }
        }
    }
}