
using CocaineCrackDown.Verktyg;
using CocaineCrackDown.Verktyg.Events;

using Nez;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocaineCrackDown.System.SpelLäggeHanterare {
    public class RoundsHandler : GameModeHandler {
        private bool Vinnare;

        public RoundsHandler(SpelSystem gameSystem) : base(gameSystem) { }

        public override void Setup(GameSettings settings) {
            base.Setup(settings);
            GameSystem.Subscribe(SpelEvents.PlayerKilled , QueueOnPlayerKilled);
        }

        private void QueueOnPlayerKilled(SpelEventParameter parameters) {
            Core.Schedule(1.0f , _ => OnPlayerKilled(parameters));
        }

        private void OnPlayerKilled(SpelEventParameter parameters) {
            SpelareDödadEventParameter pkParams = (SpelareDödadEventParameter)parameters;
            CheckForWinner();
        }

        private void CheckForWinner() {
            var alivePlayersLeft = GameSystem.Spelarna
                .Where(p => p.PlayerState == PlayerState.Normal);

            if(alivePlayersLeft.Count() > 1) {
                return;
            }

            var player = alivePlayersLeft.FirstOrDefault();
            if(player != null) {
                var playerScore = KontextHanterare.PlayerMetadataByIndex(player.PlayerIndex);
                if(playerScore == null)
                    return;

                playerScore.Poäng++;

                if(playerScore.Poäng >= Settings.MaxPoäng) {
                    Vinnare = true;
                }
            }

            GameSystem.EndRound();
        }

        public override bool WeHaveAWinner() {
            return Vinnare;
        }
    }
}
