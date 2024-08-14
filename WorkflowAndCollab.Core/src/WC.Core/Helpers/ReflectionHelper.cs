// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ReflectionHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.Helpers.Interfaces;

	[DependencyInjectionDependency]
	public class ReflectionHelper : IReflectionHelper
	{
		private readonly IAssemblyHelper _assemblyHelper;

		public ReflectionHelper(
			IAssemblyHelper assemblyHelper) => _assemblyHelper = assemblyHelper;

		public virtual IEnumerable<Type> GetTypesAssignableFromBaseType<T>(
			bool excludeObjectsWithNoDefaultConstructor = false)
		{
			Type type = typeof(T);

			IList<Assembly> assemblies = _assemblyHelper.GetAssemblies();

			IEnumerable<Type> types = assemblies.SelectMany(a => a.GetTypes())
				.Where(t => type.IsAssignableFrom(t));

			if (excludeObjectsWithNoDefaultConstructor)
			{
				types = types.Where(t => t.GetConstructor(Type.EmptyTypes) != null);
			}

			return types;
		}

		public virtual T CreateInstance<T>() => CastObjectToGenericType<T>(CreateInstance(typeof(T)));

		public virtual object CreateInstance(
			Type type) => Activator.CreateInstance(type);

		public virtual T CastObjectToGenericType<T>(
			object obj)
		{
			if (obj is T)
			{
				return (T)obj;
			}

			if (null == obj)
			{
				return default(T);
			}

			Type type = typeof(T);

			type = GetUnderlyingType(type);

			return (T)Convert.ChangeType(
				obj,
				type);
		}

		public virtual object CastObjectToType(
			object obj,
			Type type) => InvokeGenericMethod(
			nameof(CastObjectToGenericType),
			this,
			new[] {type},
			new[] {obj});

		public virtual object InvokeGenericMethod<TMethodClass>(
			string methodName,
			TMethodClass methodClass,
			Type[] genericTypes,
			object[] parameters,
			BindingFlags? bindingFlags = null)
		{
			MethodInfo methodInfo;

			if (null == bindingFlags)
			{
				methodInfo = typeof(TMethodClass).GetMethod(methodName);
			}
			else
			{
				methodInfo = typeof(TMethodClass).GetMethod(
					methodName,
					(BindingFlags)bindingFlags);
			}

			if (null == methodInfo)
			{
				throw new ArgumentException(
					$"\"{methodName}\" not found in class {typeof(TMethodClass)} with BindingFlags = ({bindingFlags})",
					nameof(methodClass));
			}

			MethodInfo method = methodInfo.MakeGenericMethod(genericTypes);

			return method.Invoke(
				methodClass,
				parameters);
		}

		public virtual Type GetUnderlyingType(
			Type type)
		{
			Type nullableType = Nullable.GetUnderlyingType(type);

			if (null != nullableType)
			{
				type = nullableType;
			}

			return type;
		}

		public virtual bool IsComplexType(
			Type type)
		{
			type = GetUnderlyingType(type);

			TypeInfo typeInfo = type.GetTypeInfo();

			return !typeInfo.IsEnum &&
			       !typeInfo.IsPrimitive &&
			       type != typeof(string) &&
			       type != typeof(DateTime);
		}

		public virtual bool IsIEnumerableType(
			Type type) => null !=
			              type.GetTypeInfo()
				              .GetInterface("IEnumerable") &&
			              type != typeof(string);

		public virtual Type GetEnumerableSubType(
			Type type)
		{
			if (!IsIEnumerableType(type))
			{
				throw new InvalidOperationException(
					$"Type does not implement IEnumerable. ({type.Name})");
			}

			if (type.GetTypeInfo()
				.IsArray)
			{
				return type.GetElementType();
			}

			return type.GetGenericArguments()
				.Single();
		}
	}
}