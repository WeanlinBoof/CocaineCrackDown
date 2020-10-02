using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Entiteter.Events;
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.Verktyg;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Entiteter.Event {

    public class InteractEventEmitter : KartEventSändare {

        public InteractEventEmitter(Karta map , string eventKey , RectangleF rectangle) : base(map , eventKey) {
            Tag = StandigaVarden.Tags.EventEmitter;
            Name = "InteractEventEmitter_" + eventKey;
            Position = new Vector2(rectangle.X + 8 , rectangle.Y + 8);

            BoxCollider collider = AddComponent(new BoxCollider(rectangle.Width , rectangle.Height));
            collider.IsTrigger = true;
            Flags.SetFlagExclusive(ref collider.PhysicsLayer , StandigaVarden.Lager.Interactables);

            AddComponent(new InteractableComponent() {
                OnInteract = player => EmitMapEvent(new object[] { player })
            });
        }
    }
}