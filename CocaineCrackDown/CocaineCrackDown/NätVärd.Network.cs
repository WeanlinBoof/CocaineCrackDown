
using System;
using Microsoft.Xna.Framework;

using Lidgren.Network;
using System.Threading;
using Nez;

namespace CocaineCrackDown {
    public partial class NätVärd 
    {
        /// <summary>
        /// A Basics Network class
        /// </summary>
        private class Network {
            /// <summary>
            /// the Server
            /// </summary>
            public static NetServer Server;
            /// <summary>
            /// the Server config
            /// </summary>
            public static NetPeerConfiguration Config;
            
            /// <summary>
            /// the incoming messages that server can read from clients
            /// </summary>
            private static NetIncomingMessage incmsg;
            /// <summary>
            /// the outgoing messages that clients can receive and read
            /// </summary>
            public static NetOutgoingMessage outmsg;

            /// <summary>
            /// below for explanation...
            /// </summary>
            private static bool SpelarUppfriskning;

            public void Update() {
                while((incmsg = Server.ReadMessage()) != null) //while the message is received, and is not equal to null...
                {
                    switch (incmsg.MessageType) //There are several types of messages (see the Lidgren Basics tutorial), but it is easier to just use it the most important thing the "Data".
                    {
                        case NetIncomingMessageType.Data:
                            {
                            //////////////////////////////////////////////////////////////
                            // You must create your own custom protocol with the        //
                            // server-client communication, and data transmission.      //
                            //////////////////////////////////////////////////////////////


                            // 1. step: The first data/message (string or int) tells the program to what is going on, that is what comes to doing.
                            // 2. step: The second tells by name (string) or id (int) which joined client(player) or object(bullets, or other dynamic items) to work.
                            // 3. step: The other data is the any some parameters you want to use, and this is setting and refhreshing the object old (player or item) state.

                            // Now this example I'm use to string (yes you can saving the bandwidth with all messages, if you use integer)
                             //the first data (1. step)

                            switch (incmsg.ReadString()) //and I'm think this is can easyli check what comes to doing
                                {
                                    case "connect": //if the firs message/data is "connect"
                                        {
                                            string name = incmsg.ReadString(); //Reading the 2. message who included the name (you can use integer, if you want to store the players in little data)
                                            int x = incmsg.ReadInt32(); //Reading the x position
                                            int y = incmsg.ReadInt32(); // -||- y postion

                                            SpelarUppfriskning = true; //just setting this "True"

                                            // Now check to see if you have at least one of our players, the subsequent attempts to connect with the same name. 
                                            for (int i = 0; i < Player.players.Count; i++)
                                            {
                                                if (Player.players[i].Namn.Equals(name)) //If its is True...
                                                {
                                                    outmsg = Server.CreateMessage(); //The Server creating a new outgoing message
                                                    outmsg.Write("deny"); //and this moment writing this to one message "deny" (the rest of the ClientAplication in interpreting)

                                                    //Sending the message
                                                    //parameters:
                                                    //1. the message which we have written
                                                    //2. whom we send (Now just only the person who sent the message to the server)
                                                    //3. delivery reliability (Since this is an important message, so be sure to be delivered)
                                                    //4. The channel on which the message is sent (I do not deal with it, just give the default value)
                                                    Server.SendMessage(outmsg, incmsg.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                                                    Thread.Sleep(100); //a little pause the current process to make sure the message is sent to the client before they break down in contact with him.
                                                    incmsg.SenderConnection.Disconnect("bye"); //ends the connection with the client who sent the message, and you can writing the any string message if you want
                                            SpelarUppfriskning = false; //Now the "if" its is True, we disable the playerRefhres bool
                                            break;
                                                }
                                            }

                                    // but if the above check is false, then the following happens:
                                    if(SpelarUppfriskning) {
                                                Thread.Sleep(100); //A little pause to make sure you connect the client before performing further operations
                                                Player.players.Add(new Player(name, new Vector2(x, y), 0)); //Add to player messages received as a parameter
                                        Console.WriteLine($"{name} connected.");

                                        for(int i = 0; i < Player.players.Count; i++) {
                                                    // Write a new message with incoming parameters, and send the all connected clients.
                                                    outmsg = Server.CreateMessage();

                                                    outmsg.Write("connect");
                                                    outmsg.Write(Player.players[i].Namn);
                                                    outmsg.Write((int)Player.players[i].Positionen.X);
                                                    outmsg.Write((int)Player.players[i].Positionen.Y);

                                                    Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                                }
                                            }

                                    Console.WriteLine($"Players: " + Player.players.Count);
                                        }
                                        break;

                                    case "move": //The moving messages
                                        {
                                            //This message is treated as plain UDP (NetDeliveryMethod.Unreliable)
                                            //The motion is not required to get clients in every FPS.
                                            //The exception handling is required if the message can not be delivered in full, 
                                            //just piece, so this time the program does not freeze.
                                            try
                                            {
                                                string name = incmsg.ReadString();
                                                int x = incmsg.ReadInt32();
                                                int y = incmsg.ReadInt32();

                                                for (int i = 0; i < Player.players.Count; i++)
                                                {
                                                    if (Player.players[i].Namn.Equals(name))
                                                    {
                                                        Player.players[i].Positionen = new Vector2(x, y);
                                                        Player.players[i].TidsGränns = 0; //below for explanation (Player class)...
                                                        break;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                continue;
                                            }
                                        }
                                        break;

                                    case "disconnect": //If the client want to disconnect from server at manually
                                        {
                                    string name = incmsg.ReadString();

                                    for(int i = 0; i < Player.players.Count; i++) {
                                        if(Player.players[i].Namn.Equals(name)) //If the [index].name equaled the incoming message name...
                                        {
                                            Server.Connections[i].Disconnect("bye"); //The server disconnect the correct client with index
                                            Console.WriteLine($"{name} disconnected.");

                                            if(Server.ConnectionsCount != 0) //After if clients count not 0
                                            {
                                                //Sending the disconnected client name to all online clients
                                                outmsg = Server.CreateMessage();
                                                outmsg.Write("disconnect");
                                                outmsg.Write(name);
                                                Server.SendMessage(outmsg , Server.Connections , NetDeliveryMethod.ReliableOrdered , 0);
                                            }

                                            Player.players.RemoveAt(i); //And remove the player object
                                            i--;
                                            break;
                                        }
                                    }

                                    Console.WriteLine($"Players: {Player.players.Count} ");
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    //All messages processed at the end of the case, delete the contents.
                    Server.Recycle(incmsg); 
                }
            }
        }
    }
}