using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameInterfaceManager : MonoBehaviour {

	[Header("Unit Info Panel")]
	public CanvasGroup unitPanel_CG;
	public Transform unitPanel_NameShade;
	public Transform unitPanel_ColouredDeco;

	public CanvasGroup CG_alwaysOnDisplay;
	public CanvasGroup CG_cityInfo;
	public CanvasGroup CG_buildingInfo;

	private bool unitInfoDisplayed = false;
	private bool cityInfoDisplayed = false;
	private bool buildingInfoDisplayed = false;

	private Vector3 initPos_alwaysOnDisplay;
	private Vector3 initPos_unitInfo_parent;
	private Vector3 initPos_unitInfo_nameShade;
	private Vector3 initPos_unitInfo_colouredDeco;

	private Vector3 initPos_cityInfo;
	private Vector3 initPos_buildingInfo;

	private bool animationInProgress = false;
	private UnitNamePanel unitPanelSelected;

	public static IngameInterfaceManager currentInstance;

	void Awake()
	{
		currentInstance = this;
	}
	void Start()
	{
		SaveInterfaceBasePositions ();
		StartCoroutine ("OpenAnim_alwaysOnDisplay");
	}
	// Update solo estara en modo debug.
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButtonDown (2)) {
			CloseUnitInfo ();
		}
	}
	void SaveInterfaceBasePositions()
	{
		initPos_alwaysOnDisplay = CG_alwaysOnDisplay.transform.localPosition;
		initPos_cityInfo = CG_cityInfo.transform.localPosition;

		initPos_unitInfo_parent = unitPanel_CG.transform.localPosition;
		initPos_unitInfo_nameShade = unitPanel_NameShade.transform.localPosition;
		initPos_unitInfo_colouredDeco = unitPanel_ColouredDeco.transform.localPosition;

		initPos_buildingInfo = CG_buildingInfo.transform.localPosition;
	}
	public void OpenUnitInfo(UnitNamePanel UNP)
	{
		unitPanelSelected = UNP;
		StartCoroutine ("OpenAnim_unitInfo");
		StopCoroutine ("CloseAnim_unitInfo");
	}
	public void CloseUnitInfo()
	{
		if (!unitInfoDisplayed)
			return;
		unitPanelSelected.DeSelect ();
		unitPanelSelected = null;
		StartCoroutine ("CloseAnim_unitInfo");
		StopCoroutine ("OpenAnim_unitInfo");
	}

	// Co-rutinas de animacion
	// Para administrar mejor las animaciones, por comodidad, limpieza de codigo, y facilidad de lectura, utilizo co-rutinas
	// ========================================================================================================================

	IEnumerator OpenAnim_unitInfo()
	{
		unitPanel_CG.gameObject.SetActive (true);
		float t = 0;
		float animSpeed = 6f;
		unitInfoDisplayed = true;
		animationInProgress = true;
		while (t < 1) {
			t = Mathf.MoveTowards (t, 1, Time.deltaTime * animSpeed * ((1-t) + 0.05f));
			unitPanel_CG.alpha = t;
			unitPanel_ColouredDeco.transform.localPosition = initPos_unitInfo_colouredDeco + Vector3.left * 100f * (1-t);
			unitPanel_NameShade.transform.localRotation = Quaternion.Euler (0, 0, ((1 - t) * 100) + 45);
			unitPanel_NameShade.transform.localPosition = initPos_unitInfo_nameShade + Vector3.left * 400f * (1 - t);
			unitPanel_ColouredDeco.transform.localRotation = Quaternion.Euler (0, 0, (1 - t) * 600);
			yield return null;
		}
		animationInProgress = false;
	}
	IEnumerator CloseAnim_unitInfo()
	{
		float t = 1;
		float animSpeed = 6f;
		unitInfoDisplayed = false;
		while (t > 0) {
			t = Mathf.MoveTowards (t, 0, Time.deltaTime * animSpeed * ((1-t) + 0.05f));
			unitPanel_CG.alpha = t;
			unitPanel_ColouredDeco.transform.localPosition = initPos_unitInfo_colouredDeco + Vector3.left * 100f * (1-t);
			unitPanel_NameShade.transform.localRotation = Quaternion.Euler (0, 0, ((1 - t) * 100) + 45);
			unitPanel_NameShade.transform.localPosition = initPos_unitInfo_nameShade + Vector3.left * 400f * (1 - t);
			unitPanel_ColouredDeco.transform.localRotation = Quaternion.Euler (0, 0, (1 - t) * 600);
			yield return null;
		}
		unitPanel_CG.gameObject.SetActive (false);
	}
	IEnumerator OpenAnim_alwaysOnDisplay()
	{
		float t = 0;
		float animSpeed = 4f;
		CG_alwaysOnDisplay.gameObject.SetActive (true);
		while (t < 1) {
			t = Mathf.MoveTowards (t, 1, Time.deltaTime * animSpeed);
			CG_alwaysOnDisplay.alpha = t;
			CG_alwaysOnDisplay.transform.localPosition = initPos_alwaysOnDisplay + Vector3.up * 50f * (1-t);
			yield return null;
		}
	}

}
