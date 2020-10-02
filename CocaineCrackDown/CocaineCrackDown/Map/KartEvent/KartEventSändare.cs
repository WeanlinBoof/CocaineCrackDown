using CocaineCrackDown.System;

using Nez;

namespace CocaineCrackDown.Entiteter.Events {

    public abstract class KartEventSändare : Entity {
        private SpelSystem spelSystem;

        protected Karta Karta { get; set; }

        protected string EventKey { get; set; }

        public bool EmitGlobally { get; set; }

        public KartEventSändare(Karta map , string eventKey) {
            Karta = map;
            EventKey = eventKey;
        }

        public override void OnAddedToScene() {
            spelSystem = Scene.GetSceneComponent<SpelSystem>();
        }

        public void EmitMapEvent(object[] parameters = null) {
            KartEvent mapEvent = new KartEvent { EventKey = EventKey , Parameters = parameters };

            Karta.EmitMapEvent(mapEvent , EmitGlobally);
        }
    }
}