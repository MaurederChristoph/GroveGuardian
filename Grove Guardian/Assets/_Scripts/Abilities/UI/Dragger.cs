using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Action<GameObject> OnParentChange { get; private set; }
    [field: SerializeField] public Transform ParentAfterDrag { get; private set; }
    [field: SerializeField] public Transform OriginalParent { get; private set; }
    [field: SerializeField] public AbilityMenuSlots CurrentSlot { get; private set; }
    private GameObject _abilityRef;
    private GameObject _tempAbility;

    private int _slot;

    private void Start() {
        _abilityRef = gameObject;
        OriginalParent = transform.parent;
        ParentAfterDrag = transform.parent;
        Image = gameObject.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        HandleDragBegin();
    }
    public void HandleDragBegin() {
        AbilityMenuManager.Instance.RemoveRaycast?.Invoke();
        if(OriginalParent == ParentAfterDrag) {
            CreateTempAbility();
        }
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        Image.raycastTarget = false;
    }

    public void CreateTempAbility() {
        _tempAbility = Instantiate(_abilityRef, transform.parent);
        var AMR = _tempAbility.GetComponent<AbilityMenuRepresenter>();
        AMR.MakeAsPlaceHolder();
        _slot = (int)AMR.AbilityBase.Slot;
        _tempAbility.transform.SetSiblingIndex(_slot);
        _tempAbility.GetComponent<Dragger>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        HandleDragEnd();
    }

    public void HandleDragEnd(bool isSwapping = false) {
        var parentBounds = ParentAfterDrag.GetComponent<BoxCollider2D>();
        if((parentBounds != null && !parentBounds.bounds.Contains(transform.position)) || isSwapping) {
            ResetParent();
        }
        transform.SetParent(ParentAfterDrag);
        if(ParentAfterDrag == OriginalParent) {
            transform.SetSiblingIndex(_slot);
            Destroy(_tempAbility);
        } else {
            transform.SetSiblingIndex(0);
        }

        if(ParentAfterDrag.TryGetComponent<AbilityMenuSlots>(out var ams)) {
            CurrentSlot = ams;
        }
        AbilityMenuManager.Instance.AddRaycast?.Invoke();
        Image.raycastTarget = true;
    }

    public void ResetParent() {
        if(CurrentSlot != null) {
            CurrentSlot.DeleteChild();
            CurrentSlot = null;
        }
        ParentAfterDrag = OriginalParent;
    }

    public void ChangeParent(Transform newParent) {
        ParentAfterDrag = newParent;
    }

    public void AddListenerParentChange(Action<GameObject> newListener) {
        OnParentChange += newListener;
    }
    public void RemoveListenerParentChange(Action<GameObject> newListener) {
        OnParentChange += newListener;
    }
}
