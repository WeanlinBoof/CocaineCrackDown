using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown {
    //GRUNDEN AV ALLA KLASSER NA AH AH AH
    public class Objekt {
        public float X { get; set; }
        public float Y { get; set; }
        public SpriteBatch SpriteBatch;
        public virtual void Draw() {
        }
    }
}
