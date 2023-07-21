using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform toLookAt;

    [SerializeField] private bool flipXAxis, flipYAxis;

    [SerializeField] private bool fixedXAxis, fixedYAxis, fixedZAxis;

    private float _startXAngle, _startYAngle, _startZAngle;

    private Camera _camera;

    #endregion

    #region MonoBehaviour Call Backs

    private void Start()
    {
        _camera = Camera.main;

        var angles = transform.eulerAngles;
        
        _startXAngle = angles.x;
        _startYAngle = angles.y;
        _startZAngle = angles.z;

        var scale = transform.localScale;

        if (!toLookAt)
            toLookAt = _camera.gameObject.transform;

        if (flipXAxis)
            transform.localScale =
                new Vector3(transform.localScale.x * -1f, scale.y, scale.z);

        if (flipYAxis)
            transform.localScale =
                new Vector3(transform.localScale.x, scale.y * -1f, scale.z);
    }

    private const int FrameRate = 1;

    private void LateUpdate()
    {
        if (Time.frameCount % FrameRate != 0)
            return;

        //transform.LookAt(toLookAt.position, -Vector3.up);
        transform.LookAt(transform.position + toLookAt.rotation * Vector3.forward, toLookAt.rotation * Vector3.up);

        if (fixedXAxis)
            transform.rotation = Quaternion.Euler(_startXAngle, transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);

        if (fixedYAxis)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _startYAngle,
                transform.rotation.eulerAngles.z);

        if (fixedZAxis)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                _startZAngle);
    }

    #endregion

    #region Methods

    #endregion
}