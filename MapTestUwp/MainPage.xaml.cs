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
				var poi = new MapIcon
				{
					Location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition { Latitude = (random.NextDouble() * 180) - 90, Longitude = (random.NextDouble() * 360) - 180 }),
					NormalizedAnchorPoint = new Point(0.5, 1.0),
					Title = $"Position {i}",
					ZIndex = 0,
				};
				myMap.MapElements.Add(poi);
			}
			myMap.MapElementClick += MyMap_MapElementClick;
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
