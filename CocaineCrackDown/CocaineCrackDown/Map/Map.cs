using CocaineCrackDown.Entiteter.Events;
using CocaineCrackDown.Map.MapBuilders;
using CocaineCrackDown.System;
using CocaineCrackDown.Verktyg;
using CocaineCrackDown.Verktyg.Events;


using Microsoft.Xna.Framework;

using Nez;
using Nez.Tiled;

using System;
using System.Collections.Generic;
using System.Linq;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace FredflixAndChell.Shared.Maps {
    public class Map : Entity
    {
        public TmxMap Tmxmap;
        private SpelSystem SpelSystem;
        private List<SpawnLocation> AnvändaSpawnPlatser;
        public List<MapEventListener> MapEventLyssnare { get; set; }

        // Map properties
        public Color AmbientLightingColor { get; set; }
        public List<SpawnLocation> SpawnLocations { get; set; }


        public Map() : base(TiledObjects.KartaEntitet)
        {
            MapEventLyssnare = new List<MapEventListener>();
            SpawnLocations = new List<SpawnLocation>();
            AnvändaSpawnPlatser = new List<SpawnLocation>();
        }

        public override void OnAddedToScene()
        {
            SpelSystem = Scene.GetSceneComponent<SpelSystem>();
        }

        public void EmitMapEvent(KartEvent mapEvent, bool emitGlobally = false)
        {
            MapEventLyssnare.Where(listener => listener.EventKey == mapEvent.EventKey).ToList().ForEach(listener => listener.EventTriggered(mapEvent));

            if (emitGlobally)
            {
                SpelSystem.Publish(SpelEvents.GlobalMapEvent, new GlobalKartEventParameter { KartEvent = mapEvent });
            }
        }
/// <summary>
/// FIXA FIXA KOLLA KOLLA 
/// </summary>
/// <param name="mapName"></param>
        public void Setup(string mapName)
        {
            //Tmxmap Lodear metod samt lite extra
            mapName = "testnr1";
            Tmxmap = Scene.Content.LoadTiledMap("testnr1");

            var tiledMapComponent = AddComponent(new TiledMapRenderer(Tmxmap));
            tiledMapComponent.LayerIndicesToRender = new int[] { 5, 2, 1, 0 };
            tiledMapComponent.RenderLayer = Lager.MapBackground;
            tiledMapComponent.SetMaterial(Material.StencilWrite(Stencils.EntityShadowStencil));

            SetupMapObjects();

            var tiledMapDetailsComponent = AddComponent(new TiledMapRenderer(Tmxmap));
            tiledMapDetailsComponent.LayerIndicesToRender = new int[] { 3, 4 };
            tiledMapDetailsComponent.RenderLayer = Lager.MapForeground;
            tiledMapDetailsComponent.SetMaterial(Material.StencilWrite(Stencils.HiddenEntityStencil));

            SetupMapProperties(Tmxmap);
        }

        private void SetupMapProperties(TmxMap tiledMap)
        {

            ApplyWeather(tiledMap);
            ApplyAmbientLighting(tiledMap);

            if (tiledMap.Properties.ContainsKey(TiledProperties.MapStartWeapon)
                && !string.IsNullOrWhiteSpace(tiledMap.Properties[TiledProperties.MapStartWeapon]))
            {
                var weaponName = tiledMap.properties[TiledProperties.MapStartWeapon];
                var collectible = CollectibleDict.Get(weaponName);
                foreach (var meta in ContextHelper.PlayerMetadata)
                {
                    meta.Weapon = collectible.Weapon;
                }
            }

        }

        private void SetupMapObjects()
        {
            TmxObjectGroup mapObjects = Tmxmap.GetObjectGroup(TiledObjects.ObjectGroup);
            

        }

        public List<SpawnLocation> GetSpawnLocations(int teamIndex = -1)
        {
            var filteredLocations = SpawnLocations.AsEnumerable();

            if (teamIndex > 0)
            {
                filteredLocations = filteredLocations.Where(loc => loc.TeamIndex == teamIndex);
            }

            var spawnLocations = filteredLocations.ToList();

            return spawnLocations;
        }

        public SpawnLocation GetUniqueSpawnLocation(int teamIndex = -1)
        {
            if (AnvändaSpawnPlatser.Count == SpawnLocations.Count)
            {
                throw new InvalidProgramException("All spawn locations occupied!");
            }

            var filteredLocations = SpawnLocations
                .Where(loc => !AnvändaSpawnPlatser.Contains(loc));

            if (teamIndex > 0)
            {
                filteredLocations = filteredLocations
                    .Where(loc => loc.TeamIndex == teamIndex);
            }

            var spawnLocation = filteredLocations
                .ToList()
                .randomItem();

            AnvändaSpawnPlatser.Add(spawnLocation);
            Core.schedule(2f, _ => AnvändaSpawnPlatser.Remove(spawnLocation));
            return spawnLocation;
        }

        /* Unused tile-based lightsources
        private void CustomizeTiles(TiledMapComponent mapComponent)
        {
            var tileSize = new Vector2(mapComponent.tiledMap.tileWidth, mapComponent.tiledMap.tileHeight);
            for (float x = 0; x < mapComponent.width; x += tileSize.X)
            {
                for (float y = 0; y < mapComponent.height; y += tileSize.X)
                {
                    var tilePos = new Vector2(x, y);
                    var tile = mapComponent.getTileAtWorldPosition(tilePos);
                    CustomizeTile(tile, tilePos, tileSize);
                }
            }
        }

        private void CustomizeTile(TiledTile tile, Vector2 pos, Vector2 size)
        {
            if (tile == null)
            {
                return;
            }

            var properties = tile?.tilesetTile?.properties;
            if (properties == null) return;
            if (properties.ContainsKey(TiledProperties.EmitsLight))
            {
                var entity = scene.createEntity("world-light", pos + size);
                entity.setScale(0.55f);

                var sprite = entity.addComponent(new Sprite(AssetLoader.GetTexture("effects/lightmask")));
                sprite.material = Material.blendLighten();
                sprite.color = new Color(Color.White, 0.4f);
                sprite.renderLayer = Layers.Lights;
            }
        }
        */

        public SceneComponent GetWeatherEffect(string name)
        {
            switch (name)
            {
                case "snowstorm":
                    return new Snowstorm();
                case "dungeongloom":
                    return new DungeonGloom();
                default:
                    Console.WriteLine("Weather effect '" + name + "' not found");
                    return null;
            }
        }

        private void ApplyWeather(TiledMap tiledMap)
        {
            try
            {
                var weatherAttribute = tiledMap.properties["weather"];
                if (weatherAttribute != null && weatherAttribute != "")
                {
                    scene.addSceneComponent(GetWeatherEffect(weatherAttribute));
                }
            }
            catch (KeyNotFoundException) { }
        }

        private void ApplyAmbientLighting(TiledMap tiledMap)
        {
            try
            {
                var ambientLightingColor = tiledMap.properties["ambient_lighting"];
                AmbientLightingColor = ColorExt.hexToColor(ambientLightingColor.Substring(3));

            }
            catch (KeyNotFoundException)
            {
                AmbientLightingColor = Color.DarkGray;
            }
        }

    }
}
