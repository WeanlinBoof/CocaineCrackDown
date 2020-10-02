using System.Collections.Generic;
using System.Linq;

using CocaineCrackDown.System;

namespace CocaineCrackDown.Verktyg {

    public static class KontextHanterare {

        public static List<PlayerMetadata> PlayerMetadata { get; set; }

        public static GameSettings GameSettings { get; set; }

        public static bool IsGameInitialized { get; set; }

        public static PlayerMetadata PlayerMetadataByIndex(int index) {
            return PlayerMetadata?.FirstOrDefault(p => p.SpelarIndex == index);
        }
    }
}