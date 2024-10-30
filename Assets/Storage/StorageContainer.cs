using System.Collections.Generic;

namespace Habby.Storage {
	public partial class StorageContainer {
		public string PrefixedKey => _prefixedKey;
		public Dictionary<string, object>.KeyCollection Keys => _rawDict.Keys;

		private readonly string _prefixedKey;
		private readonly IStorageProvider _provider;
		private readonly IStorageSerializer _serializer;
		private readonly Dictionary<string, object> _rawDict;
		private readonly Dictionary<string, IStorageProperty> _propertyDict;

		public StorageContainer(string prefixedKey, IStorageProvider provider, IStorageSerializer serializer) {
			_prefixedKey = prefixedKey;
			_provider = provider;
			_serializer = serializer;
			_propertyDict = new Dictionary<string, IStorageProperty>();
			_rawDict = _serializer.Deserialize(provider.Get(prefixedKey), new Dictionary<string, object>());
		}

		public void Get<T>(string key, out T value, T defaultValue = default) {
			value = Get(key, defaultValue).Value;
		}

		public StorageProperty<T> Get<T>(string key, T defaultValue = default) {
			if (_propertyDict.ContainsKey(key) && _propertyDict[key] is StorageProperty<T> property) {
				return property;
			}

			_rawDict.TryGetValue(key, out var raw);
			property = new StorageProperty<T>(this, key, _serializer.Deserialize(raw, defaultValue));
			_propertyDict.Remove(key);
			_propertyDict.Add(key, property);
			return property;
		}

		public void Set<T>(string key, T value) {
			Get(key, value).Value = value;
		}

		public void UpdateAndSave<T>(string key, T value, T defaultValue = default) {
			StorageProperty<T> property = Get(key, defaultValue);
			property.Value = value;
			Save<T>(property.Key);
		}

		public void Delete(string key) {
			_rawDict.Remove(key);
			_propertyDict.Remove(key);
			_provider.Set(_prefixedKey, _serializer.Serialize(_rawDict));
		}

		public void Save<T>(string key) {
			var property = Get<T>(key);
			_rawDict.Remove(key);
			_rawDict.Add(key, _serializer.Serialize(property.Value));
			_provider.Set(_prefixedKey, _serializer.Serialize(_rawDict));
			_provider.Save();
		}

		public void Save() {
			foreach (var kv in _propertyDict) {
				var key = kv.Key;
				var property = kv.Value;
				_rawDict.Remove(key);
				_rawDict.Add(key, _serializer.Serialize(property.Value));
			}

			_provider.Set(_prefixedKey, _serializer.Serialize(_rawDict));
			_provider.Save();
		}

		public void Clear() {
			_rawDict.Clear();
			_propertyDict.Clear();
			_provider.Delete(_prefixedKey);
		}
	}
}