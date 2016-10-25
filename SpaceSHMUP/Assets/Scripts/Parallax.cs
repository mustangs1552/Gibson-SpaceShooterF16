// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Parallax";
    #endregion

    #region Static

    #endregion

    #region Public
    public GameObject poi;
    public GameObject[] panels;
    public float scrollSpeed;
    public float motionMult;
    #endregion

    #region Private
    private float panelHt;
    private float depth;
    #endregion
    #endregion

    #region CustomFunction
    #region Static

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
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;

        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHt, depth);
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * .5f);

        if (poi != null) tX = -poi.transform.position.x * motionMult;

        panels[0].transform.position = new Vector3(tX, tY, depth);
        if (tY >= 0) panels[1].transform.position = new Vector3(tX, tY - panelHt, depth);
        else panels[1].transform.position = new Vector3(tX, tY + panelHt, depth);
    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}