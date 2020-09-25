using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace MapTestUwp
{
	public sealed partial class MainPage : Page
	{
		private IDictionary<MapIcon, PjcStore> storeForIcon = new Dictionary<MapIcon, PjcStore>();
		private MapIcon selectedIcon = default;
		private static readonly BasicGeoposition[] basicGeopositions = new BasicGeoposition[]
		{
			new BasicGeoposition() { Latitude = -60, Longitude = -160},
			new BasicGeoposition() { Latitude = 11, Longitude = -140},
			new BasicGeoposition() { Latitude = 58, Longitude = -120},
			new BasicGeoposition() { Latitude = -60, Longitude = 8},
			new BasicGeoposition() { Latitude = 2, Longitude = 10},
			new BasicGeoposition() { Latitude = 44, Longitude = 2},
			new BasicGeoposition() { Latitude = -44, Longitude = 120},
			new BasicGeoposition() { Latitude = 58, Longitude = 160},
			new BasicGeoposition() { Latitude = 88, Longitude = 155},
			new BasicGeoposition() { Latitude = 0, Longitude = 0},
		};
		private Dictionary<StoreType, Dictionary<bool, IRandomAccessStreamReference>> imageForStoreAndSelection = new Dictionary<StoreType, Dictionary<bool, IRandomAccessStreamReference>>();

		public MainPage()
		{
			this.InitializeComponent();
			Loaded += MainPage_Loaded;
		}

		private async void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			imageForStoreAndSelection.Add(StoreType.Bleu, new Dictionary<bool, IRandomAccessStreamReference>());
			imageForStoreAndSelection.Add(StoreType.Vert, new Dictionary<bool, IRandomAccessStreamReference>());
			imageForStoreAndSelection.Add(StoreType.Rouge, new Dictionary<bool, IRandomAccessStreamReference>());

			foreach(var storeType in imageForStoreAndSelection.Keys)
			{
				imageForStoreAndSelection[storeType][true] = await GetImageForStore(storeType, true);
				imageForStoreAndSelection[storeType][false] = await GetImageForStore(storeType, false);
			}

			for (int i = 0; i < 10; i++)
			{
				var store = new PjcStore(
					name: $"Store {i}",
					storeType: GetStoreType(i),
					geopoint: new Geopoint(basicGeopositions[i])
				);
				var poi = new MapIcon
				{
					Location = store.Geopoint,
					NormalizedAnchorPoint = new Point(0.5, 1.0),
					Title = store.Name,
					ZIndex = 0,
					Image = imageForStoreAndSelection[store.StoreType][false]
				};
				myMap.MapElements.Add(poi);
				storeForIcon.Add(poi, store);
			}
			myMap.MapElementClick += MyMap_MapElementClick;
			txtPinsSummary.Text = string.Join(Environment.NewLine, storeForIcon.Values.Select(store => $"{store.Name} - {store.StoreType}"));
		}

		private StoreType GetStoreType(int storeIndex)
		{
			return (StoreType)(storeIndex % 3);
		}

		private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
		{
			foreach (var icon in args.MapElements.OfType<MapIcon>())
			{
				var store = storeForIcon[icon];

				if (selectedIcon == icon)
				{
					// Unselect
					selectedIcon = null;
					icon.Image = imageForStoreAndSelection[store.StoreType][false];
				}
				else
				{
					// Unselect the other icon
					if (selectedIcon != null)
					{
						selectedIcon.Image = imageForStoreAndSelection[store.StoreType][false];
					}

					// Select
					selectedIcon = icon;
					icon.Image = imageForStoreAndSelection[store.StoreType][true];
				}
			}
		}

		private async Task<IRandomAccessStreamReference> GetImageForStore(StoreType store, bool isSelected)
		{
			// Cache these?
			string fileName = "";
			if (isSelected)
			{
				switch (store)
				{
					case StoreType.Bleu:
						fileName = "pin_bleu_selection_base.png";
						break;
					case StoreType.Vert:
						fileName = "pin_verte_selection_base.png";
						break;
					case StoreType.Rouge:
						fileName = "pin_rouge_selection_base.png";
						break;
				}
			}
			else
			{
				switch(store)
				{
					case StoreType.Bleu:
						fileName = "pin_bleu_normal_base.png";
						break;
					case StoreType.Vert:
						fileName = "pin_verte_normal_base.png";
						break;
					case StoreType.Rouge:
						fileName = "pin_rouge_normal_base.png";
						break;
				}
			}

			var imagePath = new Uri($"ms-appx:///Assets/PjcPins/{fileName}");
			return await StorageFile.GetFileFromApplicationUriAsync(imagePath);
		}
	}
}
