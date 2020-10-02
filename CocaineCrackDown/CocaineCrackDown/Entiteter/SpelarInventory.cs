
using CocaineCrackDown.Verktyg;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Entiteter {

    public class SpelarInventory : Component {

        private const float ThrowSpeed = 0.5f;

        private Spelare spelare;

        public Vapen Vapen { get; private set; }

        public bool Beväpnad => Vapen != null;

        public override void OnAddedToEntity() {
            spelare = Entity as Spelare;
            System.PlayerMetadata metadata = KontextHanterare.PlayerMetadataByIndex(spelare.SpelarIndex);
            EquipWeapon(metadata.Vapnet ?? CollectibleDict.Get(StandigaVarden.Strings.DefaultStartWeapon).Vapen);
        }

        public void EquipWeapon(VapenParameters weapon , CollectibleMetadata metadata = null) {
            if(Vapen != null) {
                DropWeapon();
            }

            Vapen newWeapon = null;
            //if(weapon.Typ == VapenTyp.ProjektilVapen) {
            //    newWeapon = new ProjektilVapen(spelare , weapon as GunParameters , metadata as GunMetadata);
            //}
            if(weapon.Typ == VapenTyp.NärstridsVapen) {
                newWeapon = new NärstridsVapen(spelare , weapon as NärstridsVapenParameters , metadata as MeleeMetadata);
            }

            Vapen = Entity.Scene.AddEntity(newWeapon);

            var meta = KontextHanterare.PlayerMetadataByIndex(spelare.SpelarIndex);
            if(meta != null) {
                meta.Vapnet = weapon;
            }
        }

        public void DestroyWeapon() {
            Vapen.Förinta();
            Vapen = null;
            var meta = KontextHanterare.PlayerMetadataByIndex(spelare.SpelarIndex);
            if(meta != null) {
                meta.Vapnet = null;
            }
        }

        public void SwitchWeapon() {
#if DEBUG
            if(!Beväpnad) {
                return;
            }

            VapenParameters next = CollectibleDict.GetNextWeaponAfter(Vapen.Name);
            EquipWeapon(next);
#else
            DropWeapon();
#endif
        }

        public void UnequipWeapon() {
            Vapen.Förinta();
            Vapen = null;
            var meta = KontextHanterare.PlayerMetadataByIndex(spelare.SpelarIndex);
            if(meta != null) {
                meta.Vapnet = null;
            }
        }

        //public void Reload() {
        //    if(Beväpnad && Vapen is ProjektilVapen gun)
        //        gun.ReloadMagazine();
        //}

        public void Attack() {
            if(!Beväpnad) {
                return;
            }

            Vapen.Attack();
        }

        public void DropWeapon() {
            if(Vapen == null)
                return;

            Collectible throwedItem = null;

            //if(Vapen is ProjektilVapen gun) {
            //    throwedItem = Entity.Scene.AddEntity(new Collectible(Transform.Position.X , Transform.Position.Y ,
            //        gun.Parameters.Namn , true , gun.Metadata));
            //}
            if(Vapen is NärstridsVapen melee) {
                throwedItem = Entity.Scene.AddEntity(new Collectible(Transform.Position.X , Transform.Position.Y ,
                    melee.Parameters.Namn , true , melee.Metadata));
            }

            throwedItem.Metadata?.OnDropEvent?.Invoke(throwedItem , spelare);

            throwedItem.Hastighet = new Vector2(spelare.Position.X * ThrowSpeed, spelare.Position.Y * ThrowSpeed);
            UnequipWeapon();
        }
    }
}