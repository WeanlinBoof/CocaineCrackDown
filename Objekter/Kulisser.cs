
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown.Objekter {
    public class Kulisser : Objekt {
        public float Höjd { get; set; }
        public float Bredd { get; set; }

    }
    ///////////////////////////////////////////////////////////////////////////
    public class Himmel : Kulisser {
        private Texture2D HimmelTextur { get; set; }
        public Himmel(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            HimmelTextur = spelresurser.HimmelTextur;
            Höjd = HimmelTextur.Height;
            Bredd = HimmelTextur.Width;
            SpriteBatch = spritebatch;
        }
        public override void Rita() {
            SpriteBatch.Draw(HimmelTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd, Spel.SkärmHöjd), new Rectangle(0, 0, HimmelTextur.Width, HimmelTextur.Height), Color.White);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    public class Bakgrund : Kulisser {
        private Texture2D BakgrundTextur { get; set; }
        public Bakgrund(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            BakgrundTextur = spelresurser.BakgrundTextur;
            Höjd = BakgrundTextur.Height;
            Bredd = BakgrundTextur.Width;
            SpriteBatch = spritebatch;
        }
        public override void Rita() {
            SpriteBatch.Draw(BakgrundTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd, Spel.SkärmHöjd + (BakgrundTextur.Height / 2) - Spel.SkärmHöjd), new Rectangle(0, 0, BakgrundTextur.Width, BakgrundTextur.Height), Color.White);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    public class Golv : Kulisser {
        private Texture2D GolvTextur { get; set; }
        public Golv(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            GolvTextur = spelresurser.GolvTextur;
            Höjd = GolvTextur.Height;
            Bredd = GolvTextur.Width;
            SpriteBatch = spritebatch;
        }
        public override void Rita() {
            //SpriteBatch.Draw(GolvTextur, new Vector2(Position.X, Position.Y), new Rectangle(0, 0, GolvTextur.Width, GolvTextur.Height), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
            SpriteBatch.Draw(GolvTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd,Spel.SkärmHöjd + (GolvTextur.Height / 2) - Spel.SkärmHöjd), new Rectangle(0, 0, GolvTextur.Width, GolvTextur.Height), Color.White);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    public class GolvRutor {
        public int SkärmBredd { get; set; }
        public int SkärmHöjd { get; set; }
        public Golv[,] GolvRuta { get; set; }
        private readonly float GolvX;
        private readonly float GolvY;
        public GolvRutor(float x, float y, int skärmbredd, int skärmhöjd, SpriteBatch spritebatch, SpelResurser spelresurser) {
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            GolvRuta = new Golv[SkärmBredd, SkärmHöjd];
            GolvX = x;
            GolvY = y;
            for (int i = 0; i < SkärmBredd; i++) {
                GolvY = y + i * spelresurser.GolvTextur.Height;
                for (int j = 0; j < SkärmHöjd; j++) {
                    GolvX = x + j * spelresurser.GolvTextur.Width;
                    Golv golv = new Golv(GolvX, GolvY, spritebatch, spelresurser);
                    GolvRuta[i, j] = golv;
                }
            }
        }
        public void Rita() {
            for (int i = 0; i < SkärmBredd; i++) {
                for (int j = 0; j < SkärmHöjd; j++) {
                    GolvRuta[i, j].Rita();
                }
            }
        }
    }
}
