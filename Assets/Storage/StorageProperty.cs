namespace Habby.Storage {
	public class StorageProperty<T> : IStorageProperty {
		public StorageContainer Container { get; }
		public string Key { get; }
		public T Value { get; set; }

		object IStorageProperty.Value => Value;

		public StorageProperty(StorageContainer container, string key, T value) {
			Container = container;
			Key = key;
			Value = value;
		}

		public void UpdateAndSave(T value) {
			Value = value;
			Container.Save<T>(Key);
		}

		public void Save() {
			Container.Save<T>(Key);
		}

		public static implicit operator T(StorageProperty<T> property) {
			return property != null ? property.Value : default;
		}
	}
}