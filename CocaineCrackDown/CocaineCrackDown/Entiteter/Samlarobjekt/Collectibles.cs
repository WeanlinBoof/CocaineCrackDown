

using System.Collections.Generic;
using System.Linq;

namespace CocaineCrackDown.Entiteter {
    public static class CollectibleDict {
        private static bool _isInitialized = false;
        private static Dictionary<string , CollectibleParameters> _collectibles =
            new Dictionary<string , CollectibleParameters>();

        public static List<CollectibleParameters> All() {
            if(!_isInitialized) {
                LoadFromData();
            }
            return _collectibles.Values.ToList();
        }

        public static List<CollectibleParameters> All(Ovanlighet rarity) {
            var list = All().Where(w => w.Ovanlighet == rarity).ToList();
            return list;
        }

        public static CollectibleParameters Get(string name) {
            if(!_isInitialized) {
                LoadFromData();
            }
            return _collectibles[name];
        }

        public static void LoadFromData() {
            if(_isInitialized)
                return;

            //Guns.LoadFromData();
            //foreach(var gun in Guns.All()) {
            //    _collectibles.Add(gun.Namn , new CollectibleParameters {
            //        Namn = gun.Namn ,
            //        Vapen = gun ,
            //        Typ = SamlarobjektTyp.Vapen ,
            //        ChansAttTappa = gun.ChansAttTappa ,
            //        Ovanlighet = gun.Ovanlighet
            //    });
            //}

            NärstridsVapnen.LoadFromData();
            foreach(var melee in NärstridsVapnen.All()) {
                _collectibles.Add(melee.Namn , new CollectibleParameters {
                    Namn = melee.Namn ,
                    Vapen = melee ,
                    Typ = SamlarobjektTyp.Vapen ,
                    ChansAttTappa = melee.ChansAttTappa ,
                    Ovanlighet = melee.Ovanlighet
                });
            }

            _isInitialized = true;
        }

        public static VapenParameters GetNextWeaponAfter(string name) {
            if(!_isInitialized) {
                LoadFromData();
            }

            var list = _collectibles.Values
                .Where(c => c.Typ == SamlarobjektTyp.Vapen)
                .ToList();
            var next = list[(list.IndexOf(_collectibles[name]) + 1) % _collectibles.Count];
            return next.Vapen;
        }
    }
}
