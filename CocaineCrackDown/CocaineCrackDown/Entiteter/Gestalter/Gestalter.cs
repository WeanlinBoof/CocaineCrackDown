
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CocaineCrackDown.Entiteter.Gestalter {
    public static class Gestalter {
        private static bool _isInitialized = false;
        private static readonly Dictionary<string , GestaltParameter> gestalter = new Dictionary<string , GestaltParameter>();
            
        public static List<GestaltParameter> All() {
            if(!_isInitialized) {
                LoadFromData();
            }
            
            return gestalter.Values.ToList();
        }

        public static GestaltParameter Get(string name) {
            if(!_isInitialized) {
                LoadFromData();
            }
            return gestalter[name];
        }

        public static void LoadFromData() {
            Console.WriteLine("Laddar .xml filer...");
            IEnumerable<string> FilNamn = Directory.EnumerateFiles("Content/Data/" , "*.xml");

            foreach(string characterFilename in FilNamn) {
                using StreamReader lässare = new StreamReader(new FileStream(FilNamn.ToString() , FileMode.Open));
                XmlSerializer XmlLässare = new XmlSerializer(typeof(GestaltParameter));
                GestaltParameter Gestalt = (GestaltParameter)XmlLässare.Deserialize(lässare);
                gestalter[Gestalt.KaraktärNamn] = Gestalt;
            }
            _isInitialized = true;
        }

        public static GestaltParameter GetNextAfter(string name) {
            List<GestaltParameter> list = gestalter.Values.ToList();
            return list[(list.IndexOf(gestalter[name]) + 1) % gestalter.Count];
        }
    }
}
