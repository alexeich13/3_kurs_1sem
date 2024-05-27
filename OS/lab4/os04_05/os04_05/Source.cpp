#include <iostream>
#include <windows.h>
#include <process.h>

HANDLE thread2Handle;

void os04_02_T1(void* params) {
    for (int i = 0; i < 50; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << "T1: Process ID " << processId << ", Thread ID " << threadId << std::endl;
        Sleep(100);
    }
}

void os04_02_T2(void* params) {
    for (int i = 0; i < 125; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << "T2: Process ID " << processId << ", Thread ID " << threadId << std::endl;

        if (i == 39) {
            TerminateThread(thread2Handle, 0);
            std::cout << "T2: Terminated at iteration 40" << std::endl;
        }

        Sleep(100);
    }
}

int main() {
    // Создание потоков
    _beginthread(os04_02_T1, 0, nullptr);
    thread2Handle = (HANDLE)_beginthread(os04_02_T2, 0, nullptr);
    for (int i = 0; i < 100; i++) {
        DWORD processId = GetCurrentProcessId();
        std::cout << "Main Process ID: " << processId << std::endl;
        Sleep(100);
    }
    return 0;
}