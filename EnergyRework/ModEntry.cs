using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace EnergyRework
{
	internal sealed class ModEntry : Mod
	{
		private readonly Dictionary<string, float> Parameters = new()
		{
			{"BaseEnergyLoss", 1f},
			{"EnergyFloor", 0f},
			{"MovingEnergyOffset", 2f}
		};
		public override void Entry(IModHelper helper)
		{
			helper.Events.GameLoop.TimeChanged += this.TimeChanged;
		}

		private void ReduceEnergy(float energyLoss)
		{
			float currentStamina = Game1.player.Stamina;

			if (currentStamina <= this.Parameters["EnergyFloor"])
			{
				return;
			}

			Game1.player.Stamina = Math.Max(this.Parameters["EnergyFloor"], currentStamina - energyLoss);
		}

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			if (!Context.IsWorldReady)
			{
				return;
			}

			if (!Context.IsPlayerFree)
			{
				return;
			}

			if (Game1.paused)
			{
				return;
			}

			float energyLoss = this.Parameters["BaseEnergyLoss"];

			if (Game1.player.running && Game1.player.isMoving())
			{
				energyLoss += this.Parameters["MovingEnergyOffset"];
			}

			this.ReduceEnergy(energyLoss);
		}
	}
}
