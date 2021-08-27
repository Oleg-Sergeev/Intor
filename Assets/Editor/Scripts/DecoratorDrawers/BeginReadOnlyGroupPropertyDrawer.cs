using Assets.Scripts.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.DecoratorDrawers
{
    [CustomPropertyDrawer(typeof(BeginReadOnlyGroupAttribute))]
    public class BeginReadOnlyGroupPropertyDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0;

        public override void OnGUI(Rect position)
        {
            EditorGUI.BeginDisabledGroup(true);
        }
    }
}
