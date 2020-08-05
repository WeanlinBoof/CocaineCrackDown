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

            Entity hejdukEnhet = CreateEntity("hejduk", new Vector2(Screen.Width / 4, Screen.Height / 4));

            //var HejdukKollision = hejdukEnhet.AddComponent(new BoxCollider(-20, -31, 40, 63));
            //hejdukEnhet.AddComponent(new Hejduk(HejdukKollision, "doug", EntitetRelation.Hjälte));

            hejdukEnhet.Update();

            Entity dougEnhet = CreateEntity("spelare", new Vector2(Screen.Width / 2, Screen.Height / 2));
            dougEnhet.SetTag(69);
            //dougEnhet.AddComponent(new DougSpelareEtt());
            dougEnhet.AddComponent(new DougSpelareEtt("doug", EntitetRelation.Hjälte));
            dougEnhet.Update();

            Entity randyEnhet = CreateEntity("spelaretvå", new Vector2(Screen.Width / 3, Screen.Height / 2));
            randyEnhet.AddComponent(new RandySpelareTvå("randy", EntitetRelation.Hjälte));
            randyEnhet.Update();

            ScenEtt.addEntityProcessor(KollisionsSystem);

        }
    }
}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
