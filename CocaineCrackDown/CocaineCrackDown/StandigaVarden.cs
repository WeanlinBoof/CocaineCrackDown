namespace CocaineCrackDown.Verktyg {

    public static class StandigaVarden {

        public const float AttackTimerNollstälare = 0f;
        public const int SkärmBredd = 1280;
        public const int SkärmHöjd = 720;
        public const float RenderLayerDepthFactor = 1 / 10000f;
       public static class Values
        {
            public const float ExplosionDamage = 80f;
            public const float ExplosionRadius = 80f;
            public const float ExplosionKnockback = 20f;
            public const float ExplosionAerialKnockback = 600f;

            public const int TeamIndexBlue = 1;
            public const int TeamIndexRed = 2;
        }

        public static class Nät {

            public const string SPELNAMN = "CocaineCrackDown";

            public const string VARDENSNAMN = "localhost";

            public const int PORTEN = 14882;

            public const ulong bithor = 1488200000;

            public const ulong gorbi = 1488100000;
        }

        public static class Tags {

            public const int Spelare = 1;
            public const int Avgrund = 2;
            public const int Collectible = 3;
            public const int EventEmitter = 4;
            public const int Obstacle = 5;
            public const int ReadyZone = 6;
            public const int Explosion = 7;
        }

        public static class Lager {
            public const int Explosion = 18;
            public const int Background = 17;
            public const int MapBackground = 16;
            public const int MapObstacles = 15;
            //iteraktions lager
            public const int Interactables = 14;
            public const int Shadow = 13;
            public const int Bullet = 12;
            public const int PlayerBehindest = 11;
            public const int PlayerBehind = 10;
            public const int Player = 9;
            public const int PlayerFront = 8;
            public const int PlayerFrontest = 7;
            public const int MapForeground = 6;
            public const int Lights = 5;
            public const int Lights2 = 4;
            public const int Foreground = 3;
            public const int Weather = 2;
            public const int HUD = 1;
        }

        public static class TiledProperties {

            public const string EmitsLight = "emits_light";

            public const string SpawnerMinIntervalSeconds = "min_interval_seconds";

            public const string SpawnerMaxIntervalSeconds = "max_interval_seconds";

            public const string SpawnerMaxSpawns = "max_spawns";

            public const string SpawnerCameraTracking = "camera_tracking";
            public const string BulletPassable = "bullet_passable";
            public const string SpawnerWeaponBlacklist = "weapon_blacklist";
            public const string SpawnerWeaponWhitelist = "weapon_whitelist";
            public const string SpawnerRarityBlacklist = "rarity_blacklist";
            public const string SpawnerRarityWhitelist = "rarity_whitelist";
            public const string CaptureTheFlagBlueCollisionZone = "ctf_blue";
            public const string CaptureTheFlagRedCollisionZone = "ctf_red";
            public const string CTF_Only = "ctf_only";
            public const string KOTH_Only = "koth_only";
            public const string KingOfTheHillZone = "koth_zone";
            public const string CaptureTheFlagBlueZone = "team_blue_zone";
            public const string CaptureTheFlagRedZone = "team_red_zone";
            public const string MapStartWeapon = "start_weapon";
            public const string PlayerSpawnTeamIndex = "team_index";

        }

        public static class TiledObjects {

            public const string KartaEntitet = "KakelKartaEntitet";

            public const string ObjectGroup = "Objects";

            public const string Kollision = "Kollision";

            public const string SpelarSpawn = "spelare_spawn";

            public const string SakSpawn = "sak_spawn";

            public const string Pit = "pit";

            public const string EventSändare = "event_sandare";

            public const string CameraTracker = "camera_tracker";

            public const string Monitor = "monitor";

            public const string Zone = "zone";
        }

        public static class Strings {

            public const string CollisionMapEventEnter = "enter";

            public const string CollisionMapEventExit = "exit";

            public const string TiledMapGameModeKey = "mode";

            public const string TiledMapGameModeDisplayKey = "mode_display";

            public const string TiledMapTeamsKey = "teams";

            public const string TiledMapTeamsDisplayKey = "teams_display";

            public const string TiledMapMapKey = "karta";

            public const string TiledMapMapDisplayKey = "map_display";

            public const string TiledMapCharacterSelectKey = "gestalt";

            public const string EventReady = "ready";

            public const string DefaultStartCharacter = "";

            public const string DefaultStartWeapon = "";
        }
        public static  class Stencils
        {
            public const int EntityShadowStencil = 1;
            public const int HiddenEntityStencil = 2;
            public const int PlayerTeamColorStencil = 3;
        }
    }
}