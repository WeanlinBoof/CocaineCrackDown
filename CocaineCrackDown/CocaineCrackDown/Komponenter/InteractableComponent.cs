using System;

using CocaineCrackDown.Entiteter;

using Nez;

namespace CocaineCrackDown.Komponenter {
    public class InteractableComponent : Component {
        public Action<Spelare> OnInteract { get; set; }
    }
}
