using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

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

            // Get the raycast of the crosshair
            RaycastResult Result = World.GetCrosshairCoordinates();

            // If there is not an entity being pointed, return
            if (Result.HitEntity == null)
            {
                return;
            }

            // If the entity is not a ped, return
            if (!(Result.HitEntity is Ped))
            {
                return;
            }

            // Create a ped
            Ped Target = (Ped)Result.HitEntity;

            // If the target ped is dead, return
            if (Target.IsDead)
            {
                return;
            }

            // If the target is the player ped, return
            if (Target == Game.Player.Character)
            {
                return;
            }

            // Create a marker on the top of the ped
            World.DrawMarker(MarkerType.UpsideDownCone, Target.Position + new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Cyan);

            // If the player pressed the 1 key
            if (Game.IsControlJustPressed(0, Control.VehicleDuck))
            {
                // If the ped does not has weapons
                if (Target.Weapons.BestWeapon.Hash == WeaponHash.Unarmed)
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

                // Notify the user
                UI.Notify($"You stole a ~g~{Target.Weapons.Current.Hash}~s~!");
                // Remove the weapon from the ped, we are done
                Target.Weapons.Remove(Target.Weapons.Current.Hash);
            }
        }
    }
}
