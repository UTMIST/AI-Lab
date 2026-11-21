using System.Diagnostics;
using UnityEngine;
using Unity.MLAgents;
using Debug = UnityEngine.Debug;

public class AcademyManager : MonoBehaviour
{
    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
    }
    
    void EnvironmentReset()
    {
        Debug.Log("Clearing");
    }
}
