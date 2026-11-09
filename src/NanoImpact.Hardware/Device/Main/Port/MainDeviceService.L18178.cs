// 원본 NanoImpact2020cfeDlg.cpp:18178-19403 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:18178 ----
        // USB CONTROL DATA : HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 DATA
        public int OnSemHWSetupSpotCLCalibrationData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 DATA 정보
            SEM_CONTROL_SPOTCLALIBRATION_DATA SpotCLCalibration = new SEM_CONTROL_SPOTCLALIBRATION_DATA();
            // LONG DATA 복사
            SpotCLCalibration.CopyFrom((SEM_CONTROL_SPOTCLALIBRATION_DATA)lParam);

            //  HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 DATA 변경 확인
            if (m_IntegratedData.wCL1 != SpotCLCalibration.wCL1)
            {
                // 사용값->SPOT SIZE에 따른 CL1 적용
                m_IntegratedData.wCL1 = (ushort)(SpotCLCalibration.wCL1);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.wCL1);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 DATA 변경 확인
            if (m_IntegratedData.wCL2 != SpotCLCalibration.wCL2)
            {
                // 사용값->SPOT SIZE에 따른 CL2 적용
                m_IntegratedData.wCL2 = (ushort)(SpotCLCalibration.wCL2);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.wCL2);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            // HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 값 변경 확인
            if (memcmp(m_IntegratedData.wCL1List, SpotCLCalibration.wCL1List, sizeof(ushort)*MAX_CL12_ITEM) != NULL)
            {
                // 설정값->SPOT SIZE에 따른 CL1 기본 Item 설정 값 적용
                memcpy(m_IntegratedData.wCL1List, SpotCLCalibration.wCL1List, sizeof(ushort)*MAX_CL12_ITEM);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.wCL1List[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SPOT SIZE에 따른 CL1, CL2 설정 값 변경 확인
            if (memcmp(m_IntegratedData.wCL2List, SpotCLCalibration.wCL2List, sizeof(ushort)*MAX_CL12_ITEM) != NULL)
            {
                // 설정값->SPOT SIZE에 따른 CL2 기본 Item 설정 값 적용
                memcpy(m_IntegratedData.wCL2List, SpotCLCalibration.wCL2List, sizeof(ushort)*MAX_CL12_ITEM);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.wCL2List[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18234 ----
        // USB CONTROL DATA : HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA
        public int OnSemHWSetupImageCalOptionData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 정보
            SEM_CONTROL_HWIMAGECALOPTION_DATA HWImageCalOption = new SEM_CONTROL_HWIMAGECALOPTION_DATA();
            // LONG DATA 복사
            HWImageCalOption.CopyFrom((SEM_CONTROL_HWIMAGECALOPTION_DATA)lParam);

            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.nLRMirror != HWImageCalOption.nLRMirror)
            {
                // 사용값->Left<->Right Mirror (좌우반전) 적용
                m_IntegratedData.nLRMirror = HWImageCalOption.nLRMirror;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nLRMirror);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.nTBMirror != HWImageCalOption.nTBMirror)
            {
                // 사용값->Top<->Bottom Mirror (상하반전) 적용
                m_IntegratedData.nTBMirror = HWImageCalOption.nTBMirror;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nTBMirror);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bPMTPowerOnFlag != HWImageCalOption.bPMTPowerOnFlag)
            {
                // 사용값->PMT Hv Power On Flag 적용
                m_IntegratedData.bPMTPowerOnFlag = HWImageCalOption.bPMTPowerOnFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bPMTPowerOnFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bDETPowerOnFlag != HWImageCalOption.bDETPowerOnFlag)
            {
                // 사용값->Detector Hv Power On Flag 적용
                m_IntegratedData.bDETPowerOnFlag = HWImageCalOption.bDETPowerOnFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bDETPowerOnFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bOrthogonalCheckFlag != HWImageCalOption.bOrthogonalCheckFlag)
            {
                // 사용값->직교도 교정값 사용 Flag 적용
                m_IntegratedData.bOrthogonalCheckFlag = HWImageCalOption.bOrthogonalCheckFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bOrthogonalCheckFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bStageHorzAlignCheckFlag != HWImageCalOption.bStageHorzAlignCheckFlag)
            {
                // 사용값->Stage  수평값 사용 Flag 적용
                m_IntegratedData.bStageHorzAlignCheckFlag = HWImageCalOption.bStageHorzAlignCheckFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bStageHorzAlignCheckFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bWDSRotAdjustCheckFlag != HWImageCalOption.bWDSRotAdjustCheckFlag)
            {
                // 사용값->WD에 따른 S-Rot 보정기능 사용 Flag 적용
                m_IntegratedData.bWDSRotAdjustCheckFlag = HWImageCalOption.bWDSRotAdjustCheckFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bWDSRotAdjustCheckFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bWDScaleAdjustFlag != HWImageCalOption.bWDScaleAdjustFlag)
            {
                // 사용값->WD에 따른 Zoom Scale 값 보정 여부 적용
                m_IntegratedData.bWDScaleAdjustFlag = HWImageCalOption.bWDScaleAdjustFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bWDScaleAdjustFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bWDHVCompensationFlag != HWImageCalOption.bWDHVCompensationFlag)
            {
                // 사용값->WD에 따른 Zoom 값 보정시 HV 보정 여부 적용
                m_IntegratedData.bWDHVCompensationFlag = HWImageCalOption.bWDHVCompensationFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bWDHVCompensationFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bPhotoNoneStopFlag != HWImageCalOption.bPhotoNoneStopFlag)
            {
                // 사용값->PHOTO IMAGE SCAN 시 Scan 기능을 Stop 기능없이 Scan을 완성할 것인지 설정 적용
                m_IntegratedData.bPhotoNoneStopFlag = HWImageCalOption.bPhotoNoneStopFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bPhotoNoneStopFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bZFocusLinkFlag != HWImageCalOption.bZFocusLinkFlag)
            {
                // 사용값->Z Focus Link Flag 적용
                m_IntegratedData.bZFocusLinkFlag = HWImageCalOption.bZFocusLinkFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bZFocusLinkFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bAFBaseOnWDFlag != HWImageCalOption.bAFBaseOnWDFlag)
            {
                // 사용값->Autofocus Base On WD Flag 적용
                m_IntegratedData.bAFBaseOnWDFlag = HWImageCalOption.bAFBaseOnWDFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bAFBaseOnWDFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bPhotoMedianFilterFlag != HWImageCalOption.bPhotoMedianFilterFlag)
            {
                // 사용값->Photo Median Filter Flag 적용
                m_IntegratedData.bPhotoMedianFilterFlag = HWImageCalOption.bPhotoMedianFilterFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bPhotoMedianFilterFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bNEGImageFlag != HWImageCalOption.bNEGImageFlag)
            {
                // 사용값->음영 반전 Flag 적용
                m_IntegratedData.bNEGImageFlag = HWImageCalOption.bNEGImageFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bNEGImageFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bLRMirrorFlag != HWImageCalOption.bLRMirrorFlag)
            {
                // 사용값->좌우 반전 Flag 적용
                m_IntegratedData.bLRMirrorFlag = HWImageCalOption.bLRMirrorFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bLRMirrorFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bTBMirrorFlag != HWImageCalOption.bTBMirrorFlag)
            {
                // 사용값->상하 반전 Flag 적용
                m_IntegratedData.bTBMirrorFlag = HWImageCalOption.bTBMirrorFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bTBMirrorFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bRED1FileScanFlag != HWImageCalOption.bRED1FileScanFlag)
            {
                // 사용값->RED1 File Scan Select Flag 적용
                m_IntegratedData.bRED1FileScanFlag = HWImageCalOption.bRED1FileScanFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bRED1FileScanFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bRED2FileScanFlag != HWImageCalOption.bRED2FileScanFlag)
            {
                // 사용값->RED2 File Scan Select Flag 적용
                m_IntegratedData.bRED2FileScanFlag = HWImageCalOption.bRED2FileScanFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bRED2FileScanFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bFULL1FileScanFlag != HWImageCalOption.bFULL1FileScanFlag)
            {
                // 사용값->FULL1 File Scan Select Flag 적용
                m_IntegratedData.bFULL1FileScanFlag = HWImageCalOption.bFULL1FileScanFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bFULL1FileScanFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.bFULL2FileScanFlag != HWImageCalOption.bFULL2FileScanFlag)
            {
                // 사용값->FULL2 File Scan Select Flag
                m_IntegratedData.bFULL2FileScanFlag = HWImageCalOption.bFULL2FileScanFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bFULL2FileScanFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.nOLCompensation != HWImageCalOption.nOLCompensation)
            {
                // 사용값->배율에 따른 OL 보정 방식 설정 (0->None, 1->Scale Change, 2->Image Zomm Change)
                m_IntegratedData.nOLCompensation = HWImageCalOption.nOLCompensation;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nOLCompensation);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 IMAGE CALIBRATION OPTION 설정 DATA 변경 확인
            if (m_IntegratedData.nStageHorAlign != HWImageCalOption.nStageHorAlign)
            {
                // 사용값->Stage 수평 설정값
                m_IntegratedData.nStageHorAlign = HWImageCalOption.nStageHorAlign;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nStageHorAlign);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nXScanRot45Deg, HWImageCalOption.nXScanRot45Deg, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->X Scan Rotation 45deg 설정 값 적용
                memcpy(m_IntegratedData.nXScanRot45Deg, HWImageCalOption.nXScanRot45Deg, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanRot45Deg[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nXScanRot90Deg, HWImageCalOption.nXScanRot90Deg, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->X Scan Rotation 90deg 설정 값 적용
                memcpy(m_IntegratedData.nXScanRot90Deg, HWImageCalOption.nXScanRot90Deg, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanRot90Deg[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nYScanRot45Deg, HWImageCalOption.nYScanRot45Deg, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Y Scan Rotation 45deg 설정 값 적용
                memcpy(m_IntegratedData.nYScanRot45Deg, HWImageCalOption.nYScanRot45Deg, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanRot45Deg[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nYScanRot90Deg, HWImageCalOption.nYScanRot90Deg, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Y Scan Rotation 90deg 설정 값 적용
                memcpy(m_IntegratedData.nYScanRot90Deg, HWImageCalOption.nYScanRot90Deg, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYScanRot90Deg[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nZoomXMaxValue, HWImageCalOption.nZoomXMaxValue, sizeof(int)*MAX_XY_ZOOM_ITEM) != NULL)
            {
                // 설정값->Max X Zoom Item 설정 값 적용
                memcpy(m_IntegratedData.nZoomXMaxValue, HWImageCalOption.nZoomXMaxValue, sizeof(int)*MAX_XY_ZOOM_ITEM);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nZoomXMaxValue[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 IMAGE CALIBRATION OPTION 설정 값 변경 확인
            if (memcmp(m_IntegratedData.nZoomYMaxValue, HWImageCalOption.nZoomYMaxValue, sizeof(int)*MAX_XY_ZOOM_ITEM) != NULL)
            {
                // 설정값->Max Y Zoom Item 설정 값 적용
                memcpy(m_IntegratedData.nZoomYMaxValue, HWImageCalOption.nZoomYMaxValue, sizeof(int)*MAX_XY_ZOOM_ITEM);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nZoomYMaxValue[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18530 ----
        // USB CONTROL DATA : STAGE AXIS POSITION CONTROL DATA
        public int OnSemControlStagePositionData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // STAGE AXIS CURRENT POSITION DATA 정보
            SEM_CONTROL_STAGE_AXIS_POSITION StageAxisPosition = new SEM_CONTROL_STAGE_AXIS_POSITION();
            // LONG DATA 복사
            StageAxisPosition.CopyFrom((SEM_CONTROL_STAGE_AXIS_POSITION)lParam);


            // STAGE AXIS(X,Y,Z,R,T...) CURRENT POSITION 값 적용 ( pulse )
            memcpy(m_IntegratedData.nAxisPosition, StageAxisPosition.nAxisPosition, sizeof(int)*MAX_AXIS_COUNT);
            // STAGE AXIS(X,Y,Z,R,T...) CURRENT POSITION 값 적용 ( mm,deg )
            memcpy(m_IntegratedData.dCurAxisPos, StageAxisPosition.dCurAxisPos, sizeof(double)*MAX_AXIS_COUNT);
            // 위치적용
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_X_AXIS, m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Y_AXIS, m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Z_AXIS, m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_R_AXIS, m_IntegratedData.dCurAxisPos[IDX_R_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_T_AXIS, m_IntegratedData.dCurAxisPos[IDX_T_AXIS]);

            /*
            // STAGE AXIS CURRENT POSITION -> Step(Pulse) 값 변경 확인
            if (memcmp(m_IntegratedData.nAxisPosition, StageAxisPosition.nAxisPosition, sizeof(int)*MAX_AXIS_COUNT) != NULL )
            {
                // STAGE AXIS(X,Y,Z,R,T...) CURRENT POSITION 값 적용
                memcpy(m_IntegratedData.nAxisPosition, StageAxisPosition.nAxisPosition, sizeof(int)*MAX_AXIS_COUNT);
                // 변경 및 UI 처리
                CString strText = _T("");
                strText.Format(_T("%d"), m_IntegratedData.nAxisPosition[IDX_X_AXIS]);
                m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // STAGE AXIS CURRENT POSITION -> 현재 위치(mm, deg) 값 변경 확인
            if (memcmp(m_IntegratedData.dCurAxisPos, StageAxisPosition.dCurAxisPos, sizeof(double)*MAX_AXIS_COUNT) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) CURRENT POSITION 값 적용
                memcpy(m_IntegratedData.dCurAxisPos, StageAxisPosition.dCurAxisPos, sizeof(double)*MAX_AXIS_COUNT);
                // 변경 및 UI 처리
                CString strText = _T("");
                strText.Format(_T("%g"), m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
                m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            */

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18582 ----
        // USB CONTROL DATA : STAGE AXIS mm/step CONTROL DATA
        public int OnSemControlStageMMStepData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // STAGE AXIS mm/step DATA 정보
            SEM_CONTROL_STAGE_AXIS_MM_STEP StageAxisMMStep = new SEM_CONTROL_STAGE_AXIS_MM_STEP();
            // LONG DATA 복사
            StageAxisMMStep.CopyFrom((SEM_CONTROL_STAGE_AXIS_MM_STEP)lParam);

            // STAGE AXIS(X,Y,Z,R,T...) [mm]/Pulse Unit -> 1Pulse 당 [mm]이동거리 값 적용
            memcpy(m_IntegratedData.dAxisMMPerPulse, StageAxisMMStep.dAxisMMPerPulse, sizeof(double)*MAX_AXIS_MM_PER_PULSE);

            // Stage Axis mm/step, deg/step Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_X_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Y_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Z_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_R_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_T_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS]);
            // Stage Axis mm/step, deg/step Edit Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_X_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_Y_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_Z_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_R_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_T_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS]);

            /*
            // STAGE AXIS mm/step -> Step(Pulse) 값 변경 확인
            if (memcmp(m_IntegratedData.dAxisMMPerPulse, StageAxisMMStep.dAxisMMPerPulse, sizeof(double)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) [mm]/Pulse Unit -> 1Pulse 당 [mm]이동거리 값 적용
                memcpy(m_IntegratedData.dAxisMMPerPulse, StageAxisMMStep.dAxisMMPerPulse, sizeof(double)*MAX_AXIS_MM_PER_PULSE);
            }

            // STAGE AXIS mm/step ->  1Unit 당 Pulse갯수 값 변경 확인
            if (memcmp(m_IntegratedData.dAxisPulsePerUnit, StageAxisMMStep.dAxisPulsePerUnit, sizeof(double)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) 1Unit(1mm, 1deg) 당 Pulse갯수 -> 각 축 Unit당 Pulse 갯수 값 적용
                memcpy(m_IntegratedData.dAxisPulsePerUnit, StageAxisMMStep.dAxisPulsePerUnit, sizeof(double)*MAX_AXIS_MM_PER_PULSE);
            }
            */

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18628 ----
        // USB CONTROL DATA : STAGE AXIS max mm/step CONTROL DATA
        public int OnSemControlStageMaxMMStepData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            /*
            // STAGE AXIS max mm/step DATA 정보
            SEM_CONTROL_STAGE_MAX_MM_STEP StageMaxMMStep = new SEM_CONTROL_STAGE_MAX_MM_STEP();
            StageMaxMMStep = new SEM_CONTROL_STAGE_MAX_MM_STEP();
            // LONG DATA 복사
            memcpy(&StageMaxMMStep, (SEM_CONTROL_STAGE_MAX_MM_STEP*)lParam, sizeof(SEM_CONTROL_STAGE_MAX_MM_STEP));

            // STAGE AXIS max mm/step -> 최대 이동거리 Pulse 값 변경 확인
            if (memcmp(m_IntegratedData.nAxisMaxWidthPulse, StageMaxMMStep.nAxisMaxWidthPulse, sizeof(int)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) 최대 이동거리를 Pulse 값으로 저장한 값 적용
                memcpy(m_IntegratedData.nAxisMaxWidthPulse, StageMaxMMStep.nAxisMaxWidthPulse, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nAxisMaxWidthPulse[IDX_X_AXIS]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // STAGE AXIS mm/step ->  최대 이동거리 값 변경 확인
            if (memcmp(m_IntegratedData.dAxisMaxWidthMM, StageMaxMMStep.dAxisMaxWidthMM, sizeof(double)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) 최대 이동거리를 [mm] 또는 각도[deg] 값으로 저장한 값 적용
                memcpy(m_IntegratedData.dAxisMaxWidthMM, StageMaxMMStep.dAxisMaxWidthMM, sizeof(double)*MAX_AXIS_MM_PER_PULSE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%g"), m_IntegratedData.dAxisMaxWidthMM[IDX_X_AXIS]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            */

            // 2020.12.08
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // USB Data 메시지 정보 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            if (UsbDeviceDataInfo.nCommand != SBR_CMD_RM) return 0;

            string strText = "";
            double  dMaxWidth = 0.0;

            //
            // Axis 최대 이동거리[mm] 또는 이동각도[deg] 값 읽기 
            //
            switch (UsbDeviceDataInfo.nState)
            {
                case IDX_X_AXIS:
                {
                    // X AXIS MAX WIDTH [mm]
                    strText = ToStr(UsbDeviceDataInfo.szData);
                    dMaxWidth = _wtof(strText);
                    m_wndStageControlDlg.SetXYZStageMaxWidthAndSizeDisplay(IDX_X_AXIS, dMaxWidth);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y AXIS MAX WIDTH [mm]
                    strText = ToStr(UsbDeviceDataInfo.szData);
                    dMaxWidth = _wtof(strText);
                    m_wndStageControlDlg.SetXYZStageMaxWidthAndSizeDisplay(IDX_Y_AXIS, dMaxWidth);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z AXIS MAX WIDTH [mm]
                    strText = ToStr(UsbDeviceDataInfo.szData);
                    dMaxWidth = _wtof(strText);
                    m_wndStageControlDlg.SetXYZStageMaxWidthAndSizeDisplay(IDX_Z_AXIS, dMaxWidth);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // R AXIS MAX WIDTH [mm]
                    strText = ToStr(UsbDeviceDataInfo.szData);
                    break;
                }
                case IDX_T_AXIS + 1:
                {
                    // T AXIS MAX WIDTH [mm]
                    strText = ToStr(UsbDeviceDataInfo.szData);
                    break;
                }
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18723 ----
        // USB CONTROL DATA : STAGE AXIS ACCELATION CONTROL DATA
        public int OnSemControlStageAccelData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // STAGE AXIS ACCELATION DATA 정보
            SEM_CONTROL_STAGE_AXIS_ACCEL StageAxisAccel = new SEM_CONTROL_STAGE_AXIS_ACCEL();
            // LONG DATA 복사
            StageAxisAccel.CopyFrom((SEM_CONTROL_STAGE_AXIS_ACCEL)lParam);

            // STAGE AXIS ACCELATION -> Accel(가속) 값 변경 확인
            if (memcmp(m_IntegratedData.nAxisAccCalVal, StageAxisAccel.nAxisAccCalVal, sizeof(int)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) Accel(가속) 값으로 저장한 값 적용
                memcpy(m_IntegratedData.nAxisAccCalVal, StageAxisAccel.nAxisAccCalVal, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nAxisAccCalVal[IDX_X_AXIS]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18749 ----
        // USB CONTROL DATA : STAGE AXIS FIX POSITION CONTROL DATA
        public int OnSemControlStageFixPositionData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // 2020.05.04
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            // DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // 샘플 교환 위치 정보
            // UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]; (원본: 값을 읽기만 하고 버리는 문장)
            if (UsbDeviceDataInfo.nState == IDX_X_AXIS)
            {
                // 변경 및 UI 처리
            }
            else if (UsbDeviceDataInfo.nState == IDX_Y_AXIS)
            {
                // 변경 및 UI 처리
            }
            else if (UsbDeviceDataInfo.nState == IDX_Z_AXIS)
            {
                // 변경 및 UI 처리
            }
            else if (UsbDeviceDataInfo.nState == IDX_R_AXIS)
            {
                // 변경 및 UI 처리
            }
            else if (UsbDeviceDataInfo.nState == IDX_R_AXIS+1)
            {
                // 변경 및 UI 처리
            }

            /*
            // STAGE AXIS FIX POSITION DATA 정보
            SEM_CONTROL_STAGE_AXIS_PIX_POSITION StageAxisFixPosition = new SEM_CONTROL_STAGE_AXIS_PIX_POSITION();
            StageAxisFixPosition = new SEM_CONTROL_STAGE_AXIS_PIX_POSITION();

            // LONG DATA 복사
            memcpy(&StageAxisFixPosition, (SEM_CONTROL_STAGE_AXIS_PIX_POSITION*)lParam, sizeof(SEM_CONTROL_STAGE_AXIS_PIX_POSITION));

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nBeamCenterXPos != StageAxisFixPosition.nBeamCenterXPos)
            {
                // STAGE AXIS(X,Y,Z,R,T...) X/Y Center Position 값 적용
                m_IntegratedData.nBeamCenterXPos = StageAxisFixPosition.nBeamCenterXPos;
                // 변경 및 UI 처리
            }

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nBeamCenterYPos != StageAxisFixPosition.nBeamCenterYPos)
            {
                // STAGE AXIS(X,Y,Z,R,T...) X/Y Center Position 값 적용
                m_IntegratedData.nBeamCenterYPos = StageAxisFixPosition.nBeamCenterYPos;
                // 변경 및 UI 처리
            }

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nTiltZeroAnglePulsePos != StageAxisFixPosition.nTiltZeroAnglePulsePos)
            {
                // STAGE AXIS(X,Y,Z,R,T...) Tilt 축 0도 위치값 Pulse 값 적용
                m_IntegratedData.nTiltZeroAnglePulsePos = StageAxisFixPosition.nTiltZeroAnglePulsePos;
                // 변경 및 UI 처리
            }

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nZTopLimitSwPulse != StageAxisFixPosition.nZTopLimitSwPulse)
            {
                // STAGE AXIS(X,Y,Z,R,T...) Z Axis Top Limit SW On 상태의 위치[Pulse] 값 적용
                m_IntegratedData.nZTopLimitSwPulse = StageAxisFixPosition.nZTopLimitSwPulse;
                // 변경 및 UI 처리
            }

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nWDTopLimit != StageAxisFixPosition.nWDTopLimit)
            {
                // STAGE AXIS(X,Y,Z,R,T...) WD Top Limit 설정 값 적용
                m_IntegratedData.nWDTopLimit = StageAxisFixPosition.nWDTopLimit;
                // 변경 및 UI 처리
            }

            // STAGE AXIS FIX POSITION -> ( X-Y Center, Tilt Zero, Z Top Pulse, WD Top/Bottom ) 값 변경 확인
            if (m_IntegratedData.nWDBottomLimit != StageAxisFixPosition.nWDBottomLimit)
            {
                // STAGE AXIS(X,Y,Z,R,T...) WD Bottom Limit 설정 값 적용
                m_IntegratedData.nWDBottomLimit = StageAxisFixPosition.nWDBottomLimit;
                // 변경 및 UI 처리
            }
            */

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18843 ----
        // USB CONTROL DATA : STAGE AXIS LIMIT SW STATUS CONTROL DATA
        public int OnSemControlStageLimitStatusData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // 2020.05.04
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // USB DEVICE DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // STAGE AXIS LIMIT SW STATUS DATA 정보
            SEM_CONTROL_STAGE_AXIS_LIMIT_STATUS StageAxisLimitStatus = new SEM_CONTROL_STAGE_AXIS_LIMIT_STATUS();

            // DATA 복사
            // STAGE AXIS LIMIT SW STATUS -> 모터 각 축의 Limit SW 상태 값 변경 확인
            StageAxisLimitStatus.nAxisLimitSWStatus[0] = UsbDeviceDataInfo.nData[0];
            StageAxisLimitStatus.nAxisLimitSWStatus[1] = UsbDeviceDataInfo.nData[1];
            StageAxisLimitStatus.nAxisLimitSWStatus[2] = UsbDeviceDataInfo.nData[2];
            StageAxisLimitStatus.nAxisLimitSWStatus[3] = UsbDeviceDataInfo.nData[3];
            StageAxisLimitStatus.nAxisLimitSWStatus[4] = UsbDeviceDataInfo.nData[4];
            StageAxisLimitStatus.nAxisLimitSWStatus[5] = UsbDeviceDataInfo.nData[5];
            StageAxisLimitStatus.nAxisLimitSWStatus[6] = UsbDeviceDataInfo.nData[6];
            StageAxisLimitStatus.nAxisLimitSWStatus[7] = UsbDeviceDataInfo.nData[7];
            StageAxisLimitStatus.nAxisLimitSWStatus[8] = UsbDeviceDataInfo.nData[8];
            StageAxisLimitStatus.nAxisLimitSWStatus[9] = UsbDeviceDataInfo.nData[9];
            // STAGE AXIS HOME SW STATUS -> 모터 각 축의 Limit & Home 상태 값 변경 확인
            StageAxisLimitStatus.nAxisLimitHomeStatus[0] = UsbDeviceDataInfo.nData[0];
            StageAxisLimitStatus.nAxisLimitHomeStatus[1] = UsbDeviceDataInfo.nData[1];
            StageAxisLimitStatus.nAxisLimitHomeStatus[2] = UsbDeviceDataInfo.nData[2];
            StageAxisLimitStatus.nAxisLimitHomeStatus[3] = UsbDeviceDataInfo.nData[3];
            StageAxisLimitStatus.nAxisLimitHomeStatus[4] = UsbDeviceDataInfo.nData[4];
            StageAxisLimitStatus.nAxisLimitHomeStatus[5] = UsbDeviceDataInfo.nData[5];
            StageAxisLimitStatus.nAxisLimitHomeStatus[6] = UsbDeviceDataInfo.nData[6];
            StageAxisLimitStatus.nAxisLimitHomeStatus[7] = UsbDeviceDataInfo.nData[7];
            StageAxisLimitStatus.nAxisLimitHomeStatus[8] = UsbDeviceDataInfo.nData[8];
            StageAxisLimitStatus.nAxisLimitHomeStatus[9] = UsbDeviceDataInfo.nData[9];

            // STAGE AXIS(X,Y,Z,R,T...) 모터 각 축의 Limit SW 상태 값 적용
            memcpy(m_IntegratedData.nAxisLimitSWStatus, StageAxisLimitStatus.nAxisLimitSWStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
            // 변경 및 UI 처리

            // STAGE AXIS(X,Y,Z,R,T...) 모터 각 축의 Limit & Home 상태 값 적용
            memcpy(m_IntegratedData.nAxisLimitHomeStatus, StageAxisLimitStatus.nAxisLimitHomeStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
            // 변경 및 UI 처리

            /*
            // STAGE AXIS LIMIT SW STATUS -> 모터 각 축의 Limit SW 상태 값 변경 확인
            if (memcmp(m_IntegratedData.nAxisLimitSWStatus, StageAxisLimitStatus.nAxisLimitSWStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) 모터 각 축의 Limit SW 상태 값 적용
                memcpy(m_IntegratedData.nAxisLimitSWStatus, StageAxisLimitStatus.nAxisLimitSWStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
                // 변경 및 UI 처리
            }

            // STAGE AXIS HOME SW STATUS -> 모터 각 축의 Limit & Home 상태 값 변경 확인
            if (memcmp(m_IntegratedData.nAxisLimitHomeStatus, StageAxisLimitStatus.nAxisLimitHomeStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE) != NULL)
            {
                // STAGE AXIS(X,Y,Z,R,T...) 모터 각 축의 Limit & Home 상태 값 적용
                memcpy(m_IntegratedData.nAxisLimitHomeStatus, StageAxisLimitStatus.nAxisLimitHomeStatus, sizeof(int)*MAX_AXIS_MM_PER_PULSE);
                // 변경 및 UI 처리
            }
            */

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18912 ----
        // USB CONTROL DATA : STAGE AXIS BMP POSITION CONTROL DATA
        public int OnSemControlStageBmpPositionData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            //
            // Scan Bridge : "UL" -> Sample Load / Unload String 값 읽기 및 적용
            //
            // "UL0,00" --> WAIT STEP
            // "UL0,01" --> IN  CHECK STEP
            // "UL0,02" --> OUT CHECK STEP
            // "UL0,03" --> IN_STEP0
            // "UL0,04" --> IN_STEP1
            // "UL0,05" --> IN_STEP2
            // "UL0,06" --> IN_STEP3
            // "UL0,07" --> TIME_STEP
            // "UL0,08" --> OUT_STEP0
            // "UL0,09" --> OUT_STEP1
            // "UL0,10" --> OUT_STEP2
            // "UL0,11" --> OUT_STEP3
            // "UL0,12" --> OUT_STEP4
            //-----------------------------
            // "UL1,00" --> UL_WAIT
            // "UL1,01" --> UL_LOADING
            // "UL1,02" --> UL_UNLOADING
            // "UL1,03" --> UL_LOADED
            // "UL1,04" --> UL_UNLOADED
            // "UL1,05" --> UL_FAIL_IN_STEP0
            // "UL1,06" --> UL_FAIL_IN_STEP1
            // "UL1,07" --> UL_FAIL_IN_STEP2
            // "UL1,08" --> UL_FAIL_IN_STEP3
            // "UL1,09" --> UL_FAIL_OUT_STEP0
            // "UL1,10" --> UL_FAIL_OUT_STEP1
            // "UL1,11" --> UL_FAIL_OUT_STEP2
            // "UL1,12" --> UL_FAIL_OUT_STEP3
            // "UL1,13" --> UL_FAIL_OUT_STEP4
            //-----------------------------
            // "UL2,00" --> REQUEST from PC : STEP  REQUEST
            // "UL2,01" --> REQUEST from PC : STATE REQUEST
            //

            // 2021.03.10
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // Sample Load Unload State 표시
            // "UL0" COMMAND
            // "UL1" COMMAND

            // STAGE CONTROL DLG
            m_wndStageControlDlg.ReceiveULStateRequestCommand(ToStr(UsbDeviceDataInfo.szData));
            // STAGE CALIBRATION DLG
            m_wndStageCalibrationToolDlg.m_XYAxisRewindAccelMoveEdit.SetWindowTextW(ToStr(UsbDeviceDataInfo.szData));

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18971 ----
        // USB CONTROL DATA : HW SETUP -> SCAN ROTATION CALIBRATION DATA ( ORTHOGONAL, ZOOM X-Y ) DISPLAY 메시지
        public int OnSemControlSRotOrthoZoomCaliData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // SCAN ROTATION CALIBRATION DATA ( ORTHOGONAL, ZOOM X-Y ) DATA 정보
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // LONG DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // ORTHOGONAL DATA 값 DISPLAY
            m_wndScanCalibrationToolDlg.SetOrthogonalPosition(UsbDeviceDataInfo.nData[0]);

            // ZOOM X DATA 값 DISPLAY
            m_wndScanCalibrationToolDlg.SetZoomXPosition(UsbDeviceDataInfo.nData[1]);

            // ZOOM Y DATA 값 DISPLAY
            m_wndScanCalibrationToolDlg.SetZoomYPosition(UsbDeviceDataInfo.nData[2]);


            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19029 ----
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //



        // CLIENT --> DEVICE : GET USB DEVICE DATA ( STAGE mm/step AXIS )
        public int OnGetUsbDeviceStageMMSTEPAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // GET USB DEVICE DATA : STAGE mm/step AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CNC Command : "RN" 1Pulse 당 [mm]값 정보 읽기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 0;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_X_AXIS)
            {
                // Stage step/mm : X-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_X_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_Y_AXIS)
            {
                // Stage step/mm : Y-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_Y_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_Z_AXIS)
            {
                // Stage step/mm : Z-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_Z_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_DEGSTEP_R_AXIS)
            {
                // Stage step/deg : R-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_R_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_DEGSTEP_T_AXIS)
            {
                // Stage step/deg : T-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_T_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_B_AXIS)
            {
                // Stage step/mm : BSE-Axis 정보 읽기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_B_AXIS, null);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19080 ----
        // CLIENT --> DEVICE : GET USB DEVICE DATA ( STAGE position AXIS )
        public int OnGetUsbDeviceStagePositionAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // GET USB DEVICE DATA : STAGE position AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CNC Command : "RP" 위치[mm][deg]값 정보 읽기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 0;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_X_AXIS)
            {
                // Stage position : X-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_X_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_Y_AXIS)
            {
                // Stage position : Y-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_Y_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_Z_AXIS)
            {
                // Stage position : Z-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_Z_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_R_AXIS)
            {
                // Stage position : R-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_R_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_T_AXIS)
            {
                // Stage position : T-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_T_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_B_AXIS)
            {

                // Stage position : BSE-Axis 정보 읽기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_B_AXIS, null);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19132 ----
        // CLIENT --> DEVICE : GET USB DEVICE DATA ( STAGE width AXIS )
        public int OnGetUsbDeviceStageWidthAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // GET USB DEVICE DATA : STAGE width AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CNC Command : "RM" Max[mm][deg]값 정보 읽기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 0;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_X_AXIS)
            {
                // Stage AXIS width [mm],[deg] : X-Axis 정보 읽기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_X_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_Y_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Y-Axis 정보 읽기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_Y_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_Z_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Z-Axis 정보 읽기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_Z_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_LIMIT_Z_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Z-Axis Top Limit 정보 읽기
                m_dev.AfxScanBridgeZTCommand(nWriteFlag, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_T_AXIS)
            {
                // Stage AXIS width [mm],[deg] : T-Axis 정보 읽기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_T_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ZERO_T_AXIS)
            {
                // Stage AXIS width [mm],[deg] : T-Axis ZERO 정보 읽기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_T_AXIS+1, null);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19183 ----
        // CLIENT --> DEVICE : GET USB DEVICE DATA ( STAGE accelation AXIS )
        public int OnGetUsbDeviceStageAccelAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // GET USB DEVICE DATA : STAGE accelation AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CNC Command : "RA" accel값 정보 읽기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 0;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_X_AXIS)
            {
                // Stage accelation : X-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_X_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_Y_AXIS)
            {
                // Stage accelation : Y-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_Y_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_Z_AXIS)
            {
                // Stage accelation : Z-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_Z_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_R_AXIS)
            {
                // Stage accelation : R-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_R_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_T_AXIS)
            {
                // Stage accelation : T-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_T_AXIS, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_B_AXIS)
            {
                // Stage accelation : BSE-Axis 정보 읽기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_B_AXIS, null);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19234 ----
        // CLIENT --> DEVICE : GET USB DEVICE DATA ( STAGE center AXIS )
        public int OnGetUsbDeviceStageCenterAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // GET USB DEVICE DATA : STAGE center AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CNC Command : "MC" -> Read Center Pos (X,Y축의 센터 위치 값 읽기 및 적용한다)
            int nWriteFlag = 0;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_CENTER_X_AXIS)
            {
                // Stage Center Pos : X-Axis 정보 읽기
                m_dev.AfxScanBridgeMCCommand(nWriteFlag, null, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_CENTER_Y_AXIS)
            {
                // Stage Center Pos : Y-Axis 정보 읽기
                m_dev.AfxScanBridgeMCCommand(nWriteFlag, null, null);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ROTARY_AREA_PULSE)
            {
                // CNC Command : "BF" ( ROTARY 영역 PULSE 값 요청 )
                // 2021.03.31
                // Stage Get Rotary Area Pulse
                m_dev.AfxScanBridgeGetRotaryAreaPulseCommand();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_CENTER_B_AXIS)
            {
                // Stage BSE Set Scan Position : BSE-Axis 정보 읽기

                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    double dBsePos = m_dev.AfxGetIntegratedPointer().dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION];
                    // BSE SET SCAN POSITION VALUE
                    m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_DOUBLE_TYPE, IDX_STAGE_OPTION_DOUBLE_BSE_POSITION, 0, 0, 0, 0, dBsePos);
                    m_wndStageControlDlg.SetStageOptionDataDOUBLEType(dBsePos);
                }
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19293 ----
        //
        //
        //
        //
        //
        //


        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE mm/step AXIS )
        public int OnSetUsbDeviceStageMMSTEPAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE step/mm AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // 설정값
            CharBuffer szText = new CharBuffer(255 + 1);
            memset(szText, 0x00, 255);
            strcpy(szText, RequestSetUsbDeviceData.szData);

            // CNC Command : "RN" 1Pulse 당 [mm]값 정보 쓰기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 1;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MMSTEP_X_AXIS)
            {
                // Stage step/mm : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_X_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MMSTEP_Y_AXIS)
            {
                // Stage step/mm : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_Y_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MMSTEP_Z_AXIS)
            {
                // Stage step/mm : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_Z_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_DEGSTEP_R_AXIS)
            {
                // Stage step/deg : R-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_R_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_DEGSTEP_T_AXIS)
            {
                // Stage step/deg : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_T_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MMSTEP_B_AXIS)
            {

                // Stage step/mm : BSE-Axis 정보 쓰기
                m_dev.AfxScanBridgeRNCommand(nWriteFlag, IDX_B_AXIS, ToStr(szText));
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19350 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( AXIS POS mm, deg )
        public int OnSetUsbDeviceStagePosMMDEGAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE Pos mm,deg AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // 설정값
            CharBuffer szText = new CharBuffer(255 + 1);
            memset(szText, 0x00, 255);
            strcpy(szText, RequestSetUsbDeviceData.szData);

            // CNC Command : "SP" Pos [mm]값 정보 쓰기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 1;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_X_AXIS)
            {
                // STAGE Pos mm,deg : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_X_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_Y_AXIS)
            {
                // STAGE Pos mm,deg : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_Y_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_Z_AXIS)
            {
                // STAGE Pos mm,deg : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_Z_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_R_AXIS)
            {
                // STAGE Pos mm,deg : R-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_R_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_T_AXIS)
            {
                // STAGE Pos mm,deg : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_T_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_POS_B_AXIS)
            {

                // STAGE Pos mm,deg : BSE-Axis 정보 쓰기
                m_dev.AfxScanBridgeRPCommand(nWriteFlag, IDX_B_AXIS, ToStr(szText));
            }

            return 1;
        }

    }
}
