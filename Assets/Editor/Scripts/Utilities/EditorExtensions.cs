using UnityEditor;

namespace Assets.Editor.Scripts.Utilities
{
    public static class EditorExtensions
    {
        public static SerializedProperty FindPropertyByAutoPropertyName(this SerializedObject obj, string propName) 
            => obj.FindProperty($"<{propName}>k__BackingField");
    }
}
