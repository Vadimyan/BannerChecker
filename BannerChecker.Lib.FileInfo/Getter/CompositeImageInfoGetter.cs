using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BannerChecker.Lib.FileInfo.Getter
{
	public class CompositeImageInfoGetter : IImageInfoGetter
	{
		private readonly Dictionary<string, IImageInfoGetter> _getters;

		public CompositeImageInfoGetter()
		{
			_getters = new Dictionary<string, IImageInfoGetter>();
			IEnumerable<Type> infoGetterTypes = GetImageInfoGetters();
			FillGetters(infoGetterTypes);
		}

		private void FillGetters(IEnumerable<Type> infoGetterTypes)
		{
			foreach (var infoGetter in from ig in infoGetterTypes
									   from attribute in ig.GetCustomAttributes<ImageInfoGetterAttribute>()
									   select new { attribute.FileExtrnsion, Instance = (IImageInfoGetter)Activator.CreateInstance(ig) })
			{
				_getters.Add(infoGetter.FileExtrnsion, infoGetter.Instance);
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
			return !string.IsNullOrEmpty(fileExtension) && _getters.ContainsKey(fileExtension) 
				? _getters[fileExtension].GetInfo(filePath) 
				: null;
		}
	}
}
