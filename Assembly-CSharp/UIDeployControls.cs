using System;

public class UIDeployControls : global::CanvasGroupDisabler
{
	public void Start()
	{
		this.btnPrev.SetAction("h", string.Empty, 0, true, null, null);
		this.btnNext.SetAction("h", string.Empty, 0, false, null, null);
		this.btnDeploy.SetAction("action", string.Empty, 0, false, null, null);
	}

	public global::ImageGroup btnPrev;

	public global::ImageGroup btnNext;

	public global::ImageGroup btnDeploy;
}
