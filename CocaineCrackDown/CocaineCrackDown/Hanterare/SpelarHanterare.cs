using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Hanterare {

    public class SpelarHanterare : GlobalManager{

        private readonly bool IsHost;

        private readonly Dictionary<uint , Spelare> Spelarnas = new Dictionary<uint , Spelare>();

        private static int playerIdCounter;

        private Spelare LokalSpelare;
        private string AnvändarNamn { get; }
        public SpelarHanterare(string namn, bool isHost) {
            AnvändarNamn = namn;
            IsHost = isHost;
            if(IsHost) {
                Core.Schedule(1 , true , this , HearthbeatTid);
            }
        }

        public event EventHandler<SpelarStatusÄndradArgs> SpelarStatusÄndrad;

        public IEnumerable<Spelare> Spelarna => Spelarnas.Values;

        public Spelare AddPlayer(uint id , Scene scene, bool SpelareLokalt) {
            if(Spelarnas.ContainsKey(id)) {
                return Spelarnas[id];
            }

            //fixa position och liknade (id, v2 pos, float röelsehastighet, bool islocal)
            Spelare spelare = new Spelare(AnvändarNamn);

            Spelarnas.Add(spelare.Id , spelare);

            if(SpelareLokalt) {
                LokalSpelare = spelare;
            }

            return spelare;
        }

        public Spelare AddPlayer(Scene scene, bool isLocal) {
            Spelare spelare;
            //spelare.AttachToScene(scene);
            //createentity i add player istället för uh det som är nu venne

            spelare = AddPlayer((uint)Interlocked.Increment(ref playerIdCounter), scene, isLocal);

            return spelare;
        }

        public Spelare GetPlayer(uint id) {
            return Spelarnas.ContainsKey(id) ? Spelarnas[id] : null;
        }

        public bool PayerIsLocal(Spelare player) {
            return LokalSpelare != null && LokalSpelare.Id == player.Id;
        }

        public void RemovePlayer(uint id) {
            if(Spelarnas.ContainsKey(id)) {
                Spelarnas.Remove(id);
            }
        }
        private void HearthbeatTid(ITimer time) {
            Console.WriteLine("bruh");
            foreach(Spelare spelare in Spelarna) {
                OnPlayerStateChanged(spelare);
            }
        }
        public override void Update() {
            OnPlayerStateChanged(LokalSpelare);

            foreach(Spelare spelare in Spelarna) {
                spelare.Update();
            }



        }

        protected void OnPlayerStateChanged(Spelare spelare) {
            SpelarStatusÄndrad?.Invoke(this , new SpelarStatusÄndradArgs(spelare));
        }
    }
}