using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CocaineCrackDown {
    // alla människor ska ärva från denna klass
    public class Människa {
        public float X { get; set; }
        public float Y { get; set; }
        public SpriteBatch SpriteBatch;
        public float GolvTopSlut { get; set; }

        public enum riktning {
            höger,
            vänster
        }

    }



    //alla fiender ska ärva av denna klass
    public class Fiende : Människa {

    }


    public class Hjälte : Människa {

    }

    public class Spelare : Hjälte {
        //////////////////////////////////////////
        public float SkalaPåSpelarna = 1.0f;
        //////////////////////////////////////////
        public float Höjd { get; set; }
        public float Bredd { get; set; }
        public float SkärmBredd { get; set; }
        public float SkärmHöjd { get; set; }
        public bool AttackStatus { get; set; }
        public riktning SpelarRiktning { get; set; }
        public void GåVänster(float RörelseHastighet) {
            X -= RörelseHastighet;
            if (X < 1) {
                X = 1;
                SpelarRiktning = riktning.vänster;
            }
        }
        public void GåHöger(float RörelseHastighet) {
            X += RörelseHastighet;
            if ((X + Bredd) > SkärmBredd) {
                X = SkärmBredd - Bredd;
                SpelarRiktning = riktning.höger;
            }
        }
        public void GåUpp(float RörelseHastighet) {
            Y -= RörelseHastighet;
            if (Y < GolvTopSlut) {
                Y = GolvTopSlut;
            }
        }
        public void GåNed(float RörelseHastighet) {
            Y += RörelseHastighet;
            if ((Y + Höjd) > SkärmHöjd) {
                Y = SkärmHöjd - Höjd;
            }
        }
    }
    //spelare ett SpelareTvå
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
        public void Attack(bool _AttackStatus){
            AttackStatus = _AttackStatus;
        }
        public void Draw() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth
            if(SpelarRiktning == riktning.höger) {
                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X, Y), null, Color.DodgerBlue, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
                else {
                    SpriteBatch.Draw(SpelareEttTextur, new Vector2(X, Y), null, Color.DodgerBlue, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
            }else if(SpelarRiktning == riktning.vänster) {
                if (AttackStatus == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X, Y), null, Color.DodgerBlue, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
                else {
                    SpriteBatch.Draw(SpelareEttTextur, new Vector2(X, Y), null, Color.DodgerBlue, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }

    }
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
        public void Attack(bool _AttackStatus) {
            AttackStatus = _AttackStatus;
        }
        public void Draw() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth
            if (AttackStatus == true) {
                SpriteBatch.Draw(SpelareTvåAttackTextur, new Vector2(X, Y), null, Color.MediumVioletRed, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
            }
            else {
                SpriteBatch.Draw(SpelareTvåTextur, new Vector2(X, Y), null, Color.MediumVioletRed, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
            }
        }

    }
}
