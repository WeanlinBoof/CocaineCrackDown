using System;

using CocaineCrackDown.Entiteter;

namespace CocaineCrackDown {


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpawnFiendeArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnFiendeArgs"/> class.
        /// </summary>
        /// <param name="fiende">
        /// The enemy.
        /// </param>
        public SpawnFiendeArgs(Fiende fiende) {
            Fiende = fiende;
        }

        /// <summary>
        /// Gets Enemy.
        /// </summary> 
        public Fiende Fiende { get; private set; }

    }
}