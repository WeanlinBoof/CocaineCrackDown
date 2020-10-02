
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocaineCrackDown.System.SpelLäggeHanterare {
    public abstract class GameModeHandler : IGameModeHandler {
        public SpelSystem GameSystem { get; }
        public GameSettings Settings { get; private set; }

        protected bool _weHaveAWinner;

        public GameModeHandler(SpelSystem gameSystem) {
            GameSystem = gameSystem;
        }

        public virtual void Setup(GameSettings settings) {
            Settings = settings;
            GameSystem.StartRound();
        }
        public abstract bool WeHaveAWinner();


    }
}
