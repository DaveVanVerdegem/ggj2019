﻿using UnityEngine;
using UnityEditor;
 
[CustomPropertyDrawer(typeof(ConditionalEnumHideAttribute))]
public class ConditionalEnumHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalEnumHideAttribute condHAtt = (ConditionalEnumHideAttribute)attribute;
        int enumValue = GetConditionalHideAttributeResult(condHAtt, property);
 
        bool wasEnabled = GUI.enabled;
        bool result = ((condHAtt.EnumValue1 == enumValue) || (condHAtt.EnumValue2 == enumValue));
        if(condHAtt.Inverse) result = !result;

        GUI.enabled = result;
        if (!condHAtt.HideInInspector || result)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
 
        GUI.enabled = wasEnabled;
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalEnumHideAttribute condHAtt = (ConditionalEnumHideAttribute)attribute;
        int enumValue = GetConditionalHideAttributeResult(condHAtt, property);

        bool result = ((condHAtt.EnumValue1 == enumValue) || (condHAtt.EnumValue2 == enumValue));
        if (condHAtt.Inverse) result = !result;

        if (!condHAtt.HideInInspector || result)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private int GetConditionalHideAttributeResult(ConditionalEnumHideAttribute condHAtt, SerializedProperty property)
    {
        int enumValue = 0;

        SerializedProperty sourcePropertyValue = null;
        //Get the full relative property path of the sourcefield so we can have nested hiding
        if (!property.isArray)
        {
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
            sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            //if the find failed->fall back to the old system
            if (sourcePropertyValue == null)
            {
                //original implementation (doens't work with nested serializedObjects)
                sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
            }
        }
        else
        {
            //original implementation (doens't work with nested serializedObjects)
            sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
        }


        if (sourcePropertyValue != null)
        {
            enumValue = sourcePropertyValue.enumValueIndex;
        }
        else
        {
            //Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        return enumValue;
    }

}