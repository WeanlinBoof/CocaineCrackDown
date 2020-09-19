using Microsoft.Xna.Framework;

namespace CocaineCrackDown {
    public struct SpelarData {
        public ulong ID;
        public string Användarnamn;
        public float X;
        public float Y;
        public SpelarData(ulong id, string användare, float x,float y) {
            ID = id;
            Användarnamn = användare;
            X = x;
            Y = y;
        }
        public SpelarData(ulong id , string användare , Vector2 position) : this(id , användare , position.X , position.Y) { }
    }
}
