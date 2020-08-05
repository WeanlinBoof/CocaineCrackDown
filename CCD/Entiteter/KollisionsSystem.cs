using Nez;

namespace CocaineCrackDown.Entiteter {

    public class KollisionsSystem : EntityProcessingSystem {

        public KollisionsSystem( Matcher matcher ) : base( matcher ) {}

        public override void Process( Entity entity ) {

            var colliders = Physics.BoxcastBroadphase( entity.GetComponent<Collider>.bounds );

            foreach( var coll in colliders ) {
                if( entity.GetComponent<Collider>.CollidesWith( coll, out collResult ) ) {
                    entity.Scene.RemoveEntity( entity );
                }
            }
        }
    }
}
