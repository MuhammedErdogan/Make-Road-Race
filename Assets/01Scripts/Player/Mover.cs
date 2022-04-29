using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject waveBoard;
    [SerializeField] private float restLength;
    [SerializeField] private float waveTravel;
    [SerializeField] private float waveStifness;
    [SerializeField] private float damperStiffness;

    private PlayerAnimatorController playerAnimatorController;
    private PlayerController playerController;
    private CameraController cameraController;
    private Vector3 waveForce;
    private Rigidbody rb;

    private float maxDistance;
    private float minDistance;
    private float lastDistance;
    private float waveDistance;
    private float springForce;
    private float damperForce;
    private float boardVelocity;
    private float time;

    void Start()
    {
        playerController = CGameStudio.GameManager.instance.GetActiveController().playerController;
        cameraController = CGameStudio.GameManager.instance.GetActiveController().cameraController;
        playerAnimatorController = playerController.playerAnimatorController;
        rb = transform.GetComponent<Rigidbody>();

        minDistance = restLength - waveTravel;
        maxDistance = restLength + waveTravel;

        time = Random.Range(1.5f, 2f);
    }
    private void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.forward,
            -transform.up, out RaycastHit hit, maxDistance + .15f))
        {
            ClimbYPos(hit.point.y);
            Vector3 parallelDirection = new Vector3(0, -hit.normal.z, hit.normal.y);

            playerController.ChangeMoveDirection(parallelDirection);

            SetRotationToBoard(hit.collider.transform);

            TagController(hit, parallelDirection);

            if (playerController.isFinish || playerController.isFail)
                return;

            cameraController.SetPriorityTo(Constants.CamerasType.FollowCam);
            playerController.ManuelChangingEnd(6);
        }
        else //fly
        {
            playerController.ChangeMoveDirection(transform.forward);
            playerAnimatorController.SetCharacterAnimation(2);
            playerController.ManuelControlOfspeed(0, 1.5f);

            if (playerController.isFail)
                return;

            if (!playerController.isFinish)
                cameraController.SetPriorityTo(Constants.CamerasType.ActionCam);

            SetRotationToBoard(transform);
        }
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position - Vector3.up * .5f,
            waveBoard.transform.forward, out RaycastHit hit1,
            4.5f, 1 << LayerMask.NameToLayer(Constants.Layers.PARQUET))
            && !playerController.isFinish)
            playerController.ManuelControlOfspeed(3 * hit1.distance, 4);
        else if (!playerController.isFinish)
            playerController.ManuelChangingEnd(6);

        if (Physics.Raycast(transform.position + Vector3.up * .2f,
            -transform.up, out RaycastHit hit, maxDistance + .2f, layerMask))
        {
            lastDistance = waveDistance; waveDistance = hit.distance;
            waveDistance = Mathf.Clamp(waveDistance, minDistance, maxDistance);
            boardVelocity = (lastDistance - waveDistance) / Time.fixedDeltaTime;
            springForce = waveStifness * (restLength - waveDistance);
            waveForce = Vector3.Slerp(waveForce, transform.up
                * (springForce + damperForce), Time.deltaTime * 40);
            damperForce = damperStiffness * boardVelocity;

            rb.AddForceAtPosition(waveForce, hit.normal);
            Oscillation();
        }
    }

    private void TagController(RaycastHit hit, Vector3 parallelDirection)
    {
        if (hit.collider.CompareTag(Constants.Tags.FINISHPLATFORM))
            FinishMovement(hit);
        else if (hit.collider.CompareTag(Constants.Tags.PLATFORM))
            playerAnimatorController.SetCharacterAnimation(0);
        else if ((hit.collider.CompareTag(Constants.Tags.PARQUETPATH)))
            playerAnimatorController.SetCharacterAnimation(1);
        else if (hit.collider.CompareTag(Constants.Tags.PATHEND))
            StartJump(parallelDirection * 1.8f + Vector3.up * .8f);
        else
            StopJump();
    }

    private void FinishMovement(RaycastHit hit)
    {
        waveBoard.transform.rotation = Quaternion.Slerp(waveBoard.transform.rotation,
        Quaternion.Euler(0, 150, 0),
        Time.deltaTime * 4.25f);
        CGameStudio.GameManager.instance.GetActiveController().IsFinish();
        playerAnimatorController.SetCharacterAnimation(4);
        CGameStudio.GameManager.instance.GetParticleController()
            .InstantiateFinishParticle(new Vector3(hit.collider.transform.position.x + 1.5f,
            hit.collider.transform.position.y,
            transform.position.z),
            new Vector3(hit.collider.transform.position.x - 1.5f,
            hit.collider.transform.position.y,
            transform.position.z));
    }

    private void Oscillation()
    {
        if (Time.time > time)
        {
            time += Time.time + Random.Range(0.75f, 1.25f);
            rb.AddForce(Vector3.up * .75f, ForceMode.VelocityChange);
        }
    }
    public void StartJump(Vector3 dir)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionX;
        rb.AddForce(dir, ForceMode.VelocityChange);
    }
    public void StopJump()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation |
        RigidbodyConstraints.FreezePositionZ |
        RigidbodyConstraints.FreezePositionX;
    }
    private void ClimbYPos(float floorPos)
    {
        float positionClambedY = transform.position.y;
        positionClambedY = Mathf.Clamp(positionClambedY, floorPos + .25f, floorPos + 3);
        transform.position = new Vector3(transform.position.x,
            positionClambedY,
            transform.position.z);
    }

    private void SetRotationToBoard(Transform pathTransform)
    {
        float angleLimitedX = pathTransform.rotation.eulerAngles.x;
        Vector3 angleLimited = Mathf.DeltaAngle(0, angleLimitedX) < 0 ?
            new Vector3(angleLimitedX, 0, 0) : new Vector3(-angleLimitedX, 0, 0);
        waveBoard.transform.rotation = Quaternion.Slerp(waveBoard.transform.rotation,
            Quaternion.Euler(angleLimited),
            Time.deltaTime * 2.5f);
    }
    public enum PlayerState
    {
        isFly, isForward, isClimb
    }
}
