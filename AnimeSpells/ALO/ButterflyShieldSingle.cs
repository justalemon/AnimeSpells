using Citron;
using Citron.Extensions;
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
        /// The maximum health allowed on an entity.
        /// </summary>
        private const int MaxHealth = 1000;
        /// <summary>
        /// The normal maximum health of a ped.
        /// </summary>
        private const int MaxPedHealth = 200;
        /// <summary>
        /// The normal maximum health of a vehicle.
        /// </summary>
        private const int MaxVehicleHealth = 1000;
        /// <summary>
        /// Internal activation of the spell.
        /// </summary>
        private static bool InternalEnabled = false;
        /// <summary>
        /// The value of health during the last tick.
        /// </summary>
        private static int LastHealth = 0;
        /// <summary>
        /// The activation of the Shield for the player.
        /// </summary>
        public static bool Enabled
        {
            get => InternalEnabled;
            set
            {
                if (value)
                {
                    // If the mana is higher than zero, heal the player and enable the spell
                    if (Manager.Mana > 0)
                    {
                        Game.Player.Character.SetHealth(MaxPedHealth);
                        InternalEnabled = true;
                    }
                }
                else
                {
                    // Set the shield to disabled
                    InternalEnabled = false;
                    // Set the max and current player health to 200
                    Game.Player.Character.SetMaxHealth(MaxPedHealth);
                    Game.Player.Character.SetHealth(MaxPedHealth);
                    // If the player is on a vehicle
                    if (Game.Player.Character.CurrentVehicle != null)
                    {
                        // Set the max and current health to 1000
                        Game.Player.Character.CurrentVehicle.SetMaxHealth(MaxVehicleHealth);
                        Game.Player.Character.CurrentVehicle.SetHealth(MaxVehicleHealth);
                    }
                }
            }
        }

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

            // If the spell is enabled
            if (Enabled)
            {
                // Draw a marker around the player or vehicle
                World.DrawMarker(MarkerType.DebugSphere, PlayerData.Position, Vector3.Zero, Vector3.Zero, Tools.ShieldDiameter, Color.DeepSkyBlue.Clear(), false, false, 0, true, "", "", false);

                // If the max health is not set to our value, apply it
                if (Game.Player.Character.GetMaxHealth() != MaxHealth)
                {
                    Game.Player.Character.SetMaxHealth(MaxHealth);
                }

                // Calculate the difference between the current and last health value
                int Difference = LastHealth - Game.Player.Character.GetHealth();
                // If the current mana is higher or equal than the difference
                if (Manager.Mana >= Difference)
                {
                    // Subtract the mana
                    Manager.Mana -= Difference;
                }
                // Otherwise
                else
                {
                    // Set the mana to zero and disable the script
                    Manager.Mana = 0;
                    Enabled = false;
                }

                // "Heal" the player
                Game.Player.Character.SetHealth(MaxHealth);

                // If the player is on a vehicle
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    // If the max health is not set to 10000, apply it
                    if (Game.Player.Character.CurrentVehicle.GetMaxHealth() != MaxHealth)
                    {
                        Game.Player.Character.CurrentVehicle.SetMaxHealth(MaxHealth);
                    }

                    // "Heal" the vehicle
                    Game.Player.Character.CurrentVehicle.SetHealth(MaxHealth);
                }

                // If the player has left a vehicle and is invincible
                if (Game.Player.Character.LastVehicle != null && Game.Player.Character.CurrentVehicle.GetMaxHealth() == MaxHealth)
                {
                    // Disable the invencibility of it
                    Game.Player.Character.LastVehicle.SetMaxHealth(MaxVehicleHealth);
                }

                // Store the current player health
                LastHealth = Game.Player.Character.GetHealth();
            }
        }
    }
}
