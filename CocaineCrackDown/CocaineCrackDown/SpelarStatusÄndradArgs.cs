using System;

using CocaineCrackDown.Entiteter;
namespace CocaineCrackDown {


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpelarStatusÄndradArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="SpelarStatusÄndradArgs"/> class.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        public SpelarStatusÄndradArgs(Spelare player) {
            Player = player;
        }

        /// <summary>
        /// Gets Player.
        /// </summary>
        public Spelare Player { get; private set; }

    }
}