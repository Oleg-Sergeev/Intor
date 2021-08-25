using System;
using Assets.Scripts.Components.Triggers;
using UnityEditor;
using static Assets.Scripts.Components.Triggers.TriggerComponent;

[CanEditMultipleObjects]
[CustomEditor(typeof(TriggerComponent), true)]
public class TriggerComponentEditor : Editor
{
    private SerializedProperty _triggerType;
    private SerializedProperty _colliderType;
    private SerializedProperty _targetCollider;
    private SerializedProperty _targetColliders;
    private SerializedProperty _onTriggered;
    private SerializedProperty _onTriggerEnter;
    private SerializedProperty _onTriggerExit;


    private void OnEnable()
    {
        _triggerType = serializedObject.FindProperty(nameof(_triggerType));
        _colliderType = serializedObject.FindProperty(nameof(_colliderType));
        _targetCollider = serializedObject.FindProperty(nameof(_targetCollider));
        _targetColliders = serializedObject.FindProperty(nameof(_targetColliders));
        _onTriggered = serializedObject.FindProperty(nameof(_onTriggered));
        _onTriggerEnter = serializedObject.FindProperty(nameof(_onTriggerEnter));
        _onTriggerExit = serializedObject.FindProperty(nameof(_onTriggerExit));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.PropertyField(_triggerType);

        if (_triggerType.enumValueIndex == (int)TriggerType.OnTrigger)
        {
            EditorGUILayout.PropertyField(_colliderType);

            switch ((ColliderCondition)_colliderType.enumValueIndex)
            {
                case ColliderCondition.AnyCollider: 
                    break;
                case ColliderCondition.OneCollider:
                    EditorGUILayout.PropertyField(_targetCollider); 
                    break;
                case ColliderCondition.CollidersListAny:
                case ColliderCondition.CollidersListAll:
                    EditorGUILayout.PropertyField(_targetColliders);
                    break;
                default: throw new ArgumentException($"Unknown ColliderType enum value \"{_colliderType}\"", nameof(_colliderType));
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_onTriggerEnter);
            EditorGUILayout.PropertyField(_onTriggerExit);
        }
        else
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_onTriggered);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
