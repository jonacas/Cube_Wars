using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	private const float MOVE_SPEED = 9.4728f;
	private const float TURN_RATE = 270f;
	public float PATH_REACH_NODE_THS = 3f;
	private const float PATH_REACH_PLAYER_THS = 1f;
	private const float PATH_STUCK_TIMELIMIT = 5f;
    private const float RUIDO_AL_CORRER = 15f;

	private const float OUT_OF_COMBAT_VIEWDIST = 25;
	private const float OUT_OF_COMBAT_FOV = 50;
	private const float IN_COMBAT_FOV = 100;
	private const float IN_COMBAT_VIEWDIST = 100;

	private const float WAIT_TIME_AFTER_PATROL_NODE_REACHED = 1f;
	private const float WAIT_TIME_AFTER_PATROL_FINISHED = 0.25f;

	private float moveSpeedMultiplier = 1;
	private List<Vector3> targetPathPositions;
	private Transform target_player_position;
	private Transform target_path_position;
	private Quaternion target_rotation;
	private GameObject playerReference; // the player gameobject
	private RaycastHit objectHitted;
	private Vector2 VectorBetweenPlayerAndEnemy, VectorFordwardEnemy;
	private EnemyState currentState;
	private Vector3 beforeAlert;
	private Vector3 lastKnownPlayerPosition;
    private Vector3 previousValidUnstuckNode;
    private Vector3 positionFromAnotherZone;
    private int raycastLayer = 1 << 8 | 1;
    private bool caminoListo;
    
    
    public Node[,] grafo;
    public bool playerCaptured = false;
    public float thresholdEnemyCapture = 5f;
    public Light enemyLight;
    public int enemyIDStage;
    public int enemyIDStagePart;
    public float SafetyAngle = 50f, SafetyDistance = 15f; // no hay na mas que decir
    public float followPlayerThs;

	public List<Transform> patrolPositions;

	public enum EnemyState
	{
		Patrolling,
		Alert,
		ReturnToPreAlert,
		InCombat,
        AlertFromAnotherZone
	}

	void Awake()
	{}



    void Start()
	{
        grafo = GameObject.Find("GrafoEscena").GetComponent<CreacionGrafo>().CrearGrafo();
		beforeAlert = transform.position;
		targetPathPositions = new List<Vector3> ();
		playerReference = StageData.currentInstance.GetPlayer ();
		StageData.currentInstance.enemiesInStage.Add (this);
        SetNewState(EnemyState.Patrolling);
	}
	public void SetNewState(EnemyState newstate)
	{
		if (playerCaptured)
			return;
		
		StopAllCoroutines (); // <- IMPORTANTE
		currentState = newstate;
		targetPathPositions.Clear ();

		switch (newstate) {
		case EnemyState.InCombat:
			{
				//print ("Setting new state to BehaviourInCombat");
				currentState = EnemyState.InCombat;
				StageData.currentInstance.SendAlert(StageData.currentInstance.GetPlayer().transform.position, enemyIDStage, enemyIDStagePart);
				SafetyAngle = IN_COMBAT_FOV;
				SafetyDistance = IN_COMBAT_VIEWDIST;
				moveSpeedMultiplier = 0.5f;
				StartCoroutine ("BehaviourInCombat");
				break;
			}
		case EnemyState.Alert:
			{
				//print ("Setting new state to BehaviourAlert");
				currentState = EnemyState.Alert;
				beforeAlert = transform.position;
				StartCoroutine ("BehaviourAlert");
				moveSpeedMultiplier = 0.5f;
				SafetyAngle = IN_COMBAT_FOV;
				SafetyDistance = IN_COMBAT_VIEWDIST;
				break;
			}
		case EnemyState.ReturnToPreAlert:
			{
				currentState = EnemyState.ReturnToPreAlert;
				//print ("Setting new state to BehaviourPreAlert");
				StartCoroutine ("BehaviourReturnToPreAlert");
				moveSpeedMultiplier = 0.4f;
				SafetyAngle = OUT_OF_COMBAT_FOV;
				SafetyDistance = OUT_OF_COMBAT_VIEWDIST;

				break;
			}
		case EnemyState.Patrolling:
			{
                StageData.currentInstance.CancelAlertToOtherZones(enemyIDStage, enemyIDStagePart);
				//print ("Setting new state to Patrolling");
				currentState = EnemyState.Patrolling;
				StartCoroutine ("BehaviourPatrol");
				moveSpeedMultiplier = 0.3f;
				SafetyAngle = OUT_OF_COMBAT_FOV;
				SafetyDistance = OUT_OF_COMBAT_VIEWDIST;
				break;
			}
        case EnemyState.AlertFromAnotherZone:
            {
				currentState = EnemyState.AlertFromAnotherZone;
                StartCoroutine("AlertFromAnotherZone");
                moveSpeedMultiplier = 0.5f;
                SafetyAngle = IN_COMBAT_FOV;
                SafetyDistance = IN_COMBAT_VIEWDIST;
                break;
            }
		}
	}
	IEnumerator BehaviourAlert()
	{
		//print (lastKnownPlayerPosition);
        if (lastKnownPlayerPosition != null)
        {
            caminoListo = false;
            print("camino listo =" + caminoListo);
            StageData.currentInstance.GetPathToTarget(transform.position, lastKnownPlayerPosition, this);
        }

        while (!caminoListo)
        {
            //print(this.gameObject.name + " Esperando ruta");
            yield return null;
        }

		while (!IsCurrentPathFinished ()) {
			if (IsPlayerInVisionRange ()) {
				SetNewState (EnemyState.InCombat);
				StageData.currentInstance.SendAlert (playerReference.transform.position, enemyIDStage, enemyIDStagePart);
				//print ("Player found, switching to InCombat");
			}
			yield return null;
		}
		//print ("Nothing found on alerted position, switching to ReturnToPreAlert");
        StageData.currentInstance.CancelAlertToOtherZones(enemyIDStage, enemyIDStagePart);
		SetNewState (EnemyState.ReturnToPreAlert);
	}

    IEnumerator AlertFromAnotherZone()
    {
        caminoListo = false;
        StageData.currentInstance.GetPathToTarget(transform.position, positionFromAnotherZone, this);

        while (!caminoListo)
        {
            yield return null;
        }

        while (!IsCurrentPathFinished())
        {
            yield return null;
            if (IsPlayerInVisionRange())
            {
                SetNewState(EnemyState.InCombat);
                StageData.currentInstance.SendAlert(playerReference.transform.position, enemyIDStage, enemyIDStagePart);
                //print("Player found, switching to InCombat");
            }
        }

        yield return null;
        if (IsPlayerInVisionRange())
        {
            SetNewState(EnemyState.InCombat);
            StageData.currentInstance.SendAlert(playerReference.transform.position, enemyIDStage, enemyIDStagePart);
            //print("Player found, switching to InCombat");
        }
    }


	IEnumerator BehaviourReturnToPreAlert()
	{
		if (beforeAlert != null)
			SetNewPath(StageData.currentInstance.GetPathToTarget(transform.position, beforeAlert, this));
		while (!IsCurrentPathFinished ()) {
			if (IsPlayerInVisionRange ()) {
				SetNewState (EnemyState.InCombat);
				StageData.currentInstance.SendAlert (playerReference.transform.position, enemyIDStage, enemyIDStagePart);
				//print ("Player found, switching to InCombat");
			}
			yield return null;
		}
		//print ("Returned to PreAlert position, switching to Patrol");
		SetNewState (EnemyState.Patrolling);
	}
	IEnumerator BehaviourPatrol()
	{
		for (int i = 0; i < patrolPositions.Count; i++) {
            caminoListo = false;
            //print("camino listo =" + caminoListo);
			StageData.currentInstance.GetPathToTarget(transform.position, patrolPositions[i].transform.position, this);


            while (!caminoListo)
            {
                //print(this.gameObject.name + " Esperando ruta");
                yield return null;
            }

			while (!IsCurrentPathFinished()) {
				yield return null;
			}
			yield return new WaitForSeconds (WAIT_TIME_AFTER_PATROL_NODE_REACHED);
		}
		yield return new WaitForSeconds (WAIT_TIME_AFTER_PATROL_FINISHED);
		StartCoroutine ("BehaviourPatrol");

	}
	IEnumerator BehaviourInCombat()
	{
		while (true) {
            caminoListo = false;
			StageData.currentInstance.GetPathToTarget(transform.position, StageData.currentInstance.GetPlayer().transform.position, this);
            StageData.currentInstance.SendNoise(transform.position, RUIDO_AL_CORRER);
            while (!caminoListo)
            {
                yield return null;
            }

			if (IsPlayerInVisionRange ()) {
				if (Vector3.Distance (transform.position, playerReference.transform.position) < followPlayerThs) {
					StopCoroutine ("FollowPlayer");
					StartCoroutine ("FollowPlayer");
					//print ("Player is close, following directly");
				} else {
					StopCoroutine ("FollowPath");
					StartCoroutine ("FollowPath");
					//print ("Following path to player");
				}
			} else {
				//print ("Lost Sight of player, switching to Alert");
				lastKnownPlayerPosition = playerReference.transform.position;
				SetNewState (EnemyState.Alert);
			}

			yield return null;
		}
	}
	void Update()
	{
		if (Vector3.Distance (transform.position, playerReference.transform.position) < thresholdEnemyCapture 
			&& currentState == EnemyState.InCombat)
		{
			EndGamePanelManager.currentInstance.EndGame ();
			PlayerCatched ();
			//print ("Hemos parado corutina del enemigo");
		}
		else if (!playerCaptured && IsPlayerInVisionRange ()) 
		{
			enemyLight.color = Color.red;
			SetNewState (EnemyState.InCombat);
			//print ("Iniciamos protocolo de aviso a enemigos");
		} 
		else 
		{
			enemyLight.color = Color.white;
		}

	}
	public void SetNewPath(List<Vector3> newPath)
	{
		//print ("Iniciamos el desarrollo de caminos para el jugador");
		if (newPath == null)
			return;
		StopCoroutine ("FollowPath");
		targetPathPositions = newPath;
		StartCoroutine ("FollowPath");
		//print ("Parado y restarteado nuevo camino de enemigo");
	}

	public bool IsPlayerInVisionRange()
	{
		VectorBetweenPlayerAndEnemy = (new Vector2 (playerReference.transform.position.x, playerReference.transform.position.z) - new Vector2 (this.transform.position.x, this.transform.position.z)).normalized;
		VectorFordwardEnemy = new Vector2 (transform.forward.x, transform.forward.z);

		if (Vector3.Distance (transform.position, playerReference.transform.position) < SafetyDistance &&
			Vector2.Angle (VectorBetweenPlayerAndEnemy, VectorFordwardEnemy) < SafetyAngle)
		{
			//print ("Detectamos choque con algo");
			Physics.Raycast (transform.position, (playerReference.transform.position - transform.position).normalized, out objectHitted, 30f, raycastLayer);
			return objectHitted.collider != null && objectHitted.collider.gameObject.tag == "Player";
		} 
		else 
		{
			return false;
		}
	}
	IEnumerator FollowPlayer()
	{
		StopCoroutine ("FollowPath");
		Quaternion auxRotation;
		previousValidUnstuckNode = transform.position;
		while (targetPathPositions.Count > 0) {
			while (true) {
				auxRotation = transform.rotation; 
				transform.LookAt (playerReference.transform); 
				target_rotation = transform.rotation; 
				transform.rotation = auxRotation;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, target_rotation, TURN_RATE * moveSpeedMultiplier * Time.deltaTime);

				transform.Translate (Vector3.forward * MOVE_SPEED * moveSpeedMultiplier * Time.deltaTime); 
				yield return null;
			}
		}
	}
	IEnumerator FollowPath()
	{
		StopCoroutine("FollowPlayer");
		Quaternion auxRotation;
		previousValidUnstuckNode = transform.position;
		while (targetPathPositions.Count > 0) {
			while (Vector3.Distance (transform.position, targetPathPositions [0]) > PATH_REACH_NODE_THS) {
				auxRotation = transform.rotation; 
				transform.LookAt (targetPathPositions [0]); 
				target_rotation = transform.rotation; 
				transform.rotation = auxRotation;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, target_rotation, TURN_RATE * moveSpeedMultiplier * Time.deltaTime);

				transform.Translate (Vector3.forward * MOVE_SPEED * moveSpeedMultiplier * Time.deltaTime); 
				//print ("Nos movemos hacia objetivo");
				yield return null;
			}
			targetPathPositions.RemoveAt (0);
			previousValidUnstuckNode = transform.position;
		}
	}
	public void SendAlertToPosition(Vector3 alertPosition)
	{
		lastKnownPlayerPosition = alertPosition;
		if (currentState != EnemyState.InCombat) {
			SetNewState (EnemyMovement.EnemyState.Alert);
			//print (gameObject.name + ": Alert recieved. checking alerted position.");
		} else {
			//print (gameObject.name + ": Alert recieved. Ignored beacuse already in combat.");
		}

	}

    public void AlertFromAnotherZone(Vector3 positionAlert)
    {
        positionFromAnotherZone = positionAlert;
        SetNewState(EnemyState.AlertFromAnotherZone);
    }

    public void CancelAlertFromAnotherZone()
    {
        if(currentState == EnemyState.AlertFromAnotherZone)
            SetNewState(EnemyState.Patrolling);
    }

	public bool IsEnemyPatrolling() {return currentState == EnemyState.Patrolling;}
	public bool IsCurrentPathFinished() { return targetPathPositions.Count == 0;
	}
	public void PlayerCatched()
	{
		playerCaptured = true;
		StopAllCoroutines ();
	}

    public void ResultadoAEstrella(List<Vector3> camino)
    {
        caminoListo = true;
        //print("camino listo =" + caminoListo);
       // print(this.gameObject.name + " Camino listo" + camino);
        SetNewPath(camino);
        StageData.currentInstance.LimpiarGrafo(grafo);
    }

}
