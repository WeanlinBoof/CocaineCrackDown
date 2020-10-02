using System.Collections.Generic;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Entiteter.osorterat.Players.Sprites;
using CocaineCrackDown.Entiteter.Players.Sprites;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Tweens;


namespace CocaineCrackDown.Komponenter {

    public class PlayerRenderer : Component, IUpdatable {

        private Spelare spelare;

        private Vapen Vapen;

        public List<SpriteAtlas> SpelarAtlaser { get; set; }
        public SpriteAtlas HuvudAtlas { get; set; }
        public SpriteAtlas ArmAtlas { get; set; }
        public SpriteAtlas KropAtlas { get; set; }
        public SpriteAtlas BenAtlats { get; set; }
        public PlayerRenderer(List<SpriteAtlas> spelarAtlaser , Vapen vapen) {
            SpelarAtlaser = spelarAtlaser;
            Vapen = vapen;
        }

        public override void OnAddedToEntity() {
            spelare = Entity as Spelare;

            FixaSpelarAtlasAnimationer();
            UpdateRenderLayerDepth();
        }
        private void FixaSpelarAtlasAnimationer() {
            // Assign renderable (animation) component
            var headAnimations = SetupHeadAnimations(SpelarAtlaser.Head);
            Head = entity.addComponent(headAnimations);
            Head.renderLayer = Layers.Player;

            var torsoAnimations = SetupTorsoAnimations(_playerSprite.Torso);
            _torso = entity.addComponent(torsoAnimations);
            _torso.renderLayer = Layers.Player;

            var legsAnimations = SetupLegsAnimations(_playerSprite.Legs);
            _legs = entity.addComponent(legsAnimations);
            _legs.renderLayer = Layers.Player;

            Head.play(HeadAnimation.FrontFacing);
            _torso.play(TorsoAnimation.Front);
            _legs.play(LegsAnimation.Idle);

        }

        private Sprite<HeadAnimation> SetupHeadAnimations(SpelarHuvudAtlas headSprite) {
            var animations = new Sprite<HeadAnimation>();
            animations.addAnimation(HeadAnimation.FrontFacing ,
                headSprite.Front.ToSpriteAnimation(_playerSprite.Source + "/head"));
            animations.addAnimation(HeadAnimation.BackFacing ,
                headSprite.Back.ToSpriteAnimation(_playerSprite.Source + "/head"));

            return animations;
        }

        private Sprite<TorsoAnimation> SetupTorsoAnimations(PlayerTorsoSprite torsoSprite) {
            var animations = new Sprite<TorsoAnimation>();

            animations.addAnimation(TorsoAnimation.Front ,
                torsoSprite.Front.ToSpriteAnimation(_playerSprite.Source + "/torso"));
            animations.addAnimation(TorsoAnimation.Back ,
                torsoSprite.Back.ToSpriteAnimation(_playerSprite.Source + "/torso"));
            animations.addAnimation(TorsoAnimation.FrontUnarmed ,
                torsoSprite.FrontUnarmed.ToSpriteAnimation(_playerSprite.Source + "/torso"));
            animations.addAnimation(TorsoAnimation.BackUnarmed ,
                torsoSprite.BackUnarmed.ToSpriteAnimation(_playerSprite.Source + "/torso"));

            return animations;
        }

        private Sprite<LegsAnimation> SetupLegsAnimations(PlayerLegsSprite legsSprite) {
            var animations = new Sprite<LegsAnimation>();

            animations.addAnimation(LegsAnimation.Idle ,
                legsSprite.Idle.ToSpriteAnimation(_playerSprite.Source + "/legs"));
            animations.addAnimation(LegsAnimation.Walking ,
                legsSprite.Walking.ToSpriteAnimation(_playerSprite.Source + "/legs"));

            return animations;
        }

        public void TweenColor(Color c , float durationSeconds , EaseType easeType = EaseType.CubicOut) {
            Head.tweenColorTo(c , durationSeconds)
                .setEaseType(easeType)
                .start();
            _torso.tweenColorTo(c , durationSeconds)
                .setEaseType(easeType)
                .start();
            _legs.tweenColorTo(c , durationSeconds)
                .setEaseType(easeType)
                .setCompletionHandler(_ => UpdateTeamIndex(spelare.TeamIndex))
                .start();
        }


        public void update() {
            Head.localOffset = new Vector2(0 , spelare.Altitude);
            _torso.localOffset = new Vector2(0 , spelare.Altitude);
            _legs.localOffset = new Vector2(0 , spelare.Altitude);

            _headTeamColoredSprite.localOffset = new Vector2(0 , spelare.Altitude);
            _torsoTeamColoredSprite.localOffset = new Vector2(0 , spelare.Altitude);
            _legsTeamColoredSprite.localOffset = new Vector2(0 , spelare.Altitude);

            UpdateAnimation();
            if(float.IsNaN(spelare.position.X) || float.IsNaN(spelare.position.Y)
                || float.IsNaN(spelare.FacingAngle.X) || float.IsNaN(spelare.FacingAngle.Y))
                return;
            var hit = Physics.linecast(spelare.position , spelare.position + spelare.FacingAngle * 1000f);
            Debug.drawLine(spelare.position , spelare.position + spelare.FacingAngle * 1000f , Color.Gray);
        }

        public void UpdateRenderLayerDepth() {
            if(Head != null)
                Head.layerDepth = 1 - (entity.position.Y + spelare.FacingAngle.Y + _facingDepthOffset) * Constants.RenderLayerDepthFactor;
            if(_torso != null)
                _torso.layerDepth = 1 - (entity.position.Y + spelare.FacingAngle.Y - _facingDepthOffset) * Constants.RenderLayerDepthFactor;
            if(_legs != null)
                _legs.layerDepth = 1 - (entity.position.Y + spelare.FacingAngle.Y - _facingDepthOffset) * Constants.RenderLayerDepthFactor;
        }

        private void UpdateAnimation() {
            //Todo: Fix check of unmarmed. A gun type called unarmed?
            bool armed = spelare.IsArmed();

            // Select Animations (Idle initially)
            HeadAnimation headAnimation = HeadAnimation.FrontFacing;
            TorsoAnimation torsoAnimation = TorsoAnimation.Front;
            LegsAnimation legsAnimation = LegsAnimation.Idle;

            // Head
            if(spelare.PlayerState == PlayerState.Dead) {
                headAnimation = Head.currentAnimation;
                Head.pause();
            }
            else if(spelare.VerticalFacing == (int)FacingCode.UP) {
                _facingDepthOffset = -20 * Constants.RenderLayerDepthFactor;
                headAnimation = HeadAnimation.BackFacing;
            }
            else if(spelare.VerticalFacing == (int)FacingCode.DOWN) {
                _facingDepthOffset = 20 * Constants.RenderLayerDepthFactor;
                headAnimation = HeadAnimation.FrontFacing;
            }

            // Torso
            if(spelare.PlayerState == PlayerState.Dead) {
                torsoAnimation = _torso.currentAnimation;
                Head.pause();
            }
            else if(spelare.VerticalFacing == (int)FacingCode.UP) {
                _facingDepthOffset = -20 * Constants.RenderLayerDepthFactor;
                torsoAnimation = armed ? TorsoAnimation.Back : TorsoAnimation.BackUnarmed;
            }
            else if(spelare.VerticalFacing == (int)FacingCode.DOWN) {
                _facingDepthOffset = 20 * Constants.RenderLayerDepthFactor;
                torsoAnimation = armed ? TorsoAnimation.Front : TorsoAnimation.FrontUnarmed;
            }

            // Legs
            if(spelare.PlayerState == PlayerState.Dead || spelare.PlayerState == PlayerState.Dying) {
                legsAnimation = LegsAnimation.Idle;
                _legs.pause();
            }
            else if(spelare.Acceleration.Length() > 0) {
                legsAnimation = LegsAnimation.Walking;
            }


            // Play Animations

            if(!Head.isAnimationPlaying(headAnimation)) {
                Head.play(headAnimation);
            }

            if(!_torso.isAnimationPlaying(torsoAnimation)) {
                _torso.play(torsoAnimation);
            }

            if(!_legs.isAnimationPlaying(legsAnimation)) {
                _legs.play(legsAnimation);
            }
        }

        public override void onEnabled() {
            _light.setEnabled(true);

            Head.setEnabled(true);
            _headShadow.setEnabled(true);
            _headSilhouette.setEnabled(true);
            _headTeamColoredSprite.setEnabled(true);

            _torso.setEnabled(true);
            _torsoShadow.setEnabled(true);
            _torsoSilhouette.setEnabled(true);
            _torsoTeamColoredSprite.setEnabled(true);

            _legs.setEnabled(true);
            _legsShadow.setEnabled(true);
            _legsSilhouette.setEnabled(true);
            _legsTeamColoredSprite.setEnabled(true);
        }

        public override void onDisabled() {
            _light.setEnabled(false);

            Head.setEnabled(false);
            _headShadow.setEnabled(false);
            _headSilhouette.setEnabled(false);
            _headSilhouette.setEnabled(false);

            _torso.setEnabled(false);
            _torsoShadow.setEnabled(false);
            _torsoSilhouette.setEnabled(false);
            _torsoTeamColoredSprite.setEnabled(false);

            _legs.setEnabled(false);
            _legsShadow.setEnabled(false);
            _legsSilhouette.setEnabled(false);
            _legsTeamColoredSprite.setEnabled(false);
        }

        public override void onRemovedFromEntity() {
            Head.removeComponent();
            _headShadow.removeComponent();
            _headSilhouette.removeComponent();
            _headTeamColoredSprite.setEnabled(false);
            _headTeamColoredSprite.removeComponent();

            _torso.removeComponent();
            _torsoShadow.removeComponent();
            _torsoSilhouette.removeComponent();
            _torsoTeamColoredSprite.setEnabled(false);
            _torsoTeamColoredSprite.removeComponent();

            _legs.removeComponent();
            _legsShadow.removeComponent();
            _legsSilhouette.removeComponent();
            _legsTeamColoredSprite.setEnabled(false);
            _legsTeamColoredSprite.removeComponent();

            _light.removeComponent();
        }

        public void FlipX(bool isFlipped) {
            if(Head != null)
                Head.flipX = isFlipped;
            if(_torso != null)
                _torso.flipX = isFlipped;
            if(_legs != null)
                _legs.flipX = isFlipped;
        }

        public void UpdateTeamIndex(int teamIndex) {
            var teamColor = ResolveTeamColor(teamIndex);
            if(teamIndex > 0) {
                _teamColorMaterial.effect.Parameters["draw"].SetValue(1);
                teamColor = new Color(teamColor , 0.75f);
                _teamColorMaterial.effect.Parameters["single_color"].SetValue(teamColor.ToVector4());
            }
            else {
                _teamColorMaterial.effect.Parameters["draw"].SetValue(0);
            }

        }

        public Color ResolveTeamColor(int teamIndex) {
            switch(teamIndex) {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Red;
                default:
                    return Color.Transparent;
            }
        }
    }
}
