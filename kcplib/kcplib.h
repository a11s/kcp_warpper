// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the KCPLIB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// KCPLIB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef KCPLIB_EXPORTS
#define KCPLIB_API __declspec(dllexport)
#else
#define KCPLIB_API __declspec(dllimport)
#endif

// This class is exported from the kcplib.dll
class KCPLIB_API Ckcplib {
public:
	Ckcplib(void);
	// TODO: add your methods here.
};

extern KCPLIB_API int nkcplib;

KCPLIB_API int fnkcplib(void);
