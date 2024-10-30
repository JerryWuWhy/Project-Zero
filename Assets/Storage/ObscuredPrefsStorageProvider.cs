#if ACTK_IS_HERE
using CodeStage.AntiCheat.Storage;

namespace Habby.Storage {
	public class ObscuredPrefsStorageProvider : IStorageProvider {
		public string Get(string key) {
			return ObscuredPrefs.Get<string>(key);
		}

		public void Set(string key, string value) {
			ObscuredPrefs.Set(key, value);
		}

		public void Delete(string key) {
			ObscuredPrefs.DeleteKey(key);
		}

		public void Save() {
			ObscuredPrefs.Save();
		}
	}
}
#endif