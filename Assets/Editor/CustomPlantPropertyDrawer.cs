using UnityEngine;
using UnityEditor;
using System.Collections;

// Tell the RangeDrawer that it is a drawer for properties with the RangeAttribute.
//[CustomPropertyDrawer(typeof(PlantProperty))]
//public class CustomPlantPropertyDrawer : PropertyDrawer {

//    // Draw the property inside the given rect
//    override public void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//        base.OnGUI(position, property, label);

//        // First get the attribute since it contains the range for the slider
//        //PlantProperty pp = attribute as PlantProperty;

//        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
//        //if (property.propertyType == SerializedPropertyType.Float)
//        //    EditorGUI.Slider(position, property, range.min, range.max, label);
//        //else if (property.propertyType == SerializedPropertyType.Integer)
//        //    EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
//        //else
//        //    EditorGUI.LabelField(position, label.text, "Use Range with float or int.");

        
//    }
//}