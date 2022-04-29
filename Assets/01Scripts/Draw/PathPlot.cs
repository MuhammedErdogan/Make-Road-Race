using CGameStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlot : MonoBehaviour
{
    public List<Vector3> fingerPos;

    [SerializeField][Range(0f, 1f)] private float sensibilsationInterval;
    [SerializeField] private Transform finishStartPos, finishEndPos;
    [SerializeField] private float parquetInterval;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject plotterPrefab;
    [SerializeField] private GameObject jumpArea;

    [HideInInspector] public bool isCanDraw = true;
    public PlayerStack playerStack;

    private MaterialPropertyBlock materialPropertyBlock;
    private SetGradientColor setGradientColor;
    private InputManager inputManager;
    private ObjectPool objectPool;

    private bool jumpAreaCreated = false;
    private bool lineCreated = false;
    private GameObject currentLine;
    private GameObject parquetTemp;
    private Vector3 offsetParquetPos;
    private Vector3 directionTemp;
    private int pathCount;
    private Vector3 startPos, endPos;

    void Start()
    {
        inputManager = GameManager.instance.GetInputManager();
        inputManager.TouchInput += TouchControl;

        setGradientColor = CGameStudio.GameManager.instance
            .GetActiveController().GetComponent<SetGradientColor>();

        objectPool = GameManager.instance
                .GetActiveController().GetComponent<ObjectPool>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void TouchControl(bool state)
    {
        if (state)
        {
            jumpAreaCreated = false;

            if (playerStack.GetStackCount <= 0 ||
                GetLinePoint() == Vector3.zero ||
                !isCanDraw)
                return;

            else if (!lineCreated)
            {
                CreateLine();
                InstantiateParquet();
                lineCreated = true;

                startPos = new Vector3(0,
                    fingerPos[0].y - (jumpArea.transform.localScale.y / 2),
                    fingerPos[0].z);
            }

            Vector3 tempFingerPos = GetLinePoint();
            if (Vector3.Distance(tempFingerPos, fingerPos[fingerPos.Count - 1]) > sensibilsationInterval)
            {
                InstantiateParquet();
                UpdateLine(tempFingerPos);
            }
        }
        else
        {
            lineCreated = false;
            if (isCanDraw)
                parquetTemp = null;

            if (pathCount > 3)
            {
                Vector3 pos = (endPos.z > startPos.z) ? startPos : endPos;
                if (!jumpAreaCreated)
                {
                    Instantiate(jumpArea, pos, Quaternion.identity);
                    jumpAreaCreated = true;
                }
            }

            if (isCanDraw)
                pathCount = 0;
        }
    }

    private void InstantiateParquet()
    {
        Vector3 lastFingerPos = fingerPos[Mathf.Max(0, fingerPos.Count - 2)];
        Vector3 activeFingerPos = fingerPos[Mathf.Max(0, fingerPos.Count - 1)];

        if (Vector3.Distance(activeFingerPos, lastFingerPos) < 0.1f)
            return;

        float distanceBetweenTwoPoints = Vector3.Distance(activeFingerPos, lastFingerPos);
        Vector3 directionBetweenTwoPoints = (activeFingerPos - lastFingerPos).normalized;

        for (int i = 0; i < Mathf.Ceil(distanceBetweenTwoPoints / parquetInterval); i++)
        {
            if (i > 0)
                offsetParquetPos = parquetInterval * directionBetweenTwoPoints;
            offsetParquetPos = offsetParquetPos == Vector3.zero ?
                offsetParquetPos += parquetInterval * .9f * directionTemp : offsetParquetPos;

            Vector3 referancePosition = (parquetTemp == null ?
                new Vector3(0, lastFingerPos.y, lastFingerPos.z) : parquetTemp.transform.position);

            Quaternion rotate = Quaternion.LookRotation(directionBetweenTwoPoints);
            GameObject parquetClone = objectPool.GetPooledObject(0);
            parquetClone.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            SetColorToBloc(parquetClone);

            parquetClone.transform.GetChild(0).tag = Constants.Tags.PATHEND;
            parquetClone.transform.position = referancePosition + offsetParquetPos;
            parquetClone.transform.rotation = rotate;

            if (parquetTemp != null)
                parquetTemp.transform.GetChild(0).tag = Constants.Tags.PARQUETPATH;

            parquetTemp = parquetClone;
            directionTemp = directionBetweenTwoPoints;
            playerStack.RemoveFromStack(1);

            if (playerStack.stackList.Count <= 0)
                break;

            endPos = referancePosition + offsetParquetPos
                - Vector3.up * (jumpArea.transform.localScale.y / 2);
        }
        offsetParquetPos = Vector3.zero;
    }

    private IEnumerator DelayForManuelPath()
    {
        Vector3 lastFingerPos = fingerPos[Mathf.Max(0, fingerPos.Count - 2)];
        Vector3 activeFingerPos = fingerPos[Mathf.Max(0, fingerPos.Count - 1)];

        Instantiate(jumpArea, activeFingerPos, Quaternion.identity);

        if (Vector3.Distance(activeFingerPos, lastFingerPos) < 0.1f)
            yield return 0;

        float distanceBetweenTwoPoints = Vector3.Distance(activeFingerPos, lastFingerPos);
        Vector3 directionBetweenTwoPoints = (activeFingerPos - lastFingerPos).normalized;

        for (int i = 0; i < Mathf.Ceil(distanceBetweenTwoPoints / parquetInterval); i++)
        {
            if (i > 0)
                offsetParquetPos = parquetInterval * directionBetweenTwoPoints;
            offsetParquetPos = offsetParquetPos == Vector3.zero ?
                offsetParquetPos += parquetInterval * .9f * directionTemp : offsetParquetPos;

            Vector3 referancePosition = (parquetTemp == null ?
                new Vector3(0, lastFingerPos.y, lastFingerPos.z) : parquetTemp.transform.position);

            Quaternion rotate = Quaternion.LookRotation(directionBetweenTwoPoints);
            GameObject parquetClone = objectPool.GetPooledObject(0);
            parquetClone.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            SetColorToBloc(parquetClone);

            parquetClone.transform.GetChild(0).tag = Constants.Tags.PATHEND;
            parquetClone.transform.position = referancePosition + offsetParquetPos;
            parquetClone.transform.rotation = rotate;

            if (parquetTemp != null)
                parquetTemp.transform.GetChild(0).tag = Constants.Tags.PARQUETPATH;

            parquetTemp = parquetClone;
            directionTemp = directionBetweenTwoPoints;
            playerStack.RemoveFromStack(1);

            if (playerStack.stackList.Count <= 0)
            {
                parquetClone.transform.GetChild(0).tag = Constants.Tags.PATHEND;
                yield break;
            }

            endPos = referancePosition + offsetParquetPos
                - Vector3.up * (jumpArea.transform.localScale.y / 2);
            yield return new WaitForFixedUpdate();
        }
        playerStack.CloseStack();
        offsetParquetPos = Vector3.zero;
    }

    public void ManuelPathCreator()
    {
        fingerPos.Clear();
        fingerPos.Insert(0, finishStartPos.position);
        fingerPos.Insert(1, finishEndPos.position);

        StartCoroutine(DelayForManuelPath());
    }

    private void SetColorToBloc(GameObject parquetClone)
    {
        Color color = setGradientColor.GetColor(pathCount * .02f);
        materialPropertyBlock.SetColor("_BaseColor", color);
        parquetClone.GetComponentInChildren<Renderer>()
            .SetPropertyBlock(materialPropertyBlock);
        pathCount++;
    }

    private void CreateLine()
    {
        Destroy(currentLine);
        fingerPos.Clear();
        fingerPos.Add(GetLinePoint());
        fingerPos.Add(GetLinePoint());
        currentLine = Instantiate(plotterPrefab, fingerPos[0], Quaternion.identity);
        playerStack.RemoveFromStack(1);
    }

    private void UpdateLine(Vector3 newFingerpos)
    {
        fingerPos.Add(newFingerpos);
        currentLine.transform.position = newFingerpos;
    }

    private Vector3 GetLinePoint()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, layerMask))
            if (hit.collider.CompareTag(Constants.Tags.PLOTPLANE))
                return hit.point;
            else
                return Vector3.zero;
        else
            return Vector3.zero;
    }
}
