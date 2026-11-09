#pragma warning disable IDE1006 // 원본 이름 그대로
namespace NanoImpact.Core.Native
{
    /// <summary>원본 USB_DEVICE_DATA_INFO (DefineStructCode.h:203). 장치 계층이 화면에 SendMessage로 넘기는 값 묶음.</summary>
    public sealed class USB_DEVICE_DATA_INFO
    {
        public int nCommand;
        public int nState;
        public int[] nData = new int[Def.MAX_USB_DATA_ARRAY];
        public ushort[] wData = new ushort[Def.MAX_USB_DATA_ARRAY];
        public uint[] dwData = new uint[Def.MAX_USB_DATA_ARRAY];
        public double[] dData = new double[Def.MAX_USB_DATA_ARRAY];
        public CharBuffer szData = new CharBuffer(Def.MAX_USB_TEXT_ARRAY + 1);
        public CharBuffer szRemark = new CharBuffer(Def.MAX_USB_TEXT_ARRAY + 1);
        public object pData;

        /// <summary>원본 memcpy(this, o, sizeof(USB_DEVICE_DATA_INFO))</summary>
        public void CopyFrom(USB_DEVICE_DATA_INFO o)
        {
            nCommand = o.nCommand;
            nState = o.nState;
            System.Array.Copy(o.nData, nData, nData.Length);
            System.Array.Copy(o.wData, wData, wData.Length);
            System.Array.Copy(o.dwData, dwData, dwData.Length);
            System.Array.Copy(o.dData, dData, dData.Length);
            CRuntime.memcpy(szData, o.szData, szData.Capacity);
            CRuntime.memcpy(szRemark, o.szRemark, szRemark.Capacity);
            pData = o.pData;
        }
    }

    /// <summary>원본 REQUEST_USB_DEVICE_DATA (DefineStructCode.h:4055)</summary>
    public sealed class REQUEST_USB_DEVICE_DATA
    {
        public int nRequestID;
        public int nRequestType;
        public int nRequestOption;
        public int[] nRequestData = new int[Def.REQUEST_HW_DATA_MAX_ITEM];
        public double[] dRequestData = new double[Def.REQUEST_HW_DATA_MAX_ITEM];
        public CharBuffer szData = new CharBuffer(64);

        /// <summary>원본 memcpy(this, o, sizeof(REQUEST_USB_DEVICE_DATA))</summary>
        public void CopyFrom(REQUEST_USB_DEVICE_DATA o)
        {
            nRequestID = o.nRequestID;
            nRequestType = o.nRequestType;
            nRequestOption = o.nRequestOption;
            System.Array.Copy(o.nRequestData, nRequestData, nRequestData.Length);
            System.Array.Copy(o.dRequestData, dRequestData, dRequestData.Length);
            CRuntime.memcpy(szData, o.szData, szData.Capacity);
        }
    }
}
