using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitNamePanel : MonoBehaviour {

    // Importante. Todas las unidades deben instanciar uno de estos y ponerlo como child del Canvas del nivel, ademas, las unidades deben dejar una referencia a si mismas en
    // el parametro unitReferenced al instanciarlo.

	private bool mouseOver = false;
	private bool selected = false;
	private float animationSpeed = 6f;

    private GlobalControl globalControl;
	private CanvasGroup CG;
    public Unidad unitReferenced;

	public CanvasGroup SelectedCG;
    public Transform SelectedL;
    public Transform SelectedR;
    
	public Image HealthFill;
	public Text UnitName;
	public Text HealthNum;

    private const float FOLLOW_UNIT_OFFSET_Y = 100f;

	void Start ()
	{
        globalControl = GameObject.Find("Control").GetComponent<GlobalControl>();

		CG = this.GetComponent<CanvasGroup> ();
       /* unitReferenced = this.gameObject.GetComponent<Unidad>();
        this.gameObject.transform.parent = GameObject.Find("Canvas").transform;*/
	}
	void Update () {
		if (mouseOver || selected) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one * 1f, Time.deltaTime * animationSpeed);
			CG.alpha = Mathf.MoveTowards (CG.alpha, 1, Time.deltaTime * animationSpeed);
		} else {
			transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one * 0.75f, Time.deltaTime * animationSpeed);
			CG.alpha = Mathf.MoveTowards (CG.alpha, 0.33f, Time.deltaTime * animationSpeed);
		}
        if (unitReferenced != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(unitReferenced.transform.position) + (Vector3.up * FOLLOW_UNIT_OFFSET_Y);
            UpdateDisplayedInfo();
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	public void UpdateDisplayedInfo()
	{
		HealthFill.fillAmount = unitReferenced.Vida/unitReferenced.SaludMaxima * 0.5f;
		HealthNum.text = unitReferenced.Vida + "/" + unitReferenced.SaludMaxima;
		UnitName.text = unitReferenced.name;
	}
	public void OnMouseEnter()
	{
		mouseOver = true;
	}
	public void OnMouseExit()
	{
		mouseOver = false;
	}
	public void OnMouseClick()
	{
		Select ();
	}
	public void Select()
	{
        if (IngameInterfaceManager.currentInstance == null)
            return;
        if (IngameInterfaceManager.currentInstance.currentHudState == HUDState.actionSelected)
            return;
        globalControl.UnidadSeleccionada(unitReferenced);
        IngameInterfaceManager.currentInstance.OpenUnitInfo(this);
        selected = true;
		StopCoroutine ("SelectedAnimation");
		StartCoroutine ("SelectedAnimation");
	}
	public void DeSelect()
	{
        globalControl.Deseleccionar(unitReferenced);
		selected = false;
	}
	IEnumerator SelectedAnimation()
	{
		SelectedCG.alpha = 0;
        SelectedL.localRotation = SelectedR.localRotation = Quaternion.identity;
		float turnSpeed = 20;
		while (selected) {
            SelectedCG.alpha = Mathf.MoveTowards(SelectedCG.alpha, 1, Time.deltaTime * 4f);
			SelectedL.Rotate (0, 0, turnSpeed * Time.deltaTime);
            SelectedR.Rotate(0, 0, -turnSpeed * Time.deltaTime);
            yield return null;
		}
		SelectedCG.alpha = 0;
			
	}

}

