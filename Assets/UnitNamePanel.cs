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
	private CanvasGroup CG;
    public Unidad unitReferenced;

	public CanvasGroup SelectedCG;
    public Transform SelectedL;
    public Transform SelectedR;
    
	public Image HealthFill;
	public Text UnitName;
	public Text HealthNum;

	void Start ()
	{
		CG = this.GetComponent<CanvasGroup> ();
	}
	void Update () {
		if (mouseOver || selected) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one * 1f, Time.deltaTime * animationSpeed);
			CG.alpha = Mathf.MoveTowards (CG.alpha, 1, Time.deltaTime * animationSpeed);
		} else {
			transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one * 0.75f, Time.deltaTime * animationSpeed);
			CG.alpha = Mathf.MoveTowards (CG.alpha, 0.33f, Time.deltaTime * animationSpeed);
		}
	}
	public void UpdateDisplayedInfo()
	{
		// Leer de algun lado la informacion de la imagen y ajustar

		//HealthFill.fillAmount = HEALTHPERCENT * 0.5f;
		//HealthNum.text = HEALTHCURRENT + "/" + HEALTHMAX;
		//UnitName.text = UNITNAME;
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
		IngameInterfaceManager.currentInstance.OpenUnitInfo (this);
		Select ();
	}
	public void Select()
	{
		selected = true;
		StopCoroutine ("SelectedAnimation");
		StartCoroutine ("SelectedAnimation");
	}
	public void DeSelect()
	{
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

