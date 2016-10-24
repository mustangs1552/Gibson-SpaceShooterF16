// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Projectile";
    #endregion

    #region Static

    #endregion

    #region Public

    #endregion

    #region Private
    [SerializeField]
    private WeaponType type = WeaponType.None;
    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public void SetType(WeaponType eType)
    {
        type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(type);
        this.GetComponent<Renderer>().material.color = def.projectileColor;
    }
    #endregion

    #region Private
    private void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(this.GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero) Destroy(this.gameObject);
    }
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
    public WeaponType Type
    {
        get
        {
            return type;
        }
        set
        {
            SetType(value);
        }
    }
    #endregion
    #endregion

    #region UnityFunctions

    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");

        InvokeRepeating("CheckOffscreen", 2f, 2f);
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