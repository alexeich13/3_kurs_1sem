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
    SIZE_T sizeMemory = 4096;
    HANDLE heap = HeapCreate(HEAP_NO_SERIALIZE | HEAP_ZERO_MEMORY, sizeMemory * 1024, NULL);
    cout << "Before insert" << endl;
    sh(heap);
    int* arr = (int*)HeapAlloc(heap, HEAP_NO_SERIALIZE | HEAP_ZERO_MEMORY, 300000 * sizeof(int));
    cout << endl;
    cout << "After insert" << endl;
    sh(heap);
    HeapFree(heap, NULL, arr);
    HeapDestroy(heap);
    return 0;
}
