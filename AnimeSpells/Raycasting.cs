using GTA;
using GTA.Math;
using System;
using System.Drawing;

namespace AnimeSpells
{
    /// <summary>
    /// A set of tools to make raycasting the crosshair easier.
    /// </summary>
    public class Raycasting : Script
    {
        /// <summary>
        /// The entity that is being targeted.
        /// </summary>
        public static Entity TargetedEntity
        {
            get
            {
                // Just return the entity hit by the raycast
                return World.GetCrosshairCoordinates().HitEntity;
            }
        }

        /// <summary>
        /// The ped that is being targeted.
        /// </summary>
        public static Ped TargetedPed
        {
            get
            {
                // If the entity is not null and is a ped
                if (TargetedEntity != null && TargetedEntity is Ped)
                {
                    // Return the entity as a ped
                    return (Ped)TargetedEntity;
                }

                // Otherwise, return null
                return null;
            }
        }

        /// <summary>
        /// The position that is being targeted by the raycast.
        /// </summary>
        public static Vector3 Position
        {
            get
            {
                // Just return the entity hit by the raycast
                return World.GetCrosshairCoordinates().HitCoords;
            }
        }

        /// <summary>
        /// The color for the ped marker.
        /// </summary>
        public static Color MarkerColor { get; set; } = Color.Cyan;

        public Raycasting()
        {
            // Over here we subscribe our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // If there is a ped being targeted and is not the player
            if (TargetedPed != null && TargetedPed != Game.Player.Character)
            {
                // Create a marker on the top of it
                World.DrawMarker(MarkerType.UpsideDownCone, TargetedPed.Position + new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), MarkerColor);
            }
        }
    }
}
