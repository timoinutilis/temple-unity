using UnityEngine;

[System.Serializable]
public class SVector3
{
    public float x;
    public float y;
    public float z;

    public SVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public static implicit operator Vector3(SVector3 value)
    {
        return new Vector3(value.x, value.y, value.z);
    }

    public static implicit operator SVector3(Vector3 value)
    {
        return new SVector3(value.x, value.y, value.z);
    }

}
