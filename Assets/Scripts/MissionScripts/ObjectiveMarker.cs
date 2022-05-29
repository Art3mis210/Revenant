using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour
{
    public GameObject target;
    public RectTransform canvasRect;
    Text Objective;
    Text Distance;
    Image Marker;
    public static ObjectiveMarker Reference
    {
        get;
        set;
    }
    void Awake()
    {
        Objective = transform.GetChild(0).GetComponent<Text>();
        Distance = transform.GetChild(1).GetComponent<Text>();
        Marker = GetComponent<Image>();
        Reference = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Distance.text = ((int)Vector3.Distance(PlayerController.Player.transform.position, target.transform.position)).ToString();
            if (!Marker.enabled)
                Marker.enabled = true;

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            transform.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        }
        else
        {
            Marker.enabled = false;
            Objective.text = "";
            Distance.text = "";
        }
    }
    public void ChangeTarget(GameObject target,string Objective)
    {
        this.target = target;
        Marker.enabled = true;
        this.Objective.text = Objective;
    }
}
