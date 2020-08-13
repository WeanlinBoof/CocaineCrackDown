using CocaineCrackDown.Entiteter;

using Nez;

using System;

namespace CocaineCrackDown {

    public enum EntitetTyp {
        Himmel,

        Bakgrund,

        Mark,

        SpelareEtt,

        SpelareTvå,

        Hejduk
    }

    public class EntitetsFabrik {

        public static Entity CreateEntity(EntitetTyp typ) {
            return typ switch {
                EntitetTyp.Himmel => new Himmel(),
                EntitetTyp.Bakgrund => new Bakgrund(),
                EntitetTyp.Mark => new Mark(),
                EntitetTyp.SpelareEtt => throw new ArgumentException("SpelareEtt type not supported"),
                EntitetTyp.SpelareTvå => throw new ArgumentException("SpelareTvå type not supported"),
                EntitetTyp.Hejduk => throw new ArgumentException("Hejduk type not supported"),
                _ => throw new ArgumentException("entity type not supported"),
            };
        }
    }
}