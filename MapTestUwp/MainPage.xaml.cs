using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace MapTestUwp
{
	public sealed partial class MainPage : Page
	{
		private IDictionary<PjcStore, MapIcon> allStores = new Dictionary<PjcStore, MapIcon>();
		private IList<MapIcon> selectedIcons = new List<MapIcon>();

		public MainPage()
		{
			this.InitializeComponent();
			Loaded += MainPage_Loaded;
		}

		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			var random = new Random();
			for (int i = 0; i < 10; i++)
			{
				var store = new PjcStore(
					name: $"Store {i}",
					storeType: GetRandomStoreType(random),
					geopoint: new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition { Latitude = (random.NextDouble() * 180) - 90, Longitude = (random.NextDouble() * 360) - 180 })
				);
				var poi = new MapIcon
				{
					Location = store.Geopoint,
					NormalizedAnchorPoint = new Point(0.5, 1.0),
					Title = store.Name,
					ZIndex = 0,
				};
				myMap.MapElements.Add(poi);
				allStores.Add(store, poi);
			}
			myMap.MapElementClick += MyMap_MapElementClick;
		}

		private StoreType GetRandomStoreType(Random random)
		{
			return (StoreType)(random.Next() % 3);
		}

		private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
		{
			foreach (var icon in args.MapElements.OfType<MapIcon>())
			{
				if (selectedIcons.Contains(icon))
				{
					selectedIcons.Remove(icon);
					icon.Title = icon.Title.Substring(0, icon.Title.IndexOf(" - clicked"));
				}
				else
				{
					selectedIcons.Add(icon);
					icon.Title += " - clicked";
				}
			}
		}
	}
}
