using GTA;
using GTA.Native;
using System;

namespace AnimeSpells.ALO
{
    /// <summary>
    /// Concealment
    /// https://swordartonline.fandom.com/wiki/Concealment
    /// </summary>
    public class Concealment : Script
    {
        private bool InternalEnabled { get; set; } = false;
        public bool Enabled
        {
            // Return the internal value
            get => InternalEnabled;
            set
            {
                // Set the internal value
                InternalEnabled = value;
                // If the spell has been enabled
                if (value)
                {
                    // if the player is on a vehicle, ignore return
                    if (Game.Player.Character.CurrentVehicle != null)
                    {
                        InternalEnabled = false;
                        return;
                    }

                    // Get all of the peds around the player (maybe 50 will get us in trouble, investigate later)
                    foreach (Ped ped in World.GetNearbyPeds(Game.Player.Character.Position, 50))
                    {
                        // If the ped is on combat against the player
                        if (ped.IsInCombatAgainst(Game.Player.Character))
                        {
                            // Return and disable the spell
                            InternalEnabled = false;
                            return;
                        }
                    }

                    // Set the alpha of the player to zero and hide the weapon
                    Game.Player.Character.Alpha = 50;
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true); // SET_POLICE_IGNORE_PLAYER
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, true); // SET_EVERYONE_IGNORE_PLAYER
                }
                else
                {
                    // Reset the alpha and mark as disabled
                    Game.Player.Character.Alpha = 255;
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false); // SET_POLICE_IGNORE_PLAYER
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false); // SET_EVERYONE_IGNORE_PLAYER
                    if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
                    {
                        Game.Player.Character.Weapons.CurrentWeaponObject.ResetAlpha();
                    }
                }
            }
        }

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
            // If the cheat has been entered
            if (Tools.HasCheatBeenEntered("concealment") || Tools.HasCheatBeenEntered("con"))
            {
                // Alternate the enabled status
                Enabled = !Enabled;
            }

            // If the spell is enabled
            if (Enabled)
            {
                // Disable the enter vehicle button/key
                Game.DisableControlThisFrame(0, Control.Enter);

                // If the player weapon has a prop and the alpha of it is not set to 50
                if (Game.Player.Character.Weapons.CurrentWeaponObject != null && Game.Player.Character.Weapons.CurrentWeaponObject.Alpha != 50)
                {
                    // Set the current alpha to 50
                    Game.Player.Character.Weapons.CurrentWeaponObject.Alpha = 50;
                }

                // If the player has just tried to fire a weapon
                if (Game.IsControlJustPressed(0, Control.Attack))
                {
                    // Disable the spell
                    Enabled = false;
                }
            }
        }
    }
}
