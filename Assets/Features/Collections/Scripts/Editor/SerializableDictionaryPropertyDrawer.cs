using System;
using UnityEditor;
using UnityEngine;

namespace Core.Collections
{
    [CustomPropertyDrawer(typeof(DrawableDictionary), true)]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var keysProp = property.FindPropertyRelative("m_keys");
            return (keysProp.arraySize + 2) * (EditorGUIUtility.singleLineHeight + 1f);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var expanded = property.isExpanded;
            var r = GetNextRect(ref position);
            property.isExpanded = EditorGUI.Foldout(r, property.isExpanded, label);

            if (expanded == false)
            {
                return;
            }

            var lvl = EditorGUI.indentLevel;
            EditorGUI.indentLevel = lvl + 1;

            var keysProp = property.FindPropertyRelative("m_keys");
            var valuesProp = property.FindPropertyRelative("m_values");

            var cnt = keysProp.arraySize;
            if (valuesProp.arraySize != cnt) valuesProp.arraySize = cnt;

            for (var i = 0; i < cnt; i++)
            {
                r = GetNextRect(ref position);
                //r = EditorGUI.IndentedRect(r);
                var w0 = EditorGUIUtility.labelWidth; // r.width / 2f;
                var w1 = r.width - w0;
                var r0 = new Rect(r.xMin, r.yMin, w0, r.height);
                var r1 = new Rect(r0.xMax, r.yMin, w1, r.height);

                var keyProp = keysProp.GetArrayElementAtIndex(i);
                var valueProp = valuesProp.GetArrayElementAtIndex(i);

                DrawKey(r0, keyProp);
                DrawValue(r1, valueProp);
            }

            EditorGUI.indentLevel = lvl;

            r = GetNextRect(ref position);
            var pRect = new Rect(r.xMax - 60f, r.yMin, 30f, EditorGUIUtility.singleLineHeight);
            var mRect = new Rect(r.xMax - 30f, r.yMin, 30f, EditorGUIUtility.singleLineHeight);

            if (GUI.Button(pRect, "+"))
            {
                AddKeyElement(keysProp);
                valuesProp.arraySize = keysProp.arraySize;
            }

            if (GUI.Button(mRect, "-"))
            {
                keysProp.arraySize = Mathf.Max(keysProp.arraySize - 1, 0);
                valuesProp.arraySize = keysProp.arraySize;
            }
        }

        protected virtual void DrawKey(Rect area, SerializedProperty keyProp)
        {
#if SP_LIB
            SPEditorGUI.PropertyField(area, keyProp, GUIContent.none, false);
#else
            EditorGUI.PropertyField(area, keyProp, GUIContent.none, false);
#endif
        }

        protected virtual void DrawValue(Rect area, SerializedProperty valueProp)
        {
#if SP_LIB
            SPEditorGUI.PropertyField(area, valueProp, GUIContent.none, false);
#else
            EditorGUI.PropertyField(area, valueProp, GUIContent.none, false);
#endif
        }


        private Rect GetNextRect(ref Rect position)
        {
            var r = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
            var h = EditorGUIUtility.singleLineHeight + 1f;
            position = new Rect(position.xMin, position.yMin + h, position.width, position.height = h);
            return r;
        }


        private static void AddKeyElement(SerializedProperty keysProp)
        {
            keysProp.arraySize++;
            var prop = keysProp.GetArrayElementAtIndex(keysProp.arraySize - 1);

            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    var value = 0;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).intValue == value)
                        {
                            value++;
                            if (value == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.intValue = value;
                }
                    break;
                case SerializedPropertyType.Boolean:
                {
                    var value = false;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).boolValue == value)
                        {
                            value = true;
                            break;
                        }

                    prop.boolValue = value;
                }
                    break;
                case SerializedPropertyType.Float:
                {
                    var value = 0f;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).intValue == value)
                        {
                            value++;
                            if (value == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.floatValue = value;
                }
                    break;
                case SerializedPropertyType.String:
                {
                    prop.stringValue = string.Empty;
                }
                    break;
                // case SerializedPropertyType.Color:
                // {
                //     var value = Color.black;
                //     for (var i = 0; i < keysProp.arraySize - 1; i++)
                //         if (keysProp.GetArrayElementAtIndex(i).colorValue == value)
                //         {
                //             value = ConvertUtil.ToColor(ConvertUtil.ToInt(value) + 1);
                //             if (value == Color.white)
                //                 break;
                //             i = -1;
                //         }
                //
                //     prop.colorValue = value;
                // }
                //     break;
                case SerializedPropertyType.ObjectReference:
                {
                    prop.objectReferenceValue = null;
                }
                    break;
                case SerializedPropertyType.LayerMask:
                {
                    var value = -1;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).intValue == value)
                        {
                            value++;
                            if (value == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.intValue = value;
                }
                    break;
                case SerializedPropertyType.Enum:
                {
                    var value = 0;
                    if (keysProp.arraySize > 1)
                    {
                        var first = keysProp.GetArrayElementAtIndex(0);
                        var max = first.enumNames.Length - 1;

                        for (var i = 0; i < keysProp.arraySize - 1; i++)
                            if (keysProp.GetArrayElementAtIndex(i).enumValueIndex == value)
                            {
                                value++;
                                if (value >= max)
                                    break;
                                i = -1;
                            }
                    }

                    prop.enumValueIndex = value;
                }
                    break;
                case SerializedPropertyType.Vector2:
                {
                    var value = Vector2.zero;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).vector2Value == value)
                        {
                            value.x++;
                            if (value.x == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.vector2Value = value;
                }
                    break;
                case SerializedPropertyType.Vector3:
                {
                    var value = Vector3.zero;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).vector3Value == value)
                        {
                            value.x++;
                            if (value.x == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.vector3Value = value;
                }
                    break;
                case SerializedPropertyType.Vector4:
                {
                    var value = Vector4.zero;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).vector4Value == value)
                        {
                            value.x++;
                            if (value.x == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.vector4Value = value;
                }
                    break;
                case SerializedPropertyType.Rect:
                {
                    prop.rectValue = Rect.zero;
                }
                    break;
                case SerializedPropertyType.ArraySize:
                {
                    var value = 0;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).arraySize == value)
                        {
                            value++;
                            if (value == int.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.arraySize = value;
                }
                    break;
                case SerializedPropertyType.Character:
                {
                    var value = 0;
                    for (var i = 0; i < keysProp.arraySize - 1; i++)
                        if (keysProp.GetArrayElementAtIndex(i).intValue == value)
                        {
                            value++;
                            if (value == char.MaxValue)
                                break;
                            i = -1;
                        }

                    prop.intValue = value;
                }
                    break;
                case SerializedPropertyType.AnimationCurve:
                {
                    prop.animationCurveValue = null;
                }
                    break;
                case SerializedPropertyType.Bounds:
                {
                    prop.boundsValue = default;
                }
                    break;
                default:
                    throw new InvalidOperationException("Can not handle Type as key.");
            }
        }

        private static void SetPropertyDefault(SerializedProperty prop)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    prop.intValue = 0;
                    break;
                case SerializedPropertyType.Boolean:
                    prop.boolValue = false;
                    break;
                case SerializedPropertyType.Float:
                    prop.floatValue = 0f;
                    break;
                case SerializedPropertyType.String:
                    prop.stringValue = string.Empty;
                    break;
                case SerializedPropertyType.Color:
                    prop.colorValue = Color.black;
                    break;
                case SerializedPropertyType.ObjectReference:
                    prop.objectReferenceValue = null;
                    break;
                case SerializedPropertyType.LayerMask:
                    prop.intValue = -1;
                    break;
                case SerializedPropertyType.Enum:
                    prop.enumValueIndex = 0;
                    break;
                case SerializedPropertyType.Vector2:
                    prop.vector2Value = Vector2.zero;
                    break;
                case SerializedPropertyType.Vector3:
                    prop.vector3Value = Vector3.zero;
                    break;
                case SerializedPropertyType.Vector4:
                    prop.vector4Value = Vector4.zero;
                    break;
                case SerializedPropertyType.Rect:
                    prop.rectValue = Rect.zero;
                    break;
                case SerializedPropertyType.ArraySize:
                    prop.arraySize = 0;
                    break;
                case SerializedPropertyType.Character:
                    prop.intValue = 0;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    prop.animationCurveValue = null;
                    break;
                case SerializedPropertyType.Bounds:
                    prop.boundsValue = default;
                    break;
                case SerializedPropertyType.Gradient:
                    throw new InvalidOperationException("Can not handle Gradient types.");
            }
        }
    }
}