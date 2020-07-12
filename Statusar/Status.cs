
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;

namespace CocaineCrackDown.Statusar {

    public abstract class Status {
        protected Spel CCD;

        protected ContentManager Innehåll;
        ///////////////////////////////////////////////////////////////////////////
        public Status(Spel spel, ContentManager innehåll) {
            CCD = spel;

            Innehåll = innehåll;
        }
        ///////////////////////////////////////////////////////////////////////////
        public abstract void LaddaResurser();
        ///////////////////////////////////////////////////////////////////////////
        public abstract void Uppdatera(GameTime gameTime);
        ///////////////////////////////////////////////////////////////////////////
        public abstract void EfterUppdatering(GameTime gameTime);
        ///////////////////////////////////////////////////////////////////////////
        public abstract void Rita(GameTime gameTime);
        ///////////////////////////////////////////////////////////////////////////
    }
}