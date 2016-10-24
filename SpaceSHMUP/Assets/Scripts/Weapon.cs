// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public enum WeaponType
{
    None,
    Blaster,
    Spread,
    Phaser,
    Missle,
    Laser,
    Shield
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.None;
    public string letter = "";
    public Color color = Color.white;
    public GameObject projectilePrefab = null;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuosDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Weapon";
    #endregion

    #region Static
    public static Transform PROJECTILE_ANCHOR;
    #endregion

    #region Public
    [Header("For debug view only")]
    public WeaponDefinition def;
    public GameObject collar = null;
    public float lastShot = 0;
    #endregion

    #region Private
    [SerializeField]
    private WeaponType type = WeaponType.Blaster;
    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public void SetType(WeaponType wt)
    {
        type = wt;
        if (type == WeaponType.None)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else this.gameObject.SetActive(true);

        def = Main.GetWeaponDefinition(type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShot < def.delayBetweenShots) return;

        Projectile p;
        switch(type)
        {
            case WeaponType.Blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;
            case WeaponType.Spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, .9f, 0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(.2f, .9f, 0) * def.velocity;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if(transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.Type = type;
        lastShot = Time.time;
        return p;
    }
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

        collar = transform.Find("Collar").gameObject;
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        SetType(type);

        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject parentGO = transform.parent.gameObject;
        if (parentGO.tag == "Hero") Hero.S.fireDelegate += Fire;
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