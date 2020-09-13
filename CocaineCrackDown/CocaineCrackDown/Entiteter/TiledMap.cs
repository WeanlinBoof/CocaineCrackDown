using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tiled;

namespace CocaineCrackDown.Entiteter {
    public class TiledMap : Entity {
        public override void OnAddedToScene() {
            base.OnAddedToScene();

            Name = "KakelKarta";
            TmxMap map = Scene.Content.LoadTiledMap("Content/tilemap.tmx");
            TiledMapRenderer tiledMapRenderer = AddComponent(new TiledMapRenderer(map, "collision"));
			tiledMapRenderer.SetLayersToRender(new[] { "tiles","terrain","details" });

            // render below/behind everything else. our player is at 0 and projectile is at 1.
            tiledMapRenderer.RenderLayer = 10;

            // the details layer will write to the stencil buffer so we can draw a shadow when the player is behind it. we need an AlphaTestEffect
            // here as well
            TiledMapRenderer tiledMapDetailsComp = AddComponent(new TiledMapRenderer(map));
            tiledMapDetailsComp.SetLayerToRender("above-details");
            tiledMapDetailsComp.RenderLayer = -1;

            tiledMapDetailsComp.Material = Material.StencilWrite();
            tiledMapDetailsComp.Material.Effect = Scene.Content.LoadNezEffect<SpriteAlphaTestEffect>();

            // setup our camera bounds with a 1 tile border around the edges (for the outside collision tiles)
            Vector2 topLeft = new Vector2(map.TileWidth , map.TileWidth);
            Vector2 bottomRight = new Vector2(map.TileWidth * (map.Width - 1) ,map.TileWidth * (map.Height - 1));
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
