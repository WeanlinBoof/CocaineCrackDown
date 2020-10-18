using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tiled;

namespace CocaineCrackDown.Komponenter {

    public class Röraren : Component {

        private ColliderTriggerHelper TriggerHjälpare;

        private HashSet<Collider> Grannar;
        private TmxMap Karta;
        public Röraren(TmxMap tmxMap) {
            Karta = tmxMap;
        }

        public override void OnAddedToEntity() {
            TriggerHjälpare = new ColliderTriggerHelper(Entity);
        }
        private Vector2 TilePosition(Vector2 Pos) {
            return new Vector2(Karta.WorldToTilePositionX(Pos.X) , Karta.WorldToTilePositionX(Pos.Y));
        }

        /// <summary>
        /// beräknar rörelsen som ändrar rörelsevektorn för att ta hänsyn till eventuella
        /// kollisioner som kommer att inträffa vid rörelse
        /// </summary>
        /// <returns><c>true</c> Om Rörelsen Blev Uträknad <c>false</c> Om Inte.</returns>
        /// <param name="rörelse"> Rörelse.</param>
        /// <param name="kollitionsResultat"> Kollitions Resultat.</param>
        public bool BeräknaRörelse(ref Vector2 rörelse , out CollisionResult kollitionsResultat) {
            kollitionsResultat = new CollisionResult();

            // ingen kolliderare? Strunta i det och förflytta dig bara
            if(Entity.GetComponent<Collider>() == null || TriggerHjälpare == null) {
                return false;
            }

            // 1. flytta alla icke-trigger Kollisioner och få närmaste kollision
            List<Collider> kolliders = Entity.GetComponents<Collider>();
            for(int i = 0; i < kolliders.Count; i++) {
                Collider kollider = kolliders[i];

                // hoppa över triggers för tillfället.
                //vi kommer tillbaka till de när vi förflyttar oss.
                if(kollider.IsTrigger) {
                    continue;
                }

                // hämta allt som vi kan kollidera med vid den nya positionen
                RectangleF gränser = kollider.Bounds;
                gränser.X += rörelse.X;
                gränser.Y += rörelse.Y;
                Grannar = Physics.BoxcastBroadphaseExcludingSelf(kollider , ref gränser , kollider.CollidesWithLayers);

                foreach(Collider granne in Grannar) {

                    // hoppa över triggers för tillfället.
                    //vi kommer tillbaka till de när vi förflyttar oss.
                    if(granne.IsTrigger) {
                        continue;
                    }

                    if(kollider.CollidesWith(granne , rörelse , out CollisionResult InternKollitionsResultat)) {

                        // vid träff. dra vi undan våran rörelse
                        rörelse -= InternKollitionsResultat.MinimumTranslationVector;

                        // Om vi träffar flera objekt, kolla bara med den första för att förenkla.
                        if(InternKollitionsResultat.Collider != null) {
                            kollitionsResultat = InternKollitionsResultat;
                        }
                    }
                }
            }

            ListPool<Collider>.Free(kolliders);

            return kollitionsResultat.Collider != null;
        }

        /// <summary>
        /// Beräknar rörelsen som ändrar rörelsevektorn för att ta hänsyn till eventuella
        /// kollisioner som inträffar vid rörelse. Denna version är modifierad för att matas
        /// ut genom en viss samling för att visa alla kollision som inträffade
        /// </summary>
        /// <returns> Mängden kollitioner som inträffade.</returns>
        /// <param name="rörelse"> Rörelse.</param>
        /// <param name="kollitionsResultater"> Kollitions Resultat.</param>
        public int AdvancedCalculateMovement(ref Vector2 rörelse , ICollection<CollisionResult> kollitionsResultater) {
            int kollitioner = 0;

            // ingen kolliderare? Strunta i det och förflytta dig bara
            if(Entity.GetComponent<Collider>() == null || TriggerHjälpare == null) {
                return kollitioner;
            }

            // 1. flytta alla icke-trigger Kollisioner och få närmaste kollision
            List<Collider> kolliders = Entity.GetComponents<Collider>();
            for(int i = 0; i < kolliders.Count; i++) {
                Collider kollider = kolliders[i];

                // hoppa över triggers för tillfället.
                // vi kommer tillbaka till de när vi förflyttar oss.
                if(kollider.IsTrigger) {
                    continue;
                }

                // hämta allt som vi kan kollidera med vid den nya positionen
                RectangleF gränser = kollider.Bounds;
                gränser.X += rörelse.X;
                gränser.Y += rörelse.Y;
                Grannar = Physics.BoxcastBroadphaseExcludingSelf(kollider , ref gränser , kollider.CollidesWithLayers);

                foreach(Collider granne in Grannar) {

                    // hoppa över triggers för tillfället.
                    // vi kommer tillbaka till de när vi förflyttar oss.
                    if(granne.IsTrigger) {
                        continue;
                    }

                    if(kollider.CollidesWith(granne , rörelse , out CollisionResult InternKollitionsResultat)) {

                        // vid träff. dra vi undan våran rörelse
                        rörelse -= InternKollitionsResultat.MinimumTranslationVector;
                        kollitionsResultater.Add(InternKollitionsResultat);

                        kollitioner++;
                    }
                }
            }

            ListPool<Collider>.Free(kolliders);

            return kollitioner;
        }

        /// <summary>
        /// tillämpar rörelsen från <see cref="BeräknaRörelse(ref Vector2, out CollisionResult)()"/>
        /// till entiteten och uppdaterar TriggerHjälparen
        /// </summary>
        /// <param name="rörelse"> Rörelse.</param>
        public void TillämpaRörelsen(Vector2 rörelse) {

            // 2. flyttar entiteten till dennes nya position vid kollision annars förflyttas hela
            // mängden. rörelse uppdateras när en kollision inträffar
            Entity.Transform.Position += rörelse;

            // 3. gör en överlappningskontroll av alla Kolliderare som triggrar med alla bredphase
            // kolloider, trigger eller ingen trigger. Eventuella överlappningar resulterar med
            // att trigger Events blir anropad.
            TriggerHjälpare?.Update();
        }

        /// <summary>
        /// flyttar Entiteten med hänsyn till kollisioner genom att anropa
        /// <see cref="BeräknaRörelse(ref Vector2, out CollisionResult)()"/> följt av
        /// <see cref="TillämpaRörelsen(Vector2)"/>;
        /// </summary>
        /// <returns><c>true</c>, Om Förflytta Var förnyad <c>false</c> Annars.</returns>
        /// <param name="rörelse"> Rörelse.</param>
        /// <param name="kollitionsResultat"> Kollitions Resultat.</param>
        public bool Förflytta(Vector2 rörelse , out CollisionResult kollitionsResultat) {
            BeräknaRörelse(ref rörelse , out kollitionsResultat);

            TillämpaRörelsen(rörelse);

            return kollitionsResultat.Collider != null;
        }
    }
}