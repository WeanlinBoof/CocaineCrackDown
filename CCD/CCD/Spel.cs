using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace CCD {
    public class Spel : Core {
        public Spel() : base(960,540,false,"Cocaine Crackdown") {
        }
        protected override void Initialize() {
            base.Initialize();
            Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
            Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
            Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
            dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
            Scene = sceneEtt;
        }
    }
}
