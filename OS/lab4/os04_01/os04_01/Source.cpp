#include <iostream>
#include <windows.h>

int main() {
    while (true) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadID = GetCurrentThreadId();
        std::cout << "Process identifier: " << processId << " Thread ID: " << threadID << std::endl;
        Sleep(1000);
    }
    return 0;
}