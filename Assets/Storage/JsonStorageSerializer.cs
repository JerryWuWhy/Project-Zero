using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Habby.Storage {
	public class JsonStorageSerializer : IStorageSerializer {
		private class ContractResolver : DefaultContractResolver {
			protected override IList<JsonProperty>
				CreateProperties(Type type, MemberSerialization memberSerialization) {
				var props = base.CreateProperties(type, memberSerialization);
				var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
				props = props.Where(prop => 
					fields.Any(field => field.Name == prop.PropertyName) ||
					properties.Any(property => property.Name == prop.PropertyName)).ToList();
				return props;
			}
		}

		public static readonly JsonSerializerSettings SerializerSettings = new() {
			ContractResolver = new ContractResolver()
		};

		public string Serialize(object obj) {
			return JsonConvert.SerializeObject(obj, SerializerSettings);
		}

		public T Deserialize<T>(object obj, T defaultValue = default) {
			if (obj is not string str || string.IsNullOrEmpty(str)) {
				return defaultValue;
			}

			try {
				return JsonConvert.DeserializeObject<T>(str, SerializerSettings);
			} catch (Exception) {
				return defaultValue;
			}
		}
	}
}