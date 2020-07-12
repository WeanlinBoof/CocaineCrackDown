namespace CocaineCrackDown.Objekter {

    //alla djur ska ärva djur
    public class Djur : InteraktivaObjekt {
        public float GolvTopSlut { get; set; }

        public enum Riktning {
            Höger,
            Vänster,
        };

        public bool Attackerar { get; set; } = false;

        public virtual void Attack(bool attackstatus) {
        }

        public virtual void FlyttaVänster(float RörelseHastighet) {
        }

        public virtual void FlyttaHöger(float RörelseHastighet) {
        }

        public virtual void FlyttaUpp(float RörelseHastighet) {
        }

        public virtual void FlyttaNed(float RörelseHastighet) {
        }
    }

    // alla människor ska ärva männniska
    public class Människa : Djur {
        //lägg till uhhh dialog variabel kommer inte ihåg just denna stund ok
    }
}