using GTA;
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
        /// <summary>
        /// If the wants to use the Explosions until they are manually deactivated.
        /// </summary>
        public bool Permanent = false;
        /// <summary>
        /// A blip for the current marker.
        /// </summary>
        public Blip MarkerBlip = null;

        public Explosion()
        {
            // Add our tick event and abort events
            Tick += OnTick;
            Aborted += OnAborted;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // If one of the explosion cheats has been entered
            if (Tools.HasCheatBeenEntered("explosion") || Tools.HasCheatBeenEntered("exp"))
            {
                // Set the status to the next value
                Status = NextStatus;
            }
            // Ig the user wants to alterante the activation of the permanent mode
            if (Tools.HasCheatBeenEntered("explosion permanent") || Tools.HasCheatBeenEntered("exp perm"))
            {
                // Alternate the activation of the permanent explosions
                Permanent = !Permanent;
                // And notify the user just in case
                UI.Notify("Permanent explosions " + (Permanent ? "Enabled" : "Disabled") + "!");
                // If they have been enabled
                if (Permanent)
                {
                    // Set the mode to targeting
                    Status = ExplosionStatus.Targeting;
                }
                // Otherwise
                else
                {
                    // Set the mode to disabled
                    Status = ExplosionStatus.Disabled;
                }
            }

            // If the current status is not Disabled or the permanent mode is enabled
            if (Status != ExplosionStatus.Disabled || Permanent)
            {
                // If the player is using a weapon and is not on permanent mode
                if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && !Permanent)
                {
                    // Select the fists
                    Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                }

                // Disable the fire/attack control
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
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
                // If there is not a blip created
                if (MarkerBlip == null)
                {
                    // Create a blip on the map
                    MarkerBlip = World.CreateBlip(Position);
                    // Set the color to green
                    MarkerBlip.Color = BlipColor.Green;
                    // Set the title to something more easy to find
                    MarkerBlip.Name = "Detonation";
                }
                // Set the position of the blip
                MarkerBlip.Position = Position;
            }
            // If the current status is targeted
            else if (Status == ExplosionStatus.Set)
            {
                // Draw a red rotating marker
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 10), Color.OrangeRed, false, false, 0, true, "", "", false);
                // Change the blip color to red
                MarkerBlip.Color = BlipColor.Red;
            }
            // If the current status is firing
            else if (Status == ExplosionStatus.Firing)
            {
                // Create a blimp type explosion with a radius of 1000
                World.AddExplosion(Position, ExplosionType.Blimp, 1000, 0, true, false);
                // Destroy the blip
                MarkerBlip.Remove();
                MarkerBlip = null;
                // If the permanent explosions are enabled
                if (Permanent)
                {
                    // Skip the ragdoll
                    Status = ExplosionStatus.Targeting;
                }
                // Otherwise
                else
                {
                    // Inmediately set the status to ragdoll
                    Status = ExplosionStatus.Ragdoll;
                }
            }
            // If the current status is ragdoll
            else if (Status == ExplosionStatus.Ragdoll)
            {
                // Ragdoll the player during 1ms
                Function.Call(Hash.SET_PED_TO_RAGDOLL, Game.Player.Character, 1, 1, 1, true, true, false);
            }
        }

        public void OnAborted(object sender, EventArgs args)
        {
            // If the blip is not null
            if (MarkerBlip != null)
            {
                // Remove it from the map
                MarkerBlip.Remove();
            }
        }
    }
}
