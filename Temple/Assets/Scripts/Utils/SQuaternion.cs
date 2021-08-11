using UnityEngine;

[System.Serializable]
public class SQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator Quaternion(SQuaternion value)
    {
        return new Quaternion(value.x, value.y, value.z, value.w);
    }

    public static implicit operator SQuaternion(Quaternion value)
    {
        return new SQuaternion(value.x, value.y, value.z, value.w);
    }

}
