using UnityEngine;

// Example MonoBehaviour that oscillates a transform position between two points.
public class Platform : MonoBehaviour
{
    [SerializeField]
    Vector3 m_Start = new Vector3(-10, 0f, 0f);

    [SerializeField]
    Vector3 m_End = new Vector3(10f, 0f, 0f);

    [SerializeField]
    float m_Speed = .2f;

    public Vector3 start
    {
        get => m_Start;
        set => m_Start = value;
    }

    public Vector3 end
    {
        get => m_End;
        set => m_End = value;
    }

    public float speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }

    void Update()
    {
        SnapToPath(Time.time);
    }

    public void SnapToPath(float time)
    {
        transform.position = Vector3.Lerp(m_Start, m_End, (Mathf.Sin(time * m_Speed) + 1) * .5f);
    }
}