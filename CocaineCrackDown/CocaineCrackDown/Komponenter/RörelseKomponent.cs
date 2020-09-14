using System;
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
    public class RörelseKomponent : Component, IUpdatable {
        private SubpixelVector2 V2Pixel;
        private Mover Röraren;
        public float RörelseHastighet;
        public InmatningsHanterare Inmatnings;
        public TiledMap TM;
        public RörelseKomponent(InmatningsHanterare InHatterare , float RörHastighet) {
            RörelseHastighet = RörHastighet;
            Inmatnings = InHatterare;
        }
        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            Röraren = Entity.AddComponent(new Mover());            
        }

        public void Update() {
            Vector2 moveDir = new Vector2(Inmatnings.RörelseAxelX.Value , Inmatnings.RörelseAxelY.Value);
            if(moveDir != Vector2.Zero) {

                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                Röraren.CalculateMovement(ref movement , out CollisionResult res );
                V2Pixel.Update(ref movement);
                Röraren.ApplyMovement(movement);
            }
        }
    }
}
