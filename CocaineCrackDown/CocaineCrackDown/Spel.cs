using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez.Systems;

namespace CocaineCrackDown {
    public class Spel : Game {
        private GraphicsDeviceManager graphic;
        private SpriteBatch spritebatch;
        private Spelare spelare1;
        private Spelare spelare2;
		public new static GraphicsDevice GraphicsDevice;
		/// <summary>
		/// global content manager for loading any assets that should stick around between scenes
		/// </summary>
		public new static NezContentManager Content;

		/// <summary>
		/// default SamplerState used by Materials. Note that this must be set at launch! Changing it after that time will result in only
		/// Materials created after it was set having the new SamplerState
		/// </summary>
		public static SamplerState DefaultSamplerState = new SamplerState
		{
			Filter = TextureFilter.Point
		};

		/// <summary>
		/// default wrapped SamplerState. Determined by the Filter of the defaultSamplerState.
		/// </summary>
		/// <value>The default state of the wraped sampler.</value>
		public static SamplerState DefaultWrappedSamplerState =>
			DefaultSamplerState.Filter == TextureFilter.Point
				? SamplerState.PointWrap
				: SamplerState.LinearWrap;

		/// <summary>
		/// default GameServiceContainer access
		/// </summary>
		/// <value>The services.</value>
		public new static GameServiceContainer Services => ((Game) _instance).Services;

		/// <summary>
		/// provides access to the single Core/Game instance
		/// </summary>
		public static Spel Instance => _instance;

		/// <summary>
		/// facilitates easy access to the global Content instance for internal classes
		/// </summary>
		internal static Spel _instance;

        public Spel() {
            graphic = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            
            base.Initialize();
        }

        protected override void LoadContent() {
            spritebatch = new SpriteBatch(GraphicsDevice);
            spelare1 = new Spelare(spritebatch,Content,Color.White);
            spelare2 = new Spelare(spritebatch,Content,Color.White);
            Spelare1Input();
            Spelare2Input();
            spelare1.AddedToScene(new Vector2(200,200));
            spelare2.AddedToScene(new Vector2(400,400));
            void Spelare1Input() {
                spelare1.input.Up = Keys.W;
                spelare1.input.Down = Keys.S;
                spelare1.input.Right = Keys.D;
                spelare1.input.Left = Keys.A;
                spelare1.input.Attack = Keys.Z;
            }
            void Spelare2Input() {
                spelare2.input.Up = Keys.Up;
                spelare2.input.Down = Keys.Down;
                spelare2.input.Right = Keys.Right;
                spelare2.input.Left = Keys.Left;
                spelare2.input.Attack = Keys.RightShift;
            }
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            spelare1.Update(gameTime);
            spelare2.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.PeachPuff);
            base.Draw(gameTime);
            spritebatch.Begin();
            spelare1.Draw(gameTime);
            spelare2.Draw(gameTime);
            spritebatch.End();

        }
    }
}
