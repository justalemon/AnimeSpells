using Citron;
using GTA;
using GTA.Native;
using System;

namespace AnimeSpells.ALO
{
    /// <summary>
    /// Water Breathing
    /// https://swordartonline.fandom.com/wiki/Water_Breathing
    /// </summary>
    public class WaterBreathing : Script
    {
        /// <summary>
        /// The internal player definition for stats.
        /// </summary>
        private static int PlayerId
        {
            get
            {
                switch (Game.Player.Character.Model.Hash)
                {
                    case -1692214353: // Franklin
                        return 0;
                    case 225514697: // Michael
                        return 1;
                    case -1686040670: // Trevor
                        return 2;
                    default:
                        return -1;
                }
            }
        }
        /// <summary>
        /// The value of the Lung Capacity stat.
        /// </summary>
        private static int LungCapacity
        {
            get
            {
                // Create some variables to store the output
                int Output = 25;
                // If you are playing with M, T or F
                if (PlayerId != -1)
                {
                    // Call the native and return the value of the lung capacity stat
                    unsafe
                    {
                        Function.Call<bool>(Hash.STAT_GET_INT, Game.GenerateHash($"SP{(int)PlayerId}_LUNG_CAPACITY"), &Output, 1);
                    }
                }
                // Finally return the value
                return Output;
            }
        }
        /// <summary>
        /// Internal activation of the spell.
        /// </summary>
        private static bool InternalEnabled = false;
        /// <summary>
        /// If the spell is enabled or disabled.
        /// </summary>
        public static bool Enabled
        {
            get => InternalEnabled;
            set
            {
                // If the spell has been enabled
                if (value)
                {
                    // If the player has enough mana
                    if (Manager.Mana >= 250)
                    {
                        // Reduce the mana by 250
                        Manager.Mana -= 250;
                        // Set the max time underwater to the maximum int value and notify the user
                        Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, (float)int.MaxValue);
                        Screen.ShowHelp("Water Breathing has been ~g~enabled~s~!");
                    }
                    // Otherwise
                    else
                    {
                        // Notify the user that he does not has enough mana point
                        Screen.ShowHelp("Not enough mana points to enabme Water Breathing!");
                    }
                }
                // Otherwise
                else
                {
                    // Set the max time underwater just like the stat and notify the user
                    Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, (float)LungCapacity);
                    Screen.ShowHelp("Water Breathing has been ~r~disabled~s~!");
                }
                // Set the value of the spell
                InternalEnabled = value;
            }
        }

        public WaterBreathing()
        {
            // Add the event
            Tick += OnTick;
        }

        public void OnTick(object sender, EventArgs args)
        {
            // If the user introduced one of the cheats
            if (Screen.HasCheatBeenEntered("waterbreathing") || Screen.HasCheatBeenEntered("breathing") || Screen.HasCheatBeenEntered("wb"))
            {
                // If the player is swiming underwater, notify about it and return
                if (Game.Player.Character.IsSwimmingUnderWater)
                {
                    Screen.ShowHelp("You can't enable or disable Water Breathing if you are underwater for safety reasons.");
                    return;
                }

                // Alternate the activation of the spell
                Enabled = !Enabled;
            }
        }
    }
}
