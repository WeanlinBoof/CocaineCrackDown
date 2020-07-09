using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown {
    public class Kulisser {
        public float Höjd { get; set; }
        public float Bredd { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public SpriteBatch SpriteBatch;
    }
    public class Golv : Kulisser {
        private Texture2D GolvTextur { get; set; }
        public Golv(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            X = x;
            Y = y;
            GolvTextur = spelresurser.GolvTextur;
            Höjd = GolvTextur.Height;
            Bredd = GolvTextur.Width;
            SpriteBatch = spritebatch;
        }
        public void Draw() {
            SpriteBatch.Draw(GolvTextur, new Vector2(X, Y), null, Color.DimGray, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
        }
    }
    public class Bakgrund : Kulisser {
        private Texture2D BakgrundTextur { get; set; }
        public Bakgrund(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            X = x;
            Y = y;
            BakgrundTextur = spelresurser.BakgrundTextur;
            Höjd = BakgrundTextur.Height;
            Bredd = BakgrundTextur.Width;
            SpriteBatch = spritebatch;
        }
        public void Draw() {
            SpriteBatch.Draw(BakgrundTextur, new Vector2(X, Y), null, Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
        }
    }
    public class Himmel : Kulisser {
        private Texture2D HimmelTextur { get; set; }
        public Himmel(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            X = x;
            Y = y;
            HimmelTextur = spelresurser.HimmelTextur;
            Höjd = HimmelTextur.Height;
            Bredd = HimmelTextur.Width;
            SpriteBatch = spritebatch;
        }
        public void Draw() {
            SpriteBatch.Draw(HimmelTextur, new Vector2(X, Y), null, Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
        }
    }
    public class Rutor {
        public int SkärmBredd { get; set; }
        public int SkärmHöjd { get; set; }
        public Golv[,] GolvRuta { get; set; }
        private readonly float GolvX;
        private readonly float GolvY;
        public Rutor(float x, float y,int skärmbredd, int skärmhöjd, SpriteBatch spritebatch, SpelResurser spelresurser) {
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            GolvRuta = new Golv[SkärmBredd, SkärmHöjd];
            GolvX = x;
            GolvY = y;
            for (int i = 0; i < SkärmBredd; i++) {
                GolvY = y + (i * spelresurser.GolvTextur.Height);
                for (int j = 0; j < SkärmHöjd; j++) {
                    GolvX = x + (j * spelresurser.GolvTextur.Width);
                    Golv golv = new Golv(GolvX, GolvY, spritebatch, spelresurser);
                    GolvRuta[i, j] = golv;
                }
            }
        }
        public void Draw() {
            for (int i = 0; i < SkärmBredd; i++) {
                for (int j = 0; j < SkärmHöjd; j++) {
                    GolvRuta[i, j].Draw();
                }
            }
        }
    }
}
