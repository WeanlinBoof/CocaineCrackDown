using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace CocaineCrackDown {
    public class Spel : Game {
        //dessa borde sorteras för att underlätta allt för oss vvv
        private GraphicsDeviceManager grafiker;
        private SpriteBatch spriteBatch;
        private SpelResurser spelResurser;
        private readonly string titel = "CocaineCrackDown";
        private int skärmBredd = 0;
        private int skärmHöjd = 0;
        private MouseState föregåendeMusStatus;
        private KeyboardState föregåendeTangentbordStatus;
        private float förflutenTid;
        private float rörelseHastighet;
        private Spelare SpelareEtt;
        private int SpelareEttX;
        private int SpelareEttY;
        private float Sekunder = 0f;
        private bool AttackState = false;
        //dessa borde sorteras för att underlätta allt för oss ^^^

        // class konstruktör
        public Spel() {

            // gör så vi har grafiker
            grafiker = new GraphicsDeviceManager(this);

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // inte säker var ej jag som la den där
            spelResurser = new SpelResurser(Content);

            SpelareEttX = (skärmBredd - spelResurser.SpelareEttNormalTextur.Width) / 2;
            SpelareEttY = (skärmHöjd - spelResurser.SpelareEttNormalTextur.Height) / 2;
            SpelareEtt = new Spelare(SpelareEttX, SpelareEttY, skärmBredd, skärmHöjd, spriteBatch, spelResurser);
        }

        //uppdaterar
        protected override void Update(GameTime gameTime) {

            /* gör så att spelet inte updaterar om det ej e fokuserat T.ex om man alt+tab ut från spelet
            det ska inte vara så här utan det är bara här för att man ska få ett hum hur man ska ta sig åt*/
            if (IsActive != false) {

                // förflutenTid är mängden millisekunder som har förflutit sen start
                förflutenTid = gameTime.ElapsedGameTime.Milliseconds;

                // förflutenTid dellat det numer som står är likamed rörelseHastigheten
                rörelseHastighet = förflutenTid / 5.5f;

                Sekunder += (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Metod För Tangentbords inmatning
                InmatningTangentbord(rörelseHastighet, Sekunder);

                if(Sekunder > 0.5f){
                    SpelareEtt.AttackStatus(AttackState = false);
                    Sekunder = 0f;
                }

                //Metod För Mus inmatning
                InmatningMus();

                // inte säker var ej jag som la den där
                base.Update(gameTime);
            }
        }
        //Ritar texturer btw det som ska rittas på skärm ska läggas mellan spriteBatch.Begin() & spriteBatch.End()
        protected override void Draw(GameTime gameTime) {

            // inte säker var ej jag som la den där
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // början på spriteBatch
            spriteBatch.Begin();

            SpelareEtt.Draw();

            // slut på spriteBatch
            spriteBatch.End();



            // inte säker var ej jag som la den där
            base.Draw(gameTime);
        }

        #region Metoder Som Anders Har Skapat

        //Aktiverar Grafik Ändringen/ar som gjorts
        private void ÄndraGrafik() {

            //bufferbredd = uplösning som är vald
            grafiker.PreferredBackBufferWidth = skärmBredd;

            //bufferhöjd = uplösning som är vald
            grafiker.PreferredBackBufferHeight = skärmHöjd;

            // Applicerar ändringar bruh
            grafiker.ApplyChanges();
        }

        //Titel Och Den tillfälliga Upplösning för vi inte har någon sätt att ändra instälningar på än
        private void TitelUpplösning() {

            //sätter titel på spel fönstret
            Window.Title = titel;

            // halva skärmhöjden
            skärmHöjd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;

            // halva skärmbredden
            skärmBredd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;


        }

        // Inmatning Från Mus skulle kunna ta in muskänslighet liknade till hur InmatningTangentbord tar rörelseHastighet InmatningTangentbord(float rörelseHastighet)
        private void InmatningMus() {

            //uhhh ja årkar inte tänka hur man förklarar
            MouseState nuvarandeMusStatus = Mouse.GetState();

            //kollar om mus X possiton inte är den gammla
            if (föregåendeMusStatus.X != nuvarandeMusStatus.X) {

                //kollar om mus X possition är innanför skärmen
                if (nuvarandeMusStatus.X >= 0 && nuvarandeMusStatus.X < skärmBredd && nuvarandeMusStatus.Y >= 0 && nuvarandeMusStatus.Y < skärmHöjd) {
                    //implementera något här
                }
            }

            //kollar om mus Y possition inte är den gammla
            if (föregåendeMusStatus.Y != nuvarandeMusStatus.Y) {

                //kollar om mus Y possition är innanför skärmen
                if (nuvarandeMusStatus.X >= 0 && nuvarandeMusStatus.X < skärmBredd && nuvarandeMusStatus.Y >= 0 && nuvarandeMusStatus.Y < skärmHöjd) {
                    //implementera något här
                }
            }

            //sparar nuvarande Mus status in i föregående så att man alltid vet vilken den föregående Mus possitionen var
            föregåendeMusStatus = nuvarandeMusStatus;
        }

        // Inmatning Från Tangentbord, den tar ALLTID in rörelseHastighet.någon borde fixa men behöves typ inte
        private void InmatningTangentbord(float rörelseHastighet, float Sekunder) {

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
            if (nuvarandeTangentbordStatus.IsKeyDown(Keys.Space)) {
                //attack

                SpelareEtt.AttackStatus(AttackState = true);

            }


            //sparar nuvarande tangentbord status in i föregående så att man alltid vet vilken den föregående knappen tryckt var
            föregåendeTangentbordStatus = nuvarandeTangentbordStatus;
        }
        #endregion
    }
}
