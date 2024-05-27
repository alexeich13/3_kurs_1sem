#include <iostream>
#include <windows.h>

int main() {
    DWORD processId = GetCurrentProcessId();

    for (int i = 0; i < 125; i++) {
        std::cout << "Identifier of OS03_02_2: " << processId << " Iteration: " << i + 1 << std::endl;
        Sleep(1000); 
    }

    return 0;
}