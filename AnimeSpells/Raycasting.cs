using GTA;
using GTA.Math;

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
    }
}
