// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IReflectionHelper.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Helpers;

using System.Reflection;

public interface IReflectionHelper
{
	IEnumerable<Type> GetTypesAssignableFromBaseType<T>(
		bool excludeObjectsWithNoDefaultConstructor = false);

	T CreateInstance<T>();

	object CreateInstance(
		Type type);

	T CastObjectToGenericType<T>(
		object obj);

	object CastObjectToType(
		object obj,
		Type type);

	object InvokeGenericMethod<TMethodClass>(
		string methodName,
		TMethodClass methodClass,
		Type[] genericTypes,
		object[] parameters,
		BindingFlags? bindingFlags = null);

	Type GetUnderlyingType(
		Type type);

	bool IsComplexType(
		Type type);

	bool IsIEnumerableType(
		Type type);

	Type GetEnumerableSubType(
		Type type);
}