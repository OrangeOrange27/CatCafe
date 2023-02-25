using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DragCamera2D : MonoBehaviour
{
    /*
     *TODO: 
     *  DONE: replace dolly with bezier dolly system
     *  DONE: add dolly track smoothing
     *  DONE: add dolly track straightening
     *  DONE: Dolly track + gizmo colours
     *  DONE: add non tracked constant speed dolly system(continuous movement based on time)
     *  WONTDO: [REPLACED BY FEATURE BELOW] add button to split dolly track evenly (between start and end) for time based dolly movement
     *  DONE: button to adjust times on all waypoints so camera moves at a constant speed
     *  DONE: add per waypoint time (seconds on this segment)
     *  DONE: add scaler for time to next waypoint in scene viewe gui
     *  DONE: improve GUI elements (full custom editor inspector)
     *  DONE:    add waypoint gui  scene view button
     *  DONE: better designed example scenes
     *  DONE: option to lock camera to track even if object escapes area
     *  add multiple dolly tracks to allow creating loops etc
     *  add track change triggers
     *  add bounds ids for multiple bounds
     *  add bounds triggers(e.g. small bounds until x event(obtain key etc) then larger bounds
     *  add configurable keymap to allow developers/usres to map keys to actions
     *  DONE: add in scene dolly track controls
     *  possibly add event system for lerping camera to position
     *  possibly make dolly track event system to allow camera to track dolly after an event then return to user control(for cutscenes/tutorial etc)
     *  
     *  Requests:
     *  ADDED: Zoom/Translate to double click position
     *  ADDED:  Translate to double click
     *  ADDED: Zoom to double click
     *  TODO: Scroll Snapping
     *  
     *  Bugfixes:
     *  The name does not exists in current content during build : fix supplied by  @chimerian
     *  zoom to mouse would translate camera position even when fully zoomed in our out. FIXED
     *  
     *  BUGS TO FIX:
     *  Double clicking area restricted by area clamp locks camera in lerp to double click target
    */

    [FormerlySerializedAs("cam")] 
    [SerializeField] private Camera _camera;

    [Header("Camera Movement")]
    
    [Tooltip("Allow the Camera to be dragged.")]
    [FormerlySerializedAs("dragEnabled")]
    [SerializeField] private bool _dragEnabled = true;

    [Range(-5, 5)]
    [Tooltip("Speed the camera moves when dragged.")]
    [FormerlySerializedAs("dragSpeed")]
    [SerializeField] private  float _dragSpeed = -0.06f;

    [FormerlySerializedAs("ztTarget")]
    [Header("Double Click Action (DC)")]
    [SerializeField] private  Dc2dZoomTarget _ztTarget;
    
    [FormerlySerializedAs("DCTranslate")]
    [Tooltip("Move to DC location")]
    [SerializeField] private  bool _translate = true;
    
    [FormerlySerializedAs("DCZoom")]
    [Tooltip("Zoom DC location")]
    [SerializeField] private  bool _zoom = true;
    
    [FormerlySerializedAs("DCZoomTargetIn")]
    [Tooltip("Target Zoom Level for DC")]
    [Range(0.1f, 10f)]
    [SerializeField] private  float _zoomTargetIn = 4f;
    
    [FormerlySerializedAs("DCZoomTranslateSpeed")]
    [Tooltip("DC Translation Speed")]
    [Range(0.01f, 1f)]
    [SerializeField] private  float _zoomTranslateSpeed = 0.5f;
    
    [FormerlySerializedAs("DCZoomTargetOut")]
    [Tooltip("Target Zoom Level for DC")]
    [Range(0.1f, 10f)]
    [SerializeField] private  float _zoomTargetOut = 10f;
    
    [FormerlySerializedAs("DCZoomSpeed")]
    [Tooltip("DC Zoom Speed")]
    [Range(0.01f, 1f)]
    [SerializeField] private  float _zoomSpeed = 0.5f;
    
    private bool _zoomedOut = true;
    

    [FormerlySerializedAs("edgeBoundary")]
    [Header("Edge Scrolling")]
    [Tooltip("Pixel Border to trigger edge scrolling")]
    [SerializeField] private int _edgeBoundary = 20;
    
    [FormerlySerializedAs("edgeSpeed")]
    [Range(0, 10)]
    [Tooltip("Speed the camera moves Mouse enters screen edge.")]
    [SerializeField] private float _edgeSpeed = 1f;

    [FormerlySerializedAs("keyboardInput")]
    [Header("Touch(PRO) & Keyboard Input")]
    [Tooltip("Enable or disable Keyboard input")]
    [SerializeField] private bool _keyboardInput = false;
    
    [FormerlySerializedAs("inverseKeyboard")]
    [Tooltip("Invert keyboard direction")]
    [SerializeField] private bool _inverseKeyboard = false;
    
    [FormerlySerializedAs("touchEnabled")]
    [Tooltip("Enable or disable touch input")]
    [SerializeField] private bool _touchEnabled = false;
    
    [FormerlySerializedAs("touchDragSpeed")]
    [Tooltip("Drag Speed for touch controls")]
    [Range(-5,5)]
    [SerializeField] private float _touchDragSpeed = -0.03f;

    [FormerlySerializedAs("zoomEnabled")]
    [Header("Zoom")]
    [Tooltip("Enable or disable zooming")]
    [SerializeField] private bool _zoomEnabled = true;
    
    [FormerlySerializedAs("linkedZoomDrag")]
    [Tooltip("Scale drag movement with zoom level")]
    [SerializeField] private bool _linkedZoomDrag = true;
    
    [FormerlySerializedAs("maxZoom")]
    [Tooltip("Maximum Zoom Level")]
    [SerializeField] private float _maxZoom = 10;
    
    [FormerlySerializedAs("minZoom")]
    [Tooltip("Minimum Zoom Level")]
    [Range(0.01f, 10)]
    [SerializeField] private float _minZoom = 0.5f;
    
    [FormerlySerializedAs("zoomStepSize")]
    [Tooltip("The Speed the zoom changes")]
    [Range(0.1f, 10f)]
    [SerializeField] private float _zoomStepSize = 0.5f;
    
    [FormerlySerializedAs("zoomToMouse")]
    [Tooltip("Enable Zooming to mouse pointer")]
    [SerializeField] private bool _zoomToMouse = false;

   


    [FormerlySerializedAs("followTarget")]
    [Header("Follow Object")]
    [SerializeField] private GameObject _followTarget;
    
    [FormerlySerializedAs("lerpSpeed")]
    [Range(0.01f,1f)]
    [SerializeField] private float _lerpSpeed = 0.5f;
    
    [FormerlySerializedAs("offset")]
    [SerializeField] private Vector3 _offset = new Vector3(0,0,-10);


    [FormerlySerializedAs("clampCamera")]
    [Header("Camera Bounds")]
    [SerializeField] private bool _clampCamera = true;
    
    [FormerlySerializedAs("bounds")]
    [SerializeField] private CameraBounds _bounds; 
    
    [FormerlySerializedAs("dollyRail")]
    [SerializeField] private Dc2dDolly _dollyRail;
    

    // private vars
    Vector3 bl;
    Vector3 tr;
    private Vector2 touchOrigin = -Vector2.one;

    public Dc2dSnapBox snapTarget;

    private int frameid = 0;

    private void Start() {
        if (_camera == null) {
            _camera = Camera.main;
        }
    }

    private void LateUpdate() {
        frameid++;
        
        if (_dragEnabled) {
            PanControl();
        }

        if (_edgeBoundary > 0) {
            EdgeScroll();
        }


        if (_zoomEnabled) {
            ZoomControl();
        }

        if (snapTarget != null) {
            //using snap targets do da snap
            ConformToSnapTarget();
        } else {
            if (_followTarget != null) {
                transform.position = Vector3.Lerp(transform.position, _followTarget.transform.position + _offset, _lerpSpeed);
            }
        }

        if (_translate || _zoom) {
            ZoomTarget();
        }

        if (_clampCamera) {
            CameraClamp();
        }

        if (_touchEnabled) {
            DoTouchControls();
        }

        if(_dollyRail != null) {
            StickToDollyRail();
        }
    }

    private void ZoomTarget() {
        if (_ztTarget == null) {
            throw new UnassignedReferenceException("No Dc2dZoomTarget object. Please add one to your scene from the prefab folder, create an object with the Dc2dZoomTarget script or turn off Double Click zoom actions");
        }

        if (_ztTarget.zoomToMe && _translate) {
            Vector3 targetLoc = _ztTarget.transform.position;
            targetLoc.z = _offset.z; // lock ofset to cams offset
            transform.position = Vector3.Lerp(transform.position , targetLoc, 0.3f);
            if(_ztTarget.zoomToMe && Vector3.Distance(transform.position, targetLoc) < 0.2f) {
                _ztTarget.zoomToMe = false;
            }
        }
        if (_zoom && !_ztTarget.zoomComplete) {
            if (_zoomedOut) {
                Camera.main.orthographicSize = Mathf.Lerp(_zoomTargetIn, Camera.main.orthographicSize, 0.1f);
                if (Camera.main.orthographicSize == _zoomTargetIn) {
                    _ztTarget.zoomComplete = true;
                    _zoomedOut = !_zoomedOut;
                }
            } else {
                Camera.main.orthographicSize = Mathf.Lerp(_zoomTargetOut, Camera.main.orthographicSize, 0.1f);
                if (Camera.main.orthographicSize == _zoomTargetOut) {
                    _ztTarget.zoomComplete = true;
                    _zoomedOut = !_zoomedOut;
                }
            }
        }
    }

    private void EdgeScroll() {
        float x = 0;
        float y = 0;
        if (Input.mousePosition.x >= Screen.width - _edgeBoundary) {
            // Move the camera
            x = Time.deltaTime * _edgeSpeed;
        }
        if (Input.mousePosition.x <= 0 + _edgeBoundary) {
            // Move the camera
            x = Time.deltaTime * -_edgeSpeed;
        }
        if (Input.mousePosition.y >= Screen.height - _edgeBoundary) {
            // Move the camera
            y = Time.deltaTime * _edgeSpeed
;
        }
        if (Input.mousePosition.y <= 0 + _edgeBoundary) {
            // Move the camera
            y =  Time.deltaTime * -_edgeSpeed
;
        }
        transform.Translate(x, y, 0);
    }

    public void addCameraDolly() {
        if (_dollyRail == null) {
            GameObject go = new GameObject("Dolly");
            Dc2dDolly dolly = go.AddComponent<Dc2dDolly>();

            Dc2dWaypoint wp1 = new Dc2dWaypoint();
            wp1.position = new Vector3(0, 0, 0);

            Dc2dWaypoint wp2 = new Dc2dWaypoint();
            wp2.position = new Vector3(1, 0, 0);

            Dc2dWaypoint[] dc2dwaypoints = new Dc2dWaypoint[2];
            dc2dwaypoints[0] = wp1;
            dc2dwaypoints[1] = wp2;
            wp1.endPosition = wp2.position;


            dolly.allWaypoints = dc2dwaypoints;

            //Vector3[] waypoints = new Vector3[2];
            //waypoints[0] = new Vector3(0, 0, 0);
            //waypoints[1] = new Vector3(1, 1, 0);
            //dolly.dollyWaypoints = waypoints;

            //Vector3[] bezpoints = new Vector3[1];
            //bezpoints[0] = new Vector3(0.5f, 0.5f, 0);
            //dolly.bezierWaypoints = bezpoints;

            this._dollyRail = dolly;

#if UNITY_EDITOR
            Selection.activeGameObject = go;
            SceneView.FrameLastActiveSceneView();
#endif
        }
    }

    public void addCameraBounds() {
        if (_bounds == null) {
            GameObject go = new GameObject("CameraBounds");
            CameraBounds cb = go.AddComponent<CameraBounds>();
            cb.guiColour = new Color(0,0,1f,0.1f);
            cb.pointa = new Vector3(20,20,0);
            this._bounds = cb;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    // adds a zoom target object to the scene to enable double click zooming
    public void addZoomTarget() {
        if (_ztTarget == null) {
            GameObject go = new GameObject("Dc2dZoomTarget");
            Dc2dZoomTarget zt = go.AddComponent<Dc2dZoomTarget>();
            this._ztTarget = zt;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    private void DoTouchControls() {
       // PRO Only
    }

    //click and drag
    private void PanControl() {
        // if keyboard input is allowed
        if (_keyboardInput) {
            var x = -Input.GetAxis("Horizontal") * _dragSpeed;
            var y = -Input.GetAxis("Vertical") * _dragSpeed;

            if (_linkedZoomDrag) {
                x *= Camera.main.orthographicSize;
                y *= Camera.main.orthographicSize;
            }

            if (_inverseKeyboard) {
                x = -x;
                y = -y;
            }
            transform.Translate(x, y, 0);
        }

       

        // if mouse is down
        if (Input.GetMouseButton(0)) {
            float x = Input.GetAxis("Mouse X") * _dragSpeed;
            float y = Input.GetAxis("Mouse Y") * _dragSpeed;

            if (_linkedZoomDrag) {
                x *= Camera.main.orthographicSize;
                y *= Camera.main.orthographicSize;
            }

            transform.Translate(x, y, 0);
        }

        
    }

    private void clampZoom() {
        Camera.main.orthographicSize =  Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
        Mathf.Max(_camera.orthographicSize, 0.1f);


    }

    void ZoomOrthoCamera(Vector3 zoomTowards, float amount) {
        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / Camera.main.orthographicSize * amount);
        // Move camera
        transform.position += (zoomTowards - transform.position) * multiplier;
        // Zoom camera
        Camera.main.orthographicSize -= amount;
        // Limit zoom
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
    }

    // managae zooming
    public void ZoomControl() {
        if (_zoomToMouse) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _minZoom < Camera.main.orthographicSize) // forward
            {
                ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), _zoomStepSize);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0 && _maxZoom > Camera.main.orthographicSize) // back            
            {
                ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -_zoomStepSize);
            }

        } else {

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _minZoom < Camera.main.orthographicSize) // forward
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && _maxZoom > Camera.main.orthographicSize) // back            
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize + _zoomStepSize;
            }
        }
        clampZoom();
    }


    private bool lfxmax = false;
    private bool lfxmin = false;
    private bool lfymax = false;
    private bool lfymin = false;

    // Clamp Camera to bounds
    private void CameraClamp() {
        tr = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight, -transform.position.z));
        bl = _camera.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));

        if(_bounds == null) {
            Debug.Log("Clamp Camera Enabled but no Bounds has been set.");
            return;
        }

        float boundsMaxX = _bounds.pointa.x;
        float boundsMinX = _bounds.transform.position.x;
        float boundsMaxY = _bounds.pointa.y;
        float boundsMinY = _bounds.transform.position.y;

        if (tr.x > boundsMaxX && bl.x < boundsMinX) {
            Debug.Log("User tried to zoom out past x axis bounds - locked to bounds");
            Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
            clampZoom();
        }

        if (tr.y > boundsMaxY && bl.y < boundsMinY) {
            Debug.Log("User tried to zoom out past y axis bounds - locked to bounds");
            Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
            clampZoom();
        }

        bool tfxmax = false;
        bool tfxmin = false;
        bool tfymax = false;
        bool tfymin = false;

        if (tr.x > boundsMaxX) {
            if (lfxmin) {
                Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
                clampZoom();
            } else {
                transform.position = new Vector3(transform.position.x - (tr.x - boundsMaxX), transform.position.y, transform.position.z);
                tfxmax = true;
            }
        }
        if (tr.y > boundsMaxY) {
            if (lfymin) {
                Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
                clampZoom();
            } else {
                transform.position = new Vector3(transform.position.x, transform.position.y - (tr.y - boundsMaxY), transform.position.z);
                tfymax = true;
            }
        } 
        if (bl.x < boundsMinX) {
            if (lfxmax) {
                Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
                clampZoom();
            } else {
                transform.position = new Vector3(transform.position.x + (boundsMinX - bl.x), transform.position.y, transform.position.z);
                tfxmin = true;
            }
        }
        if (bl.y < boundsMinY) {
            if (lfymax) {
                Camera.main.orthographicSize = Camera.main.orthographicSize - _zoomStepSize; // zoomControl in to compensate
                clampZoom();
            } else {
                transform.position = new Vector3(transform.position.x, transform.position.y + (boundsMinY - bl.y), transform.position.z);
                tfymin = true;
            }
        }

        lfxmax = tfxmax;
        lfxmin = tfxmin;
        lfymax = tfymax;
        lfymin = tfymin;
    }

    public void StickToDollyRail() {
        if(_dollyRail != null && _followTarget != null) {
            Vector3 campos = _dollyRail.getPositionOnTrack(_followTarget.transform.position);
            transform.position = new Vector3(campos.x, campos.y, transform.position.z);
        }
    }

    public void ConformToSnapTarget() {
        float cx = (snapTarget.endPoint.x - snapTarget.transform.position.x)/2 + snapTarget.transform.position.x;
        float cy = (snapTarget.endPoint.y - snapTarget.transform.position.y)/ 2 + snapTarget.transform.position.y;
        transform.position = Vector3.Lerp(transform.position, new Vector3(cx, cy, transform.position.z), 0.5f ); // has to be fast to counter zoom jitter
        expandToZoomTarget();
        snapTarget = null; // target set rest for next frame
    }

    private void expandToZoomTarget() {
        if(snapTarget != null) {
            
            //contractzoom
            if (snapTarget.expandMode == Dc2dSnapBox.Mode.CONTRACTY || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHY) {
                if (Camera.main.WorldToViewportPoint(snapTarget.getExpandYUpperBound()).y < 1.049f) {
                    Camera.main.orthographicSize = Camera.main.orthographicSize - snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":CONTRACTY for:" + frameid);
                }
            } else if (snapTarget.expandMode == Dc2dSnapBox.Mode.CONTRACTX || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHX) {
                if (Camera.main.WorldToViewportPoint(snapTarget.getExpandXUpperBound()).x < 1.049f) {
                    Camera.main.orthographicSize = Camera.main.orthographicSize - snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":CONTRACTX for:" + frameid);
                }
            }
            //expandzoom
            if (snapTarget.expandMode == Dc2dSnapBox.Mode.EXPANDY || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHY) {
                if (Camera.main.WorldToViewportPoint(snapTarget.getExpandYUpperBound()).y > 0.95f) {
                    Camera.main.orthographicSize = Camera.main.orthographicSize + snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":EXPANDY for:" + frameid);
                }
            } else if (snapTarget.expandMode == Dc2dSnapBox.Mode.EXPANDX || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHX) {
                if (Camera.main.WorldToViewportPoint(snapTarget.getExpandXUpperBound()).x > 0.95f) {
                    Camera.main.orthographicSize = Camera.main.orthographicSize + snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":EXPANDX for:" + frameid);
                }
            }
        }
    }

    public void setSnapTarget(Dc2dSnapBox sb) {
        if(snapTarget == null) {
            //Debug.Log("New snapboxes:"+ frameid);
            snapTarget = sb;
            return;
        }
        if(sb.priority > snapTarget.priority) {
            //Debug.Log("Swapping snapboxes:"+ frameid);
            snapTarget = sb;
        }
    }
}
