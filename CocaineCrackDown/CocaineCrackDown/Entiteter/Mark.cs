using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {
    public class Mark : Entity {
        private SpriteRenderer MarkSprite;

        public Mark() : base("Mark") {
        }

        public override void OnAddedToScene() {
            base.OnAddedToScene();

            // Load scrolling sprite
            Texture2D MarkTextur = Scene.Content.LoadTexture("Content/mark.png");
            MarkSprite = new SpriteRenderer(MarkTextur);
            AddComponent(MarkSprite);

            //AddComponent(new BoxCollider(Scene.SceneRenderTargetSize.X, Scene.SceneRenderTargetSize.Y)).IsTrigger = true;

            // Position
            //Transform.Position = Scene.SceneRenderTargetSize.ToVector2() / 2;

            Transform.Position = new Vector2(Scene.SceneRenderTargetSize.X / 2, (Scene.SceneRenderTargetSize.Y / 1.25f));
        }

        public override void Update() {
            base.Update();
        }
    }
}