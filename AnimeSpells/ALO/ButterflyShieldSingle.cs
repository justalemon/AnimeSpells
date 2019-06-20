using Citron;
using GTA;
using GTA.Math;
using System;
using System.Drawing;

namespace AnimeSpells.ALO
{
    /// <summary>
    /// Butterfly Shield: Single Target (the player)
    /// https://swordartonline.fandom.com/wiki/Butterfly_Shield
    /// </summary>
    public class ButterflyShieldSingle : Script
    {
        /// <summary>
        /// Internal activation of the spell.
        /// </summary>
        private static bool InternalEnabled = false;
        /// <summary>
        /// The activation of the Shield for the player.
        /// </summary>
        public static bool Enabled
        {
            // Set the value that we got
            get => InternalEnabled;
            set
            {
                // If the user wants to enable the shield and the ending time is lower or equal than the current time
                if (value && Time <= Game.GameTime)
                {
                    // Set the internal value to enabled
                    InternalEnabled = value;
                    // And set the time to the correct value
                    Time = Game.GameTime + MaxTime;
                }
                else if (!value)
                {
                    // Disable invencibility for the player and the vehicle (if is there)
                    if (Game.Player.Character.IsInvincible)
                    {
                        Game.Player.Character.IsInvincible = false;
                    }
                    if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle.IsInvincible)
                    {
                        Game.Player.Character.CurrentVehicle.IsInvincible = false;
                    }

                    // Set the time to zero
                    Time = 0;
                    // And disable the shield
                    InternalEnabled = false;
                }
            }
        }
        private static int Time = 0;
        private const int MaxTime = 5000;

        public ButterflyShieldSingle()
        {
            // Add our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // If the cheat has been entered
            if (Screen.HasCheatBeenEntered("shieldsingle") || Screen.HasCheatBeenEntered("shield"))
            {
                // Alternate the enabled status
                Enabled = !Enabled;
            }

            // If the spell is enabled and we are under the duration of the spell
            if (Enabled && Time >= Game.GameTime)
            {
                // Draw a marker around the player or vehicle
                World.DrawMarker(MarkerType.DebugSphere, PlayerData.Position, Vector3.Zero, Vector3.Zero, Tools.ShieldDiameter, Color.DeepSkyBlue.Clear(), false, false, 0, true, "", "", false);
                // Make the player invincible if he is not
                if (!Game.Player.Character.IsInvincible)
                {
                    Game.Player.Character.IsInvincible = true;
                }
                // If the player is on a vehicle
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    // If the vehiclle is not invincible
                    if (!Game.Player.Character.CurrentVehicle.IsInvincible)
                    {
                        // Make it invincible
                        Game.Player.Character.CurrentVehicle.IsInvincible = true;
                    }
                    // Wash and reapair the vehicle
                    Game.Player.Character.CurrentVehicle.Wash();
                    Game.Player.Character.CurrentVehicle.Repair();
                }
                // If the player has left a vehicle and is invincible
                if (Game.Player.Character.LastVehicle != null && !Game.Player.Character.LastVehicle.IsInvincible)
                {
                    // Disable the invencibility of it
                    Game.Player.Character.LastVehicle.IsInvincible = false;
                }
            }
            // If the script is enabled and the time is not zero
            else if (Enabled && Time != 0)
            {
                Enabled = false;
            }
        }
    }
}
