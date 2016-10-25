// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public class Enemy_1 : Enemy
{
    #region GlobalVareables
    [Header ("Enemy_0 values")]

    #region Static

    #endregion

    #region Public
    public float waveFrequency = 2;
    public float waveWidth = 4;
    public float waveRotY = 45;
    #endregion

    #region Private
    private float x0 = -12345;
    private float birthTime = 0;
    #endregion
    #endregion

    #region CustomFunction
    #region Static

    #endregion

    #region Public
    public override void Move()
    {
        Vector3 tempPos = Pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        Pos = tempPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

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
        x0 = Pos.x;

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