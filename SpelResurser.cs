using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CocaineCrackDown {
    //classen som inhåller alla spel resurser för tillfället
    public class SpelResurser {
        // Döp ALLTID Variablerna med likvärdig Mönster T.ex om MinVariabel är av Texture2D så Ska Det Vara MinVariabelTextur
        public Texture2D GolvTextur { get; set; }
        public Texture2D BakgrundTextur { get; set; }
        public Texture2D HimmelTextur { get; set; }
        public Texture2D DougNormalTextur { get; set; }
        public Texture2D DougAttackTextur { get; set; }
        public Texture2D RandyNormalTextur { get; set; }
        public Texture2D RandyAttackTextur { get; set; } 
        public SpelResurser(ContentManager Content) {
            /////////////// Texturer För kulissen 
            GolvTextur = Content.Load<Texture2D>("mark");
            BakgrundTextur = Content.Load<Texture2D>("bakgrund");//byt till korrekt sen
            HimmelTextur = Content.Load<Texture2D>("himmel"); //byt till korrekt sen
            ///////////////
            DougNormalTextur = Content.Load<Texture2D>("doug");
            DougAttackTextur = Content.Load<Texture2D>("doug_attack");
            RandyNormalTextur = Content.Load<Texture2D>("randy");
            RandyAttackTextur = Content.Load<Texture2D>("randy_attack");
        }
    }
}
