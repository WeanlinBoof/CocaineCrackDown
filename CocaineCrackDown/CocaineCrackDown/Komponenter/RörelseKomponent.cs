using System;
using System.Text;

using CocaineCrackDown.Client.Managers;
using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace CocaineCrackDown.Komponenter {
    public class RörelseKomponent : Component {
        public SubpixelVector2 V2Pixel;
        public Röraren Röraren;
        public float RörelseHastighet;
        public InmatningsHanterare Inmatnings;
        private Spelare Spelare;
        public RörelseKomponent(float RörHastighet,Spelare spelare,InmatningsHanterare InHatterare = null ) {
            RörelseHastighet = RörHastighet;
            Inmatnings = InHatterare;
            Spelare = spelare;
        }
        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            Röraren = Entity.AddComponent(new Röraren(Spelare.Map.Karta));

        }
    }
}
