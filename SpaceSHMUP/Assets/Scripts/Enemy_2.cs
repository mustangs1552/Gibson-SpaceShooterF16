// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Enemy_2 : Enemy
{
    #region GlobalVareables
    [Header ("Enemy_1 Variables")]
    
    #region Static

    #endregion

    #region Public
    public Vector3[] points;
    public float birthTime = 0;
    public float lifeTime = 10;
    public float sinEccentricicty = .6f;
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
        float u = (Time.time - birthTime) / lifeTime;

        if(u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        u = u + sinEccentricicty * (Mathf.Sin(u * Mathf.PI * 2));

        Pos = (1 - u) * points[0] + u * points[1];

        base.Move();
    }
    #endregion

    #region Private

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
        
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        points = new Vector3[3];

        Vector3 cbMin = Utils.CamBounds.min;
        Vector3 cbMax = Utils.CamBounds.max;

        Vector3 v = Vector3.zero;
        v.x = cbMin.x - Main.S.enemySpawnPadding;
        v.y = Random.Range(cbMin.y, cbMin.y);
        points[0] = v;

        v = Vector3.zero;
        v.x = cbMax.x + Main.S.enemySpawnPadding;
        v.y = Random.Range(cbMin.y, cbMax.y);
        points[1] = v;

        if(Random.value < .5f)
        {
            points[0].x *= -1;
            points[1].x *= -1;
        }

        birthTime = Time.time;
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