using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

enum CameraVirtualName
{
    camera1, camera2
}

public class CutSceneController : NetworkBehaviour
{

    private CinemachineBrain cameraBrain;
    [SerializeField] CinemachineVirtualCamera camera1;
    [SerializeField] CinemachineVirtualCamera camera2;

    [SyncVar(hook=nameof(handleCameraVirtualCurrent))]
    private CameraVirtualName cameraVirtualCurrent;
    


    void Awake(){
        this.cameraBrain = this.GetComponent<CinemachineBrain>();
    }

    void Start()
    {
        
    }

    [Command(requiresAuthority=false)]
    public void cmdAnimation(NetworkConnectionToClient sender = null){


        NetworkPlayer networkPlayer = sender.identity.gameObject.GetComponent<NetworkPlayer>();

        // this retrieves in server, ensure exist the reference here
        StartCoroutine(animationTest(networkPlayer.getCharacterStateMachine().gameObject));
    }

    [Server]
    public IEnumerator animationTest(GameObject target){
        
        this.cameraVirtualCurrent = CameraVirtualName.camera1;
        // this.rpcTest("BEGIN ANIMATION");
        Vector3 position1 = new Vector3(
            target.transform.position.x, 
            target.transform.position.y+5, 
            target.transform.position.z-10f
        );
        
        while (Vector3.Distance(this.camera1.transform.position, position1) > 0.1f)
        {
            this.camera1.transform.position = Vector3.Lerp(this.camera1.transform.position, position1, Time.deltaTime);
            yield return null;
        }
        // this.rpcTest("WAITING 1");
        yield return new WaitForSeconds(1);
        this.cameraVirtualCurrent = CameraVirtualName.camera2;
        // this.rpcTest("STOP ANIMATION");
    }

    void handleCameraVirtualCurrent(CameraVirtualName valueOld, CameraVirtualName valueNew){
        this.cameraVirtualCurrent = valueNew;
        switch (valueNew)
        {
            case CameraVirtualName.camera1:
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
                break;
            case CameraVirtualName.camera2:
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

}
