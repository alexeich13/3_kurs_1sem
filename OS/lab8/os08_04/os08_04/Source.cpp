#include <iostream>
#include <windows.h>
using namespace std;

int sh(HANDLE heap)
{
    SIZE_T totalSize = 0, freeSize = 0, usedSize = 0, size = 0;
    PROCESS_HEAP_ENTRY phEntry{};
    string spaces(4, ' ');
    while (HeapWalk(heap, &phEntry))
    {
        if (phEntry.wFlags & PROCESS_HEAP_ENTRY_BUSY)
        {
            size = phEntry.cbData;
            cout << " " << "distrib" << spaces << "address: " << hex << phEntry.lpData << dec << " size: " << size << endl;
            usedSize = usedSize + phEntry.cbData;
            size = 0;
        }
        else
        {
            size = phEntry.cbData;
            cout << " " << "non-distrib" << spaces << "address: " << hex << phEntry.lpData << dec << " size: " << size << endl;
            freeSize = freeSize + phEntry.cbData;
            size = 0;
        }
    }
    cout << "Total size: " << freeSize + usedSize << " byte" << endl; 
    cout << "Distribution size: " << usedSize << " byte" << endl;
    cout << "Unallocated size: " << freeSize << " byte" << endl;
    return 0;
}

int main()
{
    HANDLE heap = GetProcessHeap();
    cout << "Before insert" << endl;
    sh(heap);
    int* arr = new int[300000];
    cout << endl;
    cout << "After insert" << endl;
    sh(heap);
    delete[] arr;
    return 0;
}
