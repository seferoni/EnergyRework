using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace EnergyRework
{
	internal sealed class ModEntry : Mod
	{
		private readonly Dictionary<string, float> Parameters = new()
		{
			{"BaseEnergyLoss", -2f},
			{"EnergyFloor", 0f},
			{"MovingEnergyOffset", -3f},
			{"SittingEnergyOffset", 3f}
		};
		public override void Entry(IModHelper helper)
		{
			helper.Events.GameLoop.TimeChanged += this.TimeChanged;
		}

		private float GetEnergyLoss()
		{
			float energyLoss = this.Parameters["BaseEnergyLoss"];

			if (Game1.player.running && Game1.player.isMoving())
			{
				energyLoss += this.Parameters["MovingEnergyOffset"];
			}

			return energyLoss;
		}

		private bool IsGameStateViable()
		{
			if (!Context.IsPlayerFree)
			{
				return false;
			}

			if (Game1.isFestival())
			{
				return false;
			}

			return true;
		}

		private void ChangeEnergy(float energyChange)
		{
			Game1.player.Stamina = Math.Clamp(Game1.player.Stamina + energyChange, this.Parameters["EnergyFloor"], (float)Game1.player.MaxStamina);
		}

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			if (!IsGameStateViable())
			{
				return;
			}

			if (Game1.player.IsSitting())
			{
				this.ChangeEnergy(this.Parameters["SittingEnergyOffset"]);
				return;
			}

			this.ChangeEnergy(this.GetEnergyLoss());
		}
	}
}
