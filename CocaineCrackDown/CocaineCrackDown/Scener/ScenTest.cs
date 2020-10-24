using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Scener {
    public class ScenTest : GrundScen {  
        public KlientHanterare Klient { get; internal set; }
        public VärdHanterare Server { get; internal set; }
        public bool IsHost { get; internal set; }
        public Randy Randy;
        public Doug Doug;
        public override void Initialize() {
            base.Initialize();
            AddEntity(new TiledMap("testnr1"));
            if(IsHost) {   
                Randy = new Randy(false);
                Doug = new Doug(true);
                AddEntity(Doug);
                AddEntity(Randy);
            }
            if(!IsHost) {
                Randy = new Randy(true);
                Doug = new Doug(false);
                AddEntity(Randy);
                AddEntity(Doug);
            }

        }
        public override void Update() {
            base.Update();
            if(IsHost) {
                SpelarData SpelarData = new SpelarData(Doug.Position,Doug.atlasAnimationsKomponent.Attackerar);
                Server.SickaSpelarData(SpelarData);
                Randy.Position = new Vector2(Server.recivedSpelarData.X , Server.recivedSpelarData.Y);
                Randy.atlasAnimationsKomponent.Attackerar = Server.recivedSpelarData.Attack;

            }
            if(!IsHost) {
                SpelarData SpelarData = new SpelarData(Randy.Position,Randy.atlasAnimationsKomponent.Attackerar);
                Klient.SickaSpelarData(SpelarData);
                Doug.Position = new Vector2(Klient.recivedSpelarData.X , Klient.recivedSpelarData.Y);
                Doug.atlasAnimationsKomponent.Attackerar = Klient.recivedSpelarData.Attack;
            }
            
        }
    }
}
