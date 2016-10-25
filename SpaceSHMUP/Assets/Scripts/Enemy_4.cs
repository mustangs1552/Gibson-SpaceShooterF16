// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Part
{
    public string name = "";
    public float health = 10;
    public string[] protectedBy;

    public GameObject go = null;
    public Material mat = null;
}

public class Enemy_4 : Enemy
{
    #region GlobalVareables
    [Header("Enemy_4 variables")]

    #region Static

    #endregion

    #region Public
    public Vector3[] points;
    public float timeStart = 0;
    public float duration = 4;
    public Part[] parts;
    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if(u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);

        Pos = (1 - u) * points[0] + u * points[1];

        base.Move();
    }
    #endregion

    #region Private
    private void InitMovement()
    {
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.enemySpawnPadding;
        Bounds cBounds = Utils.CamBounds;
        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        points[0] = points[1];
        points[1] = p1;

        timeStart = Time.time;
    }

    private Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if (prt.name == n) return prt;
        }
        return null;
    }
    private Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go) return prt;
        }
        return null;
    }

    private bool Destroyed(string n)
    {
        return Destroyed(FindPart(n));
    }
    private bool Destroyed(GameObject go)
    {
        return Destroyed(FindPart(go));
    }
    private bool Destroyed(Part prt)
    {
        if (prt == null) return true;
        return prt.health <= 0;
    }

    private void ShowLocalizedDamage(Material mat)
    {
        mat.color = Color.red;
        remainingDamageFrames = showDamageForFrames;
    }
    #endregion

    #region Getters_Setters

    #endregion
    #endregion

    #region UnityFunctions
    public void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                bounds.center = transform.position + boundsCenterOffset;
                if(bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }

                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                if(prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        if(!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }
                prtHit.health -= Main.W_DEFS[p.Type].damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0) prtHit.go.SetActive(false);
                bool allDestroyed = true;
                foreach(Part prt in parts)
                {
                    if(!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if(allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        points = new Vector3[2];
        points[0] = Pos;
        points[1] = Pos;

        InitMovement();

        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);
            if(t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        Move();
    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}