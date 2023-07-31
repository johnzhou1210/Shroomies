using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityBoolEvent : UnityEvent<bool> { }
[System.Serializable] public class Unity2BoolEvent : UnityEvent<bool,bool> { }
[System.Serializable] public class UnityStringEvent : UnityEvent<string> { }
[System.Serializable] public class UnityStringBoolEvent : UnityEvent<string,bool> { }
[System.Serializable] public class UnityFloatEvent : UnityEvent<float> { }
[System.Serializable] public class UnityFloatBoolEvent : UnityEvent<float,bool> { }
[System.Serializable]  public class Unity2FloatEvent : UnityEvent<float, float> { }
[System.Serializable]  public class UnityIntEvent : UnityEvent<int> { }
[System.Serializable] public class UnityIntBoolEvent : UnityEvent<int, bool> { }
[System.Serializable] public class UnityUpgradeEvent : UnityEvent<Upgrade> { }
[System.Serializable] public class UnityUpgradeBoolEvent : UnityEvent<Upgrade, bool> { }
[System.Serializable] public class UnityBulletTypeEvent : UnityEvent<BulletType> { }
[System.Serializable] public class UnityBulletTypeBoolEvent : UnityEvent<BulletType, bool> { }

public class ParameterizedUnityEvents : MonoBehaviour
{
    
}
