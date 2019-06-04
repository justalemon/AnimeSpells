using GTA;
using GTA.Math;
using GTA.Native;

namespace AnimeSpells
{
    public static class Tools
    {
        /// <summary>
        /// Gets the correct position if the player is on foot or on a vehicle.
        /// </summary>
        public static Vector3 Position
        {
            get
            {
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    return Game.Player.Character.CurrentVehicle.Position;
                }
                else
                {
                    return Game.Player.Character.Position;
                }
            }
        }

        /// <summary>
        /// Gets the correct rotation if the player is on foot or on a vehicle.
        /// </summary>
        public static Vector3 Rotation
        {
            get
            {
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    return Game.Player.Character.CurrentVehicle.Rotation;
                }
                else
                {
                    return Game.Player.Character.Rotation;
                }
            }
        }

        public static Vector3 ShieldDiameter
        {
            get
            {
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    switch (Game.Player.Character.CurrentVehicle.ClassType)
                    {
                        case VehicleClass.Boats:
                            return new Vector3(10, 10, 10);
                        default:
                            return new Vector3(7, 7, 7);
                    }
                }
                else
                {
                    return new Vector3(2, 2, 2);
                }
            }
        }

        public static bool HasCheatBeenEntered(string cheat)
        {
            return Function.Call<bool>(Hash._0x557E43C447E700A8, Game.GenerateHash(cheat));
        }

        public static void ShowHelp(string message, int duration = 5000)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING"); // BEGIN_TEXT_COMMAND_DISPLAY_HELP
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, message); // ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME
            Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, false, true, duration); // END_TEXT_COMMAND_DISPLAY_HELP
        }

        
        public static bool IsFriendly(this Ped GamePed)
        {
            return Game.Player.Character.GetRelationshipWithPed(GamePed) <= Relationship.Like && GamePed.GetRelationshipWithPed(Game.Player.Character) <= Relationship.Like;
        }
    }
}
