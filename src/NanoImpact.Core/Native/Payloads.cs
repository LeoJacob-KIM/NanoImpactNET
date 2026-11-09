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
    }
}
