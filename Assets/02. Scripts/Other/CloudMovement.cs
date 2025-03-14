using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2.0f;  // �̵� �ӵ�
    public float distance = 10.0f; // �̵� �Ÿ�

    private Vector3 startPos;
    private int direction = 1; // �̵� ���� (1: ������, -1: ����)

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // �̵� ���� �ʰ� �� ���� ��ȯ
        if (Mathf.Abs(transform.position.x - startPos.x) > distance)
        {
            direction *= -1; // ���� ����
        }
    }
}
