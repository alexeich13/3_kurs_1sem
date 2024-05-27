#include <iostream>
#include <Windows.h>
#include <process.h>
#include <mutex>

using namespace std;

int main()
{
	HANDLE hSemaphore = OpenSemaphore(SEMAPHORE_ALL_ACCESS, FALSE, TEXT("Global\\SEMPH"));
	if (hSemaphore == NULL)
	{
		cerr << "Error: " << GetLastError() << " - semaphore not open" << endl;
		return -1;
	}

	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			WaitForSingleObject(hSemaphore, INFINITE);
		}
		else if (i == 60)
		{
			if (!ReleaseSemaphore(hSemaphore, 1, NULL))
			{
				cout << "Relase semaphore is false" << endl;
			}
		}
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}
	CloseHandle(hSemaphore);

	return 0;
}