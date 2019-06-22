using Citron;
using GTA;
using GTA.Math;
using GTA.Native;
using NAudio.Wave;
using System;
using System.Drawing;

namespace AnimeSpells.Konosuba
{
    /// <summary>
    /// The selected status of the explosion spell.
    /// </summary>
    public enum ExplosionStatus
    {
        Disabled = 0,
        Targeting = 1,
        Set = 2,
        Audio = 3,
        Firing = 4,
        Ragdoll = 5
    }

    /// <summary>
    /// EXPLOSION!
    /// https://www.youtube.com/watch?v=fIgm6lxvUfQ
    /// https://www.youtube.com/watch?v=O4tbOvKwZUw
    /// </summary>
    public class Explosion : Script
    {
        /// <summary>
        /// The value of the SFX Volume in the Audio Settings.
        /// Formulas: c / 10 (between 0-1) and c / 20 (between 0-0.5)
        /// </summary>
        private float Volume => Function.Call<int>(Hash.GET_PROFILE_SETTING, 300) / 20f;
        private WaveOutEvent Output = new WaveOutEvent();
        private AudioFileReader File = new AudioFileReader("scripts\\AnimeSpells\\Explosion.mp3");
        /// <summary>
        /// The current status of the explosion.
        /// </summary>
        public static ExplosionStatus Status = ExplosionStatus.Disabled;
        /// <summary>
        /// The next status of the targeting
        /// </summary>
        public ExplosionStatus NextStatus => (int)Status + 1 > (int)ExplosionStatus.Ragdoll ? ExplosionStatus.Disabled : (ExplosionStatus)((int)Status + 1);
        /// <summary>
        /// The position of the explosion.
        /// </summary>
        public Vector3 Position = Vector3.Zero;
        /// <summary>
        /// If the wants to use the Explosions until they are manually deactivated.
        /// </summary>
        public bool Permanent = false;
        /// <summary>
        /// A blip for the current marker.
        /// </summary>
        public Blip MarkerBlip = null;

        public Explosion()
        {
            // Add our tick event and abort events
            Tick += OnTick;
            Aborted += OnAborted;
            Output.PlaybackStopped += OnPlaybackStopped;
        }

        private void OnTick(object sender, EventArgs args)
        {
            // If one of the explosion cheats has been entered
            if (Screen.HasCheatBeenEntered("explosion") || Screen.HasCheatBeenEntered("exp"))
            {
                // Set the status to the next value
                Status = NextStatus;
            }
            // Ig the user wants to alterante the activation of the permanent mode
            if (Screen.HasCheatBeenEntered("explosion permanent") || Screen.HasCheatBeenEntered("exp perm"))
            {
                // Alternate the activation of the permanent explosions
                Permanent = !Permanent;
                // And notify the user just in case
                UI.Notify("Permanent explosions " + (Permanent ? "Enabled" : "Disabled") + "!");
                // If they have been enabled
                if (Permanent)
                {
                    // Set the mode to targeting
                    Status = ExplosionStatus.Targeting;
                }
                // Otherwise
                else
                {
                    // Set the mode to disabled
                    Status = ExplosionStatus.Disabled;
                }
            }

            // If the playback volume is not the desired one
            if (Output.Volume != Volume)
            {
                // Set the SFX volume as the NAudio volume
                Output.Volume = Volume;
            }

            // If the current status is not Disabled or the permanent mode is enabled
            if (Status != ExplosionStatus.Disabled || Permanent)
            {
                // If the player is using a weapon and is not on permanent mode
                if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && !Permanent)
                {
                    // Select the fists
                    Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                }

                // Disable the fire/attack control
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
                Game.DisableControlThisFrame(0, Control.Phone);
                // If the user pressed the open phone button while on the set phase
                if (Game.IsControlJustPressed(0, Control.Phone) && Status == ExplosionStatus.Set)
                {
                    // Return back to targeting
                    Status = ExplosionStatus.Targeting;
                }
                // If the user just used the disabled control and the current state is not audio or firing
                else if (Game.IsControlJustPressed(0, Control.Attack) && Status != ExplosionStatus.Audio && Status != ExplosionStatus.Firing)
                {
                    // Set the next status
                    Status = NextStatus;
                }
            }

            // If the current status is targeting
            if (Status == ExplosionStatus.Targeting)
            {
                // Update the explosion position
                Position = Raycasting.Position;
                // Create a green static marker
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 10), Color.LightGreen);
                // If there is not a blip created
                if (MarkerBlip == null)
                {
                    // Create a blip on the map
                    MarkerBlip = World.CreateBlip(Position);
                    // Set the color to green
                    MarkerBlip.Color = BlipColor.Green;
                    // Set the title to something more easy to find
                    MarkerBlip.Name = "Detonation";
                }
                // Set the position of the blip
                MarkerBlip.Position = Position;
            }
            // If the current status is targeted
            else if (Status == ExplosionStatus.Set)
            {
                // Draw a yellow rotating marker
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 10), Color.Yellow, false, false, 0, true, "", "", false);
                // Change the blip color to yellow
                MarkerBlip.Color = BlipColor.Yellow;
            }
            // If the current status is the explosion audio
            else if (Status == ExplosionStatus.Audio)
            {
                // Draw some markers arround the explosion location
                World.DrawMarker((MarkerType)27, Position, Vector3.Zero, Vector3.Zero, new Vector3(10, 10, 1), Color.OrangeRed, false, false, 0, true, "", "", false);
                // Change the blip color to red
                MarkerBlip.Color = BlipColor.Red;

                // If we are not playing the audio
                if (Output.PlaybackState != PlaybackState.Playing)
                {
                    // If the total time is the same as the current time (aka it has played once)
                    if (File.TotalTime == File.CurrentTime)
                    {
                        // Stop the output just in case
                        Output.Stop();
                        // Reset the current time to zero
                        File.CurrentTime = TimeSpan.Zero;
                    }
                    // Otherwise
                    else
                    {
                        // Initialize the playback of the file
                        Output.Init(File);
                    }

                    // Then play the explosion sound
                    Output.Play();
                }
            }
            // If the current status is firing
            else if (Status == ExplosionStatus.Firing)
            {
                // Create a blimp type explosion with a radius of 1000
                World.AddOwnedExplosion(Game.Player.Character, Position, ExplosionType.Blimp, 1000, 0, true, false);
                // Destroy the blip
                MarkerBlip.Remove();
                MarkerBlip = null;
                // If the permanent explosions are enabled
                if (Permanent)
                {
                    // Skip the ragdoll
                    Status = ExplosionStatus.Targeting;
                }
                // Otherwise
                else
                {
                    // Inmediately set the status to ragdoll
                    Status = ExplosionStatus.Ragdoll;
                }
            }
            // If the current status is ragdoll
            else if (Status == ExplosionStatus.Ragdoll)
            {
                // Ragdoll the player during 1ms
                Function.Call(Hash.SET_PED_TO_RAGDOLL, Game.Player.Character, 1, 1, 1, true, true, false);
            }
        }

        public void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            // If we are currently playing the audio
            if (Status == ExplosionStatus.Audio)
            {
                // Change the mode to firing
                Status = ExplosionStatus.Firing;
            }
        }

        public void OnAborted(object sender, EventArgs args)
        {
            // If the blip is not null
            if (MarkerBlip != null)
            {
                // Remove it from the map
                MarkerBlip.Remove();
            }
            // If the audio file is still loaded
            if (File != null)
            {
                // Stop the playback just in case
                Output.Stop();
                // Dispose the file
                File.Dispose();
            }
            // If the audio output is still available
            if (Output != null)
            {
                // Dispose the output
                Output.Dispose();
            }
        }
    }
}
