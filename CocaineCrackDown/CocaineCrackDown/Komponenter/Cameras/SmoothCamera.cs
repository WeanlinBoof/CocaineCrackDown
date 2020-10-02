using System;
using System.Collections.Generic;

using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tweens;

using static CocaineCrackDown.Verktyg.StandigaVarden;

namespace CocaineCrackDown.Komponenter {

    public class SmoothCamera : SceneComponent {

        private bool FönsterLäge = false;

        private Camera Kamra;

        private Camera ReflektionsKamra;

        private TiledMapRenderer karta;

        private List<CameraTracker> Spårare;

        private Vector2 ReflektionOffset = new Vector2(8 , 4);

        public float BaseZoom { get; set; } = 10f;

        public float Zoom {
            get => Kamra.RawZoom;
            set {
                Kamra.RawZoom = value;
                ReflektionsKamra.RawZoom = value;
            }
        }

        public Vector2 Position {
            get => Kamra.Position;
            set {
                Kamra.Position = value;
                ReflektionsKamra.Position = value + ReflektionOffset;
            }
        }

        public SmoothCamera(Camera reflektionsKamra) {
            ReflektionsKamra = reflektionsKamra;
            Spårare = new List<CameraTracker>();
        }

        public override void OnEnabled() {
            base.OnEnabled();
            Kamra = Scene.Camera;
            karta = Scene.FindEntity(TiledObjects.KartaEntitet).GetComponent<TiledMapRenderer>();
            Kamra.Position = new Vector2(karta.Width / 2 , karta.Height / 2);
        }

        public override void Update() {
            base.Update();
            CenterCamera();
#if DEBUG
            DebugZoom();
#endif
        }

        private void DebugZoom() {
            BaseZoom = Math.Max(Math.Min(10 , BaseZoom + (Input.MouseWheelDelta / 200.0f)) , 2.5f);
        }

        public void Register(CameraTracker cameraTracker) {
            Spårare.Add(cameraTracker);
            SortByPriority();
        }

        private void SortByPriority() {
            Spårare.Sort((a , b) => b.Prioritet.CompareTo(a.Prioritet));
        }

        public void Unregister(CameraTracker cameraTracker) {
            Spårare.Remove(cameraTracker);
            SortByPriority();
        }

        private void CenterCamera() {
            if(Spårare.Count == 0) {
                return;
            }

            float left = 10000;
            float right = -10000;
            float top = 10000;
            float bottom = -10000;
            const float paddingX = 64;
            const float paddingY = 64;

            bool anyToTrack = false;
            int lastTrackerPriority = -1;
            foreach(CameraTracker tracker in Spårare) {
                if(!tracker.Enabled || !tracker.BordeEntitetFöljas() || (FönsterLäge && !(tracker.Entity is Player))) {
                    continue;
                }

                if(tracker.Prioritet < lastTrackerPriority) {
                    break;
                }

                lastTrackerPriority = tracker.Prioritet;
                Vector2 pos = tracker.PositionFunction();
                left = Math.Min(pos.X - (paddingX / 2) , left);
                right = Math.Max(pos.X + (paddingX / 2) , right);
                top = Math.Min(pos.Y - (paddingY / 2) , top);
                bottom = Math.Max(pos.Y + (paddingY / 2) , bottom);
                anyToTrack = true;
            }
            if(!anyToTrack) {
                Zoom = Lerps.LerpTowards(Kamra.RawZoom , 2.7f , 0.95f , Time.DeltaTime * 10f);
                Position = Lerps.LerpTowards(Kamra.Position , new Vector2(karta.Bounds.Right / 2 , karta.Bounds.Bottom / 2) , 0.95f , Time.DeltaTime * 10f);
                return;
            }
            float targetWidth = Math.Max(SkärmBredd , (right - left) * BaseZoom);
            float targetHeight = Math.Max(SkärmHöjd , (bottom - top) * BaseZoom);
            float zoom = BaseZoom * Math.Min(SkärmBredd / targetWidth , SkärmHöjd / targetHeight);
            Vector2 center = new Vector2(left + ((right - left) / 2) , top + ((bottom - top) / 2));

            Zoom = Lerps.LerpTowards(Kamra.RawZoom , zoom , 0.75f , Time.DeltaTime * 10f);

            Position = Lerps.LerpTowards(Kamra.Position , center , 0.25f , Time.DeltaTime * 10f);

            // Hack to avoid weird camera stopping if players are still
            Kamra.Position += new Vector2(1f , 0);
            Kamra.Position += new Vector2(-1f , 0);

            if(Kamra.Bounds.Left < 0) {
                Kamra.Position = new Vector2(Kamra.Bounds.Width / 2 , Kamra.Position.Y);
            }

            if(Kamra.Bounds.Top < 0) {
                Kamra.Position = new Vector2(Kamra.Position.X , Kamra.Bounds.Height / 2);
            }

            if(Kamra.Bounds.Right > karta.Bounds.Right) {
                Kamra.Position = new Vector2(karta.Bounds.Right - (Kamra.Bounds.Width / 2) , Kamra.Position.Y);
            }

            if(Kamra.Bounds.Bottom > karta.Bounds.Bottom) {
                Kamra.Position = new Vector2(Kamra.Position.X , karta.Bounds.Bottom - (Kamra.Bounds.Height / 2));
            }
        }

        public void SetWinMode(bool fönsterLäge) {
            FönsterLäge = fönsterLäge;
        }
    }
}