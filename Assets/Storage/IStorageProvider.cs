namespace Habby.Storage {
	public interface IStorageProvider {
		string Get(string key);
		void Set(string key, string value);
		void Delete(string key);
		void Save();
	}
}