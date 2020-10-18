using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Client.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Komponenter {
public class StatusVärden : Component {

        public float Hastighet { get; private set; }

        public float HoppHöjd { get; private set; }

        public float TyngdKraft { get; private set; }

        public int Hälsa { get; private set; }

        public int Pansar { get; private set; }

        public float SkadaLägsta { get; private set; }

        public float SkadaHögsta { get; private set; }

        public StatusVärden(float hastighet = 70 , float hoppHöjd = 60 , float tyngdKraft = 9.82f , int hälsa = 100 , int pansar = 0,float skadaLägsta = 30,float skadaHögsta = 40) {
            Hastighet = hastighet;
            HoppHöjd = hoppHöjd;
            TyngdKraft = tyngdKraft;
            Hälsa = hälsa;
            Pansar = pansar;
            SkadaLägsta = skadaLägsta;
            SkadaHögsta = skadaHögsta;
        }

        public int RäknaSkada(int damage , int EntitetensPansar = 0) {
           damage = Mathf.Clamp(damage,EntitetensPansar,999);
           Hälsa -= damage;
           return Hälsa;
        }
    }
}
