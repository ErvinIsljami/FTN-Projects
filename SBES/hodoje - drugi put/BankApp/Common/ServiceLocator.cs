using System.Collections.Generic;

namespace Common
{
	public static class ServiceLocator
	{
		private static Dictionary<object, object> serviceContainer = new Dictionary<object, object>();

		private static Dictionary<object, object> objectContainer = new Dictionary<object, object>();

		public static void RegisterService<T>(T service)
		{
			serviceContainer[typeof(T)] = service;
		}

		public static void RegisterObject<T>(T obj)
		{
			objectContainer[typeof(T)] = obj;
		}

		public static T GetObject<T>()
		{
			return (T)objectContainer[typeof(T)];
		}

		public static T GetService<T>()
		{
			return (T)serviceContainer[typeof(T)];
		}
	}
}
