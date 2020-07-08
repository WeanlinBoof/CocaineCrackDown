using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CocaineCrackDown {
    //classen som inhåller alla spel resurser för tillfället
    public class SpelResurser {
        // Döp ALLTID Variablerna med likvärdig Mönster T.ex om MinVariabel är av Texture2D så Ska Det Vara MinVariabelTextur
        public Texture2D GolvTextur { get; set; }
        public Texture2D BakgrundTextur { get; set; }
        public Texture2D SpelareEttNormalTextur { get; set; }
        public Texture2D SpelareEttAttackTextur { get; set; }
        public SpelResurser(ContentManager Content) {
            GolvTextur = Content.Load<Texture2D>("mark");
            //BakgrundTextur = Content.Load<Texture2D>("bb");
            SpelareEttNormalTextur = Content.Load<Texture2D>("spelare");
            SpelareEttAttackTextur = Content.Load<Texture2D>("spelare_attack");
        }
    }
}
