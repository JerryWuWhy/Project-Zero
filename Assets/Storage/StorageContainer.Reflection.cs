using System;
using System.Collections.Generic;
using System.Reflection;

namespace Habby.Storage {
	public partial class StorageContainer {
		private static Type _refType = null;
		public static Type RefType => _refType ??= typeof(StorageContainer);

		private static MethodInfo _getMethodOrg = null;
		private static MethodInfo GetMethodOrg => _getMethodOrg ??= RefType.GetMethod(nameof(Get));
		private static readonly Dictionary<Type, MethodInfo> GetMethodReflections = new();
		public static MethodInfo Get_MethodReflection(Type type) {
			if (!GetMethodReflections.ContainsKey(type)) {
				GetMethodReflections[type] = GetMethodOrg.MakeGenericMethod(type);
			}
			return GetMethodReflections[type];
		}
	}
}