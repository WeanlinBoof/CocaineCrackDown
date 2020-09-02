///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  For a more detailed comments found in the server program (ServerAplication), so if you have not done it, check out.  //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using CocaineCrackDown;
using Nez;
using Nez.UI;

namespace CocaineCrackDown {
    /// <summary>
    /// The Player class and instant constructor,
    /// + 2 Rectangle, because it is already in the game, and here we must draw,
    /// but in this case, however, no need for the timeout.
    /// </summary>
    public class Spelaren {

        public string Namn;

        public Vector2 Positionen;

        
        public static List<Spelaren> Spelarna = new List<Spelaren>();

        public Spelaren(string name, Vector2 Pos) {
            Namn = name;
            Positionen = Pos;
        }
        
        public static void Update()
        {
            foreach (Spelaren Spelare in Spelarna) { 
                    Network.outmsg = Network.Client.CreateMessage();
                    Network.outmsg.Write(Spelare.Namn);
                    Network.outmsg.Write(Spelare.Positionen);
                    Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.UnreliableSequenced);
                
            }
        }
    }
}
