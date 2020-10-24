
using System.Text;

using LiteNetLib.Utils;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown {
    public struct SpelarData : INetSerializable {
        public static SpelarData SpelarDataNull { get; } = new SpelarData( Vector2.Zero, false);

        public float X;
        public float Y;
        public bool Attack;
        public SpelarData(float x,float y, bool attack) {
            X = x;
            Y = y;
            Attack = attack;
        }
        public SpelarData(Vector2 position,bool attack) : this( position.X , position.Y, attack) { }
        public static bool operator !=(SpelarData value1,SpelarData value2) {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Attack != value2.Attack;
        }
        public static bool operator ==(SpelarData value1,SpelarData value2) {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Attack == value2.Attack;
        }
        public override bool Equals(object obj) {
            return obj is SpelarData spelarData && Equals(spelarData);
        }

        public void Serialize(NetDataWriter skriv) {
            skriv.Put(X);
            skriv.Put(Y);
            skriv.Put(Attack);
        }

        public void Deserialize(NetDataReader läs) {
            X = läs.GetFloat();
            Y = läs.GetFloat();
            Attack = läs.GetBool();
        }

        public override string ToString() {
            return $"X: {X}, Y: {Y}, Attack {Attack}";

        }
    }
}
