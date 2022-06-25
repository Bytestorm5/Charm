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

public class FbxStatus : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal FbxStatus(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxStatus obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~FbxStatus() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          FbxWrapperNativePINVOKE.delete_FbxStatus(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public FbxStatus() : this(FbxWrapperNativePINVOKE.new_FbxStatus__SWIG_0(), true) {
  }

  public FbxStatus(FbxStatus.EStatusCode pCode) : this(FbxWrapperNativePINVOKE.new_FbxStatus__SWIG_1((int)pCode), true) {
  }

  public FbxStatus(FbxStatus rhs) : this(FbxWrapperNativePINVOKE.new_FbxStatus__SWIG_2(FbxStatus.getCPtr(rhs)), true) {
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public FbxStatus assign(FbxStatus rhs) {
    FbxStatus ret = new FbxStatus(FbxWrapperNativePINVOKE.FbxStatus_assign(swigCPtr, FbxStatus.getCPtr(rhs)), false);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool eq(FbxStatus rhs) {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_eq__SWIG_0(swigCPtr, FbxStatus.getCPtr(rhs));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool eq(FbxStatus.EStatusCode pCode) {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_eq__SWIG_1(swigCPtr, (int)pCode);
    return ret;
  }

  public bool ne(FbxStatus rhs) {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_ne__SWIG_0(swigCPtr, FbxStatus.getCPtr(rhs));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool ne(FbxStatus.EStatusCode rhs) {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_ne__SWIG_1(swigCPtr, (int)rhs);
    return ret;
  }

  public bool ToBool() {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_ToBool(swigCPtr);
    return ret;
  }

  public bool Error() {
    bool ret = FbxWrapperNativePINVOKE.FbxStatus_Error(swigCPtr);
    return ret;
  }

  public void Clear() {
    FbxWrapperNativePINVOKE.FbxStatus_Clear(swigCPtr);
  }

  public FbxStatus.EStatusCode GetCode() {
    FbxStatus.EStatusCode ret = (FbxStatus.EStatusCode)FbxWrapperNativePINVOKE.FbxStatus_GetCode(swigCPtr);
    return ret;
  }

  public void SetCode(FbxStatus.EStatusCode rhs) {
    FbxWrapperNativePINVOKE.FbxStatus_SetCode__SWIG_0(swigCPtr, (int)rhs);
  }

  public void SetCode(FbxStatus.EStatusCode rhs, string pErrorMsg) {
    FbxWrapperNativePINVOKE.FbxStatus_SetCode__SWIG_1(swigCPtr, (int)rhs, pErrorMsg);
  }

  public string GetErrorString() {
    string ret = FbxWrapperNativePINVOKE.FbxStatus_GetErrorString(swigCPtr);
    return ret;
  }

  public enum EStatusCode {
    eSuccess = 0,
    eFailure,
    eInsufficientMemory,
    eInvalidParameter,
    eIndexOutOfRange,
    ePasswordError,
    eInvalidFileVersion,
    eInvalidFile,
    eSceneCheckFail
  }

}

}
