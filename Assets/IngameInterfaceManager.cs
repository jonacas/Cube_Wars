using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum HUDState
{
    nothingSelected, unitSelected, actionSelected
}

public class IngameInterfaceManager : MonoBehaviour {


    public GameObject endTurnButton;
    [Header("Unit Panels Parent")]
    public Transform unitPanels_Parent;
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
    public GameObject createUnits_panel;
    public GameObject unitActions_create_villager;
    public GameObject unitActions_create_warriord;
    public GameObject unitActions_create_explorer;
    [Header("ActionInProgress")]
    public CanvasGroup actionInProgress_CG;
    public CanvasGroup actionInProgress_highlight;
    public Transform actionInProgress_panel;
    public Text actionInProgress_info;
    [Header("Create Unit Buttons")]
    public CanvasGroup createUnitButtons_CG;

    public enum ActionInProgressMode
    {
        move, attack, build, create, gather
    }


    public CanvasGroup CG_alwaysOnDisplay;
	public CanvasGroup CG_cityInfo;
	public CanvasGroup CG_buildingInfo;

	public HUDState currentHudState = HUDState.nothingSelected;

	private Vector3 initPos_alwaysOnDisplay;
	private Vector3 initPos_unitInfo_parent;
	private Vector3 initPos_unitInfo_nameShade;
	private Vector3 initPos_unitInfo_colouredDeco;

    private Vector3 initPos_unitActions_name;
    private Vector3 initPos_unitActions_panel;
    private Vector3 initPos_buildUnit_panel;

    private Vector3 initPos_endTurnButton;


    private Vector3 initPos_cityInfo;
	private Vector3 initPos_buildingInfo;

    private Vector3 initPos_createUnitButtons;

	private bool animationInProgress = false;
    private bool createUnitSubPanelOpen = false;
    private bool createBuildingSubPanelOpen = false;
	public UnitNamePanel unitPanelSelected;

	public static IngameInterfaceManager currentInstance;

    private bool interactable;

	void Awake()
	{
		currentInstance = this;
	}
	void Start()
	{
		SaveInterfaceBasePositions ();
		StartCoroutine ("OpenAnim_alwaysOnDisplay");

        StartTurn();
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

        initPos_buildUnit_panel = createUnits_panel.transform.localPosition;

        initPos_endTurnButton = endTurnButton.transform.localPosition;
	}

    void OpenActionInProgress(ActionInProgressMode mode)
    {
        if (interactable)
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
            //print(currentHudState);
            StopCoroutine("CloseAnim_unitInfo");
            StartCoroutine("CloseAnim_unitInfo");
        }
    }


    #region BUTTON HANDLERS
    // Si no se hace asi, no se puede referenciar la accion del script al boton desde el inspector, hashtag unity
    public void OnButtonClick_Create()
    {
        //if (GlobalControl.currentInstance.SeleccionarAccion(AccionID.create, false))
        StartCoroutine("ShowCreateUnitButtons");
    }
    public void OnButtonClick_Move()
    {
        StartCoroutine("HideCreateUnitButtons");
        if(GlobalControl.currentInstance.SeleccionarAccion(AccionID.move))
            OpenActionInProgress(ActionInProgressMode.move);
    }
    public void OnButtonClick_Attack() 
    {
        StartCoroutine("HideCreateUnitButtons");
        if(GlobalControl.currentInstance.SeleccionarAccion(AccionID.attack))
            OpenActionInProgress(ActionInProgressMode.attack); 
    }
    public void OnButtonClick_Build() 
    {
        StartCoroutine("HideCreateUnitButtons");
        if (GlobalControl.currentInstance.SeleccionarAccion(AccionID.build))
            OpenActionInProgress(ActionInProgressMode.build);
    }

    public void OnButtonClick_CreateExplorer()
    {
        if (!createUnitSubPanelOpen)
            return;
        StartCoroutine("HideCreateUnitButtons");
        StageData.currentInstance.unidadACrear = TipoUnidad.Explorer;
        if (GlobalControl.currentInstance.SeleccionarAccion(AccionID.create))
            OpenActionInProgress(ActionInProgressMode.create);
    }

    public void OnButtonClick_CreateVillager()
    {
        if (!createUnitSubPanelOpen)
            return;
        StartCoroutine("HideCreateUnitButtons");
        StageData.currentInstance.unidadACrear = TipoUnidad.Worker;
        if (GlobalControl.currentInstance.SeleccionarAccion(AccionID.create))
            OpenActionInProgress(ActionInProgressMode.create);
    }

    public void OnButtonClick_CreateWarrior()
    {
        if (!createUnitSubPanelOpen)
            return;
        StartCoroutine("HideCreateUnitButtons");
        StageData.currentInstance.unidadACrear = TipoUnidad.Warrior;
        if (GlobalControl.currentInstance.SeleccionarAccion(AccionID.create))
            OpenActionInProgress(ActionInProgressMode.create);
    }

    public void OnButtonClick_Gather() { OpenActionInProgress(ActionInProgressMode.gather); }

    public void OnButtonClick_EndTurn()
    {
        JugadorHumano huma = (JugadorHumano)StageData.currentInstance.GetPartidaActual().Jugadores[GlobalData.JUGADOR_HUMANO];
        huma.TerminarTurno();
        StartCoroutine("CloseAnim_actionInProgress");
        StartCoroutine("CloseAnim_unitInfo");
        StartCoroutine("HideCreateUnitButtons");
        StartCoroutine("HideEndTurnButton");
        interactable = false;
    }

    #endregion




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
        if (interactable)
        {
            if (!(currentHudState == HUDState.nothingSelected || currentHudState == HUDState.unitSelected))
                return;
            if (unitPanelSelected != null)
            {
                unitPanelSelected.DeSelect();
            }
            unitPanelSelected = UNP;
            UpdateUnitDisplayedInfo();
            StartCoroutine("OpenAnim_unitInfo");
            StopCoroutine("CloseAnim_unitInfo");
        }
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
        //TODO: ESTO DE AQUI
        // Desactivamos todas y activamos las que correspondan.
        /*
        unitActions_attack.gameObject.SetActive(false);
        unitActions_move.gameObject.SetActive(false);
        unitActions_gather.gameObject.SetActive(false);
        unitActions_build.gameObject.SetActive(false);
        unitActions_create.gameObject.SetActive(false);
        
        foreach (Accion action in unitPanelSelected.unitReferenced.Acciones)
        {
            //COMPROBAR SI TIENE CADA ACCION, E IR ACTIVANDOLAS UNA A UNA
        }
        */
        unitPanel_unitName.text = unitPanelSelected.unitReferenced.name;
        unitPanel_unitHealth.text = unitPanelSelected.unitReferenced.Vida + "/" + unitPanelSelected.unitReferenced.SaludMaxima;

    }

	// Co-rutinas de animacion
	// Para administrar mejor las animaciones, por comodidad, limpieza de codigo, y facilidad de lectura, utilizo co-rutinas
	// ========================================================================================================================

	IEnumerator OpenAnim_unitInfo()
	{
        if (interactable)
        {
            unitPanel_CG.gameObject.SetActive(true);
            unitActions_CG.gameObject.SetActive(true);
            highlightFlash.alpha = 0;
            highlightFlash.transform.localScale = Vector3.one;
            StopCoroutine("UnitCommandsHighlightFlash");
            StartCoroutine("UnitCommandsHighlightFlash");

            currentHudState = HUDState.unitSelected;
            animationInProgress = true;

            float t = 0;
            float animSpeed = 6f;

            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed * ((1 - t) + 0.05f));
                unitPanel_CG.alpha = t;
                unitPanel_ColouredDeco.transform.localPosition = initPos_unitInfo_colouredDeco + Vector3.left * 100f * (1 - t);
                unitPanel_NameShade.transform.localRotation = Quaternion.Euler(0, 0, ((1 - t) * 100) + 45);
                unitPanel_NameShade.transform.localPosition = initPos_unitInfo_nameShade + Vector3.left * 400f * (1 - t);
                unitPanel_ColouredDeco.transform.localRotation = Quaternion.Euler(0, 0, (1 - t) * 600);

                unitActions_CG.alpha = t;
                unitActions_Name.transform.localPosition = initPos_unitActions_name + Vector3.down * 400 * (1 - t);
                unitActions_Panel.transform.localPosition = initPos_unitActions_panel + Vector3.right * 200 * (1 - t);
                yield return null;
            }
            animationInProgress = false;
        }
	}


	IEnumerator CloseAnim_unitInfo()
    {
        highlightFlash.alpha = 0;
        StopCoroutine("UnitCommandsHighlightFlash");

        StartCoroutine("HideCreateUnitButtons");
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
        if (interactable)
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
    IEnumerator ShowCreateUnitButtons()
    {
        createUnitButtons_CG.gameObject.SetActive(true);
        createUnitButtons_CG.transform.localPosition = unitActions_create.transform.localPosition + Vector3.left * 600;
        initPos_createUnitButtons = createUnitButtons_CG.transform.localPosition;

        float t = 0;
        float animSpeed = 10f;

        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed * ((1 - t) + 0.05f));
            createUnitButtons_CG.alpha = t;
            createUnitButtons_CG.transform.localPosition = initPos_createUnitButtons + Vector3.right * 200 * (1 - t);
            yield return null;
        }
        createUnitSubPanelOpen = true;
    }
    IEnumerator HideCreateUnitButtons()
    {
        createUnitSubPanelOpen = false;
        float t = 1;
        float animSpeed = 10f;

        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed * ((1 - t) + 0.05f));
            createUnitButtons_CG.alpha = t;
            createUnitButtons_CG.transform.localPosition = initPos_createUnitButtons + Vector3.right * 200 * (1 - t);
            yield return null;
        }
        createUnitButtons_CG.gameObject.SetActive(false);
    }

    IEnumerator HideEndTurnButton()
    {
        float t = 1;
        float animSpeed = 6f;

        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed * ((1 - t) + 0.05f));
            endTurnButton.transform.localPosition = initPos_endTurnButton + Vector3.down * 200 * (1 - t);
            yield return null;
        }

    }

    public void StartTurn()
    {
        interactable = true;
    }

    public void MakeInteractable()
    {
        StopCoroutine("HideEndTurnButton");
        //print("ColcandoBotonEndTurn");
        interactable = true;
        endTurnButton.transform.localPosition = initPos_endTurnButton;
        //endTurnButton.transform.localPosition = new Vector3(endTurnButton.transform.localPosition.x, endTurnButton.transform.localPosition.y + 150, endTurnButton.transform.localPosition.z);
    }

}
