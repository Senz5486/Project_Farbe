using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���܂��܂ȃ^�C�v�̃I�u�W�F�N�g�̓���𐧌䂷��N���X�B
/// </summary>
public class MainGameObjectController : MonoBehaviour
{
    /// <summary>
    /// �I�u�W�F�N�g�̃^�C�v�̗񋓑́B
    /// </summary>
    public enum ObjectType
    {
        MovingFloor, // �ړ����鏰
        Coin,        // �R�C��
        GoalMedal,   // �S�[���p�̃��_��
        Pipe         // �y��
    }

    // ���ʂ̃p�����[�^�[
    public float speed; // ���x

    [Header("��{�ݒ�")]

    /// <summary>
    /// ���̃I�u�W�F�N�g�̃^�C�v�B
    /// </summary>
    public ObjectType type;

    /// <summary>
    /// �ړ����Ƃ��ē��삷��ۂ̃p�X���X�g�B
    /// </summary>
    public List<Path> paths; // �ړ����鏰�̃p�X�̃��X�g
    private int currentPathIndex = 0; // ���݂̃p�X�̃C���f�b�N�X
    private bool isReturning = false; // �p�X���������邩�ǂ���

    // �R�C���̂��߂̃p�����[�^�[
    public float rotationSpeed; // ��]���x

    // �y�ǂ̂��߂̃p�����[�^�[
    public Transform entry; // ����
    public Transform exit;  // �o��
    public bool isBidirectional; // �������Ɉړ��\���ǂ���

    public void Update()
    {
        switch (type)
        {
            case ObjectType.MovingFloor:
                HandleMovingFloor();
                break;

            case ObjectType.Coin:
                HandleCoin();
                break;
        }
    }

    /// <summary>
    /// �ړ����鏰�̓���𐧌䂷�郁�\�b�h�B
    /// </summary>
    private void HandleMovingFloor()
    {
        if (paths.Count <= 0) return;

        transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].position, speed * Time.deltaTime);

        if (transform.position == paths[currentPathIndex].position)
        {
            if (isReturning)
            {
                currentPathIndex--;
                if (currentPathIndex < 0)
                {
                    isReturning = false;
                    currentPathIndex = 0;
                }
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= paths.Count)
                {
                    isReturning = true;
                    currentPathIndex = paths.Count - 1;
                }
            }
        }
    }

    /// <summary>
    /// �R�C���̓���𐧌䂷�郁�\�b�h�B
    /// </summary>
    private void HandleCoin()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == ObjectType.Coin)
            {
                // �R�C���擾�̏���
                Destroy(gameObject);
            }
            else if (type == ObjectType.GoalMedal)
            {
                // �S�[�����̏���
                Debug.Log("Goal!");
            }
        }
    }
}

[System.Serializable]
public class Path
{
    public Vector3 position; // �ʒu���
    public Quaternion rotation; // ��]���
    public float speed = 1.0f;  // ���̃|�C���g�ł̑��x
    public float delay = 0.0f; // ���̃|�C���g�ł̒�~����
    public Vector3 startPoint = Vector3.zero; // �p�X�̎n�_
    public Vector3 endPoint = Vector3.zero;   // �p�X�̏I�_
    public List<Vector3> points = new List<Vector3>();
}
