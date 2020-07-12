using CocaineCrackDown.Statusar;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CocaineCrackDown {

    public class Spel : Game {
        public static Random Random;

        public static int SkärmBredd = 1280;
        public static int SkärmHöjd = 720;

        private Status NuvarandeStatus;
        private Status NästaStatus;
        private readonly GraphicsDeviceManager Grafiker;
        public SpriteBatch spriteBatch;

        private readonly string Titel = "CocaineCrackDown";

        // class konstruktör
        public Spel() {
            // gör så vi har grafiker
            Grafiker = new GraphicsDeviceManager(this);

            // säger vart resurserna hållerhus
            Content.RootDirectory = "Content";
        }

        //initialiserar skit inan allt börjar
        protected override void Initialize() {
            Random = new Random();
            //sätter titel på spel fönstret
            Window.Title = Titel;

            // halva skärmhöjden
            SkärmHöjd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;

            // halva skärmbredden
            SkärmBredd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;

            // uppdaterar spelet 300 gånger per sekund istället för varje frame
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 300.0f);

            // fixar en icke framerate beroende spel
            IsFixedTimeStep = true;

            //bufferbredd = uplösning som är vald
            Grafiker.PreferredBackBufferWidth = SkärmBredd;

            //bufferhöjd = uplösning som är vald
            Grafiker.PreferredBackBufferHeight = SkärmHöjd;

            // Applicerar ändringar bruh
            Grafiker.ApplyChanges();

            // inte säker var ej jag som la den där
            base.Initialize();
        }

        //laddar in resurser
        protected override void LoadContent() {
            // inte säker var ej jag som la den där
            spriteBatch = new SpriteBatch(GraphicsDevice);

            NuvarandeStatus = new SpelStatus(this, Content);
            NuvarandeStatus.LaddaResurser();
            NästaStatus = null;
        }

        //uppdaterar
        protected override void Update(GameTime gameTime) {
            if (NästaStatus != null) {
                NuvarandeStatus = NästaStatus;
                NuvarandeStatus.LaddaResurser();

                NästaStatus = null;
            }

            NuvarandeStatus.Uppdatera(gameTime);

            NuvarandeStatus.EfterUppdatering(gameTime);

            base.Update(gameTime);
        }

        //Ritar texturer btw det som ska rittas på skärm ska läggas mellan spriteBatch.Begin() & spriteBatch.End()
        protected override void Draw(GameTime gameTime) {
            // inte säker var ej jag som la den där
            GraphicsDevice.Clear(Color.PeachPuff);

            NuvarandeStatus.Rita(gameTime);

            base.Draw(gameTime);
        }
    }
}