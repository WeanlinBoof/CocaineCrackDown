using Microsoft.Xna.Framework.Input;

namespace CocaineCrackDown.Modeler {

    public class Inmatning {

        public class Tangentbord {
            public Keys Up { get; set; }

            public Keys Down { get; set; }

            public Keys Left { get; set; }

            public Keys Right { get; set; }

            public Keys Attack { get; set; }
        }

        public class Kontroller {
            public Buttons Up { get; set; }

            public Buttons Down { get; set; }

            public Buttons Left { get; set; }

            public Buttons Right { get; set; }

            public Buttons Attack { get; set; }
        }
    }
}