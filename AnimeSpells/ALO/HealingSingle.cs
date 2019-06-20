using Citron.Extensions;
using GTA;
using System;

namespace AnimeSpells.ALO
{
    /// <summary>
    /// Healing: Single Target (targeted)
    /// "Once the incantation ... is completed, ... water pour down around the caster from their hand, healing the players engulfed in the water."
    /// From https://swordartonline.fandom.com/wiki/Healing_Magic
    /// </summary>
    public class HealingSingle : Script
    {
        public HealingSingle()
        {
            // Over here we subscribe our events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Store the ped for the time being
            Ped Target = Raycasting.TargetedPed;

            // If the ped is null or is the player, return
            if (Target == null || Target == Game.Player.Character)
            {
                return;
            }

            // If the ped is friendly, the player tried to shot him and is not dead
            if (Game.IsControlJustPressed(0, Control.Attack) && Target.IsFriendly() && !Target.IsDead)
            {
                // Heal the ped
                Target.Health = Target.MaxHealth;
                // Notify the player
                UI.Notify("Ped Healed!");
            }
        }
    }
}
