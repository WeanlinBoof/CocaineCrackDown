///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  For a more detailed comments found in the server program (ServerAplication), so if you have not done it, check out.  //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;
using CocaineCrackDown;

namespace CocaineCrackDown {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerTexture;
        SpriteFont fontTexture;

        KeyboardState ActualKeyState,
                      LastKeyState;

        public static string HeadText = "Please Enter your name:"; //The main text on the upper left corner
        public static bool TextCanWrite = true; //Also important: If this is true, you can type in text, 
                                                //otherwise the players connected to the server and play. 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 240;
            //Adjusts the speed of the game. 
            //If "IsFixedTimeStep = True" and the "graphics.SynchronizeWithVerticalRetrace = False", then the game run with 60 FPS, 
            //this is important because the data transmission network can be fixed to 60 FPS.
            //NOTE: 60 FPS realtime transmission (for "move" data) is !!!not optimal!!!, but for simplicity thus now used.
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            graphics.ApplyChanges();

            Network.Config = new NetPeerConfiguration("LidgrenSimpleMovement"); //Same as the Server, so the same name to be used.
            Network.Client = new NetClient(Network.Config);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //System.Threading.Thread.Sleep(16);

            //The Actual and the Last key combination allows it to be valid in a given moment is a key strokes.
            LastKeyState = ActualKeyState;
            ActualKeyState = Keyboard.GetState();

            Network.Update();
            Spelaren.Update();

            if (TextCanWrite == true) //If its is True...
            {
                TextInput.Update(); //The TextInput is available and updated

                //If your name is not empty and the actual keystroke
                if (ActualKeyState.IsKeyDown(Keys.Enter) && LastKeyState.IsKeyUp(Keys.Enter) && TextInput.text != "")
                {
                    Network.Client.Start(); //Starting the Network Client
                    Network.Client.Connect("127.0.0.1", 14242); //And Connect the Server with IP (string) and host (int) parameters

                    //The causes are shown below pause for a bit longer. 
                    //On the client side can be a little time to properly connect to the server before the first message you send us. 
                    //The second one is also a reason. The client does not manually force the quick exit until it received a first message from the server. 
                    //If the client connect to trying one with the same name as that already exists on the server, 
                    //and you attempt to exit Esc-you do not even arrived yet reject response ("deny"), the underlying visible event is used, 
                    //so you can disconnect from the other player from the server because the name he applied for the existing exit button. 
                    //Therefore, this must be some pause. 

                    System.Threading.Thread.Sleep(300);

                    Network.outmsg = Network.Client.CreateMessage();
                    Network.outmsg.Write("connect");
                    Network.outmsg.Write(TextInput.text);
                    Network.outmsg.Write(160);
                    Network.outmsg.Write(120);
                    Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);

                    TextCanWrite = false;

                    System.Threading.Thread.Sleep(300);
                }
            }
            else
            {
                if (ActualKeyState.IsKeyDown(Keys.Escape) && LastKeyState.IsKeyUp(Keys.Escape))
                {
                    Network.outmsg = Network.Client.CreateMessage();
                    Network.outmsg.Write("disconnect");
                    Network.outmsg.Write(TextInput.text);
                    Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);

                    System.Threading.Thread.Sleep(300);

                    TextCanWrite = true;
                    Spelaren.Spelarna.Clear();
                    HeadText = "Please Enter your name:";

                    //this.Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                              BlendState.NonPremultiplied);

            if (TextCanWrite)
            {
                spriteBatch.DrawString(fontTexture, HeadText, new Vector2(), Color.White);
                spriteBatch.DrawString(fontTexture, TextInput.text + "_", new Vector2(0, 25), Color.White);
            }

            foreach (Spelaren p in Spelaren.Spelarna)
            {
                spriteBatch.Draw(playerTexture, new Rectangle((int)p.Positionen.X, (int)p.Positionen.Y, p.defaultRect.Width, p.defaultRect.Height), p.drawRect, Color.White);
                spriteBatch.DrawString(fontTexture, p.Namn, new Vector2(p.Positionen.X, p.Positionen.Y - 18), Color.White, 0, new Vector2(), 0.6f, SpriteEffects.None, 0);
            }

            spriteBatch.DrawString(fontTexture, "Players: " + Spelaren.Spelarna.Count.ToString(), new Vector2(0, 220), Color.White, 0, new Vector2(), 0.75f, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
