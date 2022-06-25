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

public class FbxBindingTableBase : FbxObject {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal FbxBindingTableBase(global::System.IntPtr cPtr, bool cMemoryOwn) : base(FbxWrapperNativePINVOKE.FbxBindingTableBase_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxBindingTableBase obj) {
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

  public static FbxClassId ClassId {
    set {
      FbxWrapperNativePINVOKE.FbxBindingTableBase_ClassId_set(FbxClassId.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxBindingTableBase_ClassId_get();
      FbxClassId ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxClassId(cPtr, false);
      return ret;
    } 
  }

  public override FbxClassId GetClassId() {
    FbxClassId ret = new FbxClassId(FbxWrapperNativePINVOKE.FbxBindingTableBase_GetClassId(swigCPtr), true);
    return ret;
  }

  public new static FbxBindingTableBase Create(FbxManager pManager, string pName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxBindingTableBase_Create(FbxManager.getCPtr(pManager), pName);
    FbxBindingTableBase ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxBindingTableBase(cPtr, false);
    return ret;
  }

  public FbxBindingTableEntry AddNewEntry() {
    FbxBindingTableEntry ret = new FbxBindingTableEntry(FbxWrapperNativePINVOKE.FbxBindingTableBase_AddNewEntry(swigCPtr), false);
    return ret;
  }

  public uint GetEntryCount() {
    uint ret = FbxWrapperNativePINVOKE.FbxBindingTableBase_GetEntryCount(swigCPtr);
    return ret;
  }

  public FbxBindingTableEntry GetEntry(uint pIndex) {
    FbxBindingTableEntry ret = new FbxBindingTableEntry(FbxWrapperNativePINVOKE.FbxBindingTableBase_GetEntry__SWIG_0(swigCPtr, pIndex), false);
    return ret;
  }

  public FbxBindingTableEntry GetEntryForSource(string pSrcName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxBindingTableBase_GetEntryForSource(swigCPtr, pSrcName);
    FbxBindingTableEntry ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxBindingTableEntry(cPtr, false);
    return ret;
  }

  public FbxBindingTableEntry GetEntryForDestination(string pDestName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxBindingTableBase_GetEntryForDestination(swigCPtr, pDestName);
    FbxBindingTableEntry ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxBindingTableEntry(cPtr, false);
    return ret;
  }

  public override FbxObject Copy(FbxObject pObject) {
    FbxObject ret = new FbxObject(FbxWrapperNativePINVOKE.FbxBindingTableBase_Copy(swigCPtr, FbxObject.getCPtr(pObject)), false);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
