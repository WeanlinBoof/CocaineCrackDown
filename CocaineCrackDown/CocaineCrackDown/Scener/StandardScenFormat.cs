using Nez;
using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Scener {
    public class StandardScenFormat : Scene {


        public StandardScenFormat() {

        }


        public override void OnStart() {
            //skapar en renderer
            AddRenderer(new DefaultRenderer());
            //Gör Clear Färgen till detta
            ClearColor = Color.DarkSalmon;
            //Fixar så att det är 640 x 360 pixlar på skärm skalade
            SetDesignResolution(640, 360, SceneResolutionPolicy.BestFit);



        }
    }
}
