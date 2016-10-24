// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Enemy";
    #endregion

    #region Static

    #endregion

    #region Public
    public float speed = 10f;
    public float fireRate = .3f;
    public float health = 10;
    public int score = 100;
    public int showDamageForFrames = 2;

    [Header("For Debug View Only")]
    public Color[] originalColors;
    public Material[] materials;
    public int remainingDamageFrames = 0;
    public Bounds bounds;
    public Vector3 boundsCenterOffset;
    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public virtual void Move()
    {
        Vector3 temPos = Pos;
        temPos.y -= speed * Time.deltaTime;
        Pos = temPos;
    }
    #endregion

    #region Private
    private void CheckOffscreen()
    {
        if(bounds.size == Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if(off != Vector3.zero)
        {
            if (off.y < 0) Destroy(this.gameObject);
        }
    }

    private void ShowDamage()
    {
        foreach (Material m in materials) m.color = Color.red;
        remainingDamageFrames = showDamageForFrames;
    }
    private void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++) materials[i].color = originalColors[i];
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
    public Vector3 Pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }
    #endregion
    #endregion

    #region UnityFunctions
    public void OnCollisionEnter(Collision col)
    {
        PrintDebugMsg("Collided: " + col.gameObject.name);
        GameObject other = col.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }
                ShowDamage();
                health -= Main.W_DEFS[p.Type].damageOnHit;
                if (health <= 0) Destroy(this.gameObject);
                Destroy(other);
                break;
        }
    }
    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) originalColors[i] = materials[i].color;
        InvokeRepeating("CheckOffscreen", 0f, 2f);
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
        Move();
        if(remainingDamageFrames > 0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0) UnShowDamage();
        }
    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}