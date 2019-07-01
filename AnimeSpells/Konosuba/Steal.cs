using GTA;
using GTA.Native;
using System;

namespace AnimeSpells.Konosuba
{
    /// <summary>
    /// Steal (-ing pants)
    /// https://www.youtube.com/watch?v=LFYKJY1L2sQ
    /// </summary>
    public class Steal : Script
    {
        public Steal()
        {
            // Add your events
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // Disable the X key
            Game.DisableControlThisFrame(0, Control.VehicleDuck);

            // Save the targeted ped here
            Ped Target = Raycasting.TargetedPed;

            // If there is not a being pointed, the ped is dead or the ped is the player, return
            if (Target == null || Target.IsDead || Target == Game.Player.Character)
            {
                return;
            }

            // If the player pressed the 1 key
            if (Game.IsControlJustPressed(0, Control.VehicleDuck))
            {
                // If the ped does not has weapons or there is not enough mana
                if (Target.Weapons.BestWeapon.Hash == WeaponHash.Unarmed || Manager.Mana < 100)
                {
                    return;
                }

                // Steal the current weapon of the enemy
                // If we don't have the weapon
                if (!Game.Player.Character.Weapons.HasWeapon(Target.Weapons.Current.Hash))
                {
                    // Give the weapon and add the ammo
                    Game.Player.Character.Weapons.Give(Target.Weapons.Current.Hash, Target.Weapons.Current.Ammo, false, true);
                }
                // If we do have it
                else
                {
                    // Add the ammo
                    Game.Player.Character.Weapons[Target.Weapons.Current.Hash].Ammo += Target.Weapons.Current.Ammo;
                }

                // Reduce the mana by 100
                Manager.Mana -= 100;
                // Notify the user
                UI.Notify($"You stole a ~g~{Target.Weapons.Current.Hash}~s~!");
                // Remove the weapon from the ped, we are done
                Target.Weapons.Remove(Target.Weapons.Current.Hash);
            }
        }
    }
}
