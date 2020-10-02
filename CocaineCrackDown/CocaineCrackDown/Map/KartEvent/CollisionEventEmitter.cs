using CocaineCrackDown.Entiteter.Events;
using CocaineCrackDown.Verktyg;

using Nez;

namespace CocaineCrackDown.Entiteter.Event {

    public class CollisionEventEmitter : KartEventSändare {

        public CollisionEventEmitter(Karta map , string eventKey , RectangleF rectangle , int physicsLayer) : base(map , eventKey) {
            Name = "CollisionEventEmitter_" + eventKey;
            Tag = StandigaVarden.Tags.EventEmitter;

            BoxCollider collider = AddComponent(new BoxCollider(rectangle));
            collider.IsTrigger = true;
            Flags.SetFlagExclusive(ref collider.PhysicsLayer , physicsLayer);
        }
    }
}