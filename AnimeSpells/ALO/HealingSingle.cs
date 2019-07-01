using Citron.Extensions;
using GTA;
using GTA.Math;
using System;
using System.Drawing;

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

            // If the ped is friendly, is not dead and the health is not at the maximum
            if (Target.IsFriendly() && !Target.IsDead && Target.GetMaxHealth() != Target.GetHealth())
            {
                // Create a marker on the top of it
                World.DrawMarker(MarkerType.UpsideDownCone, Target.Position + new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Orange);

                // If the player tried to shot him and the required mana is available
                if (Game.IsControlJustPressed(0, Control.Attack) && Manager.Mana >= 150)
                {
                    // Heal the ped
                    Target.SetHealth(Target.GetMaxHealth());
                    // Notify the user
                    UI.Notify("Your friend was healed!");
                    // And reduce the mana
                    Manager.Mana -= 150;
                }
            }
        }
    }
}
