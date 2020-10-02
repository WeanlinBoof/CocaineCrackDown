
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using CocaineCrackDown.Verktyg.Events;
namespace CocaineCrackDown.System.SpelLäggeHanterare {
    //public class DeathmatchHandler : GameModeHandler {
    //    private bool _weHaveAWinner;

    //    public DeathmatchHandler(GameSystem gameSystem) : base(gameSystem) { }

    //    public override void Setup(GameSettings settings) {
    //        base.Setup(settings);
    //        GameSystem.Subscribe(SpelEvents.PlayerKilled , QueueOnPlayerKilled);
    //        GameSystem.DebugLines.Add(new DebugLine {
    //            Text = () => $"Players Alive: {GameSystem.Players.Count(p => p.PlayerState == PlayerState.Normal) }"
    //        });
    //    }

    //    private void QueueOnPlayerKilled(GameEventParameters parameters) {
    //        Core.schedule(2.5f , false , _ => OnPlayerKilled(parameters));
    //    }

    //    private void OnPlayerKilled(GameEventParameters parameters) {
    //        var pkParams = parameters as PlayerKilledEventParameters;
    //        if(pkParams.Killer != null && pkParams.Killed != pkParams.Killer) {
    //            var playerScore = ContextHelper.PlayerMetadataByIndex(pkParams.Killer.PlayerIndex);
    //            if(playerScore != null) {
    //                playerScore.Score++;
    //            }
    //        }
    //        CheckForWinner();
    //        RespawnPlayer(pkParams.Killed);
    //    }

    //    private void RespawnPlayer(Player player) {
    //        if(_weHaveAWinner)
    //            return;

    //        var players = GameSystem.Players;
    //        var spawnLocations = GameSystem.Karta.SpawnLocations;

    //        var furthestDistance = 0.0f;
    //        var furthestSpawnPosition = new Vector2();
    //        foreach(var spawnLocation in spawnLocations) {
    //            var spawnPosition = spawnLocation.Position;
    //            var distanceToSpawnLocation = 0.0f;
    //            foreach(var otherPlayer in players) {
    //                if(otherPlayer == player)
    //                    continue;
    //                var distance = Math.Abs((otherPlayer.position - spawnPosition).Length());
    //                distanceToSpawnLocation += distance;
    //            }
    //            if(distanceToSpawnLocation >= furthestDistance) {
    //                furthestDistance = distanceToSpawnLocation;
    //                furthestSpawnPosition = spawnPosition;
    //            }
    //        }
    //        player.Respawn(furthestSpawnPosition);
    //    }

    //    private void CheckForWinner() {
    //        if(_weHaveAWinner)
    //            return;

    //        var maxScoreHolder = ContextHelper.PlayerMetadata.Max();
    //        if(maxScoreHolder.Score < Settings.MaxPoäng)
    //            return;

    //        _weHaveAWinner = true;
    //        var winningPlayer = GameSystem.Players.First(p => p.PlayerIndex == maxScoreHolder.PlayerIndex);
    //        winningPlayer.getComponent<CameraTracker>().setEnabled(true);
    //        var otherPlayers = GameSystem.Players.Where(p => p.PlayerIndex != maxScoreHolder.PlayerIndex);
    //        foreach(var otherPlayer in otherPlayers) {
    //            otherPlayer.getComponent<CameraTracker>().setEnabled(false);
    //        }
    //        GameSystem.EndRound();
    //    }

    //    public override bool WeHaveAWinner() {
    //        return _weHaveAWinner;
    //    }
    //}
}
