using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace GameBridge.Data;

public static class ReflectionUtils
{
	public static void SetValue(this MemberInfo memberInfo, object targetObject, object value)
	{
		switch (memberInfo.MemberType)
		{
			case MemberTypes.Field:
				try
				{
					((FieldInfo)memberInfo).SetValue(targetObject, value);
				}
				catch(Exception e)
				{
					throw new SerializationException($"Could not set field {memberInfo.Name} on object of type {targetObject.GetType()}.", e);
				}
				break;

			case MemberTypes.Property:
				try
				{
					((PropertyInfo)memberInfo).SetValue(targetObject, value);
				}
				catch(Exception e)
				{
					throw new SerializationException($"Could not set property {memberInfo.Name} on object of type {targetObject.GetType()}.", e);
				}

				break;
			default:
				throw new SerializationException("MemberInfo must be a subtype of FieldInfo or PropertyInfo.");
		}
	}

	public static object? GetValue(this MemberInfo memberInfo, object targetObject)
	{
		switch (memberInfo.MemberType)
		{
			case MemberTypes.Field:
				try
				{
					memberInfo.GetValue(targetObject);
					return ((FieldInfo)memberInfo).GetValue(targetObject);
				}
				catch (Exception e)
				{
					throw new SerializationException($"Could not set field {memberInfo.Name} on object of type {targetObject.GetType()}.", e);
				}
				break;
			case MemberTypes.Property:
				try
				{
					return ((PropertyInfo)memberInfo).GetValue(targetObject);
				}
				catch (Exception e)
				{
					throw new SerializationException($"Could not set property {memberInfo.Name} on object of type {targetObject.GetType()}.", e);
				}
				break;
			default:
				throw new SerializationException("MemberInfo must be a subtype of FieldInfo or PropertyInfo.");
		}
	}

	public static Type? GetUnderlyingType(this MemberInfo memberInfo)
	{
		Type? memberType = null;

		switch (memberInfo.MemberType)
		{
			case MemberTypes.Property:
			{
				var property = (PropertyInfo)memberInfo;
				memberType = property.PropertyType;
				break;
			}
			case MemberTypes.Field:
			{
				var field = (FieldInfo)memberInfo;
				memberType = field.FieldType;
				break;
			}
		}

		return memberType;
	}

	public static object[] GetCustomAttributesWithInheritance(this MemberInfo memberInfo)
	{
		var attributes = memberInfo.GetCustomAttributes(true);
		return attributes;
	}
}