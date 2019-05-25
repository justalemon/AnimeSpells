using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeSpells
{
    public abstract class BaseScript : Script
    {
        public abstract bool Enabled { get; set; }
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
