using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocaineCrackDown.Objekter {
    public class Objekt {
        public float X { get; set; }
        public float Y { get; set; }
        public SpriteBatch SpriteBatch;
        public virtual void LaddaResurser() {
        }
        public virtual void Uppdatera(GameTime gameTime) {
        }
        public virtual void Rita() {
        }
    }
}