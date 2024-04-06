using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace EnergyRework.Interfaces
{
	internal sealed class ConfigHelper
	{
		internal IGenericModConfigMenuApi API { get; set; }
		internal IManifest Mod { get; set; }
		internal ITranslationHelper TranslationHelper { get; set; }

		internal ConfigHelper(IGenericModConfigMenuApi apiInstance, IManifest modManifest, ITranslationHelper translationHelperInstance)
		{
			API = apiInstance;
			Mod = modManifest;
			TranslationHelper = translationHelperInstance;
		}

		internal void AddSimplifiedNumberSetting(string key, Func<float> getter, Action<float> setter, float min, float max, float interval)
		{	// TODO
			API.AddNumberOption(
				mod: Mod,
				name: () => TranslationHelper.Get(key),
				getValue: getter,
				setValue: setter,
				min: min,
				max: max,
				interval: interval
			);
		}

		private static Expression<Action<T>> CreateSetter<T>(Expression<Func<T>> getter)
		{
			var parameter = Expression.Parameter(typeof(T), "value");
			var body = Expression.Assign(getter.Body, parameter);
			var setter = Expression.Lambda<Action<T>>(body, parameter);
			return setter;
		}
    }
}
