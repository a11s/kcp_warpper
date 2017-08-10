// kcplib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "kcplib.h"


// This is an example of an exported variable
KCPLIB_API int nkcplib=0;

// This is an example of an exported function.
KCPLIB_API int fnkcplib(void)
{
    return 42;
}

// This is the constructor of a class that has been exported.
// see kcplib.h for the class definition
Ckcplib::Ckcplib()
{
    return;
}
