using GTA;
using GTA.Math;

namespace AnimeSpells
{
    public static class Tools
    {
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
