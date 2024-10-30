using System.Collections.Generic;
using Newtonsoft.Json;

namespace Habby.Storage {
	public static class Storage {
		public class Settings {
			public string defaultDatabase = "Storage";
			public string prefixSeparator = "$$";
#if ACTK_IS_HERE
			public IStorageProvider provider = new ObscuredPrefsStorageProvider();
#else
			public IStorageProvider provider = new PlayerPrefsStorageProvider();
#endif
			public IStorageSerializer serializer = new JsonStorageSerializer();
		}

		public static string Database => _settings.defaultDatabase;

		private static bool _initialized;
		private static Settings _settings;
		private static Dictionary<string, StorageContainer> _containers;
		private static StorageContainer _internalContainer;
		private static Dictionary<string, List<string>> _databases;

		public static void Init(Settings settings = null, bool force = false) {
			if (!_initialized || force) {
				_settings = settings ?? new Settings();
				_containers = new Dictionary<string, StorageContainer>();
				_internalContainer = new StorageContainer("StorageInternal", _settings.provider, _settings.serializer);
				_databases = _internalContainer.Get("Databases", new Dictionary<string, List<string>>());
				_initialized = true;
			}
		}

		public static List<string> GetDatabaseKeys() {
			Init();
			return new List<string>(_databases.Keys);
		}

		public static List<string> GetContainerKeys(string database) {
			Init();
			database ??= _settings.defaultDatabase;
			return _databases.GetValueOrDefault(database, new List<string>());
		}

		public static StorageContainer GetContainer(string containerKey, string database = null) {
			Init();
			database ??= _settings.defaultDatabase;

			var prefixedKey = database + _settings.prefixSeparator + containerKey;
			if (_containers.TryGetValue(prefixedKey, out var container)) {
				return container;
			}

			container = new StorageContainer(prefixedKey, _settings.provider, _settings.serializer);
			_containers.Add(prefixedKey, container);

			var containerKeys = GetContainerKeys(database);
			if (!containerKeys.Contains(containerKey)) {
				_databases[database] = containerKeys;
				containerKeys.Add(containerKey);
				_internalContainer.Save();
			}

			return container;
		}

		public static bool HasContainer(string containerKey, string database = null) {
			return GetContainerKeys(database).Contains(containerKey);
		}

		public static void Save(string containerKey, string database = null) {
			GetContainer(containerKey, database).Save();
		}

		public static void Clear(string containerKey, string database = null) {
			var container = GetContainer(containerKey, database);
			container.Clear();

			_containers.Remove(container.PrefixedKey);

			var containerKeys = GetContainerKeys(database);
			containerKeys.Remove(containerKey);
			_internalContainer.Save();
		}

		public static void SaveAll() {
			foreach (var (_, container) in _containers) {
				container.Save();
			}
		}

		public static void ClearAll() {
			Init();
			foreach (var (database, containerKeys) in _databases) {
				foreach (var containerKey in containerKeys) {
					GetContainer(containerKey, database).Clear();
				}
			}
			_databases.Clear();
			_containers.Clear();
			_internalContainer.Save();
		}

		public static void ClearAll(string database) {
			if (_databases.ContainsKey(database)) {
				foreach (var containerKey in _databases[database]) {
					var container = GetContainer(containerKey, database);
					container.Clear();
					_containers.Remove(container.PrefixedKey);
				}
				_databases.Remove(database);
			}
			_internalContainer.Save();
		}

		public static string ExportToJson() {
			Init();
			var obj = new Dictionary<string, object>();
			foreach (var (database, containerKeys) in _databases) {
				var databaseDict = new Dictionary<string, object>();
				obj[database] = databaseDict;
				foreach (var containerKey in containerKeys) {
					var container = GetContainer(containerKey, database);
					var propertyKeys = container.Keys;
					var containerDict = new Dictionary<string, object>();
					databaseDict[containerKey] = containerDict;
					foreach (var key in propertyKeys) {
						containerDict[key] = container.Get<object>(key).Value;
					}
				}
			}
			return JsonConvert.SerializeObject(obj, JsonStorageSerializer.SerializerSettings);
		}

		public static void ImportFromJson(string json) {
			var obj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, object>>>>(json);
			ClearAll();
			foreach (var (database, databaseDict) in obj) {
				foreach (var (containerKey, containerDict) in databaseDict) {
					var container = GetContainer(containerKey, database);
					foreach (var (propertyKey, propertyValue) in containerDict) {
						container.Set(propertyKey, propertyValue);
					}
					container.Save();
				}
			}
			_internalContainer.Save();
		}
	}
}