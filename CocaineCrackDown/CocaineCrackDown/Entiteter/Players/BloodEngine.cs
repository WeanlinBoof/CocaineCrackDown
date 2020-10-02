using CocaineCrackDown.Verktyg;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CocaineCrackDown.Verktyg.StandigaVarden;

using Random = System.Random;
using rng = Nez.Random;



namespace CocaineCrackDown.Entiteter {
    public class BloodEngine : Component, IUpdatable {
        private Random _rng;
        private Cooldown _leakInterval;
        private Texture2D _bloodTexture;

        private bool _leak = false;
        private int _particlesPrLeakage;

        public BloodEngine(Color bloodColor) {
            _rng = new Random();
            _leakInterval = new Cooldown(1f);
            _bloodTexture = new Texture2D(Core.GraphicsDevice , 1 , 1);
            _bloodTexture.SetData(new[] { bloodColor });
        }

        public void Sprinkle(float damage , Vector2 direction) {
            double particlesCount = Math.Max(Math.Ceiling(damage / 4) , 0);

            for(int i = 0; i < particlesCount; i++) {
                var particle = Entity.Scene.AddEntity(new BloodParticle(_bloodTexture , Transform.Position.X , Transform.Position.Y));

                float x = _rng.Next(-50 , 50) / 100f;
                float y = _rng.Next(-50 , 50) / 100f;
                Vector2 trueDirection = new Vector2(direction.X + direction.X * x , direction.Y + direction.Y * y);
                const float speedConstant = 0.015f;

                particle.Hastighet = new Vector2(
                    trueDirection.X * speedConstant ,
                    trueDirection.Y * speedConstant);
                if(particle.Hastighet.Length() == 0) {
                    particle.Hastighet = new Vector2(x , y);
                }
            }
        }


        public void Blast(int particles = 100 , float power = 4.0f) {
            for(int i = 0; i < particles; i++) {
                BloodParticle particle = Entity.Scene.AddEntity(new BloodParticle(_bloodTexture , Transform.Position.X , Transform.Position.Y , false));
                particle.Hastighet = new Vector2(rng.MinusOneToOne() * power , rng.MinusOneToOne() * power);
            }
        }

        public void Leak(int particlesPrLeakage = 20 , float duration = 10f) {
            _leak = true;
            _particlesPrLeakage = particlesPrLeakage;
            _leakInterval.Start();
            Core.Schedule(duration , _ => _leak = false);
        }

        public void StopLeaking() {
            _leak = false;
        }

        public void Update() {
            _leakInterval.Update();
            if(_leak && _leakInterval.IsReady()) {
                Blast(_particlesPrLeakage , 2.5f);
                _leakInterval.Start();
            }
        }
 
        public class BloodParticle : SpelObjekt {
            private Mover _mover;
            private CircleCollider _bloodHitbox;
            private Texture2D _bloodTexture;
            private SpriteRenderer Renderer;

            private bool _drawAbovePlayer;

            public BloodParticle(Texture2D bloodTexture , float x , float y , bool drawAbovePlayer = true) : base(x , y) {
                _drawAbovePlayer = drawAbovePlayer;
                _bloodTexture = bloodTexture;
            }

            public override void VidDespawn() { }

            public override void VidSpawn() {
                //_sprite = entity.addComponent(new Sprite(AssetLoader.GetTexture("particles/blood")));
                Renderer = AddComponent(new SpriteRenderer(_bloodTexture));
                Renderer.RenderLayer = _drawAbovePlayer ? Lager.PlayerFrontest : Lager.MapObstacles;
                Renderer.Material = new Material(blendState: BlendState.NonPremultiplied);
                Renderer.Material.SamplerState = SamplerState.PointClamp;
                _mover = AddComponent(new Mover());

                _bloodHitbox = AddComponent(new CircleCollider(0.1f));
                Flags.SetFlagExclusive(ref _bloodHitbox.CollidesWithLayers , Lager.MapObstacles);
                _bloodHitbox.PhysicsLayer = 0;

                //Scale
                float random_scale = (float)rng.Range(-20 , 20) / 100;
                Scale = new Vector2(1f + random_scale , 1f + random_scale);

                //Rotation
                Renderer.Transform.Rotation = rng.NextAngle();

                if(_drawAbovePlayer)
                    Core.Schedule(0.5f , _ => UpdateRenderLayerDepth());
            }

            public override void Uppdatera() {
                if(Hastighet.Length() == 0) {
                    Renderer.Color.A -= 2;
                    if(Renderer.Color.A <= 2) {
                        Destroy();
                    }
                    return;
                }

                Hastighet = 0.878f * Hastighet;
                var isColliding = _mover.Move(Hastighet , out CollisionResult result);
                if(isColliding && result.Collider?.Entity?.Tag == Tags.Avgrund)
                    Destroy();

                if(Hastighet.Length() < 0.001f && Hastighet.Length() > 0) {
                    Hastighet = Vector2.Zero;
                    _bloodHitbox.UnregisterColliderWithPhysicsSystem();
                    UpdateInterval = 15;
                }
            }

            private void UpdateRenderLayerDepth() {
                Renderer.RenderLayer = Lager.MapObstacles;
                Renderer.LayerDepth = 0.9f;
            }
        }

    }
}
