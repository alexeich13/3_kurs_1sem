#include <iostream>
#include <Windows.h>
using namespace std;

int CountSetBits(DWORD_PTR bitMask)
{
	int count = 0;
	while (bitMask) {
		count += bitMask & 1;
		bitMask >>= 1;
	}
	return count;
}


int main()
{
	for (int i = 0; i < 100000; i++)
	{
		DWORD idProccess = GetCurrentProcessId();
		DWORD idThread = GetCurrentThreadId();
		DWORD_PTR priorityProccess = GetPriorityClass(GetCurrentProcess());
		DWORD_PTR priorityThread = GetThreadPriority(GetCurrentThread());

		DWORD_PTR processAffinityMask, systemAffinityMask;
		GetProcessAffinityMask(GetCurrentProcess(), &processAffinityMask, &systemAffinityMask);
		int processorCount = CountSetBits(processAffinityMask);
		int currentThreadProcessor = GetCurrentProcessorNumber();
		cout << i << " - " << "PID: " << idProccess << " - " << "TID: " << idThread << " - " << "Proccess Priority: " << priorityProccess << " - " << "Thread Priority: " << priorityThread << " - " << "Currrent Proccessor: " << currentThreadProcessor << endl;
	}
	system("pause");
}