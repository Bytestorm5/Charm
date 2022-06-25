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

public class FbxLayerElementTemplateFbxColor : FbxLayerElement {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal FbxLayerElementTemplateFbxColor(global::System.IntPtr cPtr, bool cMemoryOwn) : base(FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxLayerElementTemplateFbxColor obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          throw new global::System.MethodAccessException("C++ destructor does not have public access");
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public FbxLayerElementArrayTemplateFbxColor GetDirectArray() {
    FbxLayerElementArrayTemplateFbxColor ret = new FbxLayerElementArrayTemplateFbxColor(FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_GetDirectArray__SWIG_0(swigCPtr), false);
    return ret;
  }

  public FbxLayerElementArrayTemplateFbxInt GetIndexArray() {
    FbxLayerElementArrayTemplateFbxInt ret = new FbxLayerElementArrayTemplateFbxInt(FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_GetIndexArray__SWIG_0(swigCPtr), false);
    return ret;
  }

  public override bool Clear() {
    bool ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_Clear(swigCPtr);
    return ret;
  }

  public bool eq(FbxLayerElementTemplateFbxColor pOther) {
    bool ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_eq(swigCPtr, FbxLayerElementTemplateFbxColor.getCPtr(pOther));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxLayerElementTemplateFbxColor assign(FbxLayerElementTemplateFbxColor pOther) {
    FbxLayerElementTemplateFbxColor ret = new FbxLayerElementTemplateFbxColor(FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_assign(swigCPtr, FbxLayerElementTemplateFbxColor.getCPtr(pOther)), false);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int RemapIndexTo(FbxLayerElement.EMappingMode pNewMapping) {
    int ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_RemapIndexTo(swigCPtr, (int)pNewMapping);
    return ret;
  }

  public override int MemorySize() {
    int ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_MemorySize(swigCPtr);
    return ret;
  }

  public override bool ContentWriteTo(FbxStream pStream) {
    bool ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_ContentWriteTo(swigCPtr, FbxStream.getCPtr(pStream));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public override bool ContentReadFrom(FbxStream pStream) {
    bool ret = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_ContentReadFrom(swigCPtr, FbxStream.getCPtr(pStream));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxLayerElementArrayTemplateFbxColor mDirectArray {
    set {
      FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_mDirectArray_set(swigCPtr, FbxLayerElementArrayTemplateFbxColor.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_mDirectArray_get(swigCPtr);
      FbxLayerElementArrayTemplateFbxColor ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxLayerElementArrayTemplateFbxColor(cPtr, false);
      return ret;
    } 
  }

  public FbxLayerElementArrayTemplateFbxInt mIndexArray {
    set {
      FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_mIndexArray_set(swigCPtr, FbxLayerElementArrayTemplateFbxInt.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxLayerElementTemplateFbxColor_mIndexArray_get(swigCPtr);
      FbxLayerElementArrayTemplateFbxInt ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxLayerElementArrayTemplateFbxInt(cPtr, false);
      return ret;
    } 
  }

}

}
