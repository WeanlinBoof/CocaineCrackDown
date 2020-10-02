

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace CocaineCrackDown.Entiteter {
    public class CollectibleMetadata {
        public Color Color { get; set; }
        public Action<Collectible> OnDestroyEvent { get; set; }
        public Action<Collectible , Spelare> OnDropEvent { get; set; }
        public Action<Collectible , Spelare> OnPickupEvent { get; set; }
        public List<Func<Spelare , bool>> CanCollectRules { get; set; }
        protected Dictionary<string , object> Data { get; set; }
        public CollectibleMetadata() {
            Color = Color.White;
            Data = new Dictionary<string , object>();
            CanCollectRules = new List<Func<Spelare , bool>>();
        }
    }
}
