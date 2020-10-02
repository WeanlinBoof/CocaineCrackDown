using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Client.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Komponenter {
    public class AtlasAnimationKomponent<T> : Component {
        public InmatningsHanterare Inmatnings;
        public const float AttackTimerNollstälare = 0f;
        public SpriteAtlas Atlas;
        //lägg till mer sen :)
        public BoxCollider StandardBox;
        public BoxCollider AttackBox;
        //public AtlasAnimationKomponent<T> anitmation;
        public SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;
        public bool Attackerar;
        public string animation = "doug-stilla";
        public Riktning Riktning;
        public float AttackTimer;

        public AtlasAnimationKomponent(InmatningsHanterare inmatnings) {
            Inmatnings = inmatnings;
        }

        public AtlasAnimationKomponent() : base() {
        }

        public override void OnAddedToEntity() {
            Atlas = Entity.Scene.Content.LoadSpriteAtlas($"Content/{Entity.Name}.atlas");
            Entity.Scene.Content.LoadTexture($"Content/{Entity.Name}.png");
            Animerare = Entity.AddComponent<SpriteAnimator>();
            Animerare.SetRenderLayer(1);
            StandardBox = Entity.AddComponent(new BoxCollider(-20 , -31 , 40 , 63));
            AttackBox = Entity.AddComponent(new BoxCollider(-20 , -31 , 50 , 31));
            AttackBox.IsTrigger = true;
            Animerare.AddAnimationsFromAtlas(Atlas);
        }

    }
}
