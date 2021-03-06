﻿using GTA;
using NativeUI;
using System;
using System.Linq;

namespace AnimeSpells.Menu
{
    public class Menu : Script
    {
        #region Basics

        /// <summary>
        /// The Menu Pool that stores and manages all of our menus.
        /// </summary>
        public MenuPool Pool = new MenuPool();
        /// <summary>
        /// The Main Menu for changing the spells.
        /// </summary>
        public UIMenu MainMenu = new UIMenu("", "");

        #endregion

        #region ALO

        /// <summary>
        /// Item that toggles the Butterfly Shield (Single) spell.
        /// </summary>
        public UIMenuCheckboxItem ButterflyShield = new UIMenuCheckboxItem("ALO: Butterfly Shield (Single)", ALO.ButterflyShieldSingle.Enabled, "Toggles the activation of Butterfly Shield for a single target.");
        /// <summary>
        /// Item that toggles the Water Breathing spell.
        /// </summary>
        public UIMenuCheckboxItem WaterBreathing = new UIMenuCheckboxItem("ALO: Water Breathing", ALO.WaterBreathing.Enabled, "Toggles the activation of Water Breathing.~n~~n~Warning: This bypasses the underwater check.");

        #endregion

        #region Konosuba

        /// <summary>
        /// Item that toggles the Concealment spell.
        /// </summary>
        public UIMenuCheckboxItem Concealment = new UIMenuCheckboxItem("Konosuba: Concealment", Konosuba.Concealment.Enabled, "Toggles the activation of Concealment.");
        /// <summary>
        /// Item that changes the status of the Explosion spell.
        /// </summary>
        public UIMenuListItem Explosion = new UIMenuListItem("Konosuba: Explosion", Enum.GetValues(typeof(Konosuba.ExplosionStatus)).Cast<object>().ToList(), 0, "Changes the status of the Explosion spell.");

        #endregion

        public Menu()
        {
            // First, add our menu into the pool
            Pool.Add(MainMenu);
            // Then, add the items into the menu
            MainMenu.AddItem(ButterflyShield);
            MainMenu.AddItem(WaterBreathing);
            MainMenu.AddItem(Concealment);
            MainMenu.AddItem(Explosion);

            // Over here we subscribe our events
            Tick += OnTick;
            MainMenu.OnCheckboxChange += OnCheckboxChange;
            MainMenu.OnListChange += OnListChange;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // This is going to run every game tick

            // We need to process our menus (draw them and check for changes)
            Pool.ProcessMenus();

            // If the user pressed Z
            if (Game.IsControlJustPressed(0, Control.MultiplayerInfo))
            {
                // Alternate the visibility of the menu
                MainMenu.Visible = !MainMenu.Visible;
            }

            // If the menu is open
            if (MainMenu.Visible)
            {
                // Update the checkboxes and lists
                ButterflyShield.Checked = ALO.ButterflyShieldSingle.Enabled;
                WaterBreathing.Checked = ALO.WaterBreathing.Enabled;
                Concealment.Checked = Konosuba.Concealment.Enabled;
                Explosion.Index = (int)Konosuba.Explosion.Status;
            }
        }

        private void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem item, bool check)
        {
            // Check the item that has changed and set the correct activation
            if (item == ButterflyShield)
            {
                ALO.ButterflyShieldSingle.Enabled = check;
            }
            else if (item == WaterBreathing)
            {
                ALO.WaterBreathing.Enabled = check;
            }
            else if (item == Concealment)
            {
                Konosuba.Concealment.Enabled = check;
            }
        }

        private void OnListChange(UIMenu sender, UIMenuListItem item, int index)
        {
            // Set the correct status for the specified item
            if (item == Explosion)
            {
                Konosuba.Explosion.Status = (Konosuba.ExplosionStatus)index;
            }
        }
    }
}
