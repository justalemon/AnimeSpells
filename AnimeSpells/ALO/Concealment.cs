using GTA;
using GTA.Native;
using System;

namespace AnimeSpells.ALO
{
    /// <summary>
    /// Concealment
    /// https://swordartonline.fandom.com/wiki/Concealment
    /// </summary>
    public class Concealment : BaseScript
    {
        public override string Cheat => "concealment";

        public Concealment()
        {
            // This actually works different from the anime:
            // Instead of camouflaging the player with the environment, the player becomes invisible for everyone else.
            // But all of the other attributes that are carried over from the anime:
            // - The spell is disabled when in contact with other enemies or enemy habilities
            // - You can be Heard, so certain actions like shooting or entering a vehicle will disable the incantation

            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // Execute the spell contents on a tick
            ExecuteSpell();
        }

        public override void EnableSpell()
        {
            // if the player is on a vehicle, return
            if (Game.Player.Character.CurrentVehicle != null)
            {
                return;
            }

            // Get all of the peds around the player (maybe 50 will get us in trouble, investigate later)
            foreach (Ped ped in World.GetNearbyPeds(Game.Player.Character.Position, 50))
            {
                // If the ped is on combat against the player
                if (ped.IsInCombatAgainst(Game.Player.Character))
                {
                    // Return, don't activate the spell
                    return;
                }
            }

            // Set the alpha of the player to zero and hide the weapon
            Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
            Game.Player.Character.Alpha = 0;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true); // SET_POLICE_IGNORE_PLAYER
            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, true); // SET_EVERYONE_IGNORE_PLAYER
            // Mark the spell as enabled
            Enabled = true;
        }

        public override void ExecuteSpell()
        {
            // If is not enabled, return
            if (!Enabled)
            {
                return;
            }

            // Disable the enter vehicle button/key
            Game.DisableControlThisFrame(0, Control.Enter);

            // If the player has changed the weapon from fists
            if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
            {
                // Disable the spell
                DisableSpell();
            }
        }

        public override void DisableSpell()
        {
            // Reset the alpha and mark as disabled
            Game.Player.Character.Alpha = 255;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false); // SET_POLICE_IGNORE_PLAYER
            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false); // SET_EVERYONE_IGNORE_PLAYER
            Enabled = false;
        }
    }
}
