#ifndef MEMORY_H
#define MEMORY_H

// How much memory the program uses, in megabytes.
// Returns 0 if the system is not supported.

#if defined(_WIN32)
#include <windows.h>
#include <psapi.h>

inline double currentMemoryMB() {
    PROCESS_MEMORY_COUNTERS info;
    if (!GetProcessMemoryInfo(GetCurrentProcess(), &info, sizeof(info))) {
        return 0;
    }
    return info.WorkingSetSize / 1048576.0;
}

inline double peakMemoryMB() {
    PROCESS_MEMORY_COUNTERS info;
    if (!GetProcessMemoryInfo(GetCurrentProcess(), &info, sizeof(info))) {
        return 0;
    }
    return info.PeakWorkingSetSize / 1048576.0;
}

#elif defined(__linux__)
#include <fstream>
#include <string>

// "VmRSS" is the memory used now, "VmHWM" is the highest it has been.
inline double readProcStatusMB(const std::string& name) {
    std::ifstream file("/proc/self/status");
    std::string word;
    while (file >> word) {
        if (word == name + ":") {
            double kilobytes = 0;
            file >> kilobytes;
            return kilobytes / 1024.0;
        }
    }
    return 0;
}

inline double currentMemoryMB() {
    return readProcStatusMB("VmRSS");
}

inline double peakMemoryMB() {
    return readProcStatusMB("VmHWM");
}

#else

inline double currentMemoryMB() {
    return 0;
}

inline double peakMemoryMB() {
    return 0;
}

#endif

#endif
