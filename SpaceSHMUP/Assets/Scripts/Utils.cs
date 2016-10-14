// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Utils";
    #endregion

    #region Static
    private static Bounds camBounds;
    #endregion

    #region Public

    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static
    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        if (b0.size == Vector3.zero && b1.size != Vector3.zero) return b1;
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero) return b0;
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero) return b0;

        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return b0;
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);

        if (go.GetComponent<Renderer>() != null) b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        if (go.GetComponent<Collider>() != null) b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        foreach (Transform t in go.transform) b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));

        return b;
    }

    public static Bounds CamBounds
    {
        get
        {
            if (camBounds.size == Vector3.zero) SetCameraBounds();
            return camBounds;
        }
    }

    public static void SetCameraBounds(Camera cam = null)
    {

    }
    #endregion

    #region Public

    #endregion

    #region Private

    #endregion

    #region Debug
    private void PrintDebugMsg(string msg)
    {
        if (isDebug) Debug.Log(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintWarningDebugMsg(string msg)
    {
        Debug.LogWarning(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintErrorDebugMsg(string msg)
    {
        Debug.LogError(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    #endregion

    #region Getters_Setters

    #endregion
    #endregion

    #region UnityFunctions

    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {

    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {

    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}