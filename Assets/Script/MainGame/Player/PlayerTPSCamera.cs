using UnityEngine;

/**
 * <summary>
 * TPS�J�����R���g���[���[�N���X�B
 * �v���C���[��Ǐ]���A�}�E�X��A�i���O�X�e�B�b�N�̓����ɉ����ăJ��������]������@�\�B
 * �v���C���[�̓J�����̕����������悤�ɒ�������@�\�B
 * </summary>
 * �����: �n�
 */
public class PlayerTPSCamera : MonoBehaviour
{
    /* --- �V���A���C�Y�t�B�[���h�ꗗ --- */

    [SerializeField] private Vector3 offset; // �v���C���[����̃I�t�Z�b�g
    [SerializeField] private float sensitivity = 5.0f; // �J�����̊��x
    [SerializeField] private float rotationSmoothTime = 0.1f; // �J�����̉�]�̃X���[�W���O����
    [SerializeField] private float playerRotateSpeed = 5.0f; // �v���C���[�̉�]���x
    [SerializeField] private LayerMask collisionLayers; // �J�����̏Փ˂����m���郌�C���[

    /* --- �v���C�x�[�g�t�B�[���h --- */
    private Transform playerTransform; // �v���C���[��Transform
    private float yaw; // �������̉�]�p
    private float pitch; // �c�����̉�]�p
    private Vector3 rotationSmoothVelocity; // �X���[�W���O�p�̑��x
    private Vector3 currentRotation; // ���݂̉�]�p

    private void Start()
    {
        // Player�^�O�����I�u�W�F�N�g���������āA����Transform���擾
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // �J�����̉�]����
        HandleCameraRotation();
        // �v���C���[�̌����𒲐����鏈��
        HandlePlayerRotation();
        // �J�����̏Փˏ���
        HandleCameraCollision();
    }

    /// <summary>
    /// �J�����̉�]������
    /// </summary>
    private void HandleCameraRotation()
    {
        // �}�E�X���͂Ɋ�Â��p�x�̌v�Z
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -40, 85); // �c�̊p�x�̐���

        // �X���[�W���O��p���ăJ�����̉�]���X�V
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        // �J�����̈ʒu���X�V
        transform.position = playerTransform.position - transform.forward * offset.magnitude + offset.y * transform.up;
    }

    /// <summary>
    /// �v���C���[���J�����̕����Ɍ����悤�ɒ���
    /// </summary>
    private void HandlePlayerRotation()
    {
        Vector3 lookDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, lookRotation, Time.deltaTime * playerRotateSpeed);
    }

    /// <summary>
    /// �J�����Ƒ��̃I�u�W�F�N�g�Ƃ̏Փ˂�����
    /// </summary>
    private void HandleCameraCollision()
    {
        RaycastHit hit;
        // �J��������������̃I�u�W�F�N�g�ɐڐG���Ă���ꍇ�A�J�������v���C���[�ɋ߂Â���
        if (Physics.Raycast(playerTransform.position, -transform.forward, out hit, offset.magnitude, collisionLayers))
        {
            transform.position = hit.point + hit.normal * 0.5f; // �����I�t�Z�b�g�������ăJ����������
        }
    }
}