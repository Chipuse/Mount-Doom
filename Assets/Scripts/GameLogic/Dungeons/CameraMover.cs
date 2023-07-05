using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover _instance;

    public Vector3 offset;

    public Camera cam;
    public Vector3 mousePosition;
    public Transform test;

    Vector3 targetPosition;
    Vector3 startPosition;
    Vector3 currentPosition;
    float marginDistance;

    float currentZoom;
    public float targetZoom;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    float lerpTime = 1f;
    float currentLerpTime;

    float moveSpeed = 5.0f;
    float zoomSpeed = 10.0f;

    float amplitude = .15f;
    float period = 5f;

    bool dragMode = false;
    Vector3 touchPos;
    Vector3 dragPos;
    public Vector3 speed = Vector3.zero;
    public float speedThreshold = .01f;

    void OnTouchDown()
    {
        //switch cameramover to drag mode
        dragMode = true;
        touchPos = Input.mousePosition;
    }

    void OnTouchUp()
    {
        //Swtich cameramover to lerp mode
        dragMode = false;
        SetTargetPos(targetPosition - offset);
    }

    void DragUpdate()
    {
        dragPos = Input.mousePosition;
        if ((touchPos - dragPos).sqrMagnitude >= 3f)
        {
            speed = (CameraMover._instance.cam.ScreenToWorldPoint(dragPos) - CameraMover._instance.cam.ScreenToWorldPoint(touchPos)) * .03f;
            if(speed.sqrMagnitude > (speed.normalized * speedThreshold).sqrMagnitude)
            {
                speed = (speed.normalized * speedThreshold);
            }
            transform.Translate(speed);
        }
    }


    void Start()
    {
        currentPosition = transform.position;
        SetTargetPos(transform.position);
        SetZoomPercent(0.4f);
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
            return;
        }

        if (cam == null)
            cam = gameObject.GetComponent<Camera>();
        currentZoom = cam.orthographicSize;
        targetZoom = currentZoom;
    }

    void Update()
    {
        mousePosition = CameraMover._instance.cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTouchUp();
        }
        if (dragMode)
        {
            DragUpdate();
        }
        else
        {
            DoLerp();
            //Breath();
            if (currentZoom != targetZoom)
            {
                DoZoom();
            }
        }
    }

    static void Breath()
    {
        if (_instance == null)
            return;
        float theta = Time.timeSinceLevelLoad / _instance.period;
        float distance = _instance.amplitude * Mathf.Sin(theta);
        _instance.transform.position = _instance.startPosition + Vector3.up * distance;
    }

    static void DoZoom()
    {
        if (_instance == null)
            return;
        if (_instance.targetZoom - _instance.currentZoom > 0)
        {
            _instance.currentZoom += _instance.zoomSpeed * Time.deltaTime;
            if (_instance.targetZoom - _instance.currentZoom < 0)
                _instance.currentZoom = _instance.targetZoom;
        }
        else
        {
            _instance.currentZoom -= _instance.zoomSpeed * Time.deltaTime;
            if (_instance.targetZoom - _instance.currentZoom > 0)
                _instance.currentZoom = _instance.targetZoom;
        }
        _instance.cam.orthographicSize = _instance.currentZoom;
    }

    static void SetZoom(float _targetZoom)
    {
        if (_instance == null)
            return;
        if (_targetZoom >= _instance.maxZoom)
            _instance.targetZoom = _instance.maxZoom;
        else if (_targetZoom <= _instance.minZoom)
            _instance.targetZoom = _instance.minZoom;
        else
            _instance.targetZoom = _targetZoom;
    }

    static void DoLerp()
    {
        if (_instance == null)
            return;
        if (_instance.lerpTime == 0)
            return;
        _instance.currentLerpTime += Time.deltaTime;
        
        if (_instance.currentLerpTime >= _instance.lerpTime)
        {
            _instance.currentLerpTime = _instance.lerpTime;
            
        }
        float t = _instance.currentLerpTime / _instance.lerpTime;
        //t = t * t * t * (t * (6f * t - 15f) + 10f);
        t = t * t * (3f - 2f * t);
        _instance.transform.position = Vector3.Lerp(_instance.startPosition, _instance.targetPosition, t);
    }

    public static void SetTargetPos(Vector3 _targetPos)
    {
        if (_instance == null || _targetPos == null)
            return;
        _targetPos += _instance.offset;
        //lerpTime abhängig von distance:
        _targetPos.z = _instance.startPosition.z;
        if(_instance.moveSpeed != 0)
            _instance.lerpTime = (_targetPos - _instance.transform.position).magnitude / _instance.moveSpeed;
        _instance.currentLerpTime = 0.0f;
        //Debug.Log("t: " + lerpTime.ToString());

        _instance.targetPosition = new Vector3(_targetPos.x, _targetPos.y, _instance.transform.position.z);
        _instance.startPosition = _instance.transform.position;
    }

    public static void SetZoomPercent(float zoomStep)
    {
        if (_instance == null)
            return;
        if (zoomStep > 1)
            zoomStep = 1;
        else if (zoomStep < 0)
            zoomStep = 0;
        float step = (_instance.maxZoom - _instance.minZoom);
        _instance.targetZoom = _instance.minZoom + (step * zoomStep);
    }
}
