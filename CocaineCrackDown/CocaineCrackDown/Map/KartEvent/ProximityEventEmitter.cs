using CocaineCrackDown.Verktyg;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Entiteter.Events {

    public class ProximityEventEmitter : KartEventSändare {

        public ProximityEventEmitter(Karta map , string eventKey , Vector2 center , float radius , int physicsLayer) : base(map , eventKey) {
            Name = "ProximityEventEmitter_" + eventKey;
            Tag = StandigaVarden.Tags.EventEmitter;

            CircleCollider collider = AddComponent(new CircleCollider(radius));
            collider.IsTrigger = true;

            Position = new Vector2(center.X + radius , center.Y + radius);

            Flags.SetFlagExclusive(ref collider.PhysicsLayer , physicsLayer);
        }
    }
}