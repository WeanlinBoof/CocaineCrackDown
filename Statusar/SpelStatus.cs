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

        private GolvRutor Golv;
        private float GolvX;
        private float GolvY;

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
            //spelare ett
            SpelareEttX = (Spel.SkärmBredd - Spel.SkärmBredd + SpelResurser.DougNormalTextur.Width) / 2;
            SpelareEttY = (Spel.SkärmHöjd - SpelResurser.DougNormalTextur.Height) / 6 * 5;
            SpelareEtt = new SpelareEtt(SpelareEttX, SpelareEttY, Spel.SkärmBredd, Spel.SkärmHöjd, spriteBatch, SpelResurser);
            ///////////////////////////////////////////////////////////////////////////
            SpelareTvåX = (Spel.SkärmBredd - Spel.SkärmBredd + SpelResurser.RandyNormalTextur.Width) / 2;
            SpelareTvåY = (Spel.SkärmHöjd - SpelResurser.RandyNormalTextur.Height) / 6 * 3;
            SpelareTvå = new SpelareTvå(SpelareTvåX, SpelareTvåY, Spel.SkärmBredd, Spel.SkärmHöjd, spriteBatch, SpelResurser);
            ///////////////////////////////////////////////////////////////////////////
            GolvY = Spel.SkärmHöjd / 2;
            GolvX = 0;
            Golv = new GolvRutor(GolvX, GolvY, Spel.SkärmBredd, Spel.SkärmHöjd, spriteBatch, SpelResurser);
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