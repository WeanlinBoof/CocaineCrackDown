using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

﻿using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {
    class ScenEtt : StandardScen {

        public ScenEtt() { }

        public override void Initialize() {
            ////////////////////////
            SetupScene();/////glöm ej bort denna brug
            ///////////////////////
            Entity dougEnhet = CreateEntity("spelare", new Vector2(Screen.Width / 2, Screen.Height / 2));
            dougEnhet.AddComponent(new DougSpelareEtt());
            dougEnhet.Update();

            Entity randyEnhet = CreateEntity("spelaretvå", new Vector2(Screen.Width / 3, Screen.Height / 2));
            randyEnhet.AddComponent(new RandySpelareTvå());
            randyEnhet.Update();
        }
    }

}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
