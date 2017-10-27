using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewDev : MonoBehaviour
{
    public ScrollRect scrollRect_ = null;
    public RectTransform scrollRT_ = null;
    public RectTransform contentPanelRT_ = null;
    public GameObject baseContent_ = null;
    public Vector2 contentSpacing_ = Vector2.zero;

    public int contentsCount_ = 0;

    private Content[] _contents = null;

    public void Awake()
    {
        Initialize(baseContent_, contentPanelRT_);
    }

    public void Start()
    {

    }

    private void Initialize(GameObject go, Transform parent)
    {
        _contents = new Content[contentsCount_];
        Vector2 spacing = contentSpacing_;
        Vector2 startPos = Vector2.zero;
        Vector2 contentPanelSize = Vector2.zero;
        Vector2 contentSize = baseContent_.GetComponent<RectTransform>().sizeDelta;
        Vector3 totalSize = contentSize + spacing;
        Vector3 pos = Vector3.zero;

        float newPosX = 0.0f;
        float newPosY = 0.0f;
        int column = (int)(scrollRT_.sizeDelta.x / totalSize.x);
        int row = _contents.Length / column;
        if (_contents.Length % column != 0)
        {
            row++;
        }
        startPos = new Vector2(spacing.x + ((scrollRT_.sizeDelta.x - (totalSize.x * column)) * 0.5f), -spacing.y);
        contentPanelSize = new Vector2(contentPanelRT_.sizeDelta.x, totalSize.y * row); //@TODO: 리포지션으로 사용시에 수정필요
        contentPanelRT_.sizeDelta = contentPanelSize;
        for (int i = 0; i < _contents.Length; i++)
        {
            newPosX = startPos.x + (totalSize.x * (i % column));
            newPosY = startPos.y - (totalSize.y * (i / column));
            pos = new Vector3(newPosX, newPosY, 0);

            GameObject obj = Instantiate(go, parent) as GameObject;
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }
}
