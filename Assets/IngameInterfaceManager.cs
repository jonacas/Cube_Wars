using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameInterfaceManager : MonoBehaviour {

	[Header("Unit Info Panel")]
	public CanvasGroup unitPanel_CG;
	public Transform unitPanel_NameShade;
	public Transform unitPanel_ColouredDeco;
    public Text unitPanel_unitName;
    public Text unitPanel_unitHealth;
    [Header("Unit Action Panel")]
    public CanvasGroup unitActions_CG;
    public CanvasGroup highlightFlash;
    public Transform unitActions_Name;
    public Transform unitActions_Panel;
    public GameObject unitActions_attack;
    public GameObject unitActions_move;
    public GameObject unitActions_gather;
    public GameObject unitActions_create;
    public GameObject unitActions_build;
    [Header("ActionInProgress")]
    public CanvasGroup actionInProgress_CG;
    public CanvasGroup actionInProgress_highlight;
    public Transform actionInProgress_panel;
    public Text actionInProgress_info;

    public enum HUDState
    {
        nothingSelected, unitSelected, actionSelected
    }
    public enum ActionInProgressMode
    {
        move, attack, build, create, gather
    }


    public CanvasGroup CG_alwaysOnDisplay;
	public CanvasGroup CG_cityInfo;
	public CanvasGroup CG_buildingInfo;

	private HUDState currentHudState = HUDState.nothingSelected;

	private Vector3 initPos_alwaysOnDisplay;
	private Vector3 initPos_unitInfo_parent;
	private Vector3 initPos_unitInfo_nameShade;
	private Vector3 initPos_unitInfo_colouredDeco;

    private Vector3 initPos_unitActions_name;
    private Vector3 initPos_unitActions_panel;


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

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButtonDown (1)) {
            if (currentHudState == HUDState.unitSelected)
                CloseUnitInfo();
            else if (currentHudState == HUDState.actionSelected)
                CloseActionInProgress();

		}
	}
	void SaveInterfaceBasePositions()
	{
		initPos_alwaysOnDisplay = CG_alwaysOnDisplay.transform.localPosition;
		initPos_cityInfo = CG_cityInfo.transform.localPosition;

		initPos_unitInfo_parent = unitPanel_CG.transform.localPosition;
		initPos_unitInfo_nameShade = unitPanel_NameShade.transform.localPosition;
		initPos_unitInfo_colouredDeco = unitPanel_ColouredDeco.transform.localPosition;

        initPos_unitActions_name = unitActions_Name.transform.localPosition;
        initPos_unitActions_panel = unitActions_Panel.transform.localPosition;

		initPos_buildingInfo = CG_buildingInfo.transform.localPosition;
	}
    void OpenActionInProgress(ActionInProgressMode mode)
    {
        if (currentHudState != HUDState.unitSelected)
            return;
        if (animationInProgress)
            return;
        StopCoroutine("OpenAnim_actionInProgress");
        StartCoroutine("OpenAnim_actionInProgress");
        switch (mode)
        {
            case ActionInProgressMode.move:
                {
                    actionInProgress_info.text = "Select a destination";
                    break;
                }
            case ActionInProgressMode.attack:
                {
                    actionInProgress_info.text = "Select a target";
                    break;
                }
            case ActionInProgressMode.build:
                {
                    actionInProgress_info.text = "Select a place to build";
                    break;
                }
            case ActionInProgressMode.gather:
                {
                    actionInProgress_info.text = "Select a resource";
                    break;
                }
            case ActionInProgressMode.create:
                {
                    actionInProgress_info.text = "Select where to create unit";
                    break;
                }
        }
        currentHudState = HUDState.actionSelected;
        StopCoroutine("CloseAnim_unitInfo");
        StartCoroutine("CloseAnim_unitInfo");
    }

    // Si no se hace asi, no se puede referenciar la accion del script al boton desde el inspector, hashtag unity
    public void OnButtonClick_Create() { OpenActionInProgress(ActionInProgressMode.create); }
    public void OnButtonClick_Move() { OpenActionInProgress(ActionInProgressMode.move); }
    public void OnButtonClick_Attack() { OpenActionInProgress(ActionInProgressMode.attack); }
    public void OnButtonClick_Build() { OpenActionInProgress(ActionInProgressMode.build); }
    public void OnButtonClick_Gather() { OpenActionInProgress(ActionInProgressMode.gather); }
    // En serio que cojones colega.

    public void CloseActionInProgress()
    {
        if (currentHudState != HUDState.actionSelected)
            return;
        if (animationInProgress)
            return;
        StopCoroutine("CloseAnim_actionInProgress");
        StartCoroutine("CloseAnim_actionInProgress");
        StopCoroutine("CloseAnim_unitInfo");
        StopCoroutine("OpenAnim_unitInfo");
        StartCoroutine("OpenAnim_unitInfo");
        currentHudState = HUDState.unitSelected;
    }
	public void OpenUnitInfo(UnitNamePanel UNP)
	{
        if (!(currentHudState == HUDState.nothingSelected || currentHudState == HUDState.unitSelected))
            return;
		unitPanelSelected = UNP;
		StartCoroutine ("OpenAnim_unitInfo");
		StopCoroutine ("CloseAnim_unitInfo");
	}
	public void CloseUnitInfo()
	{
		if (!(currentHudState == HUDState.unitSelected))
			return;
		unitPanelSelected.DeSelect ();
		unitPanelSelected = null;
        currentHudState = HUDState.nothingSelected;
		StartCoroutine ("CloseAnim_unitInfo");
		StopCoroutine ("OpenAnim_unitInfo");
	}
    void UpdateUnitDisplayedInfo()
    {
        // Desactivamos todas y activamos las que correspondan.
        /*
        unitActions_attack.gameObject.SetActive(false);
        unitActions_move.gameObject.SetActive(false);
        unitActions_gather.gameObject.SetActive(false);
        unitActions_build.gameObject.SetActive(false);
        unitActions_create.gameObject.SetActive(false);
        */
        foreach (Accion action in unitPanelSelected.unitReferenced.Acciones)
        {
            //COMPROBAR SI TIENE CADA ACCION, E IR ACTIVANDOLAS UNA A UNA
        }
        //unitPanel_unitName.text = unitPanelSelected.unitReferenced.... Nombre?;
        //unitPanel_unitHealth.text = unitPanelSelected.unitReferenced...vidaActual ? +"/" + unitPanelSelected.unitReferenced...vidaTotal ?;

    }

	// Co-rutinas de animacion
	// Para administrar mejor las animaciones, por comodidad, limpieza de codigo, y facilidad de lectura, utilizo co-rutinas
	// ========================================================================================================================

	IEnumerator OpenAnim_unitInfo()
	{
		unitPanel_CG.gameObject.SetActive (true);
        unitActions_CG.gameObject.SetActive(true);
        highlightFlash.alpha = 0;
        highlightFlash.transform.localScale = Vector3.one;
        StopCoroutine("UnitCommandsHighlightFlash");
        StartCoroutine("UnitCommandsHighlightFlash");

        currentHudState = HUDState.unitSelected;
        animationInProgress = true;

        float t = 0;
		float animSpeed = 6f;

		while (t < 1) {
			t = Mathf.MoveTowards (t, 1, Time.deltaTime * animSpeed * ((1-t) + 0.05f));
			unitPanel_CG.alpha = t;
			unitPanel_ColouredDeco.transform.localPosition = initPos_unitInfo_colouredDeco + Vector3.left * 100f * (1-t);
			unitPanel_NameShade.transform.localRotation = Quaternion.Euler (0, 0, ((1 - t) * 100) + 45);
			unitPanel_NameShade.transform.localPosition = initPos_unitInfo_nameShade + Vector3.left * 400f * (1 - t);
			unitPanel_ColouredDeco.transform.localRotation = Quaternion.Euler (0, 0, (1 - t) * 600);

            unitActions_CG.alpha = t;
            unitActions_Name.transform.localPosition = initPos_unitActions_name + Vector3.down * 400 * (1 - t);
            unitActions_Panel.transform.localPosition = initPos_unitActions_panel + Vector3.right * 200 * (1 - t);
            yield return null;
		}
		animationInProgress = false;
	}
	IEnumerator CloseAnim_unitInfo()
    {
        highlightFlash.alpha = 0;
        StopCoroutine("UnitCommandsHighlightFlash");

        float t = 1;
		float animSpeed = 6f;

		while (t > 0) {
			t = Mathf.MoveTowards (t, 0, Time.deltaTime * animSpeed * ((1-t) + 0.05f));
			unitPanel_CG.alpha = t;
			unitPanel_ColouredDeco.transform.localPosition = initPos_unitInfo_colouredDeco + Vector3.left * 100f * (1-t);
			unitPanel_NameShade.transform.localRotation = Quaternion.Euler (0, 0, ((1 - t) * 100) + 45);
			unitPanel_NameShade.transform.localPosition = initPos_unitInfo_nameShade + Vector3.left * 400f * (1 - t);
			unitPanel_ColouredDeco.transform.localRotation = Quaternion.Euler (0, 0, (1 - t) * 600);

            unitActions_CG.alpha = t;
            unitActions_Name.transform.localPosition = initPos_unitActions_name + Vector3.down * 400 * (1 - t);
            unitActions_Panel.transform.localPosition = initPos_unitActions_panel + Vector3.right * 200 * (1 - t);
            yield return null;
		}
		unitPanel_CG.gameObject.SetActive (false);
        unitActions_CG.gameObject.SetActive(false);
	}
    IEnumerator UnitCommandsHighlightFlash()
    {
        float animSped = 4f;
        float t = 0;
        float delayBetweenFlashes = 0.5f;
        while (true)
        {
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSped);
                highlightFlash.alpha = (1-t);
                highlightFlash.transform.localScale =new Vector3(0.15f + t*2f, 1, 1);
                yield return null;
            }
            yield return new WaitForSeconds(delayBetweenFlashes);
            t = 0;
        }
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
    IEnumerator OpenAnim_actionInProgress()
    {
        animationInProgress = true;
        float t = 0;
        float animSpeed = 4f;
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            actionInProgress_CG.alpha = t;
            actionInProgress_panel.transform.localScale = new Vector3(1, t, 1);
            yield return null;
        }
        actionInProgress_highlight.alpha = 0;
        StartCoroutine("ActionInProgressHighlightAnimation");
        animationInProgress = false;
    }
    IEnumerator CloseAnim_actionInProgress()
    {
        StopCoroutine("ActionInProgressHighlightAnimation");
        float t = 1;
        float animSpeed = 4f;
        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed);
            actionInProgress_CG.alpha = t;
            actionInProgress_panel.transform.localScale = new Vector3(1, t, 1);
            yield return null;
        }
        actionInProgress_highlight.alpha = 0;
    }
    IEnumerator ActionInProgressHighlightAnimation()
    {
        float animSped = 4f;
        float t = 0;
        float delayBetweenFlashes = 0.5f;
        while (true)
        {
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSped);
                actionInProgress_highlight.alpha = (1 - t);
                actionInProgress_highlight.transform.localScale = new Vector3(1, t*1.25f, 1);
                yield return null;
            }
            yield return new WaitForSeconds(delayBetweenFlashes);
            t = 0;
        }
    }

}
