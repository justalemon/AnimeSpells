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
    public class ButterflyShieldSingle : BaseScript
    {
        public override string Cheat => "shieldsingle";
        private int Time { get; set; } = 0;
        private const int MaxTime = 5000;

        public ButterflyShieldSingle()
        {
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // If the spell is enabled and we are under the duration of the spell
            if (Enabled && Time >= Game.GameTime)
            {
                ExecuteSpell();
            }
            // If the script is enabled and the time is not zero
            else if (Enabled && Time != 0)
            {
                DisableSpell();
            }
        }

        public override void EnableSpell()
        {
            Enabled = true;
            Time = Game.GameTime + MaxTime;
        }

        public override void ExecuteSpell()
        {
            // Draw a marker around the player or vehicle
            World.DrawMarker((MarkerType)27, Tools.Position, Vector3.Zero, Vector3.Zero, Tools.ShieldDiameter, Color.CadetBlue, false, false, 0, true, "", "", false);
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

        public override void DisableSpell()
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
            Enabled = false;
        }
    }
}
