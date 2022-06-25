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

public class FbxTimeSpan : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal FbxTimeSpan(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxTimeSpan obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~FbxTimeSpan() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          FbxWrapperNativePINVOKE.delete_FbxTimeSpan(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public FbxTimeSpan() : this(FbxWrapperNativePINVOKE.new_FbxTimeSpan__SWIG_0(), true) {
  }

  public FbxTimeSpan(FbxTime pStart, FbxTime pStop) : this(FbxWrapperNativePINVOKE.new_FbxTimeSpan__SWIG_1(FbxTime.getCPtr(pStart), FbxTime.getCPtr(pStop)), true) {
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public void Set(FbxTime pStart, FbxTime pStop) {
    FbxWrapperNativePINVOKE.FbxTimeSpan_Set(swigCPtr, FbxTime.getCPtr(pStart), FbxTime.getCPtr(pStop));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetStart(FbxTime pStart) {
    FbxWrapperNativePINVOKE.FbxTimeSpan_SetStart(swigCPtr, FbxTime.getCPtr(pStart));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetStop(FbxTime pStop) {
    FbxWrapperNativePINVOKE.FbxTimeSpan_SetStop(swigCPtr, FbxTime.getCPtr(pStop));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public FbxTime GetStart() {
    FbxTime ret = new FbxTime(FbxWrapperNativePINVOKE.FbxTimeSpan_GetStart(swigCPtr), true);
    return ret;
  }

  public FbxTime GetStop() {
    FbxTime ret = new FbxTime(FbxWrapperNativePINVOKE.FbxTimeSpan_GetStop(swigCPtr), true);
    return ret;
  }

  public FbxTime GetDuration() {
    FbxTime ret = new FbxTime(FbxWrapperNativePINVOKE.FbxTimeSpan_GetDuration(swigCPtr), true);
    return ret;
  }

  public FbxTime GetSignedDuration() {
    FbxTime ret = new FbxTime(FbxWrapperNativePINVOKE.FbxTimeSpan_GetSignedDuration(swigCPtr), true);
    return ret;
  }

  public int GetDirection() {
    int ret = FbxWrapperNativePINVOKE.FbxTimeSpan_GetDirection(swigCPtr);
    return ret;
  }

  public bool IsInside(FbxTime pTime) {
    bool ret = FbxWrapperNativePINVOKE.FbxTimeSpan_IsInside(swigCPtr, FbxTime.getCPtr(pTime));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxTimeSpan Intersect(FbxTimeSpan pTime) {
    FbxTimeSpan ret = new FbxTimeSpan(FbxWrapperNativePINVOKE.FbxTimeSpan_Intersect(swigCPtr, FbxTimeSpan.getCPtr(pTime)), true);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool ne(FbxTimeSpan pTime) {
    bool ret = FbxWrapperNativePINVOKE.FbxTimeSpan_ne(swigCPtr, FbxTimeSpan.getCPtr(pTime));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool eq(FbxTimeSpan pTime) {
    bool ret = FbxWrapperNativePINVOKE.FbxTimeSpan_eq(swigCPtr, FbxTimeSpan.getCPtr(pTime));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void UnionAssignment(FbxTimeSpan pSpan, int pDirection) {
    FbxWrapperNativePINVOKE.FbxTimeSpan_UnionAssignment__SWIG_0(swigCPtr, FbxTimeSpan.getCPtr(pSpan), pDirection);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public void UnionAssignment(FbxTimeSpan pSpan) {
    FbxWrapperNativePINVOKE.FbxTimeSpan_UnionAssignment__SWIG_1(swigCPtr, FbxTimeSpan.getCPtr(pSpan));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

}

}
