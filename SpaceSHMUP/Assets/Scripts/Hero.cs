// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Hero";
    #endregion

    #region Static
    public static Hero S = null;
    #endregion

    #region Public
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 360;
    public float shieldLevel = 1;
    public float gameRestartDelay = 2f;
    public Weapon[] weapons;

    [Header("For Debug View Only")]
    public Bounds bounds;
    public GameObject lastTriggerdGO = null;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch(pu.type)
        {
            case WeaponType.Shield:
                shieldLevel++;
                break;
            default:
                if(pu.type == weapons[0].Type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null) w.SetType(pu.type);
                    else
                    {
                        ClearWeapons();
                        weapons[0].SetType(pu.type);
                    }
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    #endregion

    #region Private
    private Weapon GetEmptyWeaponSlot()
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].Type == WeaponType.None) return weapons[i];
        }
        return null;
    }

    private void ClearWeapons()
    {
        foreach (Weapon w in weapons) w.SetType(WeaponType.None);
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
    public float ShieldLevel
    {
        get
        {
            return shieldLevel;
        }
        set
        {
            shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
    #endregion
    #endregion

    #region UnityFunctions
    public void OnTriggerEnter(Collider other)
    {
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        if (go != null)
        {
            if (go == lastTriggerdGO) return;
            lastTriggerdGO = go;

            if (go.tag == "Enemy")
            {
                ShieldLevel--;
                Destroy(go);
            }
            else if (go.tag == "PowerUp") AbsorbPowerUp(go);
            else PrintDebugMsg("Triggered by object with a tagged parent: " + go.name);
        }
        else PrintDebugMsg("Triggered by: " + other.gameObject.name);
    }
    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");

        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        ClearWeapons();
        weapons[0].SetType(WeaponType.Blaster);
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if(off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) fireDelegate();
    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}