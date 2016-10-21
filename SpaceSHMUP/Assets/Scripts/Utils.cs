// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoundsTest
{
    center,
    onScreen,
    offScreen
}

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
        if (cam == null) cam = Camera.main;

        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;

        Vector3 center = (boundTLN + boundBRF) / 2f;
        camBounds = new Bounds(center, Vector3.zero);
        camBounds.Encapsulate(boundTLN);
        camBounds.Encapsulate(boundBRF);
    }

    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return BoundsInBoundsCheck(camBounds, bnd, test);
    }

    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        Vector3 pos = lilB.center;
        Vector3 off = Vector3.zero;

        switch(test)
        {
            case BoundsTest.center:
                if (bigB.Contains(pos)) return Vector3.zero;

                if (pos.x > bigB.max.x) off.x = pos.x - bigB.max.x;
                else if (pos.x < bigB.min.x) off.x = pos.x - bigB.min.x;
                if (pos.y > bigB.max.y) off.y = pos.y - bigB.max.y;
                else if (pos.y < bigB.min.y) off.y = pos.y - bigB.min.y;
                if (pos.z > bigB.max.z) off.z = pos.z - bigB.max.z;
                else if (pos.z < bigB.min.z) off.z = pos.z - bigB.min.z;

                break;
            case BoundsTest.onScreen:
                if (bigB.Contains(pos)) return Vector3.zero;

                if (pos.x > lilB.max.x) off.x = pos.x - lilB.max.x;
                else if (pos.x < lilB.min.x) off.x = pos.x - lilB.min.x;
                if (pos.y > lilB.max.y) off.y = pos.y - lilB.max.y;
                else if (pos.y < lilB.min.y) off.y = pos.y - lilB.min.y;
                if (pos.z > lilB.max.z) off.z = pos.z - lilB.max.z;
                else if (pos.z < lilB.min.z) off.z = pos.z - lilB.min.z;

                return off;
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);

                if (cMin || cMax) return Vector3.zero;

                if (lilB.min.x > bigB.max.x) off.x = lilB.min.x - bigB.max.x;
                else if (lilB.max.x < bigB.min.x) off.x = lilB.max.x - bigB.min.x;
                if (lilB.min.y > bigB.max.y) off.y = lilB.min.y - bigB.max.y;
                else if (lilB.max.y < bigB.min.y) off.y = lilB.max.y - bigB.min.y;
                if (lilB.min.z > bigB.max.z) off.z = lilB.min.z - bigB.max.z;
                else if (lilB.max.z < bigB.min.z) off.z = lilB.max.z - bigB.min.z;

                return off;
        }

        return Vector3.zero;
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