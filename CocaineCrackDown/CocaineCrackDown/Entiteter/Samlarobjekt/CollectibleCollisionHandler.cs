

using Nez;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.Entiteter {
    public class CollectibleCollisionHandler : Component, ITriggerListener {
        private Collectible _collectible;

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            _collectible = Entity as Collectible;
        }

        public void OnTriggerEnter(Collider other , Collider local) {
            if(other == null || other.Entity == null)
                return;
            if(_collectible.CollectibleState == CollectibleState.Appearing)
                return;

            //if(other.Entity.Tag == Tags.Pit) {
            //    _collectible.FallIntoPit(other.Entity);
            //}
        }

        public void OnTriggerExit(Collider other , Collider local) {
        }

    }
}
