using System;

using Lidgren.Network;

using Nez;

namespace CocaineCrackDown {

    ///top delen av server eller nätvärd  den som hostat i all fall '
    /// <summary>
    /// NätVärd Eller Host Kalla den Vad du vill men denna har konfigen för den som är värden
    /// </summary>
    public partial class NätVärd : GlobalManager {

        public NätVärd() {

            //creating a new network config, and starting the server (with "Network" class, created below)
            // The server and the client program must also use this name, so that can communicate with each other.
            Network.Config = new NetPeerConfiguration(StandigaVarden.SPELNAMN);
            Network.Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            Network.Config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            Network.Config.EnableMessageType(NetIncomingMessageType.);
            Network.Config.EnableUPnP = true;
            Network.Config.Port = 14882;
            Network.Config.AcceptIncomingConnections = true;
            Network.Config.EnableMessageType(NetIncomingMessageType.Data);
            Network.Server = new NetServer(Network.Config);
            Network.Server.Start();

            Console.WriteLine("Server started!");
            Console.WriteLine("Waiting for connections...");
        }
    }
}

/*
/// <summary>
/// 
/// </summary>
/// <param name="">Entity.</param>
/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
/// <returns>The to world point.</returns>
/// <typeparam name="">The 1st type parameter.</typeparam>
/// <remarks>
/// This example shows how to specify the <see cref=""/> type as a cref attribute.
/// </remarks>
/// 
*/