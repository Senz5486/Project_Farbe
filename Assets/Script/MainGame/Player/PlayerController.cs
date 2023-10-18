using UnityEngine;

/**
 * <summary>
 * �V���v���ȃL�����N�^�[�R���g���[���[�N���X�B
 * ��{�I�Ȉړ��A�W�����v�A�_�b�V���A��i�W�����v�A�}�Ζʂł̊���̋@�\�N���X
 * </summary>
 * �����: �n�
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /* --- �V���A���C�Y�t�B�[���h�ꗗ --- */

    [SerializeField] private float speed = 5.0f;            // �L�����N�^�[�̈ړ����x
    [SerializeField] private float jumpHeight = 2.0f;       // �W�����v�̍���
    [SerializeField] private float gravity = 9.81f;         // �d�͂̋���
    [SerializeField] private LayerMask groundLayer;         // �n�ʃ��C���[
    [SerializeField] private float slopeLimit = 45.0f;      // �ǂꂭ�炢�̌X�΂܂łȂ���邩�̐���
    [SerializeField] private float slideSpeed = 3.0f;       // �}�Ζʂł̊��鑬�x
    [SerializeField] private float dashSpeedMultiplier = 2.0f; // �_�b�V�����̑��x�{��
    [SerializeField] private float maxDashTime = 5.0f;      // �_�b�V���ł���ő厞��
    [SerializeField] private float dashRecoveryRate = 1.0f; // �_�b�V�����Ԃ̉񕜑��x
    [SerializeField] private int maxJumpCount = 2;          // �ő�W�����v�� (��i�W�����v�p)

    /* --- �v���C�x�[�g�t�B�[���h --- */

    private Vector3 velocity;                              // ���݂̃L�����N�^�[�̑��x
    private bool isGrounded;                               // �L�����N�^�[���n�ʂɐڐG���Ă��邩
    private int currentJumpCount = 0;                      // ���݂̃W�����v��
    private float currentDashTime;                         // ���݂̃_�b�V���c�莞��
    private CharacterController controller;                // �L�����N�^�[�R���g���[���[�̃��t�@�����X

    /* --- �J�v�Z���� --- */

    public float Speed { get => speed; set => speed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
    }

    private void Update()
    {
        //�L�����N�^�[�̊�{�I�Ȉړ�����
        HandleMovement();
        //�W�����v�Ɠ�i�W�����v����
        HandleJumping();
        //�_�b�V������
        HandleDashing();
        //�}�Ζʂł̊��������
        HandleSliding();
    }

    /// <summary>
    /// �L�����N�^�[�̊�{�I�Ȉړ�����
    /// </summary>
    private void HandleMovement()
    {
        // �n�ʂɐڂ��Ă��邩�̔���
        isGrounded = controller.isGrounded;
        // �n�ʂɐڂ��Ă���Ƃ��̑��x��������
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            // �n�ʂɐG�ꂽ��W�����v�J�E���g�����Z�b�g
            currentJumpCount = 0;
        }

        // �L�[�{�[�h�̓��͂��擾���A�������ɃL�����N�^�[���ړ�
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);
    }

    /// <summary>
    /// �W�����v�Ɠ�i�W�����v����
    /// </summary>
    private void HandleJumping()
    {
        // �W�����v�{�^����������A�W�����v�̉񐔐������ł���΃W�����v
        if (Input.GetButtonDown("Jump") && currentJumpCount < maxJumpCount)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            currentJumpCount++;
        }

        // �d�͂������ăL�����N�^�[�𗎉�������
        velocity.y -= gravity * Time.deltaTime;

        // �v�Z���ꂽ���x�ŃL�����N�^�[���ړ�
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private void HandleDashing()
    {
        // �_�b�V���L�[��������Ă��āA�_�b�V���̎c�莞�Ԃ�����ꍇ�A�_�b�V��
        if (Input.GetKey(KeyCode.LeftShift) && currentDashTime > 0)
        {
            Vector3 dashDirection = transform.forward * dashSpeedMultiplier;
            controller.Move(dashDirection * Time.deltaTime);
            currentDashTime -= Time.deltaTime;
        }
        else
        {
            // �_�b�V�����Ԃ���
            currentDashTime += dashRecoveryRate * Time.deltaTime;
        }
        currentDashTime = Mathf.Clamp(currentDashTime, 0, maxDashTime);
    }

    /// <summary>
    /// �}�Ζʂł̊��������
    /// </summary>
    private void HandleSliding()
    {
        // �L�����N�^�[���}�Ζʂɂ���ꍇ�A����
        if (IsOnSteepSlope() && !controller.isGrounded)
        {
            Vector3 slideDirection = Vector3.down * slideSpeed;
            controller.Move(slideDirection * Time.deltaTime);
        }
    }

    /// <summary>
    /// �L�����N�^�[���}�Ζʂɂ��邩�̔���
    /// </summary>
    /// <returns>�}�Ζʂɂ���ꍇ��true�A����ȊO��false�B</returns>
    private bool IsOnSteepSlope()
    {
        // �n�ʂɐڂ��Ă���ꍇ�A�}�Ζʔ�����s��
        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, groundLayer))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > slopeLimit;
            }
        }
        return false;
    }
}