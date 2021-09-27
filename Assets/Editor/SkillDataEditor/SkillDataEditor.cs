using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
	private class AbilityCreationParams
	{
		public Type Type;
	}

	#region private-field
	private ReorderableList _skillList;
	private SerializedProperty _abilitiesProp;
	#endregion private-field

	#region public-method
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawDefaultInspector();

		_skillList.DoLayoutList();

		if (GUILayout.Button("Delete All Abilities"))
		{
			var path = AssetDatabase.GetAssetPath(target);
			UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
			for (int i = 0; i < assets.Length; i++)
			{
				if (assets[i] is SkillAbility)
				{
					DestroyImmediate(assets[i], true);
				}
			}
			_skillList.serializedProperty.ClearArray();
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		serializedObject.ApplyModifiedProperties();
	}
	#endregion public-method

	#region private-method
	private void OnEnable() 
	{
		_abilitiesProp = serializedObject.FindProperty("_abilities");
		
		_skillList = new ReorderableList(serializedObject, _abilitiesProp);

		_skillList.drawHeaderCallback = (rect) => 
		{
			EditorGUI.LabelField(rect, "Skill Abilities");
		};

		_skillList.onRemoveCallback = (l) =>
		{
			var obj = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue;
			if (obj != null)
			{
				AssetDatabase.RemoveObjectFromAsset(obj);

				DestroyImmediate(obj, true);

			}
			l.serializedProperty.DeleteArrayElementAtIndex(l.index);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			ReorderableList.defaultBehaviours.DoRemoveButton(l);

		};

		_skillList.drawElementCallback = (rect, index, isActive, isFocused) => 
		{
			var element = _abilitiesProp.GetArrayElementAtIndex(index);

			rect.y += 2;
			rect.width -= 10;
			rect.height = EditorGUIUtility.singleLineHeight;

			if (element.objectReferenceValue == null)
			{
				return;
			}
			var label = element.objectReferenceValue.name;
			EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);

			// Convert this element's data to a SerializedObject so we can iterate
			// through each SerializedProperty and render a PropertyField.
			var nestedObject = new SerializedObject(element.objectReferenceValue);

			// Loop over all properties and render them
			var prop = nestedObject.GetIterator();
			var y = rect.y;
			while (prop.NextVisible(true))
			{
				if (prop.name == "m_Script")
				{
					continue;
				}

				rect.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.PropertyField(rect, prop);
			}

			nestedObject.ApplyModifiedProperties();

			// Mark edits for saving
			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}

		};

		_skillList.elementHeightCallback = (index) => 
		{
			var baseProp = EditorGUI.GetPropertyHeight(
				_skillList.serializedProperty.GetArrayElementAtIndex(index), true);

			var additionalProps = (float)0;
			var element = _abilitiesProp.GetArrayElementAtIndex(index);
			if (element.objectReferenceValue != null)
			{
				var ability = new SerializedObject(element.objectReferenceValue);
				var prop = ability.GetIterator();
				while (prop.NextVisible(true))
				{
					// XXX: This logic stays in sync with loop in drawElementCallback.
					if (prop.name == "m_Script")
					{
						continue;
					}
					additionalProps += EditorGUIUtility.singleLineHeight;
				}
			}

			var spacingBetweenElements = EditorGUIUtility.singleLineHeight / 2;

			return baseProp + spacingBetweenElements + additionalProps;
		};

		var types = ReflectiveEnumerator.GetEnumerableOfType<SkillAbility>();
		_skillList.onAddDropdownCallback = (buttonRect, l) => 
		{
			var menu = new GenericMenu();
			foreach (var type in types)
			{
				menu.AddItem(
					new GUIContent(type.ToString()),
					false,
					addClickHandler,
					new AbilityCreationParams() { Type = type });
			}
			menu.ShowAsContext();
		};
	}

	private void addClickHandler(object dataObj)
	{
		// Make room in list
		var data = (AbilityCreationParams)dataObj;
		var index = _skillList.serializedProperty.arraySize;
		_skillList.serializedProperty.arraySize++;
		_skillList.index = index;
		var element = _skillList.serializedProperty.GetArrayElementAtIndex(index);

		// Create the new Ability
		var newAbility = CreateInstance(data.Type.Name);
		newAbility.name = data.Type.Name;

		// Add it to CardData
		var skillData = (SkillData)target;
		AssetDatabase.AddObjectToAsset(newAbility, skillData);
		AssetDatabase.SaveAssets();
		element.objectReferenceValue = newAbility;
		serializedObject.ApplyModifiedProperties();
	}
	#endregion private-method
}
