
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.Verktyg;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tweens;

using System;
using System.Collections.Generic;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.Entiteter {
    public enum NärstridsVapenAttackLäge {
        None,
        Forward,
        Backward
    }

    public class NärstridsVapen : Vapen {
        private const float AttackMåletsRadian = (float)Math.PI;
        private const float Epsilon = 0.01f;
        private NärstridsVapenRendrare rendrare;
        private NärstridsVapenParameters parameters;
        private MeleeMetadata metadata;
        private NärstridsVapenAttackLäge AttackStatus;

        private List<Spelare> SpelareTräffadMedAttack;

        public override VapenParameters Parameters => parameters;
        public override CollectibleMetadata Metadata => metadata;

        protected Collider TräffBox;
        private Komponenter.Riktning AttackRiktning;

        public float AttackRotation { get; set; }

        public Spelare Spelare { get; set; }

        public NärstridsVapen(Spelare spelare , NärstridsVapenParameters meleeParameters , MeleeMetadata metadata = null) : base(spelare) {
            Spelare = spelare;
            parameters = meleeParameters;
            SpelareTräffadMedAttack = new List<Spelare>();
            this.metadata = metadata ?? new MeleeMetadata();
            SetupParameters();
        }

        public override void VidSpawn() {
            base.VidSpawn();
            rendrare = AddComponent(new NärstridsVapenRendrare(this , Spelare));
            TräffBox = AddComponent(new BoxCollider(parameters.HitboxSize.X , parameters.HitboxSize.Y));
            ;
            TräffBox.SetLocalOffset(new Vector2(parameters.HitboxOffset.X , parameters.HitboxOffset.Y));

            AttackStatus = NärstridsVapenAttackLäge.None;

            Flags.SetFlagExclusive(ref TräffBox.CollidesWithLayers , Lager.Player);
        }

        private void SetupParameters() {
            Cooldown = new Cooldown(parameters.AttackHastighet);
            Cooldown.Start();
        }

        public override void Attack() {
            switch(parameters.MeleeType) {
                case NärstridsVapenTyp.Hold:
                    if(Cooldown.IsReady()) {
                        Cooldown.Start();
                        rendrare?.Attack();
                    }
                    break;
                case NärstridsVapenTyp.Swing:
                    if(Cooldown.IsReady() && AttackStatus == NärstridsVapenAttackLäge.None) {
                        Cooldown.Start();
                        rendrare?.Attack();
                        AttackRiktning = Spelare.Riktning;
                        AttackStatus = NärstridsVapenAttackLäge.Forward;
                    }
                    break;
            }

        }

        public override void VidDespawn() {
        }

        public override void Uppdatera() {
            base.Uppdatera();
            Cooldown.Update();
            CheckCollision();
            switch(AttackStatus) {
                case NärstridsVapenAttackLäge.None:
                    break;
                case NärstridsVapenAttackLäge.Forward:
                    AttackRotation = Lerps.LerpTowards(AttackRotation , (int)AttackRiktning * AttackMåletsRadian , 0.01f , Time.DeltaTime * 10f);
                    if(Math.Abs((int)AttackRiktning * AttackRotation) >= AttackMåletsRadian - Epsilon) {
                        AttackStatus = NärstridsVapenAttackLäge.Backward;
                    }
                    break;
                case NärstridsVapenAttackLäge.Backward:
                    AttackRotation = Lerps.LerpTowards(AttackRotation , 0 , 0.01f , Time.DeltaTime * 5f);
                    if(Math.Abs((int)AttackRiktning * AttackRotation) <= Epsilon) {
                        AttackStatus = NärstridsVapenAttackLäge.None;
                        SpelareTräffadMedAttack.Clear();
                    }
                    break;
            }
        }

        private void CheckCollision() {
            if(TräffBox.CollidesWithAny(out CollisionResult collision)) {
                var collidedWithEntity = collision.Collider.Entity;
                if(collidedWithEntity.Tag == Tags.Spelare) {
                    HitPlayer(collision.Collider.Entity);
                }

            }
        }

        private void HitPlayer(Entity playerEntity) {
            Spelare spelare = playerEntity as Spelare;
            if(spelare.spelarRörlighetsTillstånd == SpelarRörlighetsTillstånd.Rolling) {
                return;
            }

            if(parameters.MeleeType == NärstridsVapenTyp.Swing) {
                if(SpelareTräffadMedAttack.Contains(spelare)) {
                    return;
                }

                SpelareTräffadMedAttack.Add(spelare);
            }
            OnImpact(spelare);
        }

        public virtual void OnImpact(Spelare spelare) {
            if(spelare == null) {
                return;
            }

            switch(parameters.MeleeType) {
                case NärstridsVapenTyp.Hold:
                    if(spelare != Spelare && Cooldown.IsOnCooldown()) {
                        DamagePlayer(spelare);
                    }
                    break;
                case NärstridsVapenTyp.Swing:
                    if(spelare != Spelare & AttackStatus == NärstridsVapenAttackLäge.Forward) {
                        DamagePlayer(spelare);
                    }
                    break;
            }

        }

        protected bool DamagePlayer(Spelare spelare) {
            spelare.Damage(this);
            return true;
        }

        public override void VäxlaSpringLäge(bool Springer) {
            rendrare?.VäxlaSpringade(Springer);
        }
    }
}
