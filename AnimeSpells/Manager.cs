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

        public Manager()
        {
            // Over here we subscribe our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Draw the mana bar
            // Background
            Function.Call(Hash.DRAW_RECT, Config.ManaX, Config.ManaY, Config.ManaWidth, Config.ManaHeight, Background.R, Background.G, Background.B, Background.A);
            // Foreground
            Function.Call(Hash.DRAW_RECT, Config.ManaX + Config.ManaOffsetX, Config.ManaY + Config.ManaOffsetY, Config.ManaWidth + Config.ManaOffsetWidth, Config.ManaHeight + Config.ManaOffsetHeight, Foreground.R, Foreground.G, Foreground.B, Foreground.A);
        }
    }
}
