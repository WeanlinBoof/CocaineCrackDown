using CocaineCrackDown.Entiteter.Gestalter;

using CocaineCrackDown.Entiteter.osorterat.Players;

using CocaineCrackDown.Komponenter;
using CocaineCrackDown.System;
using CocaineCrackDown.Verktyg;
using CocaineCrackDown.Verktyg.Events;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;
using Nez.Tweens;

using System;
using System.Collections.Generic;

using static CocaineCrackDown.Verktyg.StandigaVarden;

using Riktning = CocaineCrackDown.Komponenter.Riktning;

namespace CocaineCrackDown.Entiteter {
    public class Player : SpelObjekt {

        private const float AltitudeOutOfReachTreshold = 15f;

        private const float WalkAcceleration = 0.25f;

        private const float SprintAcceleration = 0.40f;

        private const float RollAcceleration = 1.20f;

        private const float FlyAcceleration = 0.015f;

        private const float BaseDeacceleration = 0.2f;

        private const float FlyDeacceleration = 0.005f;

        private const float BaseSlownessFactor = 20f;

        private const int DodgeRollStaminaCost = 50;

        private const float LastHitTimeoutSeconds = 3.0f;

        private static bool DebugToggledRecently { get; set; }

        private Mover _mover;





        public Player(GestaltParameter gestaltParameter , int x , int y , int playerIndex) : base(x , y) {
            Parameter = gestaltParameter;
            SpelarIndex = playerIndex;
            Name = $"{Parameter.KaraktärNamn}";

            EntiteterINärhet = new List<Entity>();
        }

        public override void VidSpawn() {
            SpelSystem = Scene.GetSceneComponent<SpelSystem>();

            PlayerMetadata metadata = KontextHanterare.PlayerMetadataByIndex(SpelarIndex);
            if(metadata != null) {
                if(metadata.Initialiserad) {
                    JoinGame();
                }
            }
            else {
                Kontroller = GetComponent<PlayerController>();
            }
        }

        public void OnPitHitboxEnter(Entity entity) {
            switch(SpelareTillstånd) {
                case SpelarTillstånd.Normal:
                    break;
                case SpelarTillstånd.Idle:
                case SpelarTillstånd.Dead:
                case SpelarTillstånd.Dying:
                default:
                    return;
            }

            FallGroparUnderSpelare.AddIfNotPresent(entity);
        }
        public void OnPitHitboxExit(Entity entity) {
            FallGroparUnderSpelare.Remove(entity);
        }

        //private void SetupDebug()
        //{
        //    var gameSystem = scene.getSceneComponent<GameSystem>();
        //    gameSystem.DebugLines.Add(new DebugLine
        //    {
        //        Text = () => $"Player {PlayerIndex}",
        //        SubLines = new List<DebugLine>
        //        {
        //            new DebugLine{ Text = () => $"Health: {Health}"},
        //            new DebugLine{ Text = () => $"Stamina: {Stamina}"},
        //            new DebugLine{ Text = () => $"Weapon: {_gun?.Parameters.Name ?? "Unarmed"}"},
        //            new DebugLine{ Text = () => $"Mobility state: {PlayerMobilityState.ToString()}"},
        //        }
        //    });
        //}

        private void SetupComponents() {
            SetTag(Tags.Spelare);

            // Assign movement component
            _mover = AddComponent(new Mover());

            // Assigned by player connector
            Kontroller = GetComponent<PlayerController>();

            // Assign inventory component
            Inventory = AddComponent(new SpelarInventory());

            // Assign collider component
            StandarBox = AddComponent(new CircleCollider(4f));
            StandarBox.LocalOffset = new Vector2(0 , 4);
            Flags.SetFlagExclusive(ref StandarBox.CollidesWithLayers , Lager.MapObstacles);
            Flags.SetFlag(ref StandarBox.CollidesWithLayers , Lager.Player);
            Flags.SetFlagExclusive(ref StandarBox.PhysicsLayer , Lager.Player);

            // Assign proximity interaction hitbox
            NärhetBox = AddComponent(new CircleCollider(20f));
            Flags.SetFlagExclusive(ref NärhetBox.CollidesWithLayers , Lager.Interactables);
            Flags.SetFlag(ref NärhetBox.CollidesWithLayers , Lager.Explosion);
            Flags.SetFlagExclusive(ref NärhetBox.PhysicsLayer , 0);
            NärhetBox.IsTrigger = true;

            SpelarKollitionsHanterare = AddComponent(new SpelarKollitionsHanterare(StandarBox , NärhetBox));

            // Assign renderer component
            SetupRenderer(Parameter.SpelarAtlas);

            // Assign camera tracker component
            KamraSpåre = AddComponent(new CameraTracker(
                () => SpelareTillstånd != SpelarTillstånd.Dead && SpelareTillstånd != SpelarTillstånd.Idle ,
                () => Position + (_renderer.Head?.localOffset ?? Vector2.Zero)));

            // Blood
            Blod = AddComponent(new BloodEngine(Parameter.BlodFärg));

            SetWalkingState();
        }

        private void JoinGame() {
            SpelSystem.RegisterPlayer(this);
            SetupComponents();
            PlayerMetadata metadata = KontextHanterare.PlayerMetadataByIndex(SpelarIndex);
            SetupParameters(metadata);
            UpdateOrder = 0;

            SpelareTillstånd = SpelarTillstånd.Normal;
            metadata.Initialiserad = true;
        }
        private void SetupParameters(PlayerMetadata metadata) {
            _accelerationPlayerFactor = WalkAcceleration;
            _deaccelerationPlayerFactor = BaseDeacceleration;

            TeamIndex = metadata.LagIndex;
            Health = Parameter.MaxLivsPoäng;
            MaxHealth = Parameter.MaxLivsPoäng;
            Stamina = Parameter.MaxUthållighet;
            MaxStamina = Parameter.MaxUthållighet;
            Speed = Parameter.Hastighet;
        }

        public bool IsArmed() {
            return Inventory.Beväpnad;
        }

        public override void Uppdatera() {
            ReadInputs();

            if(SpelareTillstånd == SpelarTillstånd.Idle)
                return;

            Move();
            SetFacing();

            if(AltitudeVelocity < 0 && SpelareRörlighetsTillstånd != SpelarRörlighetsTillstånd.Flying) {
                SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Flying;
            }

            // State specific updates
            switch(SpelareRörlighetsTillstånd) {
                case SpelarRörlighetsTillstånd.Rolling:
                    break;
                case SpelarRörlighetsTillstånd.Flying:
                    Altitude += AltitudeVelocity * Time.DeltaTime;
                    Console.WriteLine($"FLYING WOOO (Altitude: {Altitude}, AltVel: {AltitudeVelocity})");
                    AltitudeVelocity = Lerps.LerpTowards(AltitudeVelocity , 200f , 0.5f , Time.DeltaTime * 5f);
                    _accelerationPlayerFactor = FlyAcceleration;
                    _deaccelerationPlayerFactor = FlyDeacceleration;
                    if(Altitude > -0.01f) {
                        Altitude = 0;
                        _accelerationPlayerFactor = WalkAcceleration;
                        _deaccelerationPlayerFactor = BaseDeacceleration;
                        SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Walking;
                    }
                    break;
                default:
                case SpelarRörlighetsTillstånd.Running:
                case SpelarRörlighetsTillstånd.Walking:
                    if(FallGroparUnderSpelare.Count > 0) {
                        FallIntoPit(FallGroparUnderSpelare.RandomItem());
                    }
                    break;
            }

            if(_lastHitTime > 0 && Time.UnscaledDeltaTime > _lastHitTime + LastHitTimeoutSeconds) {
                _lastHitTime = -1;
                _lastHitPlayerSource = null;
            }

            if(Time.FrameCount % 5 == 0) {
                DebugToggledRecently = false;
            }
        }

        private void ReadInputs() {
            if(Kontroller == null || !Kontroller.InputEnabled)
                return;

            if(Kontroller.FirePressed)
                Attack();
            if(!Kontroller.FirePressed)
                AimingSlownessFactor = BaseSlownessFactor;
            if(Kontroller.ReloadPressed)
                Reload();
            if(Kontroller.DropWeaponPressed)
                Inventory?.DropWeapon();
            if(Kontroller.SwitchWeaponPressed)
                Inventory?.SwitchWeapon();
            if(Kontroller.InteractPressed)
                Interact();
            if(Kontroller.SprintPressed)
                ToggleDodgeRoll();
            if(Kontroller.DebugModePressed && !DebugToggledRecently) {
                Core.DebugRenderEnabled = !Core.DebugRenderEnabled;
                DebugToggledRecently = true;
            }

            if(Kontroller.ExitGameButtonPressed) {
                Core.Scene = new HubScene();
            }

            HandleDodgeRollGracePeriod();
            PerformDodgeRoll();
            ToggleSprint();
            ToggleStaminaRegeneration();

            Acceleration = new Vector2(Kontroller.XLeftAxis , Kontroller.YLeftAxis);
        }

        private void HandleDodgeRollGracePeriod() {
            if(_isWithinDodgeRollGracePeriod) {
                _gracePeriod += 1 * Time.DeltaTime;
            }
            if(_gracePeriod > 1) {
                _gracePeriod = 0;
                _numSprintPressed = 0;
                _isWithinDodgeRollGracePeriod = false;
            }
        }

        private void ToggleDodgeRoll() {
            if(SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Rolling || SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Flying)
                return;
            if(_numSprintPressed == 0) {
                _numSprintPressed++;
                _isWithinDodgeRollGracePeriod = true;
            }

            else if(_numSprintPressed == 1 && _gracePeriod < 1) {
                _numSprintPressed++;
            }
            else {
                _isWithinDodgeRollGracePeriod = false;
                _numSprintPressed = 0;
                _gracePeriod = 0;
            }
        }

        private void PerformDodgeRoll() {
            if(_numSprintPressed == 2 && Kontroller.SprintPressed && Stamina > DodgeRollStaminaCost && (Acceleration.X != 0 || Acceleration.Y != 0)) {
                SetRollingState();
                _isRollingRight = FacingAngle.X > 0 ? true : false;
                _numSprintPressed = 0;
                Stamina -= DodgeRollStaminaCost * SpelSystem.Inställningar.UthålighetGångrare;

                _initialRollingDirection = 1f * Hastighet + CalculateAcceleration();
            }

            if(SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Rolling) {
                _mover.Move(_initialRollingDirection , out CollisionResult collision);

                if(_isRollingRight) {
                    LocalRotation += 4 * (float)Math.PI * Time.DeltaTime;
                }
                else {
                    LocalRotation -= 4 * (float)Math.PI * Time.DeltaTime;
                }
            }

            if(_isRollingRight && LocalRotation >= 2 * Math.PI || !_isRollingRight && LocalRotation <= -2 * Math.PI) {
                SetWalkingState();
                LocalRotation = 0;
                _numSprintPressed = 0;
            }
        }

        private Vector2 CalculateAcceleration() {
            return _accelerationPlayerFactor * AccelerationExternalFactor * Acceleration - _deaccelerationPlayerFactor * DeaccelerationExternalFactor * Hastighet;
        }

        private void ToggleStaminaRegeneration() {
            if(!_isRegeneratingStamina)
                return;

            Stamina += 25 * Time.DeltaTime;
            if(Stamina >= MaxStamina) {
                Stamina = MaxStamina;
                _isRegeneratingStamina = false;
            }
        }

        private void ToggleSprint() {
            if(SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Rolling || SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Flying)
                return;

            if(Kontroller.SprintDown && (Acceleration.X != 0 || Acceleration.Y != 0)) {
                if(Stamina <= 0) {
                    SetWalkingState();
                }
                else {
                    SetRunningState();
                    Stamina -= 50 * SpelSystem.Inställningar.UthålighetGångrare * Time.DeltaTime;
                    _isRegeneratingStamina = false;
                }
            }
            else {
                if(Stamina >= Parameter.MaxUthållighet)
                    return;
                _isRegeneratingStamina = true;
                SetWalkingState();
            }
        }

        private void SetRollingState() {
            _accelerationPlayerFactor = RollAcceleration;
            SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Rolling;
        }

        private void SetWalkingState() {
            _accelerationPlayerFactor = WalkAcceleration;
            Inventory?.Vapen?.VäxlaSpringLäge(false);
            SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Walking;
        }

        private void SetRunningState() {
            _accelerationPlayerFactor = SprintAcceleration;
            Inventory?.Vapen?.VäxlaSpringLäge(true);
            SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Running;
        }

        public void EquipWeapon(VapenParameters weapon , CollectibleMetadata metadata = null) {
            Inventory.EquipWeapon(weapon , metadata);
        }

        private void Move() {
            if(SpelareRörlighetsTillstånd == SpelarRörlighetsTillstånd.Rolling) {
                return;
            }

            Acceleration *= Speed * Time.DeltaTime;
            Hastighet += CalculateAcceleration();

            if(Hastighet.Length() < 0.001f) {
                Hastighet = Vector2.Zero;
            }

            if(Hastighet.Length() > 0) {
                _renderer.UpdateRenderLayerDepth();
            }

            bool isColliding = _mover.Move(Hastighet , out CollisionResult collision);

            if(isColliding) {
                if(collision.Collider.Entity is Player player) {
                    player.Acceleration = collision.MinimumTranslationVector * 4;
                }
            }
        }

        public void FallIntoPit(Entity pitEntity) {
            Health = 0;
            DisablePlayer(resetVelocity: true);
            DisableHitbox();

            Inventory.DropWeapon();
            FallGroparUnderSpelare.Clear();

            var easeType = EaseType.CubicOut;
            var durationSeconds = 2f;
            var targetScale = 0.2f;
            var targetRotationDegrees = 180;
            var targetColor = new Color(0 , 0 , 0 , 0.25f);
            var destination = pitEntity.localPosition;

            this.tweenRotationDegreesTo(targetRotationDegrees , durationSeconds)
                .setEaseType(easeType)
                .start();
            this.tweenScaleTo(targetScale , durationSeconds)
                .setEaseType(easeType)
                .start();
            this.tweenPositionTo(destination , durationSeconds)
                .setEaseType(easeType)
                .setCompletionHandler(_ => DeclareDead())
                .start();
            _renderer.TweenColor(targetColor , durationSeconds , easeType);

            if(SpelareTillstånd != SpelarTillstånd.Dying && SpelareTillstånd != SpelarTillstånd.Dead) {
                SpelSystem.Publish(GameEvents.PlayerKilled , new PlayerKilledEventParameters {
                    Killed = this ,
                    Killer = _lastHitPlayerSource
                });

                SpelareTillstånd = SpelarTillstånd.Dying;
            }
        }

        private void EnableProximityHitbox() {
            Flags.SetFlagExclusive(ref NärhetBox.CollidesWithLayers , Lager.Interactables);
            Flags.SetFlag(ref NärhetBox.CollidesWithLayers , Lager.Explosion);
            NärhetBox.RegisterColliderWithPhysicsSystem();
            NärhetBox.SetEnabled(true);
        }

        private void DisableProximityHitbox() {
            NärhetBox.CollidesWithLayers = 0;
            NärhetBox.UnregisterColliderWithPhysicsSystem();
            NärhetBox.SetEnabled(false);
        }

        private void EnableHitbox() {
            _mover.SetEnabled(true);
            Flags.SetFlagExclusive(ref StandarBox.CollidesWithLayers , Lager.MapObstacles);
            Flags.SetFlag(ref StandarBox.CollidesWithLayers , Lager.Player);
            Flags.SetFlagExclusive(ref StandarBox.PhysicsLayer , Lager.Player);
            StandarBox.RegisterColliderWithPhysicsSystem();
            StandarBox.SetEnabled(true);
        }

        private void DisableHitbox() {
            _mover.SetEnabled(false);
            StandarBox.UnregisterColliderWithPhysicsSystem();
            StandarBox.CollidesWithLayers = 0;
            StandarBox.SetEnabled(false);
        }

        private void DeclareDead() {
            SpelareTillstånd = SpelarTillstånd.Dead;
            KamraSpåre.SetEnabled(false);
        }

        public void DisablePlayer(bool resetVelocity) {
            Kontroller.SetInputEnabled(false);
            if(resetVelocity) {
                Hastighet = Vector2.Zero;
            }
            Acceleration = Vector2.Zero;
        }

        public void Attack() {
            if(Disarmed || SpelareTillstånd == SpelarTillstånd.Idle)
                return;

            Inventory.Attack();
        }

        public void Reload() {
            if(Disarmed || SpelareTillstånd == SpelarTillstånd.Idle)
                return;

            //_inventory.Reload();
        }

        public void Interact() {
            if(SpelareTillstånd == SpelarTillstånd.Idle) {
                JoinGame();
            }
            else {
                InteractWithNearestEntity();
            }
        }


        private void InteractWithNearestEntity() {
            SpelarKollitionsHanterare.InteractWithNearestEntity();
        }


        public void Damage(NärstridsVapen melee) {
            DirektionelSkada directionalDamage = new DirektionelSkada {
                Damage = (melee.Parameters as NärstridsVapenParameters).Damage ,
                Knockback = (melee.Parameters as NärstridsVapenParameters).Knockback ,
                Direction = melee.Spelare.FacingAngle ,
                SourceOfDamage = melee.Spelare ,
                AerialKnockback = (melee.Parameters as NärstridsVapenParameters).AerialKnockback
            };
            Damage(directionalDamage);
        }

        public void Damage(DirektionelSkada dd) {
            if(!CanBeDamagedBy(dd))
                return;

            var scaledDamage = dd.Damage * SpelSystem.Inställningar.SkadaGångrare;
            var scaledKnockback = dd.Knockback * SpelSystem.Inställningar.SwepadGångrare;

            dd.Direction.Normalize();
            Health -= scaledDamage;
            Hastighet += dd.Direction * scaledKnockback * Time.DeltaTime;

            if(dd.AerialKnockback > 0) {
                var scaledAerialKnockback = dd.AerialKnockback * SpelSystem.Inställningar.SwepadLuftburenGångrare;
                AltitudeVelocity += -scaledAerialKnockback;
            }

            Blod.Sprinkle(scaledDamage , dd.Direction);

            // Override last-hit-by player if directionaldamage came from a player (used for pushing into pit)
            _lastHitPlayerSource = dd.SourceOfDamage ?? _lastHitPlayerSource;
            _lastHitTime = Time.UnscaledDeltaTime;

            if(Health <= 0 && SpelareTillstånd != SpelarTillstånd.Dying && SpelareTillstånd != SpelarTillstånd.Dead) {
                SpelareTillstånd = SpelarTillstånd.Dying;
                DropDead();
                Inventory.DropWeapon();
                DisablePlayer(resetVelocity: false);

                SpelSystem.Publish(SpelEvents.PlayerKilled , new SpelareDödadEventParameter {
                    Killed = this ,
                    Killer = _lastHitPlayerSource
                });
            }
        }
        public bool CanBeDamagedBy(DirektionelSkada damage) {
            var isFriendlyFire = damage.SourceOfDamage.TeamIndex > 0
                && damage.SourceOfDamage != this
                && TeamIndex > 0
                && damage.SourceOfDamage.TeamIndex == TeamIndex;
            var isFriendlyFireEnabled = SpelSystem.Settings.FriendlyFire;
            var isSelfShot = damage.SourceOfDamage == this;
            var isTooHigh = Altitude <= -AltitudeOutOfReachTreshold;

            if(isTooHigh)
                return false;
            if(isFriendlyFire)
                return isFriendlyFireEnabled;
            if(isSelfShot)
                return damage.CanHitSelf;

            return true;
        }
        public void SetParameters(GestaltParameter characterParams) {
            Parameter = characterParams;
            PlayerMetadata meta = KontextHanterare.PlayerMetadataByIndex(SpelarIndex);
            SetupParameters(meta);
            ChangePlayerSprite(characterParams.SpelarAtlas);
            if(meta != null) {
                meta.Gestalt = characterParams;
            }
        }

        public void SetTeamIndex(int teamIndex) {
            TeamIndex = teamIndex;
            _renderer.UpdateTeamIndex(teamIndex);

            PlayerMetadata meta = KontextHanterare.PlayerMetadataByIndex(SpelarIndex);
            meta.LagIndex = teamIndex;
        }

        private void DropDead() {
            var easeType = EaseType.BounceOut;
            var durationSeconds = 1.5f;
            var targetRotationDegrees = 90;
            var targetColor = new Color(0.25f , 0.25f , 0.25f , 1.0f);

            this.TweenRotationDegreesTo(targetRotationDegrees , durationSeconds)
                .SetEaseType(easeType)
                .SetCompletionHandler(_ => DeclareDead())
                .Start();
            _renderer.TweenColor(targetColor , durationSeconds , easeType);
            Blod.Blast();
            Blod.Leak();
        }

        public override void VidDespawn() {
            Inventory?.Vapen?.Destroy();
        }

        private void SetFacing() {
            if(SpelareTillstånd != SpelarTillstånd.Normal)
                return;

            if(Kontroller == null || Kontroller.XRightAxis == 0 && Kontroller.YRightAxis == 0)
                return;

            FacingAngle = Lerps.AngleLerp(FacingAngle , new Vector2(Kontroller.XRightAxis , Kontroller.YRightAxis) , Time.DeltaTime * AimingSlownessFactor);

            if(FacingAngle.Y > 0) {
                VerticalFacing = (int)Riktning.ner;
            }
            else {
                VerticalFacing = (int)Riktning.upp;
            }

            if(FacingAngle.X > 0) {
                _renderer.FlipX(false);
                FlipGun = false;
                HorizontalFacing = (int)Riktning.höger;
            }
            else {
                _renderer.FlipX(true);
                FlipGun = true;
                HorizontalFacing = (int)Riktning.vänster;
            }

            _renderer.UpdateRenderLayerDepth();
        }

        public void Respawn(Vector2 furthestSpawn) {
            Position = furthestSpawn;
            SpelareTillstånd = SpelarTillstånd.Normal;
            SpelareRörlighetsTillstånd = SpelarRörlighetsTillstånd.Walking;

            LocalRotation = 0;
            Scale = new Vector2(1.0f , 1.0f);
            _renderer.TweenColor(Color.White , 0.1f);
            Kontroller.SetInputEnabled(true);
            KamraSpåre.SetEnabled(true);
            Blod.StopLeaking();

            _lastHitPlayerSource = null;
            _lastHitTime = -1;

            var meta = KontextHanterare.PlayerMetadataByIndex(SpelarIndex);

            SetupParameters(meta);
            EnableHitbox();
            EnableProximityHitbox();

            var weapon = meta.Vapnet ?? CollectibleDict.Get(Strings.DefaultStartWeapon).Vapen;
            EquipWeapon(weapon);

            // Hack to prevent newly respawned players from being invincible
            Move();
        }
    }
}
