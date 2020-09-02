///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  For a more detailed comments found in the server program (ServerAplication), so if you have not done it, check out.  //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using Lidgren.Network;

namespace CocaineCrackDown {
    // Roughly the same, except that here NetClient was used.
    public static class Network  {
        public static NetClient Client;
        public static NetPeerConfiguration Config;
        private static NetIncomingMessage incmsg;
        public static NetOutgoingMessage outmsg;

        public static void Update() {
            //The biggest difference is that the client side of things easier, 
            //since we will only consider the amount of player object is created, 
            //so there is no keeping track of separate "Server.Connections" as the server side.
            while((incmsg = Client.ReadMessage()) != null) {
                switch(incmsg.MessageType) {
                    case NetIncomingMessageType.Data: {
                        switch(incmsg.ReadString()) {
                            case "connect": {
                                string name = incmsg.ReadString();
                                int x = incmsg.ReadInt32();
                                int y = incmsg.ReadInt32();

                                //Another way to filter out the players with the same name, 
                                //where the first step in any case is added to the player, 
                                //and then check the second round with a double for loop that is in agreement with two players.

                                Spelaren.Spelarna.Add(new Spelaren(name , new Vector2(x , y));

                                for(int i1 = 0; i1 < Spelaren.Spelarna.Count; i1++) {
                                    for(int i2 = /*0*/i1 + 1; i2 < Spelaren.Spelarna.Count; i2++) {
                                        if(i1 != i2 && Spelaren.Spelarna[i1].Namn.Equals(Spelaren.Spelarna[i2].Namn)) {
                                            Spelaren.Spelarna.RemoveAt(i1);
                                            i1--;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;

                            case "move": {
                                try {
                                    string name = incmsg.ReadString();
                                    int x = incmsg.ReadInt32();
                                    int y = incmsg.ReadInt32();

                                    for(int i = 0; i < Spelaren.Spelarna.Count; i++) {
                                        //It is important that you only set the value of the player, if it is not yours, 
                                        //otherwise it would cause lagg (because you'll always be first with yours, and there is a slight delay from server-client).
                                        //Of course, sometimes have to force the server to the actual position of the player, otherwise could easily cheat.
                                        if(Spelaren.Spelarna[i].Namn.Equals(name) && Spelaren.Spelarna[i].Namn != TextInput.text) {
                                            Spelaren.Spelarna[i].Positionen = new Vector2(x , y);
                                            break;
                                        }
                                    }
                                }
                                catch {
                                    continue;
                                }
                            }
                            break;

                            case "disconnect": //Clear enough :)
                                {
                                string name = incmsg.ReadString();

                                for(int i = 0; i < Spelaren.Spelarna.Count; i++) {
                                    if(Spelaren.Spelarna[i].Namn.Equals(name)) {
                                        Spelaren.Spelarna.RemoveAt(i);
                                        i--;
                                        break;
                                    }
                                }
                            }
                            break;

                            case "deny": //If the name on the message is the same as ours
                                {
                                Game1.HeadText = "This name is already taken:";
                                Game1.TextCanWrite = true;
                                Spelaren.Spelarna.Clear();
                            }
                            break;
                        }
                    }
                    break;
                }
                Client.Recycle(incmsg);
            }
        }
    }
}
