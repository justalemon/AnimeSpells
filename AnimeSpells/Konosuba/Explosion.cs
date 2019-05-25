﻿using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace AnimeSpells.Konosuba
{
    /// <summary>
    /// The selected status of the explosion spell.
    /// </summary>
    public enum ExplosionStatus
    {
        Disabled = 0,
        Targeting = 1,
        Set = 2,
        Firing = 3,
        Ragdoll = 4
    }

    /// <summary>
    /// EXPLOSION!
    /// https://www.youtube.com/watch?v=fIgm6lxvUfQ
    /// https://www.youtube.com/watch?v=O4tbOvKwZUw
    /// </summary>
    public class Explosion : Script
    {
        /// <summary>
        /// The current status of the explosion.
        /// </summary>
        public ExplosionStatus Status = ExplosionStatus.Disabled;
        /// <summary>
        /// The next status of the targeting
        /// </summary>
        public ExplosionStatus NextStatus => (int)Status + 1 > (int)ExplosionStatus.Ragdoll ? ExplosionStatus.Disabled : (ExplosionStatus)((int)Status + 1);
        /// <summary>
        /// The position of the explosion.
        /// </summary>
        public Vector3 Position = Vector3.Zero;

        public Explosion()
        {
            // Add our tick event
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // If one of the explosion cheats has been entered
            if (Tools.HasCheatBeenEntered("explosion") || Tools.HasCheatBeenEntered("exp"))
            {
                // Set the status to the next value
                Status = NextStatus;
            }

            // If the current status is not Disabled
            if (Status != ExplosionStatus.Disabled)
            {
                // Disable the fire/attack control
                Game.DisableControlThisFrame(0, Control.Attack);
                // If the user just used the disabled control
                if (Game.IsControlJustPressed(0, Control.Attack))
                {
                    // Set the next status
                    Status = NextStatus;
                }
            }

            // If the current status is targeting
            if (Status == ExplosionStatus.Targeting)
            {
                // Update the explosion position
                Position = World.GetCrosshairCoordinates().HitCoords;
                // Create a green static marker
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 10), Color.LightGreen);
            }
            // If the current status is targeted
            else if (Status == ExplosionStatus.Set)
            {
                // Draw a red rotating marker
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 10), Color.OrangeRed, false, false, 0, true, "", "", false);
            }
            // If the current status is firing
            else if (Status == ExplosionStatus.Firing)
            {
                // Create a blimp type explosion with a radius of 1000
                World.AddExplosion(Position, ExplosionType.Blimp, 1000, 0, true, false);
                // And inmediately set the status to ragdoll
                Status = ExplosionStatus.Ragdoll;
            }
            // If the current status is ragdoll
            else if (Status == ExplosionStatus.Ragdoll)
            {
                // Ragdoll the player during 1ms
                Function.Call(Hash.SET_PED_TO_RAGDOLL, Game.Player.Character, 1, 1, 1, true, true, false);
            }
        }
    }
}
