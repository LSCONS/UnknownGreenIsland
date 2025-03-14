using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2.0f;  // 이동 속도
    public float distance = 10.0f; // 이동 거리

    private Vector3 startPos;
    private int direction = 1; // 이동 방향 (1: 오른쪽, -1: 왼쪽)

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // 이동 범위 초과 시 방향 전환
        if (Mathf.Abs(transform.position.x - startPos.x) > distance)
        {
            direction *= -1; // 방향 반전
        }
    }
}
