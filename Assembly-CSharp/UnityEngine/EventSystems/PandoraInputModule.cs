using System;

namespace UnityEngine.EventSystems
{
	[global::UnityEngine.AddComponentMenu("Event/Pandora Input Module")]
	public class PandoraInputModule : global::UnityEngine.EventSystems.PointerInputModule
	{
		public static global::UnityEngine.EventSystems.PointerEventData.InputButton ActionInput
		{
			get
			{
				return global::UnityEngine.EventSystems.PandoraInputModule.actionInput;
			}
			set
			{
				global::UnityEngine.EventSystems.PandoraInputModule.actionInput = value;
			}
		}

		public float InputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		public override void UpdateModule()
		{
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_MousePosition = global::PandoraSingleton<global::PandoraInput>.Instance.GetMousePosition();
		}

		public override bool IsModuleSupported()
		{
			return true;
		}

		public override bool ShouldActivateModule()
		{
			return base.ShouldActivateModule() && global::PandoraSingleton<global::PandoraInput>.Instance.IsActive && (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp(this.m_SubmitButton, -1) | global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp(this.m_CancelButton, -1) | (double)(this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0.0 | global::UnityEngine.Input.GetMouseButtonDown(0));
		}

		public override void ActivateModule()
		{
			if (!global::PandoraSingleton<global::PandoraInput>.Instance.IsActive)
			{
				return;
			}
			base.ActivateModule();
			this.m_MousePosition = global::UnityEngine.Input.mousePosition;
			this.m_LastMousePosition = global::UnityEngine.Input.mousePosition;
			global::UnityEngine.GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.lastSelectedGameObject;
			}
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(null, this.GetBaseEventData());
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		public override void Process()
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.IsActive && base.enabled)
			{
				bool flag = this.SendUpdateEventToSelectedObject();
				if (base.eventSystem.sendNavigationEvents)
				{
					if (!flag)
					{
						flag |= this.SendMoveEventToSelectedObject();
					}
					if (!flag)
					{
						this.SendSubmitEventToSelectedObject();
					}
				}
				this.ProcessMouseEvent();
			}
		}

		public bool IsMouseMoving()
		{
			return this.m_LastMousePosition != this.m_MousePosition;
		}

		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			global::UnityEngine.EventSystems.BaseEventData baseEventData = this.GetBaseEventData();
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp(this.m_SubmitButton, -1))
			{
				global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, global::UnityEngine.EventSystems.ExecuteEvents.submitHandler);
			}
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp(this.m_CancelButton, -1))
			{
				global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, global::UnityEngine.EventSystems.ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		private bool AllowMoveEventProcessing(float time)
		{
			return global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown(this.m_HorizontalAxis, -1) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown(this.m_VerticalAxis, -1) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown(this.m_HorizontalAxis, -1) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown(this.m_VerticalAxis, -1);
		}

		private global::UnityEngine.Vector2 GetRawMoveVector()
		{
			global::UnityEngine.Vector2 zero = global::UnityEngine.Vector2.zero;
			zero.x = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw(this.m_HorizontalAxis, -1);
			zero.y = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw(this.m_VerticalAxis, -1);
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown(this.m_HorizontalAxis, -1))
			{
				zero.x = 1f;
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown(this.m_HorizontalAxis, -1))
			{
				zero.x = -1f;
			}
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown(this.m_VerticalAxis, -1))
			{
				zero.y = 1f;
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown(this.m_VerticalAxis, -1))
			{
				zero.y = -1f;
			}
			return zero;
		}

		private bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = global::UnityEngine.Time.unscaledTime;
			if (!this.AllowMoveEventProcessing(unscaledTime))
			{
				return false;
			}
			global::UnityEngine.Vector2 rawMoveVector = this.GetRawMoveVector();
			global::UnityEngine.EventSystems.AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.9f);
			if (!global::UnityEngine.Mathf.Approximately(axisEventData.moveVector.x, 0f) || !global::UnityEngine.Mathf.Approximately(axisEventData.moveVector.y, 0f))
			{
				global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, global::UnityEngine.EventSystems.ExecuteEvents.moveHandler);
			}
			return axisEventData.used;
		}

		private void ProcessMouseEvent()
		{
			global::UnityEngine.EventSystems.PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
			bool pressed = mousePointerEventData.AnyPressesThisFrame();
			bool released = mousePointerEventData.AnyReleasesThisFrame();
			global::UnityEngine.EventSystems.PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(global::UnityEngine.EventSystems.PointerEventData.InputButton.Left).eventData;
			if (!this.UseMouse(pressed, released, eventData.buttonData))
			{
				return;
			}
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(global::UnityEngine.EventSystems.PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(global::UnityEngine.EventSystems.PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(global::UnityEngine.EventSystems.PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(global::UnityEngine.EventSystems.PointerEventData.InputButton.Middle).eventData.buttonData);
			if (global::UnityEngine.Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				return;
			}
			global::UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy<global::UnityEngine.EventSystems.IScrollHandler>(global::UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<global::UnityEngine.EventSystems.IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, global::UnityEngine.EventSystems.ExecuteEvents.scrollHandler);
		}

		private bool UseMouse(bool pressed, bool released, global::UnityEngine.EventSystems.PointerEventData pointerData)
		{
			return pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling();
		}

		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			global::UnityEngine.EventSystems.BaseEventData baseEventData = this.GetBaseEventData();
			global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, global::UnityEngine.EventSystems.ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		private void ProcessMousePress(global::UnityEngine.EventSystems.PointerInputModule.MouseButtonEventData data)
		{
			global::UnityEngine.EventSystems.PointerEventData buttonData = data.buttonData;
			if (buttonData.button == global::UnityEngine.EventSystems.PandoraInputModule.actionInput)
			{
				buttonData.button = global::UnityEngine.EventSystems.PointerEventData.InputButton.Left;
			}
			else if (buttonData.button == global::UnityEngine.EventSystems.PointerEventData.InputButton.Left)
			{
				buttonData.button = global::UnityEngine.EventSystems.PandoraInputModule.actionInput;
			}
			global::UnityEngine.GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = global::UnityEngine.Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy<global::UnityEngine.EventSystems.IPointerDownHandler>(gameObject, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = global::UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<global::UnityEngine.EventSystems.IPointerClickHandler>(gameObject);
				}
				float unscaledTime = global::UnityEngine.Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if ((double)(unscaledTime - buttonData.clickTime) < 0.300000011920929)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = global::UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<global::UnityEngine.EventSystems.IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
				}
			}
			if (!data.ReleasedThisFrame())
			{
				return;
			}
			global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IPointerUpHandler>(buttonData.pointerPress, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
			global::UnityEngine.GameObject eventHandler = global::UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<global::UnityEngine.EventSystems.IPointerClickHandler>(gameObject);
			if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
			{
				global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IPointerClickHandler>(buttonData.pointerPress, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
			}
			else if (buttonData.pointerDrag != null)
			{
				global::UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy<global::UnityEngine.EventSystems.IDropHandler>(gameObject, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.dropHandler);
			}
			buttonData.eligibleForClick = false;
			buttonData.pointerPress = null;
			buttonData.rawPointerPress = null;
			buttonData.dragging = false;
			if (buttonData.pointerDrag != null)
			{
				global::UnityEngine.EventSystems.ExecuteEvents.Execute<global::UnityEngine.EventSystems.IEndDragHandler>(buttonData.pointerDrag, buttonData, global::UnityEngine.EventSystems.ExecuteEvents.endDragHandler);
			}
			buttonData.pointerDrag = null;
			if (gameObject != buttonData.pointerEnter)
			{
				base.HandlePointerExitAndEnter(buttonData, null);
				base.HandlePointerExitAndEnter(buttonData, gameObject);
			}
		}

		private const global::UnityEngine.EventSystems.PointerEventData.InputButton DEFAULT_ACTION_INPUT = global::UnityEngine.EventSystems.PointerEventData.InputButton.Left;

		[global::UnityEngine.SerializeField]
		private string m_HorizontalAxis = "h";

		[global::UnityEngine.SerializeField]
		private string m_VerticalAxis = "v";

		[global::UnityEngine.SerializeField]
		private string m_SubmitButton = "action";

		[global::UnityEngine.SerializeField]
		private string m_CancelButton = "cancel";

		[global::UnityEngine.SerializeField]
		private float m_InputActionsPerSecond = 10f;

		private float m_NextAction;

		private global::UnityEngine.Vector2 m_LastMousePosition;

		private global::UnityEngine.Vector2 m_MousePosition;

		[global::UnityEngine.SerializeField]
		private bool m_AllowActivationOnMobileDevice;

		private static global::UnityEngine.EventSystems.PointerEventData.InputButton actionInput;
	}
}
