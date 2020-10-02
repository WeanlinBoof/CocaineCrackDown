using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Client.Managers;
using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace CocaineCrackDown.Komponenter {
    public class RörelseKomponent : Component {
        public SubpixelVector2 V2Pixel;
        public Mover Röraren;
        public float RörelseHastighet;
        public InmatningsHanterare Inmatnings;
        public Karta TM;
        public RörelseKomponent(InmatningsHanterare InHatterare , float RörHastighet) {
            RörelseHastighet = RörHastighet;
            Inmatnings = InHatterare;
        }
        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            Röraren = Entity.AddComponent(new Mover());            
        }
    }
}
