using GTA;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Drawing;
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

        public Manager()
        {
            // Over here we subscribe our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Calculate the values of the mana bar
            float Width = Config.ManaMax == 0 ? Config.ManaWidth + Config.ManaOffsetWidth : (Config.ManaWidth + Config.ManaOffsetWidth) * (Mana / Config.ManaMax);

            // Draw the mana bar
            // Background
            Function.Call(Hash.DRAW_RECT, Config.ManaX, Config.ManaY, Config.ManaWidth, Config.ManaHeight, Background.R, Background.G, Background.B, Background.A);
            // Foreground
            Function.Call(Hash.DRAW_RECT, Config.ManaX + Config.ManaOffsetX, Config.ManaY + Config.ManaOffsetY, Width, Config.ManaHeight + Config.ManaOffsetHeight, Foreground.R, Foreground.G, Foreground.B, Foreground.A);
        }
    }
}
