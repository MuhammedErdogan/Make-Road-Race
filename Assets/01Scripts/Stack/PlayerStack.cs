using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGameStudio;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private GameObject stackUnitPrefab;
    [SerializeField] private Color stackFusingColor;
    [SerializeField] private Color stackColor;

    public List<Transform> stackList = new List<Transform>();
    public Transform stackPointer;

    private MaterialPropertyBlock materialPropertyBlock, materialPropertyBlockStackColor;
    private Transform stackFollowPoint;
    private bool stackIsBusy = false;

    [SerializeField] private int startStackCount = 0;


    private void Start()
    {
        stackColor = GameManager.instance.GetActiveController().GetColorController().currentColor.stackColor;
        stackFusingColor = GameManager.instance.GetActiveController().GetColorController().currentColor.stackFusingColor;


        stackFollowPoint = CGameStudio.GameManager.instance.
            GetActiveController().playerController.stackFollowPoint;
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlockStackColor = new MaterialPropertyBlock();
        materialPropertyBlockStackColor.SetColor("_BaseColor", stackColor);
        materialPropertyBlock.SetColor("_BaseColor", stackFusingColor);

        AddToStack(startStackCount);
    }

    private void Update()
    {
        if (stackFollowPoint == null)
            return;
        Vector3 target = new Vector3(transform.position.x,
            transform.position.y,
            stackFollowPoint.position.z);
        transform.position = target;
    }

    public void AddToStack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            stackPointer = GetPointerTransform();

            GameObject clone = Instantiate(stackUnitPrefab, transform);
            stackList.Add(clone.transform);
            clone.transform.localPosition =
                new Vector3(0,transform.InverseTransformPoint(stackPointer.position).y, 
                transform.InverseTransformPoint(stackPointer.position).z) + Vector3.up * .1f;
            clone.GetComponent<MeshRenderer>()
                .SetPropertyBlock(materialPropertyBlockStackColor);
            StackWave stackWave = clone.GetComponent<StackWave>();
            stackWave.target = stackPointer;
            stackWave.ordre = stackList.Count;
        }
    }

    private Transform GetPointerTransform()
    {
        if (transform.childCount > 0)
            return stackList[stackList.Count - 1];
        else
            return stackFollowPoint.transform;
    }

    public void RemoveFromStack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (stackList.Count <= 0)
                break;
            GameObject itemToDestroy = stackList[stackList.Count - 1].gameObject;
            itemToDestroy.GetComponent<MeshRenderer>()
                .SetPropertyBlock(materialPropertyBlock);
            StartCoroutine(DelayForStackUnitDestroy(itemToDestroy));
            stackList.RemoveAt(stackList.Count - 1);
        }
    }

    public void RemoveFromStack(Transform objToRemove)
    {
        if (stackIsBusy || !stackList.Contains(objToRemove))
            return;
        stackIsBusy = true;
        int v = stackList.FindIndex(x => x.Equals(objToRemove));
        int maxCount = stackList.Count;
        for (int i = v; i < maxCount; i++)
        {

            if (stackList.Count <= 0)
                break;

            Transform itemToRemove = stackList[v];
            StackWave stackWave = itemToRemove.GetComponent<StackWave>();
            stackWave.target = null;
            itemToRemove.SetParent(null);
            stackList.RemoveAt(v);
            Rigidbody rb = itemToRemove.gameObject.AddComponent<Rigidbody>();
            Vector3 force = stackWave.isContacted ?
                Vector3.back * Random.Range(5, 12) + Vector3.up * Random.Range(-3, 5) :
                Vector3.forward * Random.Range(0, 7) + Vector3.up * Random.Range(-5, 5);
            rb.AddForce(force, ForceMode.VelocityChange);

            StartCoroutine(DelayForStackUnitDestroy(itemToRemove.gameObject, 1.75f));
        }
        stackIsBusy = false;
    }

    public void CloseStack()
    {
        StartCoroutine(DelayForStackUnitClose());
    }

    private IEnumerator DelayForStackUnitClose()
    {
        for (int i = 0; i < stackList.Count; i++)
        {
            stackList[i].gameObject.SetActive(false);
            yield return new WaitForFixedUpdate();
        }
    }


    private IEnumerator DelayForStackUnitDestroy(GameObject stackUnityToDestroy)
    {
        yield return new WaitForEndOfFrame();
        Destroy(stackUnityToDestroy);
    }

    private IEnumerator DelayForStackUnitDestroy(GameObject stackUnityToDestroy, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(stackUnityToDestroy);
    }

    public int GetStackCount => stackList.Count;
}
