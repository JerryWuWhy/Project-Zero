namespace Habby.Storage {
	public interface IStorageSerializer {
		string Serialize(object obj);
		T Deserialize<T>(object obj, T defaultValue = default);
	}
}