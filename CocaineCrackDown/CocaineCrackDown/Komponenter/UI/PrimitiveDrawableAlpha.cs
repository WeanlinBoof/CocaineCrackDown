
using Microsoft.Xna.Framework;

using Nez;
using Nez.UI;

namespace CocaineCrackDown.Komponenter.UI {
    public class PrimitiveDrawableAlpha : PrimitiveDrawable {
        public PrimitiveDrawableAlpha(Color col) {
            Color = col;
        }

        public override void Draw(Batcher batcher , float x , float y , float width , float height , Color color) {
            Color col = Color ?? color;
            if(UseFilledRect) {
                batcher.DrawRect(x , y , width , height , col);
            }
            else {
                batcher.DrawHollowRect(x , y , width , height , col);
            }
        }
    }
}