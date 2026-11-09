// Harness stand-in for the progress dialog used only by AfxThreadWaitDialog (a demo, never shown here).
#pragma once
#ifndef ASSERT
#define ASSERT(x) ((void)0)
#endif

class CUPDUPDATA
{
    LPVOID m_data;
public:
    explicit CUPDUPDATA(LPVOID data) : m_data(data) {}
    LPVOID GetAppData() const { return m_data; }
    void SetProgress(LPCTSTR, int) const {}
    void SetProgress(int) const {}
    void SetProgress(LPCTSTR) const {}
    bool ShouldTerminate() const { return true; }
    void AllowCancel(bool) const {}
    void SetDialogCaption(LPCTSTR) const {}
};

typedef bool (*LP_CUPDIALOG_USERPROC)(const CUPDUPDATA*);

class CUPDialog
{
public:
    CUPDialog(HWND, LP_CUPDIALOG_USERPROC, LPVOID) {}
    INT_PTR DoModal() { return IDOK; }
};
