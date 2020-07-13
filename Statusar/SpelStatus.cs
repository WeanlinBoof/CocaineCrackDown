using CocaineCrackDown.Modeler;
using CocaineCrackDown.Objekter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CocaineCrackDown.Statusar {

    public class SpelStatus : Status {
        public int PlayerCount;
        private readonly SpelResurser SpelResurser;
        private float FörflutenTid;
        private float RörelseHastighet;

        private readonly Spelare Spelare = new Spelare();

        private SpelareEtt SpelareEtt;
        private float SpelareEttX;
        private float SpelareEttY;

        private SpelareTvå SpelareTvå;
        private float SpelareTvåX;
        private float SpelareTvåY;

        private Golv Golv;
        private float GolvX;
        private float GolvY;

        private Bakgrund Back;
        private float BackX;
        private float BackY;

        private Himmel Himmel;
        private float HimmelX;
        private float HimmelY;



        private readonly SpriteBatch spriteBatch;
        ///////////////////////////////////////////////////////////////////////////
        public Inmatning.Tangentbord Tangentbord = new Inmatning.Tangentbord() {
            Up = Keys.Up,
            Down = Keys.Down,
            Right = Keys.Right,
            Left = Keys.Left,
            Attack = Keys.Space
        };
        ///////////////////////////////////////////////////////////////////////////
        public Inmatning.Kontroller Kontroller = new Inmatning.Kontroller() {
            Up = Buttons.LeftThumbstickUp,
            Down = Buttons.LeftThumbstickDown,
            Right = Buttons.LeftThumbstickRight,
            Left = Buttons.LeftThumbstickLeft,
            Attack = Buttons.A
        };
        ///////////////////////////////////////////////////////////////////////////
        public SpelStatus(Spel spel, ContentManager content) : base(spel, content) {
            SpelResurser = new SpelResurser(content);
            spriteBatch = spel.spriteBatch;
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void LaddaResurser() {
            ///////////////////////////////////////////
            HimmelY = 0;
            HimmelX = 0;
            Himmel = new Himmel(HimmelX, HimmelY, spriteBatch, SpelResurser);
            ///////////////////////////////////////////////////////////////
            BackY = 40;
            BackX = 0;
            Back = new Bakgrund(BackX, BackY, spriteBatch, SpelResurser);
            ///////////////////////////////////////////////////////////////////////////
            GolvY = Spel.SkärmHöjd - SpelResurser.GolvTextur.Height / 2;
            GolvX = 0;
            Golv = new Golv(GolvX, GolvY, spriteBatch, SpelResurser);
            //spelare ett
            SpelareEttX = (Spel.SkärmBredd - Spel.SkärmBredd + SpelResurser.DougNormalTextur.Width) / 2;
            SpelareEttY = (Spel.SkärmHöjd - SpelResurser.DougNormalTextur.Height * 2);
            SpelareEtt = new SpelareEtt(SpelareEttX, SpelareEttY, Spel.SkärmBredd, Spel.SkärmHöjd, spriteBatch, SpelResurser);
            ///////////////////////////////////////////////////////////////////////////
            SpelareTvåX = (Spel.SkärmBredd - Spel.SkärmBredd + SpelResurser.RandyNormalTextur.Width) / 2 + 100;
            SpelareTvåY = (Spel.SkärmHöjd - SpelResurser.RandyNormalTextur.Height * 2 - 100);
            SpelareTvå = new SpelareTvå(SpelareTvåX, SpelareTvåY, Spel.SkärmBredd, Spel.SkärmHöjd, spriteBatch, SpelResurser);

        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Uppdatera(GameTime gameTime) {
            // förflutenTid är mängden millisekunder som har förflutit sen start
            FörflutenTid = gameTime.ElapsedGameTime.Milliseconds;
            // förflutenTid dellat det numer som står är likamed rörelseHastigheten
            RörelseHastighet = FörflutenTid / 3.7f;
            Spelare.Update(SpelareEtt, SpelareTvå, RörelseHastighet, Tangentbord, Kontroller, gameTime);


        }
        ///////////////////////////////////////////////////////////////////////////
        public override void EfterUppdatering(GameTime gameTime) {
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Rita(GameTime gameTime) {
            // början på spriteBatch
            spriteBatch.Begin();
            Himmel.Rita();
            Back.Rita();
            //////////////////////////
            Golv.Rita();
            //////////////////////////
            SpelareEtt.Rita();
            //////////////////////////
            SpelareTvå.Rita();

            // slut på spriteBatch
            spriteBatch.End();
        }
    }
}
