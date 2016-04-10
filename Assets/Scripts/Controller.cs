using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Controller : MonoBehaviour {

	public GameObject Road;
	public GameObject Coin;
	public List<Material> materials = new List<Material>();

	private List<Transform> Cpoints = new List<Transform>();
	private List<GameObject> CurrentRoads = new List<GameObject>();
	private List<GameObject> CurrentCoins = new List<GameObject>();
    
	private int CurrentPoint = 1;
	private int positionNum = 0;
	private int CurrentRoadId = 0;
	private int LevelCount = 0;
	private float time = 0;
	private float distance = 0, nextDistance = 0;
	private float speed = 1.5f;
	private Transform CenterPoints;

	void Start () {
		CreateNewRoad();
	}

	void CreateNewRoad()
	{
		for (int i = CurrentCoins.Count - 1; i >= 0; i--) {
			if(CurrentCoins[i].transform.position.z < this.transform.position.z)
			{
				DestroyImmediate(CurrentCoins[i]);
				CurrentCoins.RemoveAt(i);
			}
		}
		if(CurrentRoadId > 0)
		{
			if(CurrentRoadId > 1)
			{
				DestroyImmediate(CurrentRoads[CurrentRoadId - 2]);
				CurrentRoads.RemoveAt(0);
				CurrentRoadId--;
			}
			
			CurrentRoads.Add(Instantiate (Road, Cpoints[LevelCount * 19 + LevelCount - 1].position, 
			                                Quaternion.identity) as GameObject);
		}
		else
			CurrentRoads.Add(Instantiate (Road, Vector3.zero, Quaternion.identity) as GameObject);

		CenterPoints = CurrentRoads[CurrentRoadId].transform.FindChild("Plane").FindChild("Points");
		for (int i = 1; i < 21; i++) 
		{
			Transform childPoint = CenterPoints.transform.FindChild("P" + i);
			Cpoints.Add(childPoint);

			int typeNum = Random.Range(0,  materials.Count);
			Vector3 cPOS = Vector3.zero;
			
			if(LevelCount > 0)
				cPOS = new Vector3(childPoint.transform.position.x + Random.Range(-1, 2) * 1.0f,
				                   childPoint.transform.position.y - 0.5f, 
				                   childPoint.transform.position.z);
			else
				cPOS = new Vector3(Cpoints[i - 1].position.x + Random.Range(-1,2) * 1.0f,Cpoints[i - 1].position.y - 0.5f, Cpoints[i - 1].position.z);
			
			GameObject newCoin = (Instantiate(Coin,cPOS,Quaternion.identity) as GameObject);
			CurrentCoins.Add(newCoin);
			newCoin.transform.Rotate(90,0,90);
			newCoin.transform.GetComponent<Renderer>().material = materials[typeNum];
			
			if(typeNum == 0)
				newCoin.name = "Yellow";
			
			else if(typeNum == 1)
				newCoin.name = "Red";
			
			else if(typeNum == 2)
				newCoin.name = "Blue";
			
			else if(typeNum == 3)
				newCoin.name = "Black";
		}


	}
	
	void Update () {
		Vector3 startPoint = new Vector3 (Cpoints [CurrentPoint].position.x + distance, 
		                                  Cpoints [CurrentPoint].position.y - 0.5f, 
		                                  Cpoints [CurrentPoint].position.z);
		Vector3 endPoint = new Vector3 (Cpoints [CurrentPoint + 1].position.x + distance, 
		                                Cpoints [CurrentPoint + 1].position.y - 0.5f, 
		                                Cpoints [CurrentPoint + 1].position.z);
		transform.position = Vector3.Lerp (startPoint, endPoint, time);
		transform.LookAt(Vector3.Lerp(Cpoints[CurrentPoint + 1].transform.position, 
		                                     Cpoints[CurrentPoint + 2].transform.position,time));
		time += Time.deltaTime * speed;

		if(time > 1)
		{
			time = 0;
			CurrentPoint++;
			
			if(Cpoints [CurrentPoint].name == "P17")
			{
				CurrentRoadId++;
				LevelCount++;
				CreateNewRoad();
			}

		}
		
		
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(positionNum != -1)
			{
				positionNum--;
				nextDistance = 1.0f * positionNum;
			}
		}
		
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(positionNum != 1)
			{
				positionNum++;
				nextDistance = 1.0f  * positionNum;
			}
		}
		
		distance = Mathf.Lerp (distance, nextDistance, Time.deltaTime * 3 * speed);
	
	}
}
