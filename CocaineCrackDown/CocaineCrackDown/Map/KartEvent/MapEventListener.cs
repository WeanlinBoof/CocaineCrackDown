using System;

using Nez;

namespace CocaineCrackDown.Entiteter.Events {

    public class MapEventListener : Component {

        public string EventKey { get; set; }

        public MapEventListener(string eventKey) {
            EventKey = eventKey;
        }

        public Action<KartEvent> EventTriggered { get; set; }
    }
}