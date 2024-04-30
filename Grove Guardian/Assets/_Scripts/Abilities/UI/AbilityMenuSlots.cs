using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityMenuSlots : MonoBehaviour, IDropHandler {
    [field: SerializeField] public AbilitySlot Slot { get; private set; }

    public GameObject _currentAbility;
    public bool _canDeleteChild = true;

    private void Start() {
        AbilityMenuManager.Instance.AddRaycast += AddToRaycast;
        AbilityMenuManager.Instance.RemoveRaycast += RemoveFromRaycast;
    }
    public void OnDrop(PointerEventData eventData) {
        HandleDrop(eventData);
    }
    private void HandleDrop(PointerEventData eventData) {
        HandleDrop(eventData.pointerDrag);
    }

    public void HandleDrop(GameObject other) {
        var otherDrager = other.GetComponent<Dragger>();
        if(otherDrager == null) {
            return;
        }
        if(_currentAbility == null) {
            _canDeleteChild = false;
        }
        var otherSlot = other.GetComponent<AbilityMenuRepresenter>().AbilityBase.Slot;
        if((otherSlot == AbilitySlot.Basic && Slot != AbilitySlot.Basic)
            || (otherSlot != AbilitySlot.Basic && Slot == AbilitySlot.Basic)) {
            return;
        }
        if(_currentAbility != null && _currentAbility != other) {
            var otherDragger = other.GetComponent<Dragger>();
            if(otherDragger.ParentAfterDrag == otherDragger.OriginalParent) {
                HandleNewAbilitySwap(_currentAbility, other);
            } else {
                HandleOldAbilitySwap(_currentAbility, other);
            }
            return;
        }
        if(otherDrager.CurrentSlot!= null) {
            otherDrager.CurrentSlot.DeleteChild();
        }
        _currentAbility = other;
        var dragger = _currentAbility.GetComponent<Dragger>();
        dragger.ChangeParent(transform);
    }

    private void HandleOldAbilitySwap(GameObject currentAbility, GameObject other) {
        var firstDragger = currentAbility.GetComponent<Dragger>();
        var secondDragger = other.GetComponent<Dragger>();

        var secondSlot = secondDragger.CurrentSlot;

        var firstParPos = firstDragger.ParentAfterDrag.transform.position;
        var secondParPos = secondDragger.ParentAfterDrag.transform.position;

        var par = firstDragger.ParentAfterDrag;
        firstDragger.ChangeParent(secondDragger.ParentAfterDrag);
        secondDragger.ChangeParent(par);

        firstDragger.transform.position = secondParPos;
        secondDragger.transform.position = firstParPos;

        firstDragger.CurrentSlot.DeleteChild();
        secondDragger.CurrentSlot.DeleteChild();

        firstDragger.HandleDragEnd();
        secondDragger.HandleDragEnd();

        HandleDrop(other);
        secondSlot.HandleDrop(currentAbility);
    }

    private void HandleNewAbilitySwap(GameObject firstAbility, GameObject secondAbility) {
        var firstDragger = firstAbility.GetComponent<Dragger>();
        var secondDragger = secondAbility.GetComponent<Dragger>();
        
        DeleteChild();
        firstDragger.HandleDragEnd(true);

        secondDragger.ChangeParent(transform);
        secondDragger.HandleDragEnd();
        HandleDrop(secondAbility);
    }


    public void DeleteChild() {
        _currentAbility = null;
    }

    public void RemoveFromRaycast() {
        if(_currentAbility != null) {
            _currentAbility.GetComponent<Dragger>().Image.raycastTarget = false;
        }
    }
    public void AddToRaycast() {
        if(_currentAbility != null) {
            _currentAbility.GetComponent<Dragger>().Image.raycastTarget = true;
        }
    }
}
