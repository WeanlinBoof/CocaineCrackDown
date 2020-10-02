using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Verktyg {

    public class KameraGränser : Component, IUpdatable {

        public Vector2 Min, Max;

        public KameraGränser() {

            // make sure we run last so the camera is already moved before we evaluate its position
            SetUpdateOrder(int.MaxValue);
        }

        public KameraGränser(Vector2 min , Vector2 max) : this() {
            Min = min;
            Max = max;
        }

        public override void OnAddedToEntity() {
            Entity.UpdateOrder = int.MaxValue;
        }

        void IUpdatable.Update() {
            var KameraGränser = Entity.Scene.Camera.Bounds;

            if(KameraGränser.Top < Min.Y)
                Entity.Scene.Camera.Position += new Vector2(0 , Min.Y - KameraGränser.Top);

            if(KameraGränser.Left < Min.X)
                Entity.Scene.Camera.Position += new Vector2(Min.X - KameraGränser.Left , 0);

            if(KameraGränser.Bottom > Max.Y)
                Entity.Scene.Camera.Position += new Vector2(0 , Max.Y - KameraGränser.Bottom);

            if(KameraGränser.Right > Max.X)
                Entity.Scene.Camera.Position += new Vector2(Max.X - KameraGränser.Right , 0);
        }
    }
}