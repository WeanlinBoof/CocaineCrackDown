using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace CocaineCrackDown {

    public class Spel : Game {
        //dessa borde sorteras för att underlätta allt för oss vvv

        private readonly GraphicsDeviceManager Grafiker;
        private SpriteBatch SpriteBatch;
        private SpelResurser SpelResurser;
        private readonly string Titel = "CocaineCrackDown";
        private int SkärmBredd = 0;
        private int SkärmHöjd = 0;
        private MouseState FöregåendeMusStatus;
        private KeyboardState FöregåendeTangentbordStatus;
        private GamePadState FöregåendeKontrollerStatus;
        private float FörflutenTid;
        private float RörelseHastighet;

        private SpelareEtt SpelareEtt;
        private float SpelareEttX;
        private float SpelareEttY;

        private SpelareTvå SpelareTvå;
        private float SpelareTvåX;
        private float SpelareTvåY;

        private readonly float TimerNollStällare = 0f;
        private float TimerSekunderSpelareEtt = 0f;
        private float TimerSekunderSpelareTvå = 0f;

        private Rutor Golv;
        private float GolvX;
        private float GolvY;

        private bool SpelareTvåAktiverad = false;
        //dessa borde sorteras för att underlätta allt för oss ^^^

        // class konstruktör
        public Spel() {
            // gör så vi har grafiker
            Grafiker = new GraphicsDeviceManager(this);

            // säger vart resurserna hållerhus
            Content.RootDirectory = "Content";

            // gör muspekaren är synlig
            IsMouseVisible = true;
        }

        //initialiserar skit inan allt börjar
        protected override void Initialize() {
            // fixar titel samt uplösning
            TitelUpplösning();

            // uppdaterar spelet 300 gånger per sekund istället för varje frame
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 300.0f);

            // fixar en icke framerate beroende spel
            IsFixedTimeStep = true;

            // inte säker var ej jag som la den där
            base.Initialize();

            // Aktiverar de ändringar man gör i grafiken
            ÄndraGrafik();
        }

        //laddar in resurser
        protected override void LoadContent() {
            // inte säker var ej jag som la den där
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            // inte säker var ej jag som la den där
            SpelResurser = new SpelResurser(Content);

            //spelare ett
            SpelareEttX = (SkärmBredd - SkärmBredd + SpelResurser.DougNormalTextur.Width) / 2;
            SpelareEttY = (SkärmHöjd - SpelResurser.DougNormalTextur.Height) / 6 * 5;
            SpelareEtt = new SpelareEtt(SpelareEttX, SpelareEttY, SkärmBredd, SkärmHöjd, SpriteBatch, SpelResurser);
            //////////////
            SpelareTvåX = (SkärmBredd - SkärmBredd + SpelResurser.RandyNormalTextur.Width) / 2;
            SpelareTvåY = (SkärmHöjd - SpelResurser.RandyNormalTextur.Height) / 6 * 3;
            SpelareTvå = new SpelareTvå(SpelareTvåX, SpelareTvåY, SkärmBredd, SkärmHöjd, SpriteBatch, SpelResurser);
            

            //////////////
            GolvY = SkärmHöjd / 2;
            GolvX = 0;
            Golv = new Rutor(GolvX, GolvY, SkärmBredd, SkärmHöjd, SpriteBatch, SpelResurser);
        }

        //uppdaterar
        protected override void Update(GameTime gameTime) {
            // förflutenTid är mängden millisekunder som har förflutit sen start
            FörflutenTid = gameTime.ElapsedGameTime.Milliseconds;

            // förflutenTid dellat det numer som står är likamed rörelseHastigheten
            RörelseHastighet = FörflutenTid / 5.5f;

            //timer i sekunder som är ganska viktig
            TimerSekunderSpelareEtt += (float)gameTime.ElapsedGameTime.TotalSeconds;

            TimerSekunderSpelareTvå += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // gör så att spelet inte tar input om spelet ej är fokuserat T.ex om man alt+tab ut från spelet
            if (IsActive != false) {
                // Metod För Tangentbords inmatning
                InmatningTangentbord(RörelseHastighet, TimerSekunderSpelareEtt);

                //Metod För Mus inmatning
                InmatningMus();

                //xbox kontroller
                InmatningXboxKontroller(RörelseHastighet, TimerSekunderSpelareEtt);

            }

            //alla if statements om timer sekunder går till denna metod
            IfTimerSekunder();

            // inte säker var ej jag som la den där
            base.Update(gameTime);
        }

        

        //Ritar texturer btw det som ska rittas på skärm ska läggas mellan spriteBatch.Begin() & spriteBatch.End()
        protected override void Draw(GameTime gameTime) {
            // inte säker var ej jag som la den där
            GraphicsDevice.Clear(Color.PeachPuff);

            // början på spriteBatch
            SpriteBatch.Begin();
            Golv.Draw();
            //Rittar spelare ett på skärm
            SpelareEtt.Draw();
            if (SpelareTvåAktiverad == true) {
                SpelareTvå.Draw();
            }
            // slut på spriteBatch
            SpriteBatch.End();

            // inte säker var ej jag som la den där
            base.Draw(gameTime);
        }

        #region Skapade Metoder

        #region Metoder som används inom Initialize

        //Titel Och Den tillfälliga Upplösning för vi inte har någon sätt att ändra instälningar på än
        private void TitelUpplösning() {
            //sätter titel på spel fönstret
            Window.Title = Titel;

            // halva skärmhöjden
            SkärmHöjd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;

            // halva skärmbredden
            SkärmBredd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
        }

        //Aktiverar Grafik Ändringen/ar som gjorts
        private void ÄndraGrafik() {
            //bufferbredd = uplösning som är vald
            Grafiker.PreferredBackBufferWidth = SkärmBredd;

            //bufferhöjd = uplösning som är vald
            Grafiker.PreferredBackBufferHeight = SkärmHöjd;

            // Applicerar ändringar bruh
            Grafiker.ApplyChanges();
        }

        #endregion Metoder som används inom Initialize

        #region Metoder som används inom Update

        // Inmatning Från Mus skulle kunna ta in muskänslighet liknade till hur InmatningTangentbord tar rörelseHastighet InmatningTangentbord(float rörelseHastighet)
        private void InmatningMus() {
            //uhhh ja årkar inte tänka hur man förklarar
            MouseState nuvarandeMusStatus = Mouse.GetState();

            //kollar om mus X possiton inte är den gammla
            if (FöregåendeMusStatus.X != nuvarandeMusStatus.X) {
                //kollar om mus X possition är innanför skärmen
                if (nuvarandeMusStatus.X >= 0 && nuvarandeMusStatus.X < SkärmBredd && nuvarandeMusStatus.Y >= 0 && nuvarandeMusStatus.Y < SkärmHöjd) {
                    //implementera något här
                }
            }

            //kollar om mus Y possition inte är den gammla
            if (FöregåendeMusStatus.Y != nuvarandeMusStatus.Y) {
                //kollar om mus Y possition är innanför skärmen
                if (nuvarandeMusStatus.X >= 0 && nuvarandeMusStatus.X < SkärmBredd && nuvarandeMusStatus.Y >= 0 && nuvarandeMusStatus.Y < SkärmHöjd) {
                    //implementera något här
                }
            }

            //sparar nuvarande Mus status in i föregående så att man alltid vet vilken den föregående Mus possitionen var
            FöregåendeMusStatus = nuvarandeMusStatus;
        }

        // Inmatning Från Tangentbord, den tar ALLTID in rörelseHastighet.någon borde fixa men behöves typ inte
        private void InmatningTangentbord(float rörelseHastighet, float Sekunder) {
            float TimerNollStäll = 0f;
            //uhhh ja årkar inte tänka hur man förklarar
            KeyboardState nuvarandeTangentbordStatus = Keyboard.GetState();

            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Up)) {
                //rörelse
                //Exempel Spelare.GåUpp(rörelseHastighet)
                SpelareEtt.GåUpp(rörelseHastighet);
            }
            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Down)) {
                //rörelse
                SpelareEtt.GåNed(rörelseHastighet);
            }
            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Left)) {
                //rörelse
                SpelareEtt.GåVänster(rörelseHastighet);
            }
            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Right)) {
                //rörelse
                SpelareEtt.GåHöger(rörelseHastighet);
            }
            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Space) && Sekunder != TimerNollStäll) {
                //attack
                SpelareEtt.Attack(true);
            }
            if (FöregåendeTangentbordStatus.IsKeyDown(Keys.B)) {

            }
            //sparar nuvarande tangentbord status in i föregående så att man alltid vet vilken den föregående knappen tryckt var
            FöregåendeTangentbordStatus = nuvarandeTangentbordStatus;
        }
        private void InmatningXboxKontroller(float rörelseHastighet, float Sekunder) {
            float TimerNollStäll = 0f;
            GamePadState NuvarandeKontrollerStatus = GamePad.GetState(PlayerIndex.One);
            if (NuvarandeKontrollerStatus.Buttons.Back == ButtonState.Pressed) {
                if (SpelareTvåAktiverad == false) {
                    SpelareTvåAktiverad = true;

                }
            }
            if (NuvarandeKontrollerStatus.IsButtonDown(Buttons.RightThumbstickUp)) {
                SpelareTvå.GåUpp(rörelseHastighet);
            }
            if (NuvarandeKontrollerStatus.IsButtonDown(Buttons.RightThumbstickDown)) {
                SpelareTvå.GåNed(rörelseHastighet);
            }
            if (NuvarandeKontrollerStatus.IsButtonDown(Buttons.RightThumbstickLeft)) {
                SpelareTvå.GåVänster(rörelseHastighet);
            }
            if (NuvarandeKontrollerStatus.IsButtonDown(Buttons.RightThumbstickRight)) {
                SpelareTvå.GåHöger(rörelseHastighet);
            }
            if (NuvarandeKontrollerStatus.IsButtonDown(Buttons.A) && Sekunder != TimerNollStäll) {
                SpelareTvå.Attack(true);
            }
            if (FöregåendeKontrollerStatus.IsButtonDown(Buttons.B)) {
            }

            FöregåendeKontrollerStatus = NuvarandeKontrollerStatus;
        }
        //metod för timer skeunder if statements
        private void IfTimerSekunder() {
            //gör attackstate false
            if (TimerSekunderSpelareEtt > 0.5f) {
                SpelareEtt.Attack(false);
                //nollställer timern samt ger cooldown på attack som varar 0.2 sekunder
                if (TimerSekunderSpelareEtt > 0.7f) {
                    TimerSekunderSpelareEtt = TimerNollStällare;
                }
            }
            if (TimerSekunderSpelareTvå > 0.5f) {
                SpelareTvå.Attack(false);
                //nollställer timern samt ger cooldown på attack som varar 0.2 sekunder
                if (TimerSekunderSpelareTvå > 0.7f) {
                    TimerSekunderSpelareTvå = TimerNollStällare;
                }
            }
        }

        #endregion Metoder som används inom Update

        #endregion Skapade Metoder
    }
}