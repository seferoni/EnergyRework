using StardewModdingAPI;
using StardewValley;
using System;

namespace EnergyRework.Common
{
    internal sealed class Utilities
    {
        internal static void ChangeEnergy(float energyChange)
        {
            Game1.player.Stamina = Math.Clamp(Game1.player.Stamina + energyChange, ModEntry.Config.EnergyFloor, GetMaximumEnergy());
        }

        internal static float GetEnergyLoss()
        {
            float energyLoss = ModEntry.Config.BaseEnergyLoss;

            if (Game1.player.running && Game1.player.isMoving())
            {
                energyLoss += ModEntry.Config.MovingEnergyOffset;
            }

            return energyLoss;
        }

        internal static float GetMaximumEnergy()
        {
            return Math.Min(Game1.player.MaxStamina, ModEntry.Config.SittingEnergyCeiling);
        }

        internal static bool IsGameStateViable()
        {
            if (!Context.IsPlayerFree)
            {
                return false;
            }

            if (Game1.isFestival())
            {
                return false;
            }

            if (Game1.player.swimming.Value)
            {
                return false;
            }

            return true;
        }

        internal static void UpdateEnergy()
        {
            if (!IsGameStateViable())
            {
                return;
            }

            if (Game1.player.IsSitting())
            {
                ChangeEnergy(ModEntry.Config.SittingEnergyOffset);
                return;
            }

            if (Game1.player.Stamina <= ModEntry.Config.EnergyFloor)
            {
                return;
            }

            ChangeEnergy(GetEnergyLoss());
        }
    }
}
