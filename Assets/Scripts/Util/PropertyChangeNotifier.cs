using UnityEngine;
using UnityEngine.Events;

public class PropertyChangedEvent : UnityEvent<object, string, object, object> { }

[CreateAssetMenu(fileName = "New Property Change Notifier", menuName = "Variables/Property Change Notifier")]
public class PropertyChangeNotifier : ScriptableObject
{
    public PropertyChangedEvent propertyChangedEvent = new PropertyChangedEvent();
}

