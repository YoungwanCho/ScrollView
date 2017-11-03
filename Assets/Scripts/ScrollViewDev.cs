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
    public int dataCount_ = 0;
    public int contentsCount_ = 0;

    private List<Content> _contents = new List<Content>();
    private Vector2 contentOverallSize = Vector2.zero; // 객체 하나에 필요한 사이즈
    private Vector2 _contentsTotalSize = Vector2.zero; // 객체들 모두합한 사이즈
    private int _contentColumn;
    private int _contentRow;
    private int _dataRow;
    private int _currentScrollCount;
    private int _previousScrollCount = 0;

    public void Awake()
    {
        Initialize(baseContent_, contentPanelRT_);
    }

    public void Start()
    {
        UpdateContent();
    }

    public void UpdateContentPosition(Vector2 vec)
    {
        _currentScrollCount = (int)(contentPanelRT_.anchoredPosition.y / contentOverallSize.y);

        if (_currentScrollCount == _previousScrollCount || _currentScrollCount < 0 || _currentScrollCount > (_dataRow - _contentRow))
        {
            return;
        }

        if (_currentScrollCount > _previousScrollCount)
        {
            RepositionDown();
        }
        else if (_currentScrollCount < _previousScrollCount)
        {
            RepositionUp();
        }
        _previousScrollCount = _currentScrollCount;
    }

    private void RepositionDown()
    {
        List<Content> firstRowList = new List<Content>();
        for (int i = 0; i < _contentColumn; i++)
        {
            firstRowList.Add(_contents[i]);
        }
        for (int i = 0; i < _contentColumn; i++)
        {
            _contents.Remove(firstRowList[i]);
        }
        for (int i = 0; i < firstRowList.Count; i++)
        {
            firstRowList[i].UpdateContent(((_currentScrollCount * _contentColumn) + _contents.Count + i).ToString());
        }
        _contents.AddRange(firstRowList);

        Vector3 newPos = Vector3.zero;
        Transform targetTransform = null;

        for (int i = 0; i < firstRowList.Count; i++)
        {
            targetTransform = firstRowList[i].transform;
            newPos = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y - _contentsTotalSize.y, targetTransform.localPosition.z);
            targetTransform.localPosition = newPos;
        }
    }

    private void RepositionUp()
    {
        List<Content> lastRowList = new List<Content>();
        for (int i = _contents.Count - _contentColumn; i < _contents.Count; i++)
        {
            lastRowList.Add(_contents[i]);
        }
        for (int i = 0; i < lastRowList.Count; i++)
        {
            _contents.Remove(lastRowList[i]);
        }
        for (int i = 0; i < lastRowList.Count; i++)
        {
            lastRowList[i].UpdateContent(((_currentScrollCount * _contentColumn) + i).ToString());
        }
        _contents.InsertRange(0, lastRowList);

        Vector3 newPos = Vector3.zero;
        Transform targetTransform = null;
        for (int i = 0; i < lastRowList.Count; i++)
        {
            targetTransform = lastRowList[i].transform;
            newPos = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y + _contentsTotalSize.y, targetTransform.localPosition.z);
            targetTransform.localPosition = newPos;
        }
    }

    private void UpdateContent()
    {
        for (int i = 0; i < _contents.Count; i++)
        {
            _contents[i].UpdateContent(i.ToString());
        }
    }

    private void Initialize(GameObject go, Transform parent)
    {
        Vector2 spacing = contentSpacing_;
        Vector2 startPos = Vector2.zero;
        Vector2 contentPanelSize = Vector2.zero;
        Vector2 contentSize = baseContent_.GetComponent<RectTransform>().sizeDelta;
        contentOverallSize = contentSize + spacing;
        Vector3 pos = Vector3.zero;

        float newPosX = 0.0f;
        float newPosY = 0.0f;
        _contentColumn = (int)(scrollRT_.sizeDelta.x / contentOverallSize.x);
        _contentRow = contentsCount_ / _contentColumn;
        _dataRow = dataCount_ / _contentColumn;

        if (contentsCount_ % _contentColumn != 0)
        {
            contentsCount_ += contentsCount_ % _contentColumn;
            _contentRow++;
        }
        if (dataCount_ % _contentColumn != 0)
        {
            _dataRow++;
        }

        _contentsTotalSize = new Vector2(contentOverallSize.x * _contentColumn, contentOverallSize.y * _contentRow);

        startPos = new Vector2(spacing.x + ((scrollRT_.sizeDelta.x - (contentOverallSize.x * _contentColumn)) * 0.5f), -spacing.y);
        contentPanelSize = new Vector2(scrollRT_.sizeDelta.x, contentOverallSize.y * _dataRow);
        contentPanelRT_.sizeDelta = contentPanelSize;
        for (int i = 0; i < contentsCount_; i++)
        {
            newPosX = startPos.x + (contentOverallSize.x * (i % _contentColumn));
            newPosY = startPos.y - (contentOverallSize.y * (i / _contentColumn));
            pos = new Vector3(newPosX, newPosY, 0);

            GameObject obj = Instantiate(go, parent) as GameObject;
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            _contents.Add(obj.GetComponent<Content>());
        }
    }
}
