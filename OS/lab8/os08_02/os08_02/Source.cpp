#include <iostream>
#include <Windows.h>
#include <thread>

int main()
{
	for (int i = 0; i < 1000; i++)
	{
		DWORD idProccess = GetCurrentProcessId();
		std::cout << i << " " << "PID: " << idProccess << std::endl;
		std::this_thread::sleep_for(std::chrono::seconds(3));
	}
	return 0;
}