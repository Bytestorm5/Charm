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

public class FbxExporter : FbxIOBase {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal FbxExporter(global::System.IntPtr cPtr, bool cMemoryOwn) : base(FbxWrapperNativePINVOKE.FbxExporter_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxExporter obj) {
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
      FbxWrapperNativePINVOKE.FbxExporter_ClassId_set(FbxClassId.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_ClassId_get();
      FbxClassId ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxClassId(cPtr, false);
      return ret;
    } 
  }

  public override FbxClassId GetClassId() {
    FbxClassId ret = new FbxClassId(FbxWrapperNativePINVOKE.FbxExporter_GetClassId(swigCPtr), true);
    return ret;
  }

  public new static FbxExporter Create(FbxManager pManager, string pName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_Create__SWIG_0(FbxManager.getCPtr(pManager), pName);
    FbxExporter ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxExporter(cPtr, false);
    return ret;
  }

  public new static FbxExporter Create(FbxObject pContainer, string pName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_Create__SWIG_1(FbxObject.getCPtr(pContainer), pName);
    FbxExporter ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxExporter(cPtr, false);
    return ret;
  }

  public override bool Initialize(string pFileName, int pFileFormat, FbxIOSettings pIOSettings) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_0(swigCPtr, pFileName, pFileFormat, FbxIOSettings.getCPtr(pIOSettings));
    return ret;
  }

  public override bool Initialize(string pFileName, int pFileFormat) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_1(swigCPtr, pFileName, pFileFormat);
    return ret;
  }

  public override bool Initialize(string pFileName) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_2(swigCPtr, pFileName);
    return ret;
  }

  public virtual bool Initialize(FbxStream pStream, SWIGTYPE_p_void pStreamData, int pFileFormat, FbxIOSettings pIOSettings) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_3(swigCPtr, FbxStream.getCPtr(pStream), SWIGTYPE_p_void.getCPtr(pStreamData), pFileFormat, FbxIOSettings.getCPtr(pIOSettings));
    return ret;
  }

  public virtual bool Initialize(FbxStream pStream, SWIGTYPE_p_void pStreamData, int pFileFormat) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_4(swigCPtr, FbxStream.getCPtr(pStream), SWIGTYPE_p_void.getCPtr(pStreamData), pFileFormat);
    return ret;
  }

  public virtual bool Initialize(FbxStream pStream, SWIGTYPE_p_void pStreamData) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_5(swigCPtr, FbxStream.getCPtr(pStream), SWIGTYPE_p_void.getCPtr(pStreamData));
    return ret;
  }

  public virtual bool Initialize(FbxStream pStream) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Initialize__SWIG_6(swigCPtr, FbxStream.getCPtr(pStream));
    return ret;
  }

  public bool GetExportOptions() {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_GetExportOptions__SWIG_0(swigCPtr);
    return ret;
  }

  public FbxIOSettings GetIOSettings() {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_GetIOSettings(swigCPtr);
    FbxIOSettings ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxIOSettings(cPtr, false);
    return ret;
  }

  public void SetIOSettings(FbxIOSettings pIOSettings) {
    FbxWrapperNativePINVOKE.FbxExporter_SetIOSettings(swigCPtr, FbxIOSettings.getCPtr(pIOSettings));
  }

  public bool Export(FbxDocument pDocument, bool pNonBlocking) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Export__SWIG_0(swigCPtr, FbxDocument.getCPtr(pDocument), pNonBlocking);
    return ret;
  }

  public bool Export(FbxDocument pDocument) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Export__SWIG_1(swigCPtr, FbxDocument.getCPtr(pDocument));
    return ret;
  }

  public bool IsExporting(SWIGTYPE_p_bool pExportResult) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_IsExporting(swigCPtr, SWIGTYPE_p_bool.getCPtr(pExportResult));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public float GetProgress(FbxString pStatus) {
    float ret = FbxWrapperNativePINVOKE.FbxExporter_GetProgress__SWIG_0(swigCPtr, FbxString.getCPtr(pStatus));
    return ret;
  }

  public float GetProgress() {
    float ret = FbxWrapperNativePINVOKE.FbxExporter_GetProgress__SWIG_1(swigCPtr);
    return ret;
  }

  public void SetProgressCallback(SWIGTYPE_p_f_p_void_float_p_q_const__char__bool pCallback, SWIGTYPE_p_void pArgs) {
    FbxWrapperNativePINVOKE.FbxExporter_SetProgressCallback__SWIG_0(swigCPtr, SWIGTYPE_p_f_p_void_float_p_q_const__char__bool.getCPtr(pCallback), SWIGTYPE_p_void.getCPtr(pArgs));
  }

  public void SetProgressCallback(SWIGTYPE_p_f_p_void_float_p_q_const__char__bool pCallback) {
    FbxWrapperNativePINVOKE.FbxExporter_SetProgressCallback__SWIG_1(swigCPtr, SWIGTYPE_p_f_p_void_float_p_q_const__char__bool.getCPtr(pCallback));
  }

  public int GetFileFormat() {
    int ret = FbxWrapperNativePINVOKE.FbxExporter_GetFileFormat(swigCPtr);
    return ret;
  }

  public bool IsFBX() {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_IsFBX(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_p_char GetCurrentWritableVersions() {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_GetCurrentWritableVersions(swigCPtr);
    SWIGTYPE_p_p_char ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_p_char(cPtr, false);
    return ret;
  }

  public bool SetFileExportVersion(FbxString pVersion, FbxSceneRenamer.ERenamingMode pRenamingMode) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_SetFileExportVersion__SWIG_0(swigCPtr, FbxString.getCPtr(pVersion), (int)pRenamingMode);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool SetFileExportVersion(FbxString pVersion) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_SetFileExportVersion__SWIG_1(swigCPtr, FbxString.getCPtr(pVersion));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetResamplingRate(double pResamplingRate) {
    FbxWrapperNativePINVOKE.FbxExporter_SetResamplingRate(swigCPtr, pResamplingRate);
  }

  public void SetDefaultRenderResolution(FbxString pCamName, FbxString pResolutionMode, double pW, double pH) {
    FbxWrapperNativePINVOKE.FbxExporter_SetDefaultRenderResolution(swigCPtr, FbxString.getCPtr(pCamName), FbxString.getCPtr(pResolutionMode), pW, pH);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
  }

  public FbxIOFileHeaderInfo GetFileHeaderInfo() {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxExporter_GetFileHeaderInfo(swigCPtr);
    FbxIOFileHeaderInfo ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxIOFileHeaderInfo(cPtr, false);
    return ret;
  }

  public bool GetExportOptions(FbxIO pFbxObject) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_GetExportOptions__SWIG_1(swigCPtr, FbxIO.getCPtr(pFbxObject));
    return ret;
  }

  public bool Export(FbxDocument pDocument, FbxIO pFbxObject) {
    bool ret = FbxWrapperNativePINVOKE.FbxExporter_Export__SWIG_2(swigCPtr, FbxDocument.getCPtr(pDocument), FbxIO.getCPtr(pFbxObject));
    return ret;
  }

}

}
