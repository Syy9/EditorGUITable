using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ReflectionHelpers 
{


	public static bool FieldIsSerialized (FieldInfo fieldInfo)
	{
		return fieldInfo.IsPublic || fieldInfo.GetCustomAttributes (true).Any (att => att.ToString () == "UnityEngine.SerializeField");
	}


	public static Type GetType( string TypeName )
	{

		// Try Type.GetType() first. This will work with types defined
		// by the Mono runtime, in the same assembly as the caller, etc.
		var type = Type.GetType( TypeName );

		// If it worked, then we're done here
		if( type != null )
			return type;

		// If the TypeName is a full name, then we can try loading the defining assembly directly
		if( TypeName.Contains( "." ) )
		{

			// Get the name of the assembly (Assumption is that we are using 
			// fully-qualified type names)
			var assemblyName = TypeName.Substring( 0, TypeName.IndexOf( '.' ) );

			// Attempt to load the indicated Assembly
			var assembly = Assembly.Load( assemblyName );
			if( assembly == null )
				return null;

			// Ask that assembly to return the proper Type
			type = assembly.GetType( TypeName );
			if( type != null )
				return type;

		}

		// If we still haven't found the proper type, we can enumerate all of the 
		// loaded assemblies and see if any of them define the type
		var currentAssembly = Assembly.GetExecutingAssembly();
		var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
		foreach( var assemblyName in referencedAssemblies )
		{

			// Load the referenced assembly
			var assembly = Assembly.Load( assemblyName );
			if( assembly != null )
			{
				// See if that assembly defines the named type
				type = assembly.GetType( TypeName );
				if( type != null )
					return type;
			}
		}

		// The type just couldn't be found...
		return null;

	}

	public static List<string> GetElementsSerializedFields (SerializedProperty collectionProperty)
	{
		List<string> properties = new List<string>();
		// If the collection is not empty, use the first element
		if (collectionProperty.arraySize > 0)
		{
			
			string firstElementRelativePath = "Array.data[0]";
			string firstElementFullPath = collectionProperty.propertyPath + "." + firstElementRelativePath;

			SerializedProperty firstElement = collectionProperty.FindPropertyRelative (firstElementRelativePath);
			// If the elements are object references, get the properties of the target object type
			if (firstElement.propertyType == SerializedPropertyType.ObjectReference)
			{
				Debug.Log (firstElement.objectReferenceValue);
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
				foreach (SerializedProperty prop in firstElement)
				{
					string subPropName = prop.propertyPath.Substring(firstElementFullPath.Length + 1);
					// Avoid drawing properties more than 1 level deep
					if (!subPropName.Contains("."))
						properties.Add (subPropName);
				}
			}
		}
		// If the collection is empty, try to use reflection
		else
		{
			string elementType = collectionProperty.arrayElementType;
			// If the elements are object references, we use the target object type 
			if (elementType.StartsWith ("PPtr"))
				// The type for object references will be in the form "PPtr<ObjectType>", so we just extract the "ObjectType"
				elementType = elementType.Substring (6, elementType.Length - 7);
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
				.Where (ReflectionHelpers.FieldIsSerialized)
				.Select (pi => pi.Name)
				.ToList ();
			
		}

		return properties;

	}

}
