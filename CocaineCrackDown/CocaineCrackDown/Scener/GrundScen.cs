using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;

using System;
using System.Collections.Generic;
using System.Text;

namespace CocaineCrackDown.Scener {
    //den classen som fixar dimensioner blir lika på alla scener
    public enum NivåNamn {
        ScenEtt,
        ScenTvå
    }
    public class GrundScen : Scene {

        public NivåNamn Status = NivåNamn.ScenEtt;
        public GrundScen() { 
        
        }
        //kalla på denna i början på varje scen
        protected void SeneStandard() {
            //skapar en renderer
            AddRenderer(new DefaultRenderer());
            //Gör Clear Färgen till detta
            ClearColor = new Color(58, 61, 101);
            //Fixar så att det är 640 x 360 pixlar på skärm skalade
            SetDesignResolution(640, 360, SceneResolutionPolicy.BestFit);

        }


    } 
}
