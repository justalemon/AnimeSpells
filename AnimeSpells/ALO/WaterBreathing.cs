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
        private float UnderwaterTime = Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Game.Player);
        private bool InternalEnabled = false;
        public bool Enabled
        {
            get => InternalEnabled;
            set
            {
                if (value)
                {
                    UnderwaterTime = Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Game.Player);
                }
                else
                {
                    Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, UnderwaterTime);
                }

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
            if (Tools.HasCheatBeenEntered("waterbreathing") || Tools.HasCheatBeenEntered("breathing") || Tools.HasCheatBeenEntered("wb"))
            {
                // If the player is swiming underwater, return
                if (Game.Player.Character.IsSwimmingUnderWater)
                {
                    return;
                }

                // Alternate the activation of the spell
                Enabled = !Enabled;
            }

            if (Enabled)
            {
                Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, UnderwaterTime);
            }
        }
    }
}
