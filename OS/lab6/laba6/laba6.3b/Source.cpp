#include <iostream>
#include <Windows.h>
#include <process.h>
#include <mutex>
using namespace std;

int main()
{
	HANDLE hMutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, L"MTX");
	if (hMutex == NULL)
	{
		cerr << "Error" << GetLastError() << " - mutex not open" << endl;
		CloseHandle(hMutex);
		return -1;
	}
	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			WaitForSingleObject(hMutex, INFINITE);
		}
		else if (i == 60)
		{
			if (!ReleaseMutex(hMutex))
			{
				cout << "Relase mutex is false" << endl;
			}
		}
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}
	CloseHandle(hMutex);
	return 0;
}