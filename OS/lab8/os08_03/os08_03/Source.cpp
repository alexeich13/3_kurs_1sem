#include <iostream>

const int PAGE_SIZE = 4096;

int main()
{
    const int NUM_PAGES = 256;
    const int NUM_INTS_PER_PAGE = PAGE_SIZE / sizeof(int);

    int* arr = new int[NUM_PAGES * NUM_INTS_PER_PAGE];

    for (int i = 0; i < NUM_PAGES * NUM_INTS_PER_PAGE; i++)
    {
        arr[i] = i + 1;
    }

    int* byte = arr + 196 * NUM_INTS_PER_PAGE + 3851;

    std::cout << "Address: " << byte << std::endl;
    std::cout << "Count: " << *byte << std::endl;

    delete[] arr;
    return 0;
}

//Ä = C4 = 196
//ð = 240 = F0
//î = BE
//F0B = 3851