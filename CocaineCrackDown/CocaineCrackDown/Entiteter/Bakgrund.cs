using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {

    public class Bakgrund : Entity {

        private ScrollingSpriteRenderer ScrollBakgrund;

        public Bakgrund() : base("Bakgrund") {
        }

        public override void OnAddedToScene() {
            base.OnAddedToScene();

            // Load scrolling sprite
            Texture2D BakgrundTextur = Scene.Content.LoadTexture("Content/bakgrund.png");
            ScrollBakgrund = new ScrollingSpriteRenderer(BakgrundTextur) { ScrollSpeedX = 10, };
            AddComponent(ScrollBakgrund);
            //AddComponent(new BoxCollider(Scene.SceneRenderTargetSize.X, Scene.SceneRenderTargetSize.Y)).IsTrigger = true;

            // Position
            //Transform.Position = Scene.SceneRenderTargetSize.ToVector2() / 2;
            //                                  Dublar storleken på den             Samma här                   20 är helften av 40 som den normalt var venne varför det är så
            Transform.Position = new Vector2(Scene.SceneRenderTargetSize.X / 2, (Scene.SceneRenderTargetSize.Y / 2) + 20);
        }

        public override void Update() {
            base.Update();

            // Move colliders together with ScrollingSprite
            if ((Scene as GrundScen).Status != NivåNamn.ScenEtt) {
                ScrollBakgrund.ScrollSpeedX = 0;
            }
        }
    }
}