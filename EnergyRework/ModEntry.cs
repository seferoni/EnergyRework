using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace EnergyRework
{
	internal sealed class ModEntry : Mod
	{
		private ModConfig Config;
		public override void Entry(IModHelper helper)
		{
			this.Config = this.Helper.ReadConfig<ModConfig>();
			helper.Events.GameLoop.TimeChanged += this.TimeChanged;
		}

		private float GetEnergyLoss()
		{
			float energyLoss = this.Config.BaseEnergyLoss;

			if (Game1.player.running && Game1.player.isMoving())
			{
				energyLoss += this.Config.MovingEnergyOffset;
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

			if (Game1.player.swimming.Value)
			{
				return false;
			}

			return true;
		}

		private void ChangeEnergy(float energyChange)
		{
			Game1.player.Stamina = Math.Clamp(Game1.player.Stamina + energyChange, this.Config.EnergyFloor, (float)Game1.player.MaxStamina);
		}

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			if (!this.IsGameStateViable())
			{
				return;
			}

			if (Game1.player.IsSitting())
			{
				this.ChangeEnergy(this.Config.SittingEnergyOffset);
				return;
			}

			if (Game1.player.Stamina <= this.Config.EnergyFloor)
			{
				return;
			}

			this.ChangeEnergy(this.GetEnergyLoss());
		}
	}
}
