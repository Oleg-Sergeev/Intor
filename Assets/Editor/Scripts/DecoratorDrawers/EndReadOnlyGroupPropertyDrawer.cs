using Assets.Scripts.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.DecoratorDrawers
{
    [CustomPropertyDrawer(typeof(EndReadOnlyGroupAttribute))]
    public class EndReadOnlyGroupPropertyDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0;

        public override void OnGUI(Rect position)
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}
