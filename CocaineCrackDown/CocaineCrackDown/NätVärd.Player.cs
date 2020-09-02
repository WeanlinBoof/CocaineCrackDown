using System;
using System.Collections.Generic;
using System.Threading;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown {

    public partial class NätVärd {

        /// <summary>
        /// The Player class and instant constructor
        /// </summary>
        private class Player {

            public string Namn;

            public Vector2 Positionen;

            /// <summary>
            /// This disconnects the client, even if no message from him within a certain period of
            /// time and not been reset value.
            /// </summary>
            public int TidsGränns;

            public static List<Player> players = new List<Player>();

            public Player(string namn , Vector2 pos , int tidsgränns) {
                Namn = namn;
                Positionen = pos;
                TidsGränns = tidsgränns;
            }

            public static void Update() {
                int MängdSpelare;

                //If the number of the player object actually corresponds to the number of connected clients.
                if(Network.Server.ConnectionsCount == players.Count) {
                    for(MängdSpelare = 0; MängdSpelare < players.Count; MängdSpelare++) {

                        //This data member continuously counts up with every frame/tick.
                        players[MängdSpelare].TidsGränns++;

                        //The server simply always sends data to the all players current position of all clients.
                        Network.UtGående = Network.Server.CreateMessage();

                        Network.UtGående.Write("move");
                        Network.UtGående.Write(players[MängdSpelare].Namn);
                        Network.UtGående.Write((int)players[MängdSpelare].Positionen.X);
                        Network.UtGående.Write((int)players[MängdSpelare].Positionen.Y);

                        Network.Server.SendMessage(Network.UtGående , Network.Server.Connections , NetDeliveryMethod.Unreliable , 0);

                        //If this is true, so that is the player not sent information with himself
                        if(players[MängdSpelare].TidsGränns > 180) {

                            //The procedure will be the same as the above when "disconnect" message
                            Network.Server.Connections[MängdSpelare].Disconnect("bye");
                            Console.WriteLine($"{players[MängdSpelare].Namn} is timed out.");
                            Thread.Sleep(100);
                            if(Network.Server.ConnectionsCount != 0) {
                                Network.UtGående = Network.Server.CreateMessage();

                                Network.UtGående.Write("disconnect");
                                Network.UtGående.Write(players[MängdSpelare].Namn);

                                Network.Server.SendMessage(Network.UtGående , Network.Server.Connections , NetDeliveryMethod.ReliableOrdered , 0);
                            }

                            players.RemoveAt(MängdSpelare);
                            Console.WriteLine($"Players: {players.Count}");
                            break;
                        }
                    }
                }
            }
        }
    }
}