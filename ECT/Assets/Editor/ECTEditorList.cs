using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ECT.UnityEditor
{
    public class ECTEditorList
    {
        public ECTEditorList(string title, bool draggable, bool displayButtons)
        {
            Title = title;
            Draggable = draggable;
            DisplayButtons = displayButtons;
            DrawEvent = DrawElement;
        }

        public string Title { get; }
        public bool Draggable { get; }
        public bool DisplayButtons { get; }

        public Action<Rect, int, SerializedProperty> DrawEvent { get; set; }

        ReorderableList list;

        public void DrawList(Rect pos, SerializedProperty property, SerializedProperty listProperty)
        {
            if(list == null) PrepareList(property, listProperty);
            
            list.DoList(pos);
        }
        
        public void PrepareList (SerializedProperty property, SerializedProperty listProperty)
        {
            list = new(property.serializedObject, listProperty, Draggable, true, DisplayButtons, DisplayButtons)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    DrawEvent.Invoke(rect, index, listProperty);
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, Title);
                }
            };
        }

        public void DrawElement(Rect rect, int index, SerializedProperty listProperty) => EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index));

        public float? GetHeight() => list?.GetHeight();
    }
}