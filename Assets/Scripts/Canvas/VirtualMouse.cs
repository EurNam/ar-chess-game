using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class VirtualMouse : MonoBehaviour
    {
        public static VirtualMouse Instance;
        public RectTransform cursor; // UI element representing the cursor
        public float moveSpeed = 500f; // Reduced speed of cursor movement
        public float longPressDuration = 1f; // Duration to detect a long press
        public float doubleTapTime = 0.3f; // Maximum time interval between taps for a double tap
        private Vector2 touchpadArea; // Area for the touchpad
        private bool isLongPress = false; // Check if the user is trying to click and drag
        private float touchTime = 0f; // Time of the last touch
        private bool isDragging = false; // Check if the user is trying to click and drag
        private GameObject draggedObject = null; // Object being dragged
        private float lastTapTime = 0f; // Time of the last tap
        private int tapCount = 0; // Number of taps
        private bool isEnabled = false; // Check if the virtual mouse is enabled
        private float sensitivityFactor = 0.3f; // Sensitivity factor to slow down cursor movement

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            // Define the touchpad area (bottom third of the screen)
            touchpadArea = new Vector2(Screen.height, Screen.height / 3);

            // Initialize the cursor at the bottom left of the screen
            cursor.anchoredPosition = new Vector2(0, 0);

            // Hide the cursor initially
            cursor.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!isEnabled) return;

            // Handle touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Check if the touch is within the touchpad area
                if (touch.position.y <= touchpadArea.y)
                {
                    HandleCursorMovement(touch.deltaPosition * sensitivityFactor);

                    // Handle double tap
                    if (touch.phase == TouchPhase.Began)
                    {
                        tapCount++;
                        if (tapCount == 1)
                        {
                            lastTapTime = Time.time;
                        }
                        else if (tapCount == 2 && Time.time - lastTapTime <= doubleTapTime)
                        {
                            SimulatePointerDown();
                            tapCount = 0;
                        }
                    }

                    // Handle long press for mouse click
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        touchTime += Time.deltaTime;
                        if (touchTime >= longPressDuration)
                        {
                            if (!isLongPress)
                            {
                                isLongPress = true;
                                SimulatePointerDown();
                            }
                        }
                    }
                    else
                    {
                        touchTime = 0f;
                        isLongPress = false;
                    }
                }
            }

            // Handle mouse input
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                // Check if the mouse is within the touchpad area
                if (mousePosition.y <= touchpadArea.y)
                {
                    Vector2 deltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * moveSpeed * Time.deltaTime * sensitivityFactor;
                    HandleCursorMovement(deltaPosition);

                    // Handle dragging
                    if (isDragging)
                    {
                        SimulateMouseDrag();
                    }
                    else
                    {
                        // Handle long press for mouse click
                        touchTime += Time.deltaTime;
                        if (touchTime >= longPressDuration)
                        {
                            if (!isLongPress)
                            {
                                isLongPress = true;
                                SimulatePointerDown();
                            }
                        }
                    }
                }
                else
                {
                    touchTime = 0f;
                    isLongPress = false;
                }
            }
            else
            {
                if (isDragging)
                {
                    SimulatePointerUp();
                }
                touchTime = 0f;
                isLongPress = false;
            }

            // Handle arrow key input for cursor movement
            Vector2 arrowKeyMovement = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow))
                arrowKeyMovement.y += moveSpeed * Time.deltaTime * sensitivityFactor;
            if (Input.GetKey(KeyCode.DownArrow))
                arrowKeyMovement.y -= moveSpeed * Time.deltaTime * sensitivityFactor;
            if (Input.GetKey(KeyCode.LeftArrow))
                arrowKeyMovement.x -= moveSpeed * Time.deltaTime * sensitivityFactor;
            if (Input.GetKey(KeyCode.RightArrow))
                arrowKeyMovement.x += moveSpeed * Time.deltaTime * sensitivityFactor;

            HandleCursorMovement(arrowKeyMovement);

            // Handle space bar for simulating mouse click
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SimulatePointerDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SimulatePointerUp();
            }

            // Reset tap count if time between taps exceeds doubleTapTime
            if (tapCount > 0 && Time.time - lastTapTime > doubleTapTime)
            {
                tapCount = 0;
            }
        }

        public bool IsEnabled()
        {
            return isEnabled;
        }

        void HandleCursorMovement(Vector2 deltaPosition)
        {
            // Move the cursor
            cursor.anchoredPosition += deltaPosition;

            // Clamp the cursor position to the screen bounds
            cursor.anchoredPosition = new Vector2(
                Mathf.Clamp(cursor.anchoredPosition.x, 0 - 430 / 2, 430 / 2),
                Mathf.Clamp(cursor.anchoredPosition.y, 0 - 466, 466)
            );
        }

        void SimulatePointerDown()
        {
            // Prevent new pointer down events while dragging
            if (isDragging) return;

            // Convert cursor position to a ray
            Ray ray = GameManager.Instance.portalController.ARCamera.ScreenPointToRay(cursor.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Simulate pointer down
                ExecuteEvents.Execute(hitObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);

                // Start dragging if the object is draggable
                if (hitObject != null)
                {
                    isDragging = true;
                    draggedObject = hitObject;
                }
            }
            else
            {
                Debug.Log("No 3D object found at cursor position");
            }
        }

        void SimulatePointerUp()
        {
            if (draggedObject != null)
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = cursor.position
                };

                ExecuteEvents.Execute(draggedObject, pointerData, ExecuteEvents.pointerUpHandler);
                isDragging = false;
                draggedObject = null;
            }
        }

        void SimulateMouseDrag()
        {
            // if (draggedObject != null)
            // {
            //     PointerEventData pointerData = new PointerEventData(EventSystem.current)
            //     {
            //         position = cursor.position
            //     };

            //     ExecuteEvents.Execute(draggedObject, pointerData, ExecuteEvents.dragHandler);
            // }
        }

        public Vector3 GetCursorPosition()
        {
            return cursor.position;
        }

        public void ToggleVirtualMouse()
        {
            isEnabled = !isEnabled;
            cursor.gameObject.SetActive(isEnabled);
        }
    }
}