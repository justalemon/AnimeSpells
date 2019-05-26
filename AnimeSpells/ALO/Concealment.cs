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
        /// <summary>
        /// The peds to interate for the combat check.
        /// </summary>
        private Ped[] Peds = new Ped[0];
        /// <summary>
        /// The game time where we should fetch a new copy of the list of peds.
        /// </summary>
        private int NextFetch = 0;
        /// <summary>
        /// Internal activation of the spell.
        /// </summary>
        private bool InternalEnabled = false;
        /// <summary>
        /// Activation for the Concealment spell.
        /// </summary>
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
                    // if the player is on a vehicle
                    if (Game.Player.Character.CurrentVehicle != null)
                    {
                        // Disable the spell and return
                        InternalEnabled = false;
                        return;
                    }

                    // Iterate over all of the peds in the map
                    foreach (Ped ped in Peds)
                    {
                        // If the ped is on combat against the player
                        if (ped.IsInCombatAgainst(Game.Player.Character))
                        {
                            // Return and disable the spell
                            InternalEnabled = false;
                            return;
                        }
                    }

                    // Set the alpha of the player to 50 (to make it near invisible)
                    Game.Player.Character.Alpha = 50;
                    // And make him ignored by all other peds
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, true);
                }
                else
                {
                    // Reset the alpha to the normal value
                    Game.Player.Character.Alpha = 255;
                    // Make the player no longer ignored
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);
                    // If the current player weapon prop exists
                    if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
                    {
                        // Reset the alpha of it
                        Game.Player.Character.Weapons.CurrentWeaponObject.Alpha = 255;
                    }
                }
            }
        }

        public Concealment()
        {
            // This actually works different from the anime:
            // Instead of camouflaging with the environment, the player becomes invisible for everyone else.
            // But all of the other attributes that are carried over from the anime:
            // - The spell is disabled when in contact with other enemies or enemy habilities
            // - You can be Heard, so certain actions like shooting or entering a vehicle will disable the incantation
            Tick += OnTick;
            Aborted += OnAborted;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // If the cheat has been entered
            if (Tools.HasCheatBeenEntered("concealment") || Tools.HasCheatBeenEntered("con"))
            {
                // Alternate the enabled status
                Enabled = !Enabled;
            }

            // If the current time is higher or equal than the next fetch
            if (Game.GameTime >= NextFetch)
            {
                // Update the list with all of the peds
                Peds = World.GetAllPeds();
                // And set the next time to fetch
                NextFetch = Game.GameTime + 1000;
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

                // If the player just tried to fire a weapon
                if (Game.IsControlJustPressed(0, Control.Attack))
                {
                    // Disable the spell
                    Enabled = false;
                }
            }
        }

        private void OnAborted(object sender, EventArgs args)
        {
            // Disable the spell
            Enabled = false;
        }
    }
}
