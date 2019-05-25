using GTA;
using System;

namespace AnimeSpells
{
    public abstract class BaseScript : Script
    {
        public bool Enabled { get; set; } = false;
        public abstract string Cheat { get; }

        public BaseScript()
        {
            Tick += OnBaseTick;
        }

        public void OnBaseTick(object sender, EventArgs args)
        {
            // If the cheat has been entered
            if (Tools.HasCheatBeenEntered(Cheat))
            {
                if (Enabled)
                {
                    DisableSpell();
                }
                else
                {
                    EnableSpell();
                }
            }
        }

        public abstract void EnableSpell();
        public abstract void ExecuteSpell();
        public abstract void DisableSpell();
    }
}
