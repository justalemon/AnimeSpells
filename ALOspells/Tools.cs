using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOspells
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
    }
}
