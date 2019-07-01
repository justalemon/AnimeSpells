using Citron;
using GTA;
using GTA.Native;
using System;

namespace AnimeSpells.Konosuba
{
    /// <summary>
    /// Concealment
    /// "A superior version of the Lurk skill. This grants ... invisibility for a very short span of time."
    /// From https://konosuba.fandom.com/wiki/Satou_Kazuma#Abilities
    /// </summary>
    public class Concealment : Script
    {
        /// <summary>
        /// The wanted level set previously to enabling the spell.
        /// </summary>
        private static int PreviousWanted = 0;
        /// <summary>
        /// Internal activation of the spell.
        /// </summary>
        private static bool InternalEnabled = false;
        /// <summary>
        /// Activation for the Concealment spell.
        /// </summary>
        public static bool Enabled
        {
            // Return the internal value
            get => InternalEnabled;
            set
            {
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

                    // If the player does not has enough mana
                    if (Manager.Mana < 500)
                    {
                        // Disable the spell and return
                        InternalEnabled = false;
                        return;
                    }

                    // If the player wanted level is set to anything other than zero
                    if (Game.Player.WantedLevel != 0)
                    {
                        // Save the current wanted level
                        PreviousWanted = Game.Player.WantedLevel;
                        // Set the current wanted to zero
                        Game.Player.WantedLevel = 0;
                        // And set a fake wanted level value
                        Function.Call(Hash.SET_FAKE_WANTED_LEVEL, PreviousWanted);
                    }

                    // Reduce the mana by 500
                    Manager.Mana -= 500;
                    // Set the internal value
                    InternalEnabled = true;
                    // Set the alpha of the player to 50 (to make it near invisible)
                    Game.Player.Character.Alpha = 50;
                    // And make him ignored by all other peds
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, true);
                }
                else
                {
                    // Set the spell to disabled
                    InternalEnabled = false;
                    // Reset the alpha to the normal value
                    Game.Player.Character.Alpha = 255;

                    // Make the player no longer ignored
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);

                    // If the previous wanted level is set to something other than zero
                    if (PreviousWanted != 0)
                    {
                        // Disable the wanted override
                        Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
                        // Restore the wanted level
                        Game.Player.WantedLevel = PreviousWanted;
                        // And set the previous wanted to zero
                        PreviousWanted = 0;
                    }

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
            Tick += OnTick;
            Aborted += OnAborted;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // If the cheat has been entered
            if (Screen.HasCheatBeenEntered("concealment") || Screen.HasCheatBeenEntered("con"))
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
            }
        }

        private void OnAborted(object sender, EventArgs args)
        {
            // Disable the spell
            Enabled = false;
        }
    }
}
