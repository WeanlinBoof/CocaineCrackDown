using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tiled;

namespace CocaineCrackDown.Entiteter {
    public class TiledMap : Entity {
        public string TMXKartaPlats { get; set; }
        public string NamnKartaTMX { get; set; }
        public string KollitionsLager { get; set; }
        public TmxMap Karta { get; set; }
        public TmxObjectGroup ObjGrupp;
        public TiledMapRenderer TiledMapRenderer { get; set; }
        public Vector2 bottomRight;
        public Vector2 topLeft;
        public TmxObject SpawnSpelareEtt;
        public TmxObject SpawnSpelareTvå;
        public TiledMap(string TMXKartanamn) {
            NamnKartaTMX = TMXKartanamn;
            TMXKartaPlats = $"Content/{NamnKartaTMX}.tmx";
            Name = NamnKartaTMX;
            KollitionsLager = "Kollision";
        }
        public override void OnAddedToScene() {
            base.OnAddedToScene();
            Karta = Scene.Content.LoadTiledMap(TMXKartaPlats);
            TiledMapRenderer = AddComponent(new TiledMapRenderer(Karta , KollitionsLager));
            TiledMapRenderer.SetLayersToRender(new[] { "Mark"  });
            //här är huur man får fram object gruper från tiléd 
            ObjGrupp = Karta.GetObjectGroup("SpelarSpawn");
            SpawnSpelareEtt = ObjGrupp.Objects["spelare_ett"];
            SpawnSpelareTvå = ObjGrupp.Objects["spelare_två"];
            // render below/behind everything else. higher number = further back
            TiledMapRenderer.SetRenderLayer(2);
            
            KamraGränser();
        }

        protected void KamraGränser() {
            topLeft = new Vector2(Karta.TileWidth , Karta.TileWidth);
            bottomRight = new Vector2(Karta.TileWidth * Karta.Width , Karta.TileWidth * Karta.Height);
            AddComponent(new KameraGränser(topLeft , bottomRight));
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
        }

        public override void Update() {
            base.Update();
        }
    }
}
