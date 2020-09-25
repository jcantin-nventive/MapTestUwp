using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MapTestUwp
{
	public class PjcStore
	{
		public PjcStore(string name, StoreType storeType, Geopoint geopoint)
		{
			Name = name;
			StoreType = storeType;
			Geopoint = geopoint;
		}

		public string Name { get; }

		public StoreType StoreType { get; }

		public Geopoint Geopoint { get; }
	}
}
