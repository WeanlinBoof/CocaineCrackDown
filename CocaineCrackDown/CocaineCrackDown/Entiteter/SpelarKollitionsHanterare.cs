using System;
using System.Collections.Generic;
using System.Linq;

using CocaineCrackDown.Entiteter.Events;
using CocaineCrackDown.Entiteter.Event;
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.Verktyg;

using Nez;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.Entiteter {

    public class SpelarKollitionsHanterare : Component, ITriggerListener {

        private Collider SpelarSlagBox;

        private Collider SpelarNärhetBox;

        private List<Entity> EntiteterInomNärhetBox;

        private Spelare spelare;

        public SpelarKollitionsHanterare(Collider hitboxCollider , Collider proximityCollider) {
            EntiteterInomNärhetBox = new List<Entity>();
            SpelarSlagBox = hitboxCollider;
            SpelarNärhetBox = proximityCollider;
        }

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            spelare = Entity as Spelare;
        }

        public void OnTriggerEnter(Collider other , Collider local) {
            if(local == SpelarSlagBox) {
                HitboxTriggerEnter(other , local);
            }
            else if(local == SpelarNärhetBox) {
                ProximityTriggerEnter(other , local);
            }
        }

        private void ProximityTriggerEnter(Collider other , Collider local) {
            if(other == null || other.Entity == null) {
                return;
            }

            if(other.Entity is ProximityEventEmitter NärhetEvent) // wee
            {
                NärhetEvent.EmitMapEvent(new object[] { true });
                return;
            }
            InteractableComponent interactable = other.Entity.GetComponent<InteractableComponent>();
            if(interactable == null) {
                return;
            }

            if(EntiteterInomNärhetBox.Contains(other.Entity)) {
                return;
            }

            EntiteterInomNärhetBox.Add(other.Entity);
        }


        private void HitboxTriggerEnter(Collider other , Collider local) {
            switch(other.Entity.Tag) {
                case Tags.EventEmitter:
                    (other.Entity as CollisionEventEmitter)?.EmitMapEvent(new object[] { Strings.CollisionMapEventEnter , spelare });
                    break;
            }
        }

        public void OnTriggerExit(Collider other , Collider local) {
            if(local == SpelarSlagBox) {
                HitboxTriggerExit(other , local);
            }
            else if(local == SpelarNärhetBox) {
                ProximityTriggerExit(other , local);
            }
        }

        private void HitboxTriggerExit(Collider other , Collider local) {
            if(other.Entity?.Tag == Tags.EventEmitter) {
                (other.Entity as CollisionEventEmitter)?.EmitMapEvent(new object[] { Strings.CollisionMapEventExit , spelare });
            }
        }

        private void ProximityTriggerExit(Collider other , Collider local) {
            if(other == null || other.Entity == null) {
                return;
            }
            if(other.Entity is ProximityEventEmitter pee) // wee
            {
                pee.EmitMapEvent(new object[] { false });
                Console.WriteLine("Emitting proximity exit event " + pee.Name);
            }

            if(!EntiteterInomNärhetBox.Contains(other.Entity)) {
                return;
            }

            EntiteterInomNärhetBox.Remove(other.Entity);
        }

        public void InteractWithNearestEntity() {
            if(EntiteterInomNärhetBox.Count == 0) {
                return;
            }

            // Find closest entity based on distance between player and collectible
            Entity closestEntity = EntiteterInomNärhetBox.Aggregate((other1 , other2) =>
            (other2.Position - Entity.Position).Length() < (other1.Position - Entity.Position).Length() ? other2 : other1);

            Console.WriteLine("Attempting to interact with " + closestEntity);

            InteractableComponent interactable = closestEntity.GetComponent<InteractableComponent>();
            if(interactable == null) {
                return;
            }

            interactable.OnInteract(spelare);

            if(closestEntity is Collectible collectible) {
                EntiteterInomNärhetBox.Remove(closestEntity);
            }
        }
    }
}