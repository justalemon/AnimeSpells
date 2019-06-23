using Citron;
using GTA;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace AnimeSpells
{
    public class Manager : Script
    {
        /// <summary>
        /// The general configuration of the mod.
        /// </summary>
        public static Configuration Config { get; } = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("scripts\\AnimeSpells.json"));
        /// <summary>
        /// Color for the background of the Mana bar.
        /// </summary>
        private Color Background = Color.FromArgb(220, 0, 0, 0);
        /// <summary>
        /// Color for the foreground of the Mana bar.
        /// </summary>
        private Color Foreground = Color.FromArgb(255, 73, 149, 182);
        /// <summary>
        /// The internal value of the mana.
        /// </summary>
        private static int PrivateMana = Config.ManaMax;
        /// <summary>
        /// The current points of mana.
        /// </summary>
        public static int Mana
        {
            get
            {
                // If the maximum of mana is set to zero
                if (Config.ManaMax == 0)
                {
                    // Return the max value of an int
                    return int.MaxValue;
                }
                // Otherwise, return the set value
                return PrivateMana;
            }
            set
            {
                // If the maximum of mana is set to zero
                if (Config.ManaMax == 0)
                {
                    // Don't set anything, just return
                    return;
                }
                // If the mana value is set to something over the maxiimum
                else if (value > Config.ManaMax)
                {
                    // Just set the maximum
                    PrivateMana = Config.ManaMax;
                }
                // Otherwise
                else
                {
                    // Just set the value
                    PrivateMana = value;
                }
            }
        }
        /// <summary>
        /// The next time that we need to update the mana count.
        /// </summary>
        private int NextTime = 0;

        public Manager()
        {
            // Over here we subscribe our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // If the player entered the mana cheat
            if (Screen.HasCheatBeenEntered("mana"))
            {
                // Ask for the user input
                string Input = Game.GetUserInput(30);

                // If the user input is valid and it can be parsed
                if (!string.IsNullOrWhiteSpace(Input) && int.TryParse(Input, out int Output))
                {
                    // Set the mana value to the parsed input
                    Mana = Output;
                }
            }

            // If the mana is under zero
            if (Mana < 0)
            {
                // Ragdoll the player during 1ms
                Function.Call(Hash.SET_PED_TO_RAGDOLL, Game.Player.Character, 1, 1, 1, true, true, false);
            }
            // If the mana is not at the maximum
            else if (Mana != Config.ManaMax && Game.GameTime >= NextTime)
            {
                // Increase the mana by one
                Mana += 1;
                // And set the next time
                NextTime = Game.GameTime + Config.ManaRegen;
            }

            // Calculate the values of the mana bar
            float Width = Config.ManaMax == 0 ? Config.BarWidth + Config.BarOffsetWidth : (Config.BarWidth + Config.BarOffsetWidth) * (Mana / (float)Config.ManaMax);

            // Draw the mana bar
            // Background
            Function.Call(Hash.DRAW_RECT, Config.BarX, Config.BarY, Config.BarWidth, Config.BarHeight, Background.R, Background.G, Background.B, Background.A);
            // Foreground
            Function.Call(Hash.DRAW_RECT, Config.BarX + Config.BarOffsetX, Config.BarY + Config.BarOffsetY, Width, Config.BarHeight + Config.BarOffsetHeight, Foreground.R, Foreground.G, Foreground.B, Foreground.A);

            // Draw the mana number
            Function.Call(Hash.SET_TEXT_FONT, (int)GTA.Font.ChaletLondon);
            Function.Call(Hash.SET_TEXT_SCALE, 1f, 0.3f);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, 255);
            Function.Call(Hash.SET_TEXT_DROP_SHADOW);
            Function.Call(Hash._SET_TEXT_ENTRY, "CELL_EMAIL_BCON");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, Mana.ToString(CultureInfo.InvariantCulture));
            Function.Call(Hash._DRAW_TEXT, Config.CountX, Config.CountY);
        }
    }
}
