

using CocaineCrackDown.Komponenter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Tweens;

using System;
using System.Linq;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.Entiteter {
    public enum CollectibleState {
        Appearing,
        Available,
        Unavailable
    }
    public class Collectible : SpelObjekt, ITriggerListener {
        private CollectibleState _collectibleState;

        private ITween<Vector2> _hoverTween;

        private Effect _flashEffect;
        private Mover _mover;
        private NärstridsVapenRendrare Rendrare;
        private Vector2 _acceleration;
        private Collider _pickupHitbox;
        private Collider _collisionHitbox;
        private CollectibleCollisionHandler _collisionHandler;

        private bool _dropped;
        private bool _isHighlighted;
        private int _numberOfPlayersInProximity;

        public CollectibleState CollectibleState => _collectibleState;
        public CollectibleParameters Preset { get; set; }
        public CollectibleMetadata Metadata { get; set; }

        public Collectible(float x , float y , string name , bool dropped , CollectibleMetadata Metadata = null) : base(x , y) {
            Preset = CollectibleDict.Get(name);
            _dropped = dropped;
            _acceleration = new Vector2();
            this.Metadata = Metadata;
        }

        public override void VidDespawn() {
        }

        public override void VidSpawn() {
            SetupComponents();
            if(Preset.Namn == "Flag") {
                //AddComponent(new LightSource(Metadata.Color , this));
            }
        }

        private void SetupComponents() {
            _collisionHandler = AddComponent(new CollectibleCollisionHandler());

            //NärstridsVapenRendrare sprite = null;
            ////if(Preset.Vapen is GunParameters gunParams) {
            ////    sprite = gunParams.Sprite;
            ////} 
            //if(Preset.Vapen is NärstridsVapenParameters meleeParams) {
            //    NärstridsVapnenAnimationer T = meleeParams.Sprite;
            //    sprite.atlasAnimationsKomponent = new AtlasAnimationKomponent<>();

            //}

            //_sprite = AddComponent(new SpriteAnimator(sprite.Icon.ToSpriteAnimation(sprite.Source).frames[0]));
            //_sprite.renderLayer = Layers.Interactables;
            //_sprite.material = new Material();
            //_sprite.color = Metadata?.Color ?? Color.White;

            Scale = new Vector2(0.5f , 0.5f);
            _hoverTween = this.TweenLocalScaleTo(0.5f , 0.5f)
                .SetEaseType(EaseType.ExpoOut)
                .SetCompletionHandler(_ => Hover(2f));
            _hoverTween.Start();

            _mover = AddComponent(new Mover());

            // Delay pickup (debug)
            Core.Schedule(0.5f , _ => SetupPickupHitbox());

            //Collision
            _collisionHitbox = AddComponent(new CircleCollider(4));
            Flags.SetFlagExclusive(ref _collisionHitbox.CollidesWithLayers , Lager.MapObstacles);
        }

        private void SetupPickupHitbox() {
            SetTag(Tags.Collectible);

            if(_collectibleState == CollectibleState.Unavailable)
                return;

            _collectibleState = CollectibleState.Available;
            _pickupHitbox = AddComponent(new BoxCollider());
            _pickupHitbox.IsTrigger = true;
            Flags.SetFlagExclusive(ref _pickupHitbox.PhysicsLayer , Lager.Interactables);

            var interactable = AddComponent(new InteractableComponent {
                OnInteract = player => OnPickup(player)
            });
        }

        //public void FallIntoPit(Entity pitEntity) {
        //    Velocity = Vector2.Zero;
        //    _acceleration = Vector2.Zero;
        //    _collectibleState = CollectibleState.Unavailable;

        //    UpdateHighlightRendering();

        //    _mover.SetEnabled(false);
        //    _collisionHitbox.SetEnabled(false);

        //    if(_pickupHitbox != null) {
        //        _pickupHitbox.SetEnabled(false);
        //        _pickupHitbox.CollidesWithLayers = 0;
        //        _pickupHitbox.PhysicsLayer = 0;
        //    }

        //    var easeType = EaseType.CubicOut;
        //    var durationSeconds = 1.25f;
        //    var targetScale = 0.2f;
        //    var targetRotationDegrees = 180;
        //    var targetColor = new Color(0 , 0 , 0 , 0.25f);
        //    var destination = pitEntity.LocalPosition;

        //    _hoverTween?.Stop(true);

        //    this.TweenPositionTo(destination , durationSeconds)
        //        .SetEaseType(easeType)
        //        .SetCompletionHandler(_ => this.SetEnabled(false))
        //        .Start();
        //    this.TweenRotationDegreesTo(targetRotationDegrees , durationSeconds)
        //        .SetEaseType(easeType)
        //        .Start();
        //    this.TweenScaleTo(targetScale , durationSeconds)
        //        .SetEaseType(easeType)
        //        .Start();
        //    //Rendrare.TweenColorTo(targetColor , durationSeconds)
        //    //    .SetEaseType(easeType)
        //    //    .Start();

        //    Metadata?.OnDestroyEvent?.Invoke(this);
        //}

        private void Hover(float yOffset) {
            if(Transform == null
                || !Enabled
                || _collectibleState != CollectibleState.Available)
                return;
            _hoverTween?.Stop(true);
            _hoverTween = this.TweenLocalPositionTo(new Vector2(Transform.Position.X , Transform.Position.Y + yOffset) , 1f)
           .SetEaseType(EaseType.SineInOut)
           .SetCompletionHandler(_ => Hover(-yOffset));
            _hoverTween.Start();
        }

        //public void Highlight() {
        //    _numberOfPlayersInProximity++;
        //    UpdateHighlightRendering();
        //}

        //public void Unhighlight() {
        //    _numberOfPlayersInProximity--;
        //    UpdateHighlightRendering();
        //}

        //private void UpdateHighlightRendering() {
        //    if(CollectibleState == CollectibleState.Available && _numberOfPlayersInProximity > 0 && !_isHighlighted) {
        //        _isHighlighted = true;

        //        var flashEffect = Assets.AssetLoader.GetEffect("shader_flash");
        //        var flashTexture = Assets.AssetLoader.GetTexture("effects/lava2");

        //        flashEffect.Parameters["flash_texture"].SetValue(flashTexture);
        //        flashEffect.Parameters["flashRate"].SetValue(0f);
        //        flashEffect.Parameters["flashOffset"].SetValue(1f);
        //        flashEffect.Parameters["scrollSpeed"].SetValue(new Vector2(0.45f , 0.45f));
        //        flashEffect.Parameters["replace_color"].SetValue(Metadata.Color.ToVector4());

        //        _flashEffect = flashEffect;
        //        Rendrare.material.effect = _flashEffect;
        //    }
        //    else if(CollectibleState != CollectibleState.Available || _numberOfPlayersInProximity == 0 && _isHighlighted) {
        //        _isHighlighted = false;

        //        Rendrare.material.effect = null;
        //    }
        //}

        public override void Uppdatera() {
            //if(_isHighlighted) {
            //    _flashEffect.Parameters["gameTime"].SetValue(Time.UnscaledDeltaTime);
            //}

            if(_collectibleState == CollectibleState.Unavailable)
                return;

            Move();
        }

        private void Move() {
            if(Hastighet.Length() == 0)
                return;
            Hastighet = 0.975f * Hastighet + 0.025f * _acceleration;
            bool isColliding = _mover.Move(Hastighet , out CollisionResult result);

            if(Hastighet.Length() < 0.001f) {
                Hastighet = Vector2.Zero;
            }

            if(Hastighet.Length() > 0) {}
            //UpdateRenderLayerDepth();
        }

        //private void UpdateRenderLayerDepth() {
        //    Rendrare.LayerDepth = 1 - Position.Y * RenderLayerDepthFactor;
        //}

        public bool CanBeCollectedByPlayer(Spelare p) {
            if(Metadata?.CanCollectRules.Any(r => r.Invoke(p) == false) == true) {
                return false;
            }
            return _collectibleState == CollectibleState.Available;
        }

        public void OnPickup(Spelare player) {
            if(_collectibleState != CollectibleState.Available)
                return;
            if(!CanBeCollectedByPlayer(player))
                return;

            player.EquipWeapon(Preset.Vapen , Metadata);

            _collectibleState = CollectibleState.Unavailable;
            _pickupHitbox.SetEnabled(false);
            _collisionHitbox.SetEnabled(false);

            Metadata?.OnPickupEvent?.Invoke(this , player);

            Destroy();
        }

        public void OnTriggerEnter(Collider other , Collider local) {
            if(other == null || other.Entity == null)
                return;
            if(_collectibleState == CollectibleState.Appearing)
                return;

            //if(other.Entity.Tag == Tags.Pit) {
            //    FallIntoPit(other.Entity);
            //}
        }

        public void OnTriggerExit(Collider other , Collider local) {
        }
    }
}
