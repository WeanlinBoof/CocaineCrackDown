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
        public TiledMapRenderer TiledMapRenderer { get; set; }
        public Vector2 bottomRight;
        public Vector2 topLeft;
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
            TiledMapRenderer.SetLayersToRender(new[] { "Mark" });

            // render below/behind everything else. our player is at 0 and projectile is at 1.
            TiledMapRenderer.SetRenderLayer(2);

            //// the details layer will write to the stencil buffer so we can draw a shadow when the player is behind it. we need an AlphaTestEffect
            //// here as well
            //TiledMapRenderer tiledMapDetailsComp = AddComponent(new TiledMapRenderer(map));
            //tiledMapDetailsComp.SetLayerToRender("above-details");
            //tiledMapDetailsComp.RenderLayer = -1;

            //tiledMapDetailsComp.Material = Material.StencilWrite();
            //tiledMapDetailsComp.Material.Effect = Scene.Content.LoadNezEffect<SpriteAlphaTestEffect>();

            // setup our camera bounds with a 1 tile border around the edges (for the outside collision tiles)
            KamraGränser();
        }

        protected void KamraGränser() {
            topLeft = new Vector2(Karta.TileWidth , Karta.TileWidth);
            bottomRight = new Vector2(Karta.TileWidth * (Karta.Width - 1) , Karta.TileWidth * (Karta.Height - 1));
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
