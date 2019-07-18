using Citron;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnimeSpells.GabrielDropOut
{
    /// <summary>
    /// First Trumpet of the Apocalypse
    /// Not really a lot of information about it, but it should "Destroy the humanity" according to Gabriel DropOut.
    /// I ended up using the information of https://en.wikipedia.org/wiki/Seven_trumpets that might not be acurate.
    /// </summary>
    public class FirstTrumpetOfTheApocalypse : Script
    {
        /// <summary>
        /// List of IPLs that we are going to unload.
        /// </summary>
        private string[] IPLs = File.ReadAllLines("scripts\\AnimeSpells.IPL.txt");
        /// <summary>
        /// The IPLs that are currently loaded.
        /// </summary>
        private List<string> Loaded = null;

        public FirstTrumpetOfTheApocalypse()
        {
            // Add our events
            Tick += OnTick;
        }

        public void OnTick(object sender, EventArgs e)
        {
            // If the user has entered the correct cheat
            if (Screen.HasCheatBeenEntered("trumpet"))
            {
                // If the spell has not been activated
                if (Loaded == null)
                {
                    // Create a new list of loaded IPLs
                    Loaded = new List<string>();

                    // Iterate over the IPL files that we have
                    foreach (string IPL in IPLs)
                    {
                        // If the IPL is loaded
                        if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, IPL))
                        {
                            // Add the IPL to our list
                            Loaded.Add(IPL);
                            // And unload it
                            Function.Call(Hash.REMOVE_IPL, IPL);
                        }
                    }

                    // Set the player position to the current one with a couple of thoused on the air
                    Game.Player.Character.Position = new Vector3(0, 0, 2500);
                    // Disable the invencibility
                    Game.Player.Character.IsInvincible = false;
                    // Set the health to 1 (peds start from 100)
                    Game.Player.Character.Health = 101;
                }
                // Otherwise
                else
                {
                    // For every IPL that needs to be loaded
                    foreach (string IPL in Loaded)
                    {
                        // Request the IPL
                        Function.Call(Hash.REQUEST_IPL, IPL);
                    }

                    // Finally, set the list to null
                    Loaded = null;
                }
            }
        }
    }
}
