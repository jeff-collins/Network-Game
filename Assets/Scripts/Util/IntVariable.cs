using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IntVariableEvent : UnityEvent<int, int> { }

[CreateAssetMenu(fileName = "New Int Variable", menuName = "Variables/Int Variable")]
public class IntVariable : ScriptableObject
{
    int m_Value;
    [SerializeField] int m_DefaultValue;

    [SerializeField] bool m_ClampValue;
    [SerializeField] int m_MinimumValue;
    [SerializeField] int m_MaximumValue;

    [SerializeField] IntVariableEvent m_OnValueChanged = new IntVariableEvent();
    [SerializeField] IntVariableEvent m_onValueReset = new IntVariableEvent();

    public int value
    {
        get { return m_Value; }
        set
        {
            int previousValue = m_Value;
            m_Value = value;

            if (m_ClampValue)
            {
                m_Value = Mathf.Clamp(m_Value, m_MinimumValue, m_MaximumValue);
            }

            if (Application.isPlaying)
            {
                m_OnValueChanged.Invoke(m_Value, previousValue);
            }
        }
    }

    public int defaultValue
    {
        get { return m_DefaultValue; }
        set { m_DefaultValue = value; }
    }

    public bool clamp
    {
        get { return m_ClampValue; }
        set { m_ClampValue = value; }
    }

    public int minimumValue
    {
        get { return m_MinimumValue; }
        set { m_MinimumValue = value; }
    }

    public int maximumValue
    {
        get { return m_MaximumValue; }
        set { m_MaximumValue = value; }
    }

    public IntVariableEvent onValueChange
    {
        get { return m_OnValueChanged; }
    }

    public IntVariableEvent onValueReset
    {
        get { return m_onValueReset; }
    }

    public void ResetValue()
    {
        int previousValue = m_Value;
        m_Value = m_DefaultValue;
        m_onValueReset.Invoke(m_Value, previousValue);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(IntVariable))]
public class IntVariableInspector : Editor
{
    IntVariable variable = null;

    void OnEnable()
    {
        variable = (IntVariable)target;
    }

    public override void OnInspectorGUI()
    {
        variable.value = EditorGUILayout.IntField("Value", variable.value);
        variable.defaultValue = EditorGUILayout.IntField("Default Value", variable.defaultValue);

        variable.clamp = EditorGUILayout.Toggle("Clamp Value", variable.clamp);

        EditorGUI.BeginDisabledGroup(!variable.clamp);

        variable.minimumValue = EditorGUILayout.IntField("Minimum Value", variable.minimumValue);
        variable.maximumValue = EditorGUILayout.IntField("Maximum Value", variable.maximumValue);

        EditorGUI.EndDisabledGroup();
    }
}

#endif