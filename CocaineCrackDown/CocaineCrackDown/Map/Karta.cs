using CocaineCrackDown.Entiteter.Events;
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

namespace CocaineCrackDown.Entiteter {
    public class Karta : Entity {

        public TmxMap Tmxmap;
        private SpelSystem SpelSystem;
        public List<MapEventListener> MapEventLyssnare { get; set; }
        public Color AmbientLightingColor { get; set; }
        public List<SpawnLocation> SpawnLocations { get; set; }
        public string TMXKartaPlats { get; set; }
        public string TmxKartaNamn { get; set; }
        public string KollitionsLager { get; set; }
        public TiledMapRenderer TiledMapRenderer { get; set; }
        public Vector2 bottomRight;
        public Vector2 topLeft;
        public Karta(string TMXKartanamn) : base(TiledObjects.KartaEntitet) {
            TmxKartaNamn = TMXKartanamn;
            TMXKartaPlats = $"Content/{TmxKartaNamn}.tmx";
            KollitionsLager = "Kollision";
            MapEventLyssnare = new List<MapEventListener>();
            SpawnLocations = new List<SpawnLocation>();
        }
        public override void OnAddedToScene() {
            base.OnAddedToScene();
            SpelSystem = Scene.GetSceneComponent<SpelSystem>();
            Tmxmap = Scene.Content.LoadTiledMap(TMXKartaPlats);
            TiledMapRenderer = AddComponent(new TiledMapRenderer(Tmxmap , KollitionsLager));
            TiledMapRenderer.SetLayersToRender(new[] { "Mark" });
            TiledMapRenderer.SetRenderLayer(2);
            KamraGränser();
        }
        public void EmitMapEvent(KartEvent mapEvent, bool emitGlobally = false)
        {
            MapEventLyssnare.Where(listener => listener.EventKey == mapEvent.EventKey).ToList().ForEach(listener => listener.EventTriggered(mapEvent));

            if (emitGlobally)
            {
                SpelSystem.Publish(SpelEvents.GlobalMapEvent, new GlobalKartEventParameter { KartEvent = mapEvent });
            }
        }
        public void Setup(string mapName) {
            //Tmxmap Lodear metod samt lite extra skräp som e temporet
            mapName = "testnr1";
            Tmxmap = Scene.Content.LoadTiledMap(mapName);

            TiledMapRenderer tiledMapComponent = AddComponent(new TiledMapRenderer(Tmxmap));
            tiledMapComponent.LayerIndicesToRender = new int[] { 5, 2, 1, 0 };
            tiledMapComponent.RenderLayer = Lager.MapBackground;
            tiledMapComponent.SetMaterial(Material.StencilWrite(Stencils.EntityShadowStencil));

            SetupMapObjects();

            TiledMapRenderer tiledMapDetailsComponent = AddComponent(new TiledMapRenderer(Tmxmap));
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
                string weaponName = tiledMap.Properties[TiledProperties.MapStartWeapon];
                CollectibleParameters collectible = CollectibleDict.Get(weaponName);
                foreach (PlayerMetadata meta in KontextHanterare.PlayerMetadata)
                {
                    meta.Vapnet = collectible.Vapen;
                }
            }

        }

        private void SetupMapObjects()
        {
            TmxObjectGroup mapObjects = Tmxmap.GetObjectGroup(TiledObjects.ObjectGroup);

            this.BuildCollisionZones(mapObjects);
            this.BuildPits(mapObjects);
            this.BuildItemSpawns(mapObjects);
            this.BuildEventEmitters(mapObjects);
            this.BuildMonitors(mapObjects);
            this.BuildLightSources(mapObjects);
            this.BuildCameraTrackers(mapObjects);
            this.BuildZones(mapObjects);
            this.BuildPlayerSpawns(mapObjects);

        }

        public List<SpawnLocation> GetSpawnLocations()
        {
            IEnumerable<SpawnLocation> filteredLocations = SpawnLocations.AsEnumerable();

            List<SpawnLocation> spawnLocations = filteredLocations.ToList();

            return spawnLocations;
        }

        public SpawnLocation GetUniqueSpawnLocation()
        {
            if (AnvändaSpawnPlatser.Count == SpawnLocations.Count)
            {
                throw new InvalidProgramException("All spawn locations occupied!");
            }

            IEnumerable<SpawnLocation> filteredLocations = SpawnLocations.Where(loc => !AnvändaSpawnPlatser.Contains(loc));

            SpawnLocation spawnLocation = filteredLocations.ToList().RandomItem();

            AnvändaSpawnPlatser.Add(spawnLocation);
            Core.Schedule(2f, _ => AnvändaSpawnPlatser.Remove(spawnLocation));
            return spawnLocation;
        }

        /// <summary>
        /// Fixa Väder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SceneComponent GetWeatherEffect(string name)
        {
            switch(name) {
                //case "snowstorm":
                //    return new Snowstorm();
                default:
                    Console.WriteLine("Weather effect '" + name + "' not found");
                    return null;
            }
        }

        private void ApplyWeather(TmxMap tiledMap)
        {
            try
            {
                var weatherAttribute = tiledMap.Properties["weather"];
                if(!string.IsNullOrEmpty(weatherAttribute))
                {
                    Scene.AddSceneComponent(GetWeatherEffect(weatherAttribute));
                }
            }
            catch (KeyNotFoundException) { }
        }

        private void ApplyAmbientLighting(TmxMap tiledMap)
        {
            try
            {
                var ambientLightingColor = tiledMap.Properties["ambient_lighting"];
                AmbientLightingColor = ColorExt.HexToColor(ambientLightingColor.Substring(3));

            }
            catch (KeyNotFoundException)
            {
                AmbientLightingColor = Color.DarkGray;
            }
        }


        protected void KamraGränser() {
            topLeft = new Vector2(Tmxmap.TileWidth , Tmxmap.TileWidth);
            bottomRight = new Vector2(Tmxmap.TileWidth * (Tmxmap.Width - 1) , Tmxmap.TileWidth * (Tmxmap.Height - 1));
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
