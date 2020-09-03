using System;
using System.Collections.Generic;
using System.Text;

using RedGrin.Interfaces;

namespace CocaineCrackDown.Scener {
    class NätLobby : GrundScen, INetworkArena {
        public INetworkEntity HandleCreateEntity(ulong uniqueId , object entityData) {
            throw new NotImplementedException();
        }

        public void HandleDestroyEntity(INetworkEntity entity) {
            throw new NotImplementedException();
        }

        public void HandleGenericMessage(ulong messageId , object message , double messageTime) {
            throw new NotImplementedException();
        }
    }
}
