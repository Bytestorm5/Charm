//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Internal.Fbx {

public class FbxConnectEvent : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal FbxConnectEvent(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxConnectEvent obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~FbxConnectEvent() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          FbxWrapperNativePINVOKE.delete_FbxConnectEvent(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public FbxConnectEvent(FbxConnectEvent.EType pType, FbxConnectEvent.EDirection pDir, FbxProperty pSrc, FbxProperty pDst) : this(FbxWrapperNativePINVOKE.new_FbxConnectEvent((int)pType, (int)pDir, FbxProperty.getCPtr(pSrc), FbxProperty.getCPtr(pDst)), true) {
  }

  public FbxConnectEvent.EType GetType() {
    FbxConnectEvent.EType ret = (FbxConnectEvent.EType)FbxWrapperNativePINVOKE.FbxConnectEvent_GetType(swigCPtr);
    return ret;
  }

  public FbxConnectEvent.EDirection GetDirection() {
    FbxConnectEvent.EDirection ret = (FbxConnectEvent.EDirection)FbxWrapperNativePINVOKE.FbxConnectEvent_GetDirection(swigCPtr);
    return ret;
  }

  public FbxProperty GetSrc() {
    FbxProperty ret = new FbxProperty(FbxWrapperNativePINVOKE.FbxConnectEvent_GetSrc(swigCPtr), false);
    return ret;
  }

  public FbxProperty GetDst() {
    FbxProperty ret = new FbxProperty(FbxWrapperNativePINVOKE.FbxConnectEvent_GetDst(swigCPtr), false);
    return ret;
  }

  public enum EType {
    eConnectRequest,
    eConnect,
    eConnected,
    eDisconnectRequest,
    eDisconnect,
    eDisconnected
  }

  public enum EDirection {
    eSource,
    eDestination
  }

}

}