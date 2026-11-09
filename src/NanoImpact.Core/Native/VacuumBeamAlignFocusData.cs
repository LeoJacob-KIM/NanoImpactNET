#pragma warning disable IDE1006 // 원본 이름 그대로
namespace NanoImpact.Core.Native
{
    /// <summary>
    /// 원본 VACUUM_BEAM_ALIGN_FOCUS_DATA (DefineStructCode.h:5189). 메인 화면 m_VacuumBeamAlignFocusData — 진공·빔·정렬·포커스 패널이 같이 쓰는 값 묶음 (파일로 저장하지 않음).
    /// 형식 대응: BOOL → int, char[N] → CharBuffer(N).
    /// </summary>
    public sealed class VACUUM_BEAM_ALIGN_FOCUS_DATA
    {
        public int m_nGunHVIndex; // GUN HV Combo : HV 선택 Index
        public int m_nSemGunHVSpinPos; // Gun HV Spin Index
        public int m_nSemSpotSizeCombo; // Spot Size Combo Index
        public int m_nSemSpotSizeSpinPos; // Spot Size Spin Index
        public int m_nSemMagnificationCombo; // Magnification Combo Index
        public int m_nSemMagnificationSpinPos; // Magnification Spin Index
        public int m_nSemWobbleCombo; // Wobble Combo Index
        public int m_nSemWobbleSpinPos; // Wobble Spin Index
        public int m_nSemAverageRedCombo; // Red  Average Combo Index
        public int m_nSemAverageRedSpinPos; // Red  Average Spin  Index
        public int m_nSemAverageFastCombo; // Fast Average Combo Index
        public int m_nSemAverageFastSpinPos; // Fast Average Spin  Index
        public int m_nIntegratedVacuumState; // SEM DEVICE 진공상태 값
        public int m_nSemVacuumState; // SEM CLIENT 진공상태 값
        public int m_nSemVacuumUsbData; // SEM 진공게이지 값
        public int m_nSemVacuumOrgData; // SEM 진공게이지 Source 값
        public double m_dSemVacuumValue; // SEM 진공게이지 계산값
        public int m_nChamberState; // CHAMBER OPEN/CLOSE
        public int m_nGunValveState; // GUN VALVE OPEN/CLOSE
        public int m_nGunHVOnOffControl; // SEM GunHV OnOff Control : 0->Not Used, 1->On, 2->Off
        public int m_nGunHVOnState; // SEM GunHV OnOff State   : 0->Off, 1->On
        public int m_nGunHVOnOffFlag; // SEM GunHV OnOff Flag    : 0->Not Used, 1->Used
        public int m_nGunHVUpEndFlag; // SEM GunHV Up End Flag   : 0->Not, 1->Up End
        public int m_nSemGunHVDelayCombo; // GUN HV DELAY : UP Combo Index
        public int m_nSemGunHVUnitCombo; // GUN HV DELAY : UP Unit Combo Index
        public double m_dSemGunHVDelayTime; // GUN HV DELAY : UP Time Value
        public int m_nSemGunHVDirectDelayCombo; // GUN HV DELAY : Direct UP/DOWN Delay Combo Index
        public int m_nSemGunHVDirectDelay; // GUN HV DELAY : Direct UP/DOWN Delay Execute Flag
        public int m_nSemFilamentDelayCombo; // FILAMENT DELAY : UP Combo Index
        public int m_nSemFilamentUnitCombo; // FILAMENT DELAY : UP Unit Combo Index
        public double m_dSemFilamentDelayTime; // FILAMENT DELAY : UP Time Value
        public int m_nSemBiasDelayCombo; // BIAS DELAY : UP Combo Index
        public int m_nSemBiasUnitCombo; // BIAS DELAY : UP Unit Combo Index
        public double m_dSemBiasDelayTime; // BIAS DELAY : UP Time Value
        public double m_dGunHVMaxVoltage; // GUN HV : UP/DOWN Max     Voltage
        public double m_dGunHVMinVoltage; // GUN HV : UP/DOWN Min     Voltage
        public double m_dGunHVRate; // GUN HV : UP/DOWN Delay   진행율 (%)
        public double m_dGunHVTargetVoltage; // GUN HV : UP/DOWN 목표    Voltage
        public double m_dGunHVStartVoltage; // GUN HV : UP/DOWN Start   Voltage
        public double m_dGunHVStopVoltage; // GUN HV : UP/DOWN Stop    Voltage
        public double m_dGunHVCurVoltage; // GUN HV : UP/DOWN Current Voltage
        public double m_dGunHVRunVoltage; // GUN HV : UP/DOWN Run     Voltage
        public double m_dGunHVDelayVoltage; // GUN HV : UP/DOWN Gun HV  Voltage : [V]/[SEC]
        public int m_nGunHVTimeRemaining; // GUN HV : UP/DOWN Delay   남은시간
        public int m_nGunHVDelayTotalStep; // GUN HV : UP/DOWN Total Step 수
        public double m_dGunHVDelay1StepValue; // GUN HV : UP/DOWN 1step 당 증가값
        public int m_nGunHVDelayStepIndex; // GUN HV : UP/DOWN Step Index
        public int m_nGunHVDelayTime; // GUN HV : UP/DOWN Delay Time Value
        public int m_nGunHVDelayUnit; // GUN HV : UP/DOWN Delay Time Unit
        public int m_nGunHVUpDownProcess; // GUN HV : Delay 실행 방식 UP->0, DOWN->1
        public double m_dGunHVRatio;
        public double m_dMagHVRatio;
        public readonly CharBuffer m_szGunHVRemaining = new CharBuffer(32+1); // GUN HV : 남은시간 String
        public int m_bOnlyGunnOffFlag; // GUN HV : ON --> OFF  
        public int m_nFilamentTargetPosition; // FILAMENT : 목표 위치값
        public int m_nFilamentStartPosition; // FILAMENT : 시작 Position
        public int m_nFilamentStopPosition; // FILAMENT : 중지 Position
        public int m_nFilamentCurPosition; // FILAMENT : 현재 Position
        public int m_nFilamentRunPosition; // FILAMENT : 실행 Position
        public double m_dFilamentRunPosition; // FILAMENT : 실행 위치값
        public double m_dFilamentTargetVoltage; // FILAMENT : 목표 Voltage
        public double m_dFilamentStartVoltage; // FILAMENT : 시작 Voltage
        public double m_dFilamentStopVoltage; // FILAMENT : 중지 Voltage
        public double m_dFilamentCurVoltage; // FILAMENT : 현재 Voltage
        public double m_dFilamentRunVoltage; // FILAMENT : 실행 Voltage
        public double m_dFilament1StepValue; // FILAMENT : 1step 당 증가값(step)
        public double m_dFilament1StepVolt; // FILAMENT : 1step 당 Voltage
        public double m_dFilamentDelayRate; // FILAMENT : 진행율(%)
        public int m_nFilamentTimeRemaining; // FILAMENT : Delay 남은시간 [sec]
        public int m_nFilamentUpDownProcess; // FILAMENT : Delay 실행 방식 UP->0, DOWN->1
        public int m_nFilamentScrollPos; // FILAMENT : Scrollbar Position
        public int m_nFilamentScrollPage; // FILAMENT : Scrollbar Page 값
        public int m_nFilamentDelayTime; // FILAMENT : UP/DOWN Delay Time Value
        public int m_nFilamentDelayUnit; // FILAMENT : UP/DOWN Delay Time Unit
        public int m_nNextFilamentUpDownExecute; // FILAMENT : FILAMENT UP/DOWN 후, BIAS UP/DOWN을 실행한다
        public int m_nBackupFilamentTargetPosition; // FILAMENT : BACKUP 목표 위치값
        public int m_nBackupFilamentStartPosition; // FILAMENT : BACKUP 시작 Position
        public int m_nBackupFilamentStopPosition; // FILAMENT : BACKUP 중지 Position
        public int m_nBackupFilamentCurPosition; // FILAMENT : BACKUP 현재 Position
        public int m_nBackupFilamentRunPosition; // FILAMENT : BACKUP 실행 Position
        public double m_dBackupFilamentRunPosition; // FILAMENT : BACKUP 실행 위치값
        public double m_dBackupFilamentTargetVoltage; // FILAMENT : BACKUP 목표 Voltage
        public double m_dBackupFilamentStartVoltage; // FILAMENT : BACKUP 시작 Voltage
        public double m_dBackupFilamentStopVoltage; // FILAMENT : BACKUP 중지 Voltage
        public double m_dBackupFilamentCurVoltage; // FILAMENT : BACKUP 현재 Voltage
        public double m_dBackupFilamentRunVoltage; // FILAMENT : BACKUP 실행 Voltage
        public readonly CharBuffer m_szFilamentRemaining = new CharBuffer(32+1); // FILAMENT : 남은시간 String
        public int m_nBiasTargetPosition; // BIAS : 목표 위치값
        public int m_nBiasStartPosition; // BIAS : 시작 Position
        public int m_nBiasStopPosition; // BIAS : 중지 Position
        public int m_nBiasCurPosition; // BIAS : 현재 Position
        public int m_nBiasRunPosition; // BIAS : 실행 Position
        public double m_dBiasRunPosition; // BIAS : 실행 Position
        public double m_dBiasTargetVoltage; // BIAS : 목표 Voltage
        public double m_dBiasStartVoltage; // BIAS : 시작 Voltage
        public double m_dBiasStopVoltage; // BIAS : 중지 Voltage
        public double m_dBiasCurVoltage; // BIAS : 현재 Voltage
        public double m_dBiasRunVoltage; // BIAS : 실행 Voltage
        public double m_dBias1StepValue; // BIAS : 1step 당 증가값(step)
        public double m_dBias1StepVolt; // BIAS : 1step 당 Voltage
        public double m_dBiasDelayRate; // BIAS : 진행율(%)
        public int m_nBiasTimeRemaining; // BIAS : Delay 남은시간 [sec]
        public int m_nBiasUpDownProcess; // BIAS : Delay 실행 방식 UP->0, DOWN->1
        public int m_nBiasScrollPos; // BIAS : Scrollbar Position
        public int m_nBiasScrollPage; // BIAS : Scrollbar Page 값
        public int m_nBiasDelayTime; // BIAS : UP/DOWN Delay Time Value
        public int m_nBiasDelayUnit; // BIAS : UP/DOWN Delay Time Unit
        public int m_nNextBiasUpDownExecute; // BIAS : FILAMENT UP/DOWN 후, BIAS UP/DOWN을 실행한다
        public int m_nBackupBiasTargetPosition; // BIAS : BACKUP 목표 위치값
        public int m_nBackupBiasStartPosition; // BIAS : BACKUP 시작 Position
        public int m_nBackupBiasStopPosition; // BIAS : BACKUP 중지 Position
        public int m_nBackupBiasCurPosition; // BIAS : BACKUP 현재 Position
        public int m_nBackupBiasRunPosition; // BIAS : BACKUP 실행 Position
        public double m_dBackupBiasRunPosition; // BIAS : BACKUP 실행 Position
        public double m_dBackupBiasTargetVoltage; // BIAS : BACKUP 목표 Voltage
        public double m_dBackupBiasStartVoltage; // BIAS : BACKUP 시작 Voltage
        public double m_dBackupBiasStopVoltage; // BIAS : BACKUP 중지 Voltage
        public double m_dBackupBiasCurVoltage; // BIAS : BACKUP 현재 Voltage
        public double m_dBackupBiasRunVoltage; // BIAS : BACKUP 실행 Voltage
        public readonly CharBuffer m_szBiasRemaining = new CharBuffer(32+1); // BIAS : 남은시간 String
        public int m_nSemGunAlignXScrollPos; // GUN ALIGN X : 현재 Position
        public int m_nSemGunAlignYScrollPos; // GUN ALIGN Y : 현재 Position
        public int m_nSemGunAlignXScrollPage; // GUN ALIGN X : Scrollbar Page 값
        public int m_nSemGunAlignYScrollPage; // GUN ALIGN Y : Scrollbar Page 값
        public int m_nSemBeamAlignXScrollPos; // BEAM ALIGN X : 현재 Position
        public int m_nSemBeamAlignYScrollPos; // BEAM ALIGN Y : 현재 Position
        public int m_nSemBeamAlignXScrollPage; // BEAM ALIGN X : Scrollbar Page 값
        public int m_nSemBeamAlignYScrollPage; // BEAM ALIGN Y : Scrollbar Page 값
        public int m_nSemImageShiftXScrollPos; // IMAGE SHIFT X : 현재 Position
        public int m_nSemImageShiftYScrollPos; // IMAGE SHIFT Y : 현재 Position
        public int m_nSemImageShiftXScrollPage; // IMAGE SHIFT X : Scrollbar Page 값
        public int m_nSemImageShiftYScrollPage; // IMAGE SHIFT Y : Scrollbar Page 값
        public int m_nSemStigXScrollPos; // STIG X : 현재 Position
        public int m_nSemStigYScrollPos; // STIG Y : 현재 Position
        public int m_nSemStigXScrollPage; // STIG X : Scrollbar Page 값
        public int m_nSemStigYScrollPage; // STIG Y : Scrollbar Page 값
        public int m_nSemContrastScrollPos; // CONTRAST : 현재 Position
        public int m_nSemBrightnessScrollPos; // BRIGHT   : 현재 Position
        public int m_nSemContrastScrollPage; // CONTRAST : Scrollbar Page 값 
        public int m_nSemBrightnessScrollPage; // BRIGHT   : Scrollbar Page 값
        public int m_nSemFocusCoarseFineScrollPos; // FOCUS    : 현재 Position
        public int m_nSemFocusCoarseFineScrollPage; // FOCUS    : Scrollbar Page 값 
        public int m_nSemScanRotationScrollPos; // SCAN ROTATION : 현재 Position
        public int m_nSemScanRotationScrollPage; // SCAN ROTATION : Scrollbar Page 값 
        public int m_nSemFocusCoarseValue; // FOCUS COARSE : 현재 값
        public int m_nSemFocusFineValue; // FOCUS FINE   : 현재 값
        public int m_nSemStigXValue; // STIG-X : 현재 값
        public int m_nSemStigYValue; // STIG-Y : 현재 값
        public int[] m_nSemDetailMin = new int[Def.MAX_SEM_DETAIL_MODE];
        public int[] m_nSemDetailMax = new int[Def.MAX_SEM_DETAIL_MODE];
        public int m_nEvacRunCount; // EVAC Command Execite Count
        public int m_nSemStartStandbyRun; // 0->Standby, 1->Start 실행
        public int m_nSemScrollbarDetailControl; // SCROLLBAR의 마우스 휠을 이용한 디테일 컨트롤 ITEM
        public int[] m_nWobblerTable = new int[Def.MAX_SIN_TABLE_ITEM]; // WOBBLER DATA
        public int m_nWobblerMode; // WOBBLER Run Mode  (0:Focus, 1:Stig-X, 2:Stig-Y)
        public int m_nWobblerCount; // WOBBLER Run Count   
        public int m_nWobblerValue; // WOBBLER Run Value
        public double m_dWobblerValue; // WOBBLER Run Value
        public int m_nSpellmanGunHV; // SPELLMAN GUN HV   DISABLED/ENABLED/RUN 상태값
        public int m_nSpellmanFilament; // SPELLMAN FILAMENT DISABLED/ENABLED/RUN 상태값
        public int m_nSpellmanBias; // SPELLMAN BIAS     DISABLED/ENABLED/RUN 상태값
        public int m_nSpellmanPMT; // SPELLMAN PMT      DISABLED/ENABLED/RUN 상태값
        public int m_nSpellmanSCIN; // SPELLMAN SCIN     DISABLED/ENABLED/RUN 상태값
        public int m_nSpellmanCOLL; // SPELLMAN COLL     DISABLED/ENABLED/RUN 상태값
        // FOCUS DEGAUSS
        public int m_nDegaussCoarse; // FOCUS COARSE 디가우징 현재 실행값  
        public int m_nDegaussMax; // FOCUS COARSE 디가우징 목표 최대값  
        public int m_nDegaussOffset; // FOCUS COARSE 디가우징 실행 증가값  
        // AUTO START PROCESS
        public int m_bAutoStartFlag; // AUTO START : 실행상태 Flag 
        public int m_bAutoStartCancel; // AUTO START : Cancel   Flag 
        public int m_nAutoStartProcess; // AUTO START : 진행상태 Flag 
        public int m_nAutoStartStep; // AUTO START : 상태 Flag 
        public readonly CharBuffer m_szContrastPos = new CharBuffer(32+1); // CONTRAST          POSITION STRING DATA   
        public readonly CharBuffer m_szBrightnessPos = new CharBuffer(32+1); // BRIGHTNESS        POSITION STRING DATA 
        public readonly CharBuffer m_szFocusCoarseFinePos = new CharBuffer(32+1); // FOCUS COARSE/FINE POSITION STRING DATA 
        // MAIN CONTROL TOOL SELECT
        public int m_nMainControlToolSelect; // MAIN CONTROL TOOL : SCAN, FOCUS, BEAM, ALIGN, VACUUM, OPTION CONTROL SELECT FLAG
        // AIR --> EVAC WAIT TIMER MESSAGE STATE
        public int m_nAirToEvacWaitTimerState; // 0->HV 사용가능, 1->진공완료시간까지 대기상태 HV 사용못함
    }
}
