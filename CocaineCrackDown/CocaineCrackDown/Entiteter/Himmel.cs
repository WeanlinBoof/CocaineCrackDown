using CocaineCrackDown.Scener;

using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown.Entiteter {
    public class Himmel : Entity {
        ScrollingSpriteRenderer ScrollHimmel;
        public Himmel() : base("Himmel") {

        }

        public override void OnAddedToScene() {
            base.OnAddedToScene();
            Transform.Position = Scene.SceneRenderTargetSize.ToVector2() / 2;
            Texture2D Himmel = Scene.Content.LoadTexture("Content/himmel.png");
            // Load background sprite (scrolling background with a specific speed)
            ScrollHimmel = new ScrollingSpriteRenderer(Himmel) { ScrollSpeedX = 25f, };

            // Add sprite component with renderLayer to 2 (in this way, background renders are in the back of screen)
            AddComponent(ScrollHimmel).RenderLayer = 2;
        }

        public override void Update() {
            base.Update();

            // If it is GameOver, stop background scrolling
            if ((Scene as GrundScen).Status != NivåNamn.ScenEtt) {
                ScrollHimmel.ScrollSpeedX = 0;
            }
        }
    }
}
