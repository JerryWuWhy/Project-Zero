using UnityEngine;

namespace Habby.Storage {
	public class PlayerPrefsStorageProvider : IStorageProvider {
		public string Get(string key) {
			return PlayerPrefs.GetString(key);
		}

		public void Set(string key, string value) {
			PlayerPrefs.SetString(key, value);
		}

		public void Delete(string key) {
			PlayerPrefs.DeleteKey(key);
		}

		public void Save() {
			PlayerPrefs.Save();
		}
	}
}