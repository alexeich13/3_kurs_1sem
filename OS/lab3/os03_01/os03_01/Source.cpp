#include <iostream>
#include <windows.h>

int main() {
    while (true) {
        DWORD processId = GetCurrentProcessId();
        std::cout << "Process identifier: " << processId << std::endl;
        Sleep(2000);
    }

    return 0;
}