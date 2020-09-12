using LiteNetLib.Utils;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown {
    class SpelarDataPacket {
        public SpelarData SpelarDatan { get; set; }
    }
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

        public static void Serialize(NetDataWriter writer, SpelarData SD) {
            writer.Put(SD.ID);
            writer.Put(SD.Användarnamn);
            writer.Put(SD.X);
            writer.Put(SD.Y);

        }

        public static SpelarData Deserialize(NetDataReader reader) {
            SpelarData SD;
            SD.ID = reader.GetULong();
            SD.Användarnamn = reader.GetString();
            SD.X = reader.GetFloat();
            SD.Y = reader.GetFloat();
            return SD;

        }
    }
}
