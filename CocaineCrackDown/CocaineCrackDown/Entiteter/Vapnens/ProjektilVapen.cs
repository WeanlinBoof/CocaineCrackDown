﻿

using Microsoft.Xna.Framework;

using System;

namespace CocaineCrackDown.Entiteter {
   /* public class ProjektilVapen : Vapen {
        private FredflixAndChell.Shared.Components.Weapons.ProjektilVapen _renderer;
        private Player _player;

        private GunMetadata _metadata { get; set; }
        private GunParameters _parameters { get; set; }

        public override CollectibleMetadata Metadata => _metadata;
        public override VapenParameters Parameters => _parameters;

        private int _ammo;
        private int _magazineAmmo;
        private int _maxAmmo;
        private int _magazineSize;
        private int _bulletCount;
        private float _accuracy;
        private float _bulletSpread;

        private Vector2 _barrelOffset;

        public Cooldown Reload { get; set; }
        public int Ammo {
            get => _ammo;
            set {
                _ammo = value;
                _metadata.SetAmmo(value);
            }
        }
        public int MagazineAmmo {
            get => _magazineAmmo;
            set {
                _magazineAmmo = value;
                _metadata.SetMagazineAmmo(value);
            }
        }

        public ProjektilVapen(Spelare player , GunParameters gunParameters , GunMetadata metadata = null) : base(player) {
            _player = player;
            _parameters = gunParameters;
            _metadata = metadata ?? new GunMetadata(gunParameters.Ammo , gunParameters.MagazineAmmo);
            SetupParameters();
        }

        public override void OnSpawn() {
            base.OnSpawn();
            _renderer = addComponent(new FredflixAndChell.Shared.Components.Weapons.ProjektilVapen(this , _player));
        }

        private void SetupParameters() {
            _accuracy = _parameters.Accuracy;
            Ammo = _metadata.GetAmmo().Value;
            MagazineAmmo = _metadata.GetMagazineAmmo().Value;
            _maxAmmo = _parameters.MaxAmmo;
            _magazineSize = _parameters.MagazineSize;
            _barrelOffset = _parameters.BarrelOffset;
            Cooldown = new Cooldown(_parameters.AttackHastighet);
            Reload = new Cooldown(_parameters.ReloadTime);

            _bulletCount = _parameters.BulletCount;
            _bulletSpread = _parameters.BulletSpread;
        }

        public override void Attack() {
            CheckAmmo();
            if(Cooldown.IsReady() && Reload.IsReady() && MagazineAmmo > 0) {
                //Functionality
                Cooldown.Start();
                var dir = (float)Math.Atan2(_player.FacingAngle.Y , _player.FacingAngle.X);
                var x = (float)(position.X
                    + Math.Cos(localRotation) * _barrelOffset.X
                    + Math.Cos(localRotation) * _barrelOffset.Y);
                var y = (float)(position.Y
                    + Math.Sin(localRotation) * _barrelOffset.Y
                    + Math.Sin(localRotation) * _barrelOffset.X);
                for(float i = 0; i < _bulletCount; i++) {
                    var direction = dir + (1 - _accuracy) * Nez.Random.minusOneToOne() / 2
                    + (1 - _accuracy) * _player.Velocity.Length() * Nez.Random.minusOneToOne()
                    + (i * 2 - _bulletCount) * _bulletSpread / _bulletCount;

                    var bullet = Bullet.Create(_player , x , y , direction , BulletDict.Get(_parameters.Bullet));

                    if(_parameters.InheritsPlayerSpeed) {
                        bullet.Velocity += _player.Velocity * 50f;
                    }
                }
                MagazineAmmo--;

                //Animation
                _renderer?.Attack();
            }
        }

        private void CheckAmmo() {
            if(MagazineAmmo == 0) {
                if(Ammo <= 0) {
                    //Totally out of ammo? 
                    _renderer.Empty();
                }
                else {
                    //Reload
                    ReloadMagazine();
                }
            }
        }

        public void ReloadMagazine() {
            if(!Reload.IsOnCooldown() && MagazineAmmo != _magazineSize) {
                //Function
                Reload.Start();
                int newBullets = Math.Min(_magazineSize - MagazineAmmo , Ammo);

                Ammo = Ammo - newBullets;
                MagazineAmmo = MagazineAmmo + newBullets;

                //Animation
                _renderer.Reload();
            }
        }

        public override void OnDespawn() {
        }

        public override void Update() {
            base.Update();
            Reload.Update();
        }

        public override void VäxlaSpringLäge(bool isRunning) {
            _renderer?.VäxlaSpringade(isRunning);
        }
    }*/
}
