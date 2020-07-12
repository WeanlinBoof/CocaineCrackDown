using CocaineCrackDown.Modeler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CocaineCrackDown.Objekter {

    //alla spelare ska ärva spelare
    public class Spelare : Kamrater {
        public float SkalaPåSpelarna = 1.0f;
        public float Höjd { get; set; }
        public float Bredd { get; set; }
        public float SkärmBredd { get; set; }
        public float SkärmHöjd { get; set; }
        public Riktning SpelarRiktning { get; set; }
        public string PlayerName { get; set; }
        public string Health { get; set; }

        private readonly float TimerNollStällare = 0f;
        private float TimerSekunderSpelareEtt = 0f;
        private float TimerSekunderSpelareTvå = 0f;
        private KeyboardState FöregåendeTangentbordStatus;
        private GamePadState FöregåendeKontrollerStatus;
        private KeyboardState NuvarandeTangentbordStatus;
        private GamePadState NuvarandeKontrollerStatus;

        ///////////////////////////////////////////////////////////////////////////
        public void Update(SpelareEtt SpelareEtt, SpelareTvå SpelareTvå, float RörelseHastighet, Inmatning.Tangentbord Tangentbord, Inmatning.Kontroller Kontroller, GameTime gameTime) {
            ///////////////////////////////////////////////////////////////////////////
            GamePadCapabilities SpelKontroller = GamePad.GetCapabilities(PlayerIndex.One);
            if (SpelKontroller.IsConnected) {
                FöregåendeKontrollerStatus = NuvarandeKontrollerStatus;
                NuvarandeKontrollerStatus = GamePad.GetState(PlayerIndex.One);
                ///////////////////////////////////////////////////////////////////////////
                if (NuvarandeKontrollerStatus.IsButtonDown(Kontroller.Up)) {
                    SpelareTvå.FlyttaUpp(RörelseHastighet);
                }
                ///////////////////////////////////////////////////////////////////////////
                if (NuvarandeKontrollerStatus.IsButtonDown(Kontroller.Down)) {
                    SpelareTvå.FlyttaNed(RörelseHastighet);
                }
                ///////////////////////////////////////////////////////////////////////////
                if (NuvarandeKontrollerStatus.IsButtonDown(Kontroller.Right)) {
                    SpelareTvå.FlyttaHöger(RörelseHastighet);
                }
                ///////////////////////////////////////////////////////////////////////////
                if (NuvarandeKontrollerStatus.IsButtonDown(Kontroller.Left)) {
                    SpelareTvå.FlyttaVänster(RörelseHastighet);
                }
                ///////////////////////////////////////////////////////////////////////////
                if (NuvarandeKontrollerStatus.IsButtonDown(Kontroller.Attack)) {
                    SpelareTvå.Attack(true);
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            FöregåendeTangentbordStatus = NuvarandeTangentbordStatus;
            NuvarandeTangentbordStatus = Keyboard.GetState();
            if (NuvarandeTangentbordStatus.IsKeyDown(Tangentbord.Up)) {
                SpelareEtt.FlyttaUpp(RörelseHastighet);
            }
            if (NuvarandeTangentbordStatus.IsKeyDown(Tangentbord.Down)) {
                SpelareEtt.FlyttaNed(RörelseHastighet);
            }
            if (NuvarandeTangentbordStatus.IsKeyDown(Tangentbord.Right)) {
                SpelareEtt.FlyttaHöger(RörelseHastighet);
            }
            if (NuvarandeTangentbordStatus.IsKeyDown(Tangentbord.Left)) {
                SpelareEtt.FlyttaVänster(RörelseHastighet);
            }
            if (NuvarandeTangentbordStatus.IsKeyDown(Tangentbord.Attack)) {
                SpelareEtt.Attack(true);
            }
            //////////////////////////////////////////////////////////////////
            //timer i sekunder som är ganska viktig
            TimerSekunderSpelareEtt += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerSekunderSpelareEtt > 0.5f) {
                SpelareEtt.Attack(false);
                //nollställer timern samt ger cooldown på attack som varar 0.2 sekunder
                if (TimerSekunderSpelareEtt > 0.7f) {
                    TimerSekunderSpelareEtt = TimerNollStällare;
                }
            }
            //////////////////////////////////////////////////////////////////////
            TimerSekunderSpelareTvå += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerSekunderSpelareTvå > 0.5f) {
                SpelareTvå.Attack(false);
                //nollställer timern samt ger cooldown på attack som varar 0.2 sekunder
                if (TimerSekunderSpelareTvå > 0.7f) {
                    TimerSekunderSpelareTvå = TimerNollStällare;
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void FlyttaVänster(float RörelseHastighet) {
            X -= RörelseHastighet;
            SpelarRiktning = Riktning.Vänster;
            if (X < 1) {
                X = 1;
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void FlyttaHöger(float RörelseHastighet) {
            X += RörelseHastighet;
            SpelarRiktning = Riktning.Höger;
            if (X + Bredd > SkärmBredd) {
                X = SkärmBredd - Bredd;
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void FlyttaUpp(float RörelseHastighet) {
            Y -= RörelseHastighet;
            if (Y < GolvTopSlut) {
                Y = GolvTopSlut;
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void FlyttaNed(float RörelseHastighet) {
            Y += RörelseHastighet;
            if (Y + Höjd > SkärmHöjd) {
                Y = SkärmHöjd - Höjd;
            }
        }
    }

    //spelare ett
    public class SpelareEtt : Spelare {
        private Texture2D SpelareEttTextur { get; set; }
        private Texture2D SpelareEttAttackTextur { get; set; }
        private readonly string SpelareEttNamn = "Doug";
        ///////////////////////////////////////////////////////////////////////////
        public SpelareEtt(float x, float y, float skärmbredd, float skärmhöjd, SpriteBatch spritebatch, SpelResurser SpelResurser) {
            X = x;
            Y = y;
            PlayerName = SpelareEttNamn;
            SpelareEttTextur = SpelResurser.DougNormalTextur;
            SpelareEttAttackTextur = SpelResurser.DougAttackTextur;
            //
            Höjd = SpelareEttTextur.Height * SkalaPåSpelarna;//SkalaPåSpelarna Är i klassen Spelare
            Bredd = SpelareEttTextur.Width * SkalaPåSpelarna;
            //
            SpriteBatch = spritebatch;
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            //                halva skärm            spelar textur höjd gånger skala delat på 8 gånger 7
            GolvTopSlut = skärmhöjd / 2 - SpelareEttTextur.Height * SkalaPåSpelarna / 8 * 7;
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Attack(bool AttackStatusSpelareEtt) {
            Attackerar = AttackStatusSpelareEtt;
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Rita() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth
            ///////////////////////////////////////////////////////////////////////////
            if (SpelarRiktning == Riktning.Höger) {
                ///////////////////////////////////////////////////////////////////////////
                if (Attackerar == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
                ///////////////////////////////////////////////////////////////////////////
                else {
                    SpriteBatch.Draw(SpelareEttTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            if (SpelarRiktning == Riktning.Vänster) {
                ///////////////////////////////////////////////////////////////////////////
                if (Attackerar == true) {
                    SpriteBatch.Draw(SpelareEttAttackTextur, new Vector2(X - (SpelareEttAttackTextur.Width - SpelareEttTextur.Width), Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
                ///////////////////////////////////////////////////////////////////////////
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
        private readonly string SpelareTvåNamn = "Randy";
        ///////////////////////////////////////////////////////////////////////////
        public SpelareTvå(float x, float y, float skärmbredd, float skärmhöjd, SpriteBatch spritebatch, SpelResurser SpelResurser) {
            X = x;
            Y = y;
            //
            PlayerName = SpelareTvåNamn;
            //
            SpelareTvåTextur = SpelResurser.RandyNormalTextur;
            SpelareTvåAttackTextur = SpelResurser.RandyAttackTextur;
            //
            Höjd = SpelareTvåTextur.Height * SkalaPåSpelarna;//SkalaPåSpelarna Är i klassen Spelare
            Bredd = SpelareTvåTextur.Width * SkalaPåSpelarna;
            //
            SpriteBatch = spritebatch;
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            //                halva skärm            spelar textur höjd gånger skala delat på 8 gånger 7
            GolvTopSlut = skärmhöjd / 2 - SpelareTvåTextur.Height * SkalaPåSpelarna / 8 * 7;
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Attack(bool AttackStatusSpelareTvå) {
            Attackerar = AttackStatusSpelareTvå;
        }
        ///////////////////////////////////////////////////////////////////////////
        public override void Rita() {  //bild      position       rectangel   färg  rotation  origin      skala   SpriteEffect    layerdeapth
            ///////////////////////////////////////////////////////////////////////////
            if (SpelarRiktning == Riktning.Höger) {
                ///////////////////////////////////////////////////////////////////////////
                if (Attackerar == true) {
                    SpriteBatch.Draw(SpelareTvåAttackTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
                ///////////////////////////////////////////////////////////////////////////
                else {
                    SpriteBatch.Draw(SpelareTvåTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.None, 0);
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            if (SpelarRiktning == Riktning.Vänster) {
                ///////////////////////////////////////////////////////////////////////////
                if (Attackerar == true) {
                    SpriteBatch.Draw(SpelareTvåAttackTextur, new Vector2(X - (SpelareTvåAttackTextur.Width - SpelareTvåTextur.Width), Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
                ///////////////////////////////////////////////////////////////////////////
                else {
                    SpriteBatch.Draw(SpelareTvåTextur, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), SkalaPåSpelarna, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }
    }
}