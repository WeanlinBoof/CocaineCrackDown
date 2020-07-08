using Microsoft.Xna.Framework;
﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CocaineCrackDown {
    //classen som inhåller alla spel resurser för tillfället
    public class Spelare {
        public float X { get; set; }
        public float Y { get; set; }
        public float Höjd { get; set; }
        public float Bredd { get; set; }
        public float SkärmBredd { get; set; }
        public float SkärmHöjd { get; set; }
        private Texture2D Spelarebild { get; set; }
        private Texture2D SpelarebildAttack { get; set; }
        private readonly SpriteBatch SpriteBatch;
        private int kuk = 0;
        private bool AttackState;

        public Spelare(float x, float y, float skärmbredd,float skärmhöjd, SpriteBatch spritebatch, SpelResurser spelresurs) {
            X = x;
            Y = y;
            Spelarebild = spelresurs.SpelareEttNormalTextur;
            SpelarebildAttack = spelresurs.SpelareEttAttackTextur;
            Höjd = Spelarebild.Height;
            Bredd = Spelarebild.Width;
            SpriteBatch = spritebatch;
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
        }
        public void AttackStatus(bool _AttackState){
            AttackState = _AttackState;
        }
        public void Draw() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth
            if(AttackState == true){
                SpriteBatch.Draw(SpelarebildAttack, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            }else {
                SpriteBatch.Draw(Spelarebild, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            }
        }
        public void GåVänster(float RörelseHastighet) {
            X -= RörelseHastighet;
            if (X < 1) {
                X = 1;
            }
        }
        public void GåHöger(float RörelseHastighet) {
            X += RörelseHastighet;
            if ((X + Bredd) > SkärmBredd) {
                X = SkärmBredd - Bredd;
            }
        }
        public void GåUpp(float RörelseHastighet) {
            Y -= RörelseHastighet;
            if (Y < 1) {
                Y = 1;
            }
        }
        public void GåNed(float RörelseHastighet) {
            Y += RörelseHastighet;
            if ((Y + Höjd) > SkärmHöjd) {
                Y = SkärmHöjd - Höjd;
            }
        }
    }
}
