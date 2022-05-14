# Third Party Assets

## Socket.IO V3 / V4 Client for Unity (Standalone & WebGL)

- **Creator:** Firesplash Entertainment
- **Link:** https://assetstore.unity.com/packages/tools/network/socket-io-v3-v4-client-for-unity-standalone-webgl-196557
- **Version:** 1.4.0
- **Path:** ..\Client\Assets\Third Party Assets\Firesplash Entertainment\
- **Changes:**
  - SocketIOMananger
    - added the method RemoveSocketInstance(string)
     ```c#
     internal void RemoveSocketInstance(string instanceName)
     {
        if (SIOInstances.ContainsKey(instanceName))
        {
           SIOInstances[instanceName].Close();
           SIOInstances.Remove(instanceName);
        }
      }
      ```
  - SocketIOCommunicator
    - added the method RemoveInstance()
     ```c#
     public void RemoveInstance()
     {
        SocketIOManager.Instance.RemoveSocketInstance(gameObject.name);
        _instance = null;
     }
     ```