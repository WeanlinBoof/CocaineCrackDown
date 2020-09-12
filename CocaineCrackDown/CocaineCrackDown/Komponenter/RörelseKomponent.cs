using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Client.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Komponenter {
    public class RörelseKomponent : Component, IUpdatable, ITriggerListener {
        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();
        private BoxCollider BoxKollision;
        private Mover Röraren;
        public float RörelseHastighet;
        public InmatningsHanterare Inmatnings;
        public RörelseKomponent(InmatningsHanterare InHatterare , float RörHastighet) {
            RörelseHastighet = RörHastighet;
            Inmatnings = InHatterare;
        }
        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            BoxKollision = Entity.AddComponent(new BoxCollider(-20 , -31 , 40 , 63));
            Röraren = Entity.AddComponent(new Mover());
        }

        public void Update() {
            Vector2 moveDir = new Vector2(Inmatnings.RörelseAxelX.Value , Inmatnings.RörelseAxelY.Value);
            if(moveDir != Vector2.Zero) {

                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                Röraren.CalculateMovement(ref movement , out CollisionResult res);
                SubPixelVecTvå.Update(ref movement);
                Röraren.ApplyMovement(movement);
            }
        }

        public void OnTriggerEnter(Collider other , Collider local) {
            Console.WriteLine("Enter");
        }

        public void OnTriggerExit(Collider other , Collider local) {
            Console.WriteLine("Exit");
        }
    }
}
