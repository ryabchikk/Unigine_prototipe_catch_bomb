using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "e512600290d09f79496242849bd0ec1371f58256")]
public class UiElement : Component
{
	[ShowInEditor][Parameter(Tooltip = "Type of UI element")]
	private Element uiType = Element.None;

	[ShowInEditor][Parameter(Tooltip = "Main parent of all UI objects, commonly the plane on which all UI elements are located")]
	private Node uiPlane = null;

	[ShowInEditor][ParameterMask(Tooltip = "Mask for detecting mouse intersection")]
	private int uiMask = 1;

	[ShowInEditor][Parameter(Tooltip = "Type of alignment")]
	private HorizontalAlign align = HorizontalAlign.Center;

	[ShowInEditor][Parameter(Tooltip = "Horizontal offset for alignment")]
	private float horizontalOffset = 0;

	[ShowInEditor][Parameter(Tooltip = "Scale on hover")]
	private float selectScale = 1;

	[ShowInEditor][ParameterColor(Tooltip = "Emission color on hover")]
	private vec4 selectEmission = vec4.ONE;

	static public event Action<Element> onClick;

	public enum Element { Restart, Exit, None, Drop, Item0, Item1, Item2 }

	public enum HorizontalAlign { Left, Center, Right }

	public uint Id { get; private set; }
	static public bool AnyIsSelect
	{
		get
		{
			foreach (var element in uiElements)
			{
				if (element.isSelect)
					return true;
			}

			return false;
		}
	}

	// ID of UI element
	static private uint idCount = 0;
	static private List<UiElement> uiElements = new List<UiElement>();

	// the last window size
	private int lastWidht = 0;
	private int lastHeight = 0;

	private float unitPerPixel = 0;

	// size of the UI element
	private vec3 size = vec3.ONE;

	private WorldIntersection intersection = null;
	private bool isSelect = false;

	Unigine.Object uiObject = null;

	private vec3 sourceScale = vec3.ONE;
	private vec4 sourceEmission = vec4.ONE;

	void Init()
	{
		// set ID
		Id = idCount;
		uiElements.Add(this);
		++idCount;

		// remember the window size
		lastHeight = WindowManager.MainWindow.SelfGui.Height;
		lastWidht = WindowManager.MainWindow.SelfGui.Width;

		// calculate the unit per pixel value
		float fov = Game.Player.Fov;
		float halfHeight = (float)MathLib.Abs(uiPlane.Position.z) * MathLib.Tan(MathLib.DEG2RAD * fov / 2);
		unitPerPixel = 2.0f * halfHeight / lastHeight;

		// get the size of the UI element
		BoundBox box = node.BoundBox;
		size = box.maximum - box.minimum;
		size.x *= node.Scale.x;
		size.y *= node.Scale.y;
		size.z *= node.Scale.z;

		// get the UI element
		uiObject = node as Unigine.Object;

		// remember the source scale and emission color
		sourceScale = node.Scale;
		sourceEmission = uiObject.GetMaterialParameterFloat4("emission_color", 0);

		OnWindowResize();

		intersection = new WorldIntersection();
	}

	protected override void OnDisable()
	{
		OnLeave();
	}

	void Update()
	{
		if (Input.MouseGrab)
			return;

		EngineWindow window = WindowManager.MainWindow;

		// check the window resize
		if (lastHeight != window.SelfGui.Height || lastWidht != window.SelfGui.Width)
		{
			lastHeight = window.SelfGui.Height;
			lastWidht = window.SelfGui.Width;

			OnWindowResize();
		}

		// get points for intersection
		var mouse_coord = Input.MousePosition;
		vec3 dir = Game.Player.GetDirectionFromMainWindow(mouse_coord.x, mouse_coord.y);
		vec3 p0 = (vec3)Game.Player.WorldPosition;
		vec3 p1 = p0 + dir * 25.0f;

		// try to find the intersection with UI element
		UiElement uiElement = null;

		Unigine.Object obj = World.GetIntersection(p1, p0, uiMask, intersection);
		if (obj != null)
			uiElement = ComponentSystem.GetComponent<UiElement>(obj);

		// enter / leave
		if (!isSelect)
		{
			if (uiElement != null && uiElement.Id == Id)
				OnEnter();
		}
		else
		{
			if (uiElement == null || uiElement.Id != Id)
				OnLeave();
		}

		// invoke the mouse click event
		if (isSelect && Input.IsMouseButtonDown(Input.MOUSE_BUTTON.LEFT))
			OnClick();
	}

	void Shutdown()
	{
		uiElements.Remove(this);
	}

	private void OnWindowResize()
	{
		// get a new unit per pixel value
		float fov = Game.Player.Fov;
		float halfHeight = (float)MathLib.Abs(uiPlane.Position.z) * MathLib.Tan(MathLib.DEG2RAD * fov / 2);
		unitPerPixel = 2.0f * halfHeight / lastHeight;

		// update the element's position
		if (align != HorizontalAlign.Center)
		{
			vec3 pos = (vec3)node.Position;

			if (align == HorizontalAlign.Left)
				pos.x = -lastWidht / 2.0f * unitPerPixel + size.x / 2.0f + horizontalOffset;
			else if (align == HorizontalAlign.Right)
				pos.x = lastWidht / 2.0f * unitPerPixel - size.x / 2.0f + horizontalOffset;

			node.Position = pos;
		}
	}

	private void OnEnter()
	{
		isSelect = true;

		// set a visual effect on selection
		node.Scale = sourceScale * selectScale;
		uiObject.SetMaterialParameterFloat4("emission_color", selectEmission, 0);
	}

	private void OnLeave()
	{
		isSelect = false;

		// remove a visual effect when the UI element is not selected anymore
		node.Scale = sourceScale;
		uiObject.SetMaterialParameterFloat4("emission_color", sourceEmission, 0);
	}

	private void OnClick()
	{
		onClick?.Invoke(uiType);
	}
}
