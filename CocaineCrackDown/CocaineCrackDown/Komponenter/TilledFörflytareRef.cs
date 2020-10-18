using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Tiled;

namespace CocaineCrackDown.Komponenter {
    public class TilledFörflytareRef : Component {
        public class KollitionsStatusTiled {
			public bool Höger;
            public bool Vänster;
            public bool Ovanför;
            public bool Under;
			public bool BlevGrundadDennaFrame;
			public bool VarGrundadFöraFramen;
			public bool GrundadPåEnkelriktadPlatform;
			public float SlopeVinkel;
            public bool HarKollition => Under || Höger || Vänster || Ovanför;

			// state used by the TiledMapMover
			internal SubpixelFloat RörelsePåminareX;
            internal SubpixelFloat RörelsePåminareY;
			internal TmxLayerTile SitaGrundadeTile;


			public void RänsaSitaGrundadeTile() => SitaGrundadeTile = null;


			public void Återställ() {
				BlevGrundadDennaFrame = GrundadPåEnkelriktadPlatform = Höger = Vänster = Ovanför = Under = false;
				SlopeVinkel = 0f;
			}

			/// <summary>
			/// resets collision state and does sub-pixel movement calculations
			/// </summary>
			/// <param name="rörelse">Motion.</param>
			public void Återställ(ref Vector2 rörelse) {
				if(rörelse.X == 0) {
                    Höger = Vänster = false;
                }

                if(rörelse.Y == 0) {
                    Ovanför = Under = false;
                }

                BlevGrundadDennaFrame = GrundadPåEnkelriktadPlatform = false;
				SlopeVinkel = 0f;

				// deal with subpixel movement, storing off any non-integar remainder for the next frame
				RörelsePåminareX.Update(ref rörelse.X);
				RörelsePåminareY.Update(ref rörelse.Y);

                // due to subpixel movement we might end up with 0 gravity when we really want there to be at least 1 pixel so slopes can work
                if(Under && rörelse.Y == 0 && RörelsePåminareY.Remainder > 0) {
					rörelse.Y = 1;
					RörelsePåminareY.Reset();
				}
			}

            public override string ToString() {
				return string.Format("[CollisionState] r: {0}, l: {1}, a: {2}, b: {3}, angle: {4}, wasGroundedLastFrame: {5}, becameGroundedThisFrame: {6}", Höger, Vänster, Ovanför, Under, SlopeVinkel, VarGrundadFöraFramen, BlevGrundadDennaFrame);
			}
		}

		/// <summary>
		/// the inset on the horizontal plane that the BoxCollider will be shrunk by when moving vertically
		/// </summary>
		public int ColliderHorizontalInset = 2;

		/// <summary>
		/// the inset on the vertical plane that the BoxCollider will be shrunk by when moving horizontally
		/// </summary>
		public int ColliderVerticalInset = 6;

		/// <summary>
		/// Används För att kolla Kollitioner
		/// </summary>
		public TmxLayer KollitionsLager;

		/// <summary>
		/// kartan som har kollitions lagret
		/// </summary>
		public TmxMap Karta;

		/// <summary>
		/// temporary storage for all the tiles that intersect the bounds being checked
		/// </summary>
		private List<TmxLayerTile> KolliderandeTiles = new List<TmxLayerTile>();

		/// <summary>
		/// temporary storage to avoid having to pass it around
		/// </summary>
		private Rectangle BoxColliderGränser;


		public TilledFörflytareRef()
		{ }

		public TilledFörflytareRef(TmxLayer collisionLayer) {
			Insist.IsNotNull(collisionLayer, nameof(collisionLayer) + " is required");
			KollitionsLager = collisionLayer;
			Karta = collisionLayer.Map;
		}

        /// <summary>
        /// moves the Entity taking into account the tiled map
        /// </summary>
        /// <param name="rörelse">Motion.</param>
        /// <param name="boxCollider">Box collider.</param>
        public void Move(Vector2 rörelse , BoxCollider boxCollider , KollitionsStatusTiled KollitionsStatus) {
			if (Karta == null) {
                return;
            }

            // test for collisions then move the Entity
            TestCollisions(ref rörelse, boxCollider.Bounds, KollitionsStatus);

			boxCollider.UnregisterColliderWithPhysicsSystem();
			boxCollider.Entity.Transform.Position += rörelse;
			boxCollider.RegisterColliderWithPhysicsSystem();
		}

		public void TestCollisions(ref Vector2 rörelse, Rectangle boxColliderGränser, KollitionsStatusTiled KollitionsStatus)
		{
			BoxColliderGränser = boxColliderGränser;

			// save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
			KollitionsStatus.VarGrundadFöraFramen = KollitionsStatus.Under;

			// reset our collisions state
			KollitionsStatus.Återställ(ref rörelse);

            // reset rounded motion for us while dealing with subpixel movement so fetch the rounded values to use for our actual detection
            int motionX = (int)rörelse.X;
            int motionY = (int)rörelse.Y;

			// first, check movement in the horizontal dir
			if (motionX != 0)
			{
                Edge direction = motionX > 0 ? Edge.Right : Edge.Left;
                Rectangle sweptBounds = CollisionRectForSide(direction, motionX);

				int collisionResponse;
				if (TestMapCollision(sweptBounds, direction, KollitionsStatus, out collisionResponse))
				{
					// react to collision. get the distance between our leading edge and what we collided with
					rörelse.X = collisionResponse - boxColliderGränser.GetSide(direction);
					KollitionsStatus.Vänster = direction == Edge.Left;
					KollitionsStatus.Höger = direction == Edge.Right;
					KollitionsStatus.RörelsePåminareX.Reset();
				}
				else
				{
					KollitionsStatus.Vänster = false;
					KollitionsStatus.Höger = false;
				}
			}

			// next, check movement in the vertical dir
			{
                Edge riktning = motionY >= 0 ? Edge.Bottom : Edge.Top;
                Rectangle sweptGränser = CollisionRectForSide(riktning,motionY);
				sweptGränser.X += (int)rörelse.X;

                int KollitsionResponse;
				if (TestMapCollision(sweptGränser, riktning, KollitionsStatus, out KollitsionResponse))
				{
					// react to collision. get the distance between our leading edge and what we collided with
					rörelse.Y = KollitsionResponse - boxColliderGränser.GetSide(riktning);
					KollitionsStatus.Ovanför = riktning == Edge.Top;
					KollitionsStatus.Under = riktning == Edge.Bottom;
					KollitionsStatus.RörelsePåminareY.Reset();

					if (KollitionsStatus.Under && KollitionsStatus.SitaGrundadeTile?.IsSlope() == true) {
                        KollitionsStatus.SlopeVinkel = MathHelper.ToDegrees((float)Math.Atan(KollitionsStatus.SitaGrundadeTile.GetSlope()));
                    }
                }
				else {
					KollitionsStatus.Ovanför = false;
					KollitionsStatus.Under = false;
					KollitionsStatus.SitaGrundadeTile = null;
				}


				// when moving down we also check for collisions in the opposite direction. this needs to be done so that ledge bumps work when
				// a jump is made but misses by the colliderVerticalInset
				if (riktning == Edge.Bottom) {
					riktning = riktning.OppositeEdge();
					sweptGränser = CollisionRectForSide(riktning, 0);
					sweptGränser.X += (int)rörelse.X;
					sweptGränser.Y += (int)rörelse.Y;

					if (TestMapCollision(sweptGränser, riktning, KollitionsStatus, out KollitsionResponse))
					{
						// react to collision. get the distance between our leading edge and what we collided with
						rörelse.Y = KollitsionResponse - boxColliderGränser.GetSide(riktning);

						// if we collide here this is an overlap of a slope above us. this small bump down will prevent hitches when hitting
						// our head on a slope that connects to a solid tile. It puts us below the slope when the normal response would put us
						// above it
						rörelse.Y += 2;
						KollitionsStatus.Ovanför = true;
					}
				}
			}

			// set our becameGrounded state based on the previous and current collision state
			if (!KollitionsStatus.VarGrundadFöraFramen && KollitionsStatus.Under) {
                KollitionsStatus.BlevGrundadDennaFrame = true;
            }
        }

        private bool TestMapCollision(Rectangle kollitionsRect,Edge riktning,KollitionsStatusTiled KollitionsStatus,out int KollitsionResponse) {
			KollitsionResponse = 0;
            Edge side = riktning.OppositeEdge();
            int perpindicularPosition = side.IsVertical() ? kollitionsRect.Center.X : kollitionsRect.Center.Y;
            int leadingPosition = kollitionsRect.GetSide(riktning);
            bool shouldTestSlopes = side.IsVertical();
			PopulateCollidingTiles(kollitionsRect, riktning);

			for (int i = 0; i < KolliderandeTiles.Count; i++)
			{
				if (KolliderandeTiles[i] == null) {
                    continue;
                }

                // disregard horizontal collisions with tiles on the same row as a slope if the last tile we were grounded on was a slope.
                // the y collision response will push us up on the slope.
                if (riktning.IsHorizontal() && KollitionsStatus.SitaGrundadeTile != null &&
					KollitionsStatus.SitaGrundadeTile.IsSlope() && IsSlopeCollisionRow(KolliderandeTiles[i].Y)) {
                    continue;
                }

                if (TestTileCollision(KolliderandeTiles[i], side, perpindicularPosition, leadingPosition,
					shouldTestSlopes, out KollitsionResponse))
				{
					// store off our last ground tile if we collided below
					if (riktning == Edge.Bottom)
					{
						KollitionsStatus.SitaGrundadeTile = KolliderandeTiles[i];
						KollitionsStatus.GrundadPåEnkelriktadPlatform = KollitionsStatus.SitaGrundadeTile.IsOneWayPlatform();
					}

					return true;
				}

				// special case for sloped ground tiles
				if (KollitionsStatus.SitaGrundadeTile != null && riktning == Edge.Bottom)
				{
					// if grounded on a slope and intersecting a slope or if grounded on a wall and intersecting a tall slope we go sticky.
					// tall slope here means one where the the slopeTopLeft/Right is 0, i.e. it connects to a wall
					var isHighSlopeNearest = KolliderandeTiles[i].IsSlope() &&
											 KolliderandeTiles[i].GetNearestEdge(perpindicularPosition) ==
											 KolliderandeTiles[i].GetHighestSlopeEdge();
					if ((KollitionsStatus.SitaGrundadeTile.IsSlope() && KolliderandeTiles[i].IsSlope()) || (!KollitionsStatus.SitaGrundadeTile.IsSlope() && isHighSlopeNearest)) {
						// store off our last ground tile if we collided below
						KollitionsStatus.SitaGrundadeTile = KolliderandeTiles[i];
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Checks whether collision is occurring with a slope on a given row.
		/// </summary>
		/// <returns>Whether collision is occurring with a slope on a given row</returns>
		/// <param name="radY">the row to check</param>
		private bool IsSlopeCollisionRow(int radY)
		{
			for (var i = 0; i < KolliderandeTiles.Count; i++)
			{
				if (KolliderandeTiles[i] != null && KolliderandeTiles[i].IsSlope() && KolliderandeTiles[i].Y == radY) {
                    return true;
                }
            }

			return false;
		}

		/// <summary>
		/// Tests the tile for a collision. Returns via out the position in world space where the collision occured.
		/// </summary>
		/// <returns>The tile collision.</returns>
		/// <param name="tile">Tile.</param>
		/// <param name="edgeAttTesta">the opposite side of movement, the side the leading edge will collide with</param>
		/// <param name="perpindicularPosition">Perpindicular position.</param>
		/// <param name="leadingPosition">Leading position.</param>
		/// <param name="bordeTestaSlope">Should test slopes.</param>
		/// <param name="KollitsionResponse">Collision response.</param>
		private bool TestTileCollision(TmxLayerTile tile, Edge edgeAttTesta, int perpindicularPosition, int leadingPosition, bool bordeTestaSlope, out int KollitsionResponse) {
			KollitsionResponse = leadingPosition;

			// one way platforms are only collideable from the top when the player is already above them
			if (tile.IsOneWayPlatform())
			{
				// only the top edge of one way platforms are checked for collisions
				if (edgeAttTesta != Edge.Top) {
                    return false;
                }

                // our response should be the top of the platform
                KollitsionResponse = Karta.TileToWorldPositionX(tile.Y);
				return BoxColliderGränser.Bottom <= KollitsionResponse;
			}

            bool forceSlopedTileCheckAsWall = false;

            // when moving horizontally the only time a slope is considered for collision testing is when its closest side is the tallest side
            // and we were not intesecting the tile before moving.
            // this prevents clipping through a tile when hitting its edge: -> |\
            if(edgeAttTesta.IsHorizontal() && tile.IsSlope() && tile.GetNearestEdge(leadingPosition) == tile.GetHighestSlopeEdge()) {
				var moveDir = edgeAttTesta.OppositeEdge();
				var leadingPositionPreMovement = BoxColliderGränser.GetSide(moveDir);

				// we need the tile x position that is on the opposite side of our move direction. Moving right we want the left edge
				var tileX = moveDir == Edge.Right
					? Karta.TileToWorldPositionX(tile.X)
					: Karta.TileToWorldPositionX(tile.X + 1);

				// using the edge before movement, we see if we were colliding before moving.
				var wasCollidingBeforeMove = moveDir == Edge.Right
					? leadingPositionPreMovement > tileX
					: leadingPositionPreMovement < tileX;

				// if we were not colliding before moving we need to consider this tile for a collision check as if it were a wall tile
				forceSlopedTileCheckAsWall = !wasCollidingBeforeMove;
			}


			if (forceSlopedTileCheckAsWall || !tile.IsSlope())
			{
				switch (edgeAttTesta)
				{
					case Edge.Top:
						KollitsionResponse = Karta.TileToWorldPositionY(tile.Y);
						break;
					case Edge.Bottom:
						KollitsionResponse = Karta.TileToWorldPositionY(tile.Y + 1);
						break;
					case Edge.Left:
						KollitsionResponse = Karta.TileToWorldPositionX(tile.X);
						break;
					case Edge.Right:
						KollitsionResponse = Karta.TileToWorldPositionX(tile.X + 1);
						break;
				}

				return true;
			}

			if (bordeTestaSlope)
			{
				var tileWorldX = Karta.TileToWorldPositionX(tile.X);
				var tileWorldY = Karta.TileToWorldPositionX(tile.Y);
				var slope = tile.GetSlope();
				var offset = tile.GetSlopeOffset();

				// calculate the point on the slope at perpindicularPosition
				KollitsionResponse = (int)(edgeAttTesta.IsVertical()
					? slope * (perpindicularPosition - tileWorldX) + offset + tileWorldY
					: (perpindicularPosition - tileWorldY - offset) / slope + tileWorldX);
				var isColliding = edgeAttTesta.IsMax()
					? leadingPosition <= KollitsionResponse
					: leadingPosition >= KollitsionResponse;

				// this code ensures that we dont consider collisions on a slope while jumping up that dont intersect our collider.
				// It also makes sure when testing the bottom edge that the leadingPosition is actually above the collisionResponse.
				// HACK: It isn't totally perfect but it does the job
				if (isColliding && edgeAttTesta == Edge.Bottom && leadingPosition <= KollitsionResponse)
					isColliding = false;

				return isColliding;
			}

			return false;
		}

		/// <summary>
		/// gets a list of all the tiles intersecting bounds. The returned list is ordered for collision detection based on the
		/// direction passed in so they can be processed in order.
		/// </summary>
		/// <returns>The colliding tiles.</returns>
		/// <param name="bounds">Bounds.</param>
		/// <param name="riktining">Direction.</param>
		private void PopulateCollidingTiles(Rectangle bounds, Edge riktining)
		{
			KolliderandeTiles.Clear();
			var isHorizontal = riktining.IsHorizontal();
			var primaryAxis = isHorizontal ? Axis.X : Axis.Y;
			var oppositeAxis = primaryAxis == Axis.X ? Axis.Y : Axis.X;

			var oppositeDirection = riktining.OppositeEdge();
			var firstPrimary = TilePosition(bounds.GetSide(oppositeDirection), primaryAxis);
			var lastPrimary = TilePosition(bounds.GetSide(riktining), primaryAxis);
			var primaryIncr = riktining.IsMax() ? 1 : -1;

			var min = TilePosition(isHorizontal ? bounds.Top : bounds.Left, oppositeAxis);
			var mid = TilePosition(isHorizontal ? bounds.GetCenter().Y : bounds.GetCenter().X, oppositeAxis);
			var max = TilePosition(isHorizontal ? bounds.Bottom : bounds.Right, oppositeAxis);

			var isPositive = mid - min < max - mid;
			var secondaryIncr = isPositive ? 1 : -1;
			var firstSecondary = isPositive ? min : max;
			var lastSecondary = !isPositive ? min : max;

			for (var primary = firstPrimary; primary != lastPrimary + primaryIncr; primary += primaryIncr)
			{
				for (var secondary = firstSecondary;
					secondary != lastSecondary + secondaryIncr;
					secondary += secondaryIncr)
				{
					var col = isHorizontal ? primary : secondary;
					var row = !isHorizontal ? primary : secondary;
					KolliderandeTiles.Add(KollitionsLager.GetTile(col, row));

				}
			}
		}

        /// <summary>
        /// returns Tile positionen clamped till tilemapen
        /// </summary>
        /// <returns>Värd Till Tile Position.</returns>
        /// <param name="världPosition">World position.</param>
        /// <param name="Axel">Axis.</param>
        private int TilePosition(float världPosition,Axis Axel) {

            return Axel == Axis.Y ? Karta.WorldToTilePositionY(världPosition) : Karta.WorldToTilePositionX(världPosition);
        }

        /// <summary>
        /// gets a collision rect for the given side expanded to take into account motion
        /// </summary>
        /// <returns>The rect for side.</returns>
        /// <param name="sida">Side.</param>
        /// <param name="rörelse">Motion.</param>
        private Rectangle CollisionRectForSide(Edge sida,int rörelse) {
            Rectangle bounds = sida.IsHorizontal() ? BoxColliderGränser.GetRectEdgePortion(sida) : BoxColliderGränser.GetHalfRect(sida);

            // for horizontal collision checks we use just a sliver for our bounds. Vertical gets the half rect so that it can properly push
            // up when intersecting a slope which is ignored when moving horizontally.

            // we contract horizontally for vertical movement and vertically for horizontal movement
            if(sida.IsVertical()) {
                RectangleExt.Contract(ref bounds, ColliderHorizontalInset, 0);
            }
            else {
                RectangleExt.Contract(ref bounds, 0, ColliderVerticalInset);
            }

            // finally expand the side in the direction of movement
            RectangleExt.ExpandSide(ref bounds,sida,rörelse);

			return bounds;
		}
                     
    }
}