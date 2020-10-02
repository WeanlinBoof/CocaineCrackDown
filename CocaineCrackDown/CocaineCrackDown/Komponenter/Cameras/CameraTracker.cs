using System;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Komponenter {

    public class CameraTracker : Component {

        private Func<bool> BordeFöljaEntitet;

        private Func<Vector2> PositionFunc;

        private int InternPrioritet;

        public Func<bool> BordeEntitetFöljas => BordeFöljaEntitet ??= Always;

        public Func<Vector2> PositionFunction => PositionFunc ??= TrackEntity;

        public int Prioritet { get; set; }

        public CameraTracker() {
        }

        public CameraTracker(Func<bool> bordeFöljaEntitet , Func<Vector2> positionFunction = null , int prioritet = 0) {
            PositionFunc = positionFunction;
            BordeFöljaEntitet = bordeFöljaEntitet;
            InternPrioritet = prioritet;
        }

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            SmoothCamera Kamra = Entity.Scene.GetSceneComponent<SmoothCamera>();
            Kamra.Register(this);
        }

        public override void OnRemovedFromEntity() {
            base.OnRemovedFromEntity();
            SmoothCamera Kamra = Entity.Scene.GetSceneComponent<SmoothCamera>();
            Kamra.Unregister(this);
        }

        private bool Always() {
            return true;
        }

        private Vector2 TrackEntity() {
            return Entity.Position;
        }
    }
}