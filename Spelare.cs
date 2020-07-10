using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown {
    //alla spelare ska ärva spelare
    public class Spelare : Kamrater {
        public float SkalaPåSpelarna = 1.0f;
        public float Höjd { get; set; }
        public float Bredd { get; set; }
        public float SkärmBredd { get; set; }
        public float SkärmHöjd { get; set; }
        public Riktning SpelarRiktning { get; set; }

        public override void FlyttaVänster(float RörelseHastighet) {
            X -= RörelseHastighet;
            SpelarRiktning = Riktning.Vänster;
            if (X < 1) {
                X = 1;
            }
        }
        public override void FlyttaHöger(float RörelseHastighet) {
            X += RörelseHastighet;
            SpelarRiktning = Riktning.Höger;
            if ((X + Bredd) > SkärmBredd) {
                X = SkärmBredd - Bredd;
            }
        }
        public override void FlyttaUpp(float RörelseHastighet) {
            Y -= RörelseHastighet;
            if (Y < GolvTopSlut) {
                Y = GolvTopSlut;
            }
        }
        public override void FlyttaNed(float RörelseHastighet) {
            Y += RörelseHastighet;
            if ((Y + Höjd) > SkärmHöjd) {
                Y = SkärmHöjd - Höjd;
            }
        }
    }
    //spelare ett
    public class SpelareEtt : Spelare {
        private Texture2D SpelareEttTextur { get; set; }
        private Texture2D SpelareEttAttackTextur { get; set; }

        public SpelareEtt(float x, float y, float skärmbredd, float skärmhöjd, SpriteBatch spritebatch, SpelResurser SpelResurser) {
            X = x;
            Y = y;

            SpelareEttTextur = SpelResurser.DougNormalTextur;
            SpelareEttAttackTextur = SpelResurser.DougAttackTextur;
            //
            Höjd = (SpelareEttTextur.Height * SkalaPåSpelarna);//SkalaPåSpelarna Är i klassen Spelare
            Bredd = (SpelareEttTextur.Width * SkalaPåSpelarna);
            //
            SpriteBatch = spritebatch;
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            //                halva skärm            spelar textur höjd gånger skala delat på 8 gånger 7
            GolvTopSlut = (skärmhöjd / 2) - (SpelareEttTextur.Height * SkalaPåSpelarna / 8 * 7);

        }
        public override void Attack(bool AttackStatusSpelareEtt) {
            AttackStatus = AttackStatusSpelareEtt;
        }
        public override void Draw() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth

            if (SpelarRiktning == Riktning.Höger) {

                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }

                else {
                    SpriteBatch.Draw(SpelareEttTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
            }

            if (SpelarRiktning == Riktning.Vänster) {

                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }

                else {
                    SpriteBatch.Draw(SpelareEttTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }
    }
    //spelare två
    public class SpelareTvå : Spelare {
        private Texture2D SpelareTvåTextur { get; set; }
        private Texture2D SpelareTvåAttackTextur { get; set; }

        public SpelareTvå(float x, float y, float skärmbredd, float skärmhöjd, SpriteBatch spritebatch, SpelResurser SpelResurser) {
            X = x;
            Y = y;

            SpelareTvåTextur = SpelResurser.RandyNormalTextur;
            SpelareTvåAttackTextur = SpelResurser.RandyAttackTextur;
            //
            Höjd = (SpelareTvåTextur.Height * SkalaPåSpelarna);//SkalaPåSpelarna Är i klassen Spelare
            Bredd = (SpelareTvåTextur.Width * SkalaPåSpelarna);
            //
            SpriteBatch = spritebatch;
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            //                halva skärm            spelar textur höjd gånger skala delat på 8 gånger 7
            GolvTopSlut = (skärmhöjd / 2) - (SpelareTvåTextur.Height * SkalaPåSpelarna / 8 * 7);

        }
        public override void Attack(bool AttackStatusSpelareTvå) {
            AttackStatus = AttackStatusSpelareTvå;
        }
        public override void Draw() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth

            if (SpelarRiktning == Riktning.Höger) {

                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareTvåAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }

                else {
                    SpriteBatch.Draw(SpelareTvåTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
            }
            if (SpelarRiktning == Riktning.Vänster) {

                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareTvåAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }

                else {
                    SpriteBatch.Draw(SpelareTvåTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }
    }
}
