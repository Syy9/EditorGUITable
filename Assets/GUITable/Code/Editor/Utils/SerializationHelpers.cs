using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SerializationHelpers
{


	public static bool FieldIsSerialized (FieldInfo fieldInfo)
	{
		return fieldInfo.IsPublic || fieldInfo.GetCustomAttributes (true).Any (att => att.ToString () == "UnityEngine.SerializeField");
	}

	static SerializedProperty GetFirstNonNullElement (SerializedProperty collectionProperty)
	{

		if (collectionProperty.arraySize == 0)
			return null;

		SerializedProperty firstElement = collectionProperty.FindPropertyRelative (GetElementAtIndexRelativePath (0));
		// If the elements are object references, look for the first with non-null object reference
		if (firstElement.propertyType == SerializedPropertyType.ObjectReference)
		{
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				SerializedProperty elementI = collectionProperty.FindPropertyRelative (GetElementAtIndexRelativePath (i));
				if (elementI.objectReferenceValue != null)
					return elementI;
			}
		}
		// If the elements are direct serialized objects, just use the first element
		else
		{
			return firstElement;
		}
		return null;
	}

	public static List<string> GetElementsSerializedFields (SerializedProperty collectionProperty, out bool isObjectReferencesCollection)
	{
		isObjectReferencesCollection = false;

		List<string> properties = new List<string>();

		SerializedProperty firstElement = GetFirstNonNullElement (collectionProperty);

		// If the collection is not empty, use the first element
		if (firstElement != null)
		{
			// If the elements are object references, get the properties of the target object type
			if (firstElement.propertyType == SerializedPropertyType.ObjectReference)
			{
				isObjectReferencesCollection = true;
				SerializedProperty sp = new SerializedObject (firstElement.objectReferenceValue).GetIterator ();
				sp.Next (true);
				while (sp.NextVisible(false))
				{
					if (!sp.propertyPath.Contains(".") && sp.name != "m_Script")
					{
						properties.Add (sp.propertyPath);
					}
				}
			}
			// If the elements are direct serialized objects, just use its properties
			else
			{
				string firstElementFullPath = collectionProperty.propertyPath + "." + GetElementAtIndexRelativePath (0);

				foreach (SerializedProperty prop in firstElement)
				{
					string subPropName = prop.propertyPath.Substring(firstElementFullPath.Length + 1);
					// Avoid drawing properties more than 1 level deep
					if (!subPropName.Contains("."))
						properties.Add (subPropName);
				}
			}
		}
		// If there is no non-null element, try to use reflection
		else
		{
			string elementType = collectionProperty.arrayElementType;
			// If the elements are object references, we use the target object type 
			if (elementType.StartsWith ("PPtr"))
			{
				// The type for object references will be in the form "PPtr<ObjectType>", so we just extract the "ObjectType"
				elementType = elementType.Substring (6, elementType.Length - 7);
				isObjectReferencesCollection = true;
			}
			// Type.GetType will only search in a few assemblies, so we use this more complete function
			Type type = ReflectionHelpers.GetType (elementType);
			// arrayElementType will not tell us if the type is a nested type, so we try the nested types of the serializedObject
			// as this would be the most frequent use case
			if (type == null)
				type = collectionProperty.serializedObject.targetObject.GetType ().GetNestedType (elementType);
			// If all this doesn't work, there seems to be nothing we can do to find the element type
			if (type == null)
				return null;
			properties = type
				.GetFields (BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
				.Where (FieldIsSerialized)
				.Select (pi => pi.Name)
				.ToList ();

		}

		return properties;

	}

	public static string GetElementAtIndexRelativePath (int index)
	{
		return string.Format ("Array.data[{0}]", index);
	}
	
}
