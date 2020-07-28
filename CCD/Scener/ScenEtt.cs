using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

﻿using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {
    class ScenEtt : StandardScen {

        public ScenEtt() { }

        public override void Initialize() {
            Entity dougEnhet = CreateEntity("spelare", new Vector2(Screen.Width / 2, Screen.Height / 2));
            dougEnhet.AddComponent(new DougSpelareEtt());
        }
    }

}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
