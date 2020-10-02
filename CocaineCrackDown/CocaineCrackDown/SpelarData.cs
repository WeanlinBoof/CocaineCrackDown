using Microsoft.Xna.Framework;

namespace CocaineCrackDown {
    public struct SpelarData {
        public static SpelarData SpelarDataNull { get; } = new SpelarData(0 , null , Vector2.Zero, false);
        public ulong ID;
        public string Namn;
        public float X;
        public float Y;
        public bool Attack;
        public SpelarData(ulong id, string namn, float x,float y, bool attack) {
            ID = id;
            Namn = namn;
            X = x;
            Y = y;
            Attack = attack;
        }
        public SpelarData(ulong id , string användare , Vector2 position,bool attack) : this(id , användare , position.X , position.Y, attack) { }
        public static bool operator !=(SpelarData value1,SpelarData value2) {
            return value1.X != value2.X || value1.Y != value2.Y ||value1.Namn != value2.Namn || value1.ID != value2.ID || value1.Attack != value2.Attack;
        }
        public static bool operator ==(SpelarData value1,SpelarData value2) {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Namn == value2.Namn && value1.ID == value2.ID && value1.Attack == value2.Attack;
        }
        public override bool Equals(object obj) {
            return obj is SpelarData spelarData && Equals(spelarData);
        }

        public override int GetHashCode() {
            throw new System.NotImplementedException();
        }
    }
}
