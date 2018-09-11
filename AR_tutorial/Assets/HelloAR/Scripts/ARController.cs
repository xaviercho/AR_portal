using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ARController : MonoBehaviour {

    //we will fill this list with the pleanes that ArCore detected inthe current frame
    private  List<TrackedPlane> m_NewTrackedPlanes = new List<TrackedPlane>();

    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	  
		//check arcore session status
	    if (Session.Status != SessionStatus.Tracking)
	    {
            return;
	    }

        //following function will fill m_NewTrackedAPlanes with the planes that Arcore detected in the current frame
        Session.GetTrackables<TrackedPlane>(m_NewTrackedPlanes,TrackableQueryFilter.New);

        //Instantiate a grid for each tracked pleane in m_TrackedPlanes
	    for (int i = 0; i < m_NewTrackedPlanes.Count;++i)
	    {
	        GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);

            //This function will set position of grid and modify the verticies of the attached mesh
            grid.GetComponent<GridVisualiser>().Initialize(m_NewTrackedPlanes[i]);
	    }
        //checked if user touched the screen
	    Touch touch;
	    if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
	    {
            return;
	    }

        //let now check if user touched any of the tracked planes
	    TrackableHit hit;
	    if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
	    {
            // now place the portal on top of th tracked plane that hit/touched

            //enable porta;
            Portal.SetActive(true);

            //create a new anchor

	        Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

	        //set the position of the portal to be the same as the hit position
	        Portal.transform.position = hit.Pose.position;
	        Portal.transform.rotation = hit.Pose.rotation;

            //we want the portal to face the camera upon
	        Vector3 cameraPosition = ARCamera.transform.position;

            // the portal shoul only rotate around the Y axis
	        cameraPosition.y = hit.Pose.position.y;

            //Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition,Portal.transform.up);

            //ARCore will keep understanding the world and update the anchors accordingly hence we need to attach our portal to the anchor
	        Portal.transform.parent = anchor.transform;


	    }
	}
}
