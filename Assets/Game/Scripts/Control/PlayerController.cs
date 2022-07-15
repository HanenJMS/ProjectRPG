using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        UnitCore unit;
        enum CursorType
        {
            None, Movement, Combat, UI
        }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        private void Awake()
        {
            health = this.gameObject.GetComponent<Health>();
            unit = this.gameObject.GetComponent<UnitCore>();
        }
        private void Start()
        {
            
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        private void Update()
        {
            if (IneteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
            print($"{gameObject.name} is currently doing nothing.");
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable == null) continue;
                    if (raycastable.HandleRayCast(this))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IneteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    this.gameObject.GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                    print($"{gameObject.name} is running to point");
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        private void OnMouseDown()
        {
            CameraController.instance.followTransform = transform;
        }
    }
}