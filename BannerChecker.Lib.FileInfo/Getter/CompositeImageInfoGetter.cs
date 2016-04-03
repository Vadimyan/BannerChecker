using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BannerChecker.Lib.FileInfo.Getter
{
	class CompositeImageInfoGetter : IImageInfoGetter
	{
		private readonly Dictionary<string, IImageInfoGetter> getters;

		public CompositeImageInfoGetter()
		{
			getters = new Dictionary<string, IImageInfoGetter>();
			IEnumerable<Type> infoGetterTypes = GetImageInfoGetters();
			FillGetters(infoGetterTypes);
		}

		private void FillGetters(IEnumerable<Type> infoGetterTypes)
		{
			foreach (var infoGetter in from ig in infoGetterTypes
									   from attribute in ig.GetCustomAttributes<ImageInfoGetterAttribute>()
									   select new { attribute.FileExtrnsion, Instance = (IImageInfoGetter)Activator.CreateInstance(ig) })
			{
				getters.Add(infoGetter.FileExtrnsion, infoGetter.Instance);
			}
		}

		private IEnumerable<Type> GetImageInfoGetters()
		{
			Type interfaceType = typeof(IImageInfoGetter);
			return GetType().Assembly.GetTypes().Where(x => x.IsClass && interfaceType.IsAssignableFrom(x));
		}

		public ImageInfo GetInfo(string filePath)
		{
			var fileExtension = Path.GetExtension(filePath);
			return !string.IsNullOrEmpty(fileExtension) && getters.ContainsKey(fileExtension) 
				? getters[fileExtension].GetInfo(filePath) 
				: null;
		}
	}
}
