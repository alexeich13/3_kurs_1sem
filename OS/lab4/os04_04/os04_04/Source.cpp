#include <iostream>
#include <windows.h>
#include <process.h>

void os04_02_T1(void* params) {
    for (int i = 0; i < 50; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << "T1: Process ID " << processId << ", Thread ID " << threadId << std::endl;

        if (i == 24) {
            Sleep(1000); 
        }

        Sleep(100);
    }
}

void os04_02_T2(void* params) {
    for (int i = 0; i < 125; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << "T2: Process ID " << processId << ", Thread ID " << threadId << std::endl;

        if (i == 79) {
            Sleep(1500); 
        }

        Sleep(100);
    }
}

int main() {
    _beginthread(os04_02_T1, 0, nullptr);
    _beginthread(os04_02_T2, 0, nullptr);
    for (int i = 0; i < 100; i++) {
        DWORD processId = GetCurrentProcessId();
        std::cout << "Main Process ID: " << processId << std::endl;
        if (i == 29) {
            Sleep(1000); 
        }
        Sleep(100);
    }
    return 0;
}