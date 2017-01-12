/*--------------------------------------------------------------------------
 * Copyright (C) 1992-1998 Dallas Semiconductor Corporation. 
 * All rights Reserved. Printed in U.S.A.
 * This software is protected by copyright laws of
 * the United States and of foreign countries.
 * This material may also be protected by patent laws of the United States 
 * and of foreign countries.
 * This software is furnished under a license agreement and/or a
 * nondisclosure agreement and may only be used or copied in accordance
 * with the terms of those agreements.
 * The mere transfer of this software does not imply any licenses
 * of trade secrets, proprietary technology, copyrights, patents,
 * trademarks, maskwork rights, or any other form of intellectual
 * property whatsoever. Dallas Semiconductor retains all ownership rights.
 *--------------------------------------------------------------------------
 * Version 3.10
 */

/* includes */
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <windows.h>
#ifndef _WIN32_WCE
   #include <stdio.h>
   #include <conio.h>
   #include <dos.h>
   #include <fcntl.h>
   #include <io.h>
   #include <time.h>
#endif

/* type defs */
typedef unsigned char  uchar;
typedef unsigned short ushort;
typedef unsigned long  ulong;

/* typedef structure sent back by TMEX routines */

typedef struct      
{
   uchar name[4];
   uchar extension;
   uchar startpage;
   uchar numpages;
   uchar attrib;
   uchar bitmap[32];
} FileEntry;

/* structure to hold all device parameters by family code */

typedef struct
{
   uchar maxp;    /* max page number 0-255 */
   uchar memtype; /* 0 no mem , 1 nvram, 2 DS1982 type, 3 DS1985 and DS1986, 
                     4 DS2407, 5 eeprom DS1971 */
   char  *dsnum;  /* Dallas Semiconductor part number */
} Family;

typedef struct
{
   uchar NumEntries;      /* number of entries in path 0-10 */
   char  Ref;             /* reference character '\' or '.' */
   char  Entries[10][4];  /* sub-directory entry names */                                          
} DirectoryPath;

struct DirNumInfo // Holds info of each object in the Directory list
{  
   unsigned char Name[4];
   unsigned char Extension;
   char Attrib;
};

/* Error codes from TMEX functions */
#define    XNO_DEVICE             -1 
#define    XWRONG_TYPE            -2 
#define    XFILE_READ_ERR         -3 
#define    XBUFFER_TOO_SMALL      -4 
#define    XHANDLE_NOT_AVAIL      -5  
#define    XFILE_NOT_FOUND        -6 
#define    XREPEAT_FILE           -7 
#define    XHANDLE_NOT_USED       -8 
#define    XFILE_WRITE_ONLY       -9 
#define    XOUT_OF_SPACE          -10 
#define    XFILE_WRITE_ERR        -11 
#define    XFILE_READ_ONLY        -12 
#define    XFUNC_NOT_SUP          -13 
#define    XBAD_FILENAME          -14 
#define    XCANT_DEL_READ_ONLY    -15 
#define    XHANDLE_NOT_EXIST      -16 
#define    XONE_WIRE_PORT_ERROR   -17 
#define    XINVALID_DIRECTORY     -18 
#define    XDIRECTORY_NOT_EMPTY   -19 
#define    XUNABLE_TO_CREATE_DIR  -20 
#define    XNO_PROGRAM_JOB        -21 
#define    XPROGRAM_WRITE_PROTECT -22
#define    XNON_PROGRAM_PARTS     -23 
#define    XADDFILE_TERMINATED    -24 
#define    XTIMEOUT               -25 
#define    XINVALID_ARGUMENT      -26 
#define    XBAD_ACK               -27 
#define    XINVALID_SESSION       -200
#define    XNO_BASDRV_FOUND       -201

/* Basic physical level error codes  */
#define    BPORT_NOT_INITIALIZED   -1 
#define    BPORT_NOT_EXIST         -2 
#define    BNO_SUCH_FUNCTION       -3 

/* Transport level error codes  */
#define    TERROR_READ_WRITE       -4 
#define    TBUFFER_TOO_SMALL       -5 
#define    TDEVICE_TOO_SMALL       -6 
#define    TNO_DEVICE              -7 
#define    TBLOCK_TOO_BIG          -8 
#define    TWRONG_TYPE             -9 
#define    TPAGE_REDIRECTED        -10
#define    TPROGRAM_NOT_POSSIBLE   -11
#define    BCOM_FAILURE            -12
#define    BCOM_EVENT              -13

/* other  */
#define    GENERAL_FAIL            -2 
#define    BAD_ARGUMENT            50
#define    NO_DRIVERS              51
#define    KEY_ABORT               52
#define    OUT_MEMORY              53
#define    NORMAL_EXIT             0

#define    TRUE                    1
#define    FALSE                   0
#define    DIR_READ                1
#define    DIR_SET                 0
#define    DIR_REMOVE              1
#define    DIR_MAKE                0

// for TMOneWireLevel
#define    LEVEL_NORMAL            0
#define    LEVEL_STRONG_PULLUP     1
#define    LEVEL_BREAK             2
#define    LEVEL_PROGRAM           3  
#define    PRIMED_NONE             0
#define    PRIMED_BIT              1
#define    PRIMED_BYTE             2        
#define    LEVEL_READ              1
#define    LEVEL_SET               0


#ifdef __cplusplus
const int MaxDbnc=1;
/*
#ifdef TMEX
    int PortNum, PortType, SetupDone; // Initialized once when program is opened
    long SHandle; // Global for the session handles excluding the timer
    short ROM[8]; // Global that holds the current rom number
    unsigned char state_buffer[15360];
    AnsiString devicestr, pathstr,tstr; // Globals for form captions
    DirectoryPath Path; // Global that holds current path info
    FileEntry* SFile; // Used when selecting an item(file or dir) from the file list
#else
    extern int PortNum, PortType, SetupDone;
    extern long SHandle;
    extern short ROM[8];
    extern unsigned char state_buffer[15360];
    extern AnsiString devicestr, pathstr,tstr;
    extern DirectoryPath Path;
    extern FileEntry* SFile;
#endif
*/
#endif

#ifdef __cplusplus
   extern "C" {
// TMEX - Sesssion
 __declspec(dllimport) long pascal  TMExtendedStartSession(short, short, void *);
 __declspec(dllimport) long pascal  TMStartSession(short);
 __declspec(dllimport) short pascal TMValidSession(long);
 __declspec(dllimport) short pascal TMEndSession(long);
// TMEX - File Operations
 __declspec(dllimport) short pascal TMFirstFile(long, void *, FileEntry *);
 __declspec(dllimport) short pascal TMNextFile(long, void *, FileEntry *);
 __declspec(dllimport) short pascal TMOpenFile(long, void *, FileEntry *);
 __declspec(dllimport) short pascal TMCreateFile(long, void *, short *, FileEntry *);
 __declspec(dllimport) short pascal TMCloseFile(long, void *, short);
 __declspec(dllimport) short pascal TMReadFile(long, void *, short, unsigned char*, short);
 __declspec(dllimport) short pascal TMWriteFile(long, void *, short, unsigned char *, short);
 __declspec(dllimport) short pascal TMDeleteFile(long, void *, FileEntry *);
 __declspec(dllimport) short pascal TMFormat(long, void *);
 __declspec(dllimport) short pascal TMAttribute(long, void *, short, FileEntry *);
 __declspec(dllimport) short pascal TMReNameFile(long, void *, short, FileEntry *);
 __declspec(dllimport) short pascal TMChangeDirectory(long, void *, short, DirectoryPath *);
 __declspec(dllimport) short pascal TMDirectoryMR(long, void *, short, FileEntry *);
 __declspec(dllimport) short pascal TMCreateProgramJob(long, void *);
 __declspec(dllimport) short pascal TMDoProgramJob(long, void *);
 __declspec(dllimport) short pascal TMWriteAddFile(long, void *, short, short, short, unsigned char *, short);
 __declspec(dllimport) short pascal TMTerminateAddFile(long, void *, FileEntry *);
// TMEX - Transport
 __declspec(dllimport) short pascal TMReadPacket(long, void *, short, unsigned char *, short);
 __declspec(dllimport) short pascal TMWritePacket(long, void *, short, unsigned char *, short);
 __declspec(dllimport) short pascal TMBlockIO(long, void *, unsigned char *, short);
 __declspec(dllimport) short pascal TMExtendedReadPage(long, void *, short, unsigned char *, short);
 __declspec(dllimport) short pascal TMProgramByte(long, void *, short, short, short, short *, short);
// TMEX - Network
 __declspec(dllimport) short pascal TMFirst(long, void *);
 __declspec(dllimport) short pascal TMNext(long, void *);
 __declspec(dllimport) short pascal TMAccess(long, void *);
 __declspec(dllimport) short pascal TMStrongAccess(long, void *);
 __declspec(dllimport) short pascal TMStrongAlarmAccess(long, void *);
 __declspec(dllimport) short pascal TMOverAccess(long, void *);
 __declspec(dllimport) short pascal TMRom(long, void *, short *);
 __declspec(dllimport) short pascal TMFirstAlarm(long, void *);
 __declspec(dllimport) short pascal TMNextAlarm(long, void *);
 __declspec(dllimport) short pascal TMFamilySearchSetup(long, void *, short);
 __declspec(dllimport) short pascal TMSkipFamily(long, void *);
 __declspec(dllimport) short pascal TMAutoOverDrive(long, void *, short);
// TMEX - Hardware Specific
 __declspec(dllimport) short pascal TMSetup(long);
 __declspec(dllimport) short pascal TMTouchReset(long);
 __declspec(dllimport) short pascal TMTouchByte(long, short);
 __declspec(dllimport) short pascal TMTouchBit(long, short);
 __declspec(dllimport) short pascal TMProgramPulse(long);
 __declspec(dllimport) short pascal TMClose(long);
 __declspec(dllimport) short pascal TMOneWireCom(long, short, short);
 __declspec(dllimport) short pascal TMOneWireLevel(long, short, short, short);
 __declspec(dllimport) short pascal TMGetTypeVersion(short, char *);
 __declspec(dllimport) short pascal Get_Version(char *);
 __declspec(dllimport) short pascal TMBlockStream(long, void *, unsigned char *, short);
//---------------------------------------------------------------------------
  }
#else
// session
extern long  far pascal TMExtendedStartSession(short, short, void far *);
extern short far pascal TMValidSession(long); 
extern short far pascal TMEndSession(long); 
extern short far pascal Get_Version(char far *); 
// file_operations
extern short far pascal TMFirstFile(long, void far *, FileEntry far *); 
extern short far pascal TMNextFile(long, void far *, FileEntry far *);
extern short far pascal TMOpenFile(long, void far *, FileEntry far *);
extern short far pascal TMCreateFile(long, void far *, short far *, FileEntry far *);
extern short far pascal TMCloseFile(long, void far *, short);
extern short far pascal TMReadFile(long, void far *, short, uchar far *, short);
extern short far pascal TMWriteFile(long, void far *, short, uchar far *, short);
extern short far pascal TMDeleteFile(long, void far *, FileEntry far *);
extern short far pascal TMFormat(long, void far *);
extern short far pascal TMAttribute(long, void far *, short, FileEntry far *);
extern short far pascal TMReNameFile(long, void far *, short, FileEntry far *); 
extern short far pascal TMChangeDirectory(long, void far *, short, DirectoryPath far *); 
extern short far pascal TMDirectoryMR(long, void far *, short, FileEntry far *);
extern short far pascal TMCreateProgramJob(long, void far *);
extern short far pascal TMDoProgramJob(long, void far *);
extern short far pascal TMWriteAddFile(long, void far *, short, short, short, uchar far *, short);
extern short far pascal TMTerminateAddFile(long, void far *, FileEntry far *);
// transport
extern short far pascal TMReadPacket(long, void far *, short, uchar far *, short);
extern short far pascal TMWritePacket(long, void far *, short, uchar far *, short);
extern short far pascal TMBlockIO(long, uchar far *, short);
extern short far pascal TMExtendedReadPage(long, void far *, short, uchar far *, short);
extern short far pascal TMProgramByte(long, void far *, short, short, short, short far *, short);
// network
extern short far pascal TMSkipFamily(long, void far *); 
extern short far pascal TMFamilySearchSetup(long, void far *, short); 
extern short far pascal TMFirst(long, void far *);
extern short far pascal TMNext(long, void far *);
extern short far pascal TMAccess(long, void far *);
extern short far pascal TMOverAccess(long, void far *);
extern short far pascal TMStrongAccess(long, void far *);
extern short far pascal TMStrongAlarmAccess(long, void far *);
extern short far pascal TMRom(long, void far *, short far *);
extern short far pascal TMFirstAlarm(long, void far *);
extern short far pascal TMNextAlarm(long, void far *);  
extern short far pascal TMAutoOverDrive(long, void far *, short);  
// hardware_specific
extern short far pascal TMSetup(long);
extern short far pascal TMTouchByte(long, short);
extern short far pascal TMTouchReset(long);
extern short far pascal TMTouchBit(long, short);
extern short far pascal TMClose(long);
extern short far pascal TMProgramPulse(long); 
extern short far pascal TMOneWireCom(long, short, short); 
extern short far pascal TMOneWireLevel(long, short, short, short); 
extern short far pascal TMGetTypeVersion(short,char far *); 
extern short far pascal TMBlockStream(long, uchar far *, short);

#endif

/* Global Variables */
#ifdef TMEXUTIL

/* specification for each device supported in tmex */
Family FamSpec[] = { {   0, 0, ""      }, // 00
                     {   0, 0, "1990"  }, // 01
                     {   0, 0, "1991"  }, // 02
                     {   0, 0, ""      }, // 03
                     {  15, 1, "1994"  }, // 04
                     {   0, 0, ""      }, // 05
                     {  15, 1, "1993"  }, // 06
                     {   0, 0, ""      }, // 07
                     {   3, 1, "1992"  }, // 08
                     {   3, 2, "1982"  }, // 09
                     {  63, 1, "1995"  }, // 0A
                     {  63, 3, "1985"  }, // 0B
                     { 255, 1, "1996"  }, // 0C
                     {   0, 0, ""      }, // 0D
                     {   0, 0, ""      }, // 0E
                     { 255, 3, "1986"  }, // 0F     
                     {   0, 0, ""      }, // 10
                     {   0, 0, ""      }, // 11
                     {   3, 4, "2407"  }, // 12  /* (2.10) */
                     {   0, 0, ""      }, // 13
                     {   0, 5, "1971"  }, // 14  /* (2.10) */
                     {   0, 0, ""      }, // 15
                     {   0, 0, ""      }, // 16
                     {   0, 0, ""      }, // 17
                     {   3, 1, "1962"  }, // 18 /* (3.10) */
                     {   0, 0, ""      }, // 19
                     {  15, 1, "1963"  }, // 1A /* (3.10) */
                     {   0, 0, ""      }, // 1B
                     {   3, 1, "2422"  }, // 1C /* (3.10) */
                     {  15, 1, "2423"  }, // 1D /* (3.10) */
                     {   0, 0, ""      }, // 1E
                     {   0, 0, ""      }, // 1F
                     {   0, 0, ""      }, // 20
                     {   0, 0, ""      }, // 21
                     {   0, 0, ""      }, // 22
                     {   0, 0, ""      }, // 23
                     {   0, 0, ""      }, // 24
                     {   0, 0, ""      }, // 25
                     {   0, 0, ""      }, // 26
                     {   0, 0, ""      }, // 27
                     {   0, 0, ""      }, // 28
                     {   0, 0, ""      }, // 29
                     {   0, 0, ""      }, // 2A
                     {   0, 0, ""      }, // 2B
                     {   0, 0, ""      }, // 2C
                     {   0, 0, ""      }, // 2D
                     {   0, 0, ""      }, // 2E
                     {   0, 0, ""      }, // 2F
                     {   0, 0, ""      }, // 30
                     {   0, 0, ""      }, // 31
                     {   0, 0, ""      }, // 32
                     {   0, 0, ""      }, // 33
                     {   0, 0, ""      }, // 34
                     {   0, 0, ""      }, // 35
                     {   0, 0, ""      }, // 36
                     {   0, 0, ""      }, // 37
                     {   0, 0, ""      }, // 38
                     {   0, 0, ""      }, // 39
                     {   0, 0, ""      }, // 3A
                     {   0, 0, ""      }, // 3B
                     {   0, 0, ""      }, // 3C
                     {   0, 0, ""      }, // 3D
                     {   0, 0, ""      }, // 3E
                     {   0, 0, ""      }, // 3F
                     {  15, 1, "1608"  }, // 40 /* (3.10) */
                     {   0, 0, ""      }, // 41
                     {   0, 0, ""      }, // 42
                     {   0, 0, ""      }, // 43
                     {   0, 0, ""      }, // 44
                     {   0, 0, ""      }, // 45
                     {   0, 0, ""      }, // 46
                     {   0, 0, ""      }, // 47
                     {   0, 0, ""      }, // 48
                     {   0, 0, ""      }, // 49
                     {   0, 0, ""      }, // 4A
                     {   0, 0, ""      }, // 4B
                     {   0, 0, ""      }, // 4C
                     {   0, 0, ""      }, // 4D
                     {   0, 0, ""      }, // 4E
                     {   0, 0, ""      }, // 4F
                     {   0, 0, ""      }, // 50
                     {   0, 0, ""      }, // 51
                     {   0, 0, ""      }, // 52
                     {   0, 0, ""      }, // 53
                     {   0, 0, ""      }, // 54
                     {   0, 0, ""      }, // 55
                     {   0, 0, ""      }, // 56
                     {   0, 0, ""      }, // 57
                     {   0, 0, ""      }, // 58
                     {   0, 0, ""      }, // 59
                     {   0, 0, ""      }, // 5A
                     {   0, 0, ""      }, // 5B
                     {   0, 0, ""      }, // 5C
                     {   0, 0, ""      }, // 5D
                     {   0, 0, ""      }, // 5E
                     {   0, 0, ""      }, // 5F
                     {   0, 0, ""      }, // 60
                     {   0, 0, ""      }, // 61
                     {   0, 0, ""      }, // 62
                     {   0, 0, ""      }, // 63
                     {   0, 0, ""      }, // 64
                     {   0, 0, ""      }, // 65
                     {   0, 0, ""      }, // 66
                     {   0, 0, ""      }, // 67
                     {   0, 0, ""      }, // 68
                     {   0, 0, ""      }, // 69
                     {   0, 0, ""      }, // 6A
                     {   0, 0, ""      }, // 6B
                     {   0, 0, ""      }, // 6C
                     {   0, 0, ""      }, // 6D
                     {   0, 0, ""      }, // 6E
                     {   0, 0, ""      }, // 6F
                     {   0, 0, ""      }, // 70
                     {   0, 0, ""      }, // 71
                     {   0, 0, ""      }, // 72
                     {   0, 0, ""      }, // 73
                     {   0, 0, ""      }, // 74
                     {   0, 0, ""      }, // 75
                     {   0, 0, ""      }, // 76
                     {   0, 0, ""      }, // 77
                     {   0, 0, ""      }, // 78
                     {   0, 0, ""      }, // 79
                     {   0, 0, ""      }, // 7A
                     {   0, 0, ""      }, // 7B
                     {   0, 0, ""      }, // 7C
                     {   0, 0, ""      }, // 7D
                     {   0, 0, ""      }, // 7E
                     {   0, 0, ""      }  // 7F            
                  };

#else

extern Family FamSpec[128];  /* family specification values */

#endif
