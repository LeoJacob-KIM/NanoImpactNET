// 원본 DefineStructCode.h를 MSVC x86으로 컴파일해 저장 구조체의 sizeof/offsetof를 찍는다.
// NanoImpact.Verify layout 출력과 같아야 한다. 빌드·실행: build.bat
#define WIN32_LEAN_AND_MEAN
#define _CRT_SECURE_NO_WARNINGS
#include <winsock2.h>
#include <windows.h>
#include <stddef.h>
#include <stdio.h>
#include <tchar.h>

// 이 PC에는 MFC 헤더가 없다. MFC 형식은 실행 중에만 쓰는 구조체(스레드 인자 등)에만 나오고
// 저장 구조체 배치에는 영향이 없으므로 빈 형식으로 대신한다.
class CString { void* p; public: CString& operator=(const TCHAR*) { return *this; } };
class CBrush { void* p; };
class CPen { void* p; };
class CFont { void* p; };
class CDC { void* p; };
class CStatic { void* p; };
class CWnd { void* p; };
class CWinThread { void* p; };
class CVideoGrabber { void* p; };
struct CSize { LONG cx, cy; CSize(int x = 0, int y = 0) : cx(x), cy(y) {} };

#include "DefineStructCode.h"

#define S(T) printf("%s size %u\n", #T, (unsigned)sizeof(T));
#define F(T, f) printf("%s.%s %u %u\n", #T, #f, (unsigned)offsetof(T, f), (unsigned)sizeof(((T*)0)->f));

int main()
{
#include "probe_fields.inc"
    return 0;
}
