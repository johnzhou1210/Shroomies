using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityBoolEvent : UnityEvent<bool> { }
[System.Serializable] public class UnityStringEvent : UnityEvent<string> { }
[System.Serializable]  public class Unity2FloatEvent : UnityEvent<float, float> { }
[System.Serializable]  public class UnityIntEvent : UnityEvent<int> { }
[System.Serializable] public class UnityUpgradeEvent : UnityEvent<Upgrade> { }

public class ParameterizedUnityEvents : MonoBehaviour
{
    
}
