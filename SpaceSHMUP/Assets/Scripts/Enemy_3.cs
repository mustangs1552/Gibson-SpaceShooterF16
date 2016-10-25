// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Enemy_3 : Enemy
{
    #region GlobalVareables
    [Header("Enemy_2 Variables")]

    #region Static

    #endregion

    #region Public
    public Vector3[] points;
    public float birthTime = 0;
    public float lifeTime = 0;
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

        Vector3 p01, p12;
        u = u - .2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        Pos = (1 - u) * p01 + u * p12;

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

        points[0] = Pos;

        float xMin = Utils.CamBounds.min.x + Main.S.enemySpawnPadding;
        float xMax = Utils.CamBounds.min.x - Main.S.enemySpawnPadding;

        Vector3 v;
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = Random.Range(Utils.CamBounds.min.y, 0);
        points[1] = v;

        v = Vector3.zero;
        v.y = Pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

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