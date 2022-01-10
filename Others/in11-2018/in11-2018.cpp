//Nikolina Radulovic,IN11/2018
#include <iostream>
#include <thread>
#include <vector>
#include <future>
#include <atomic>
#include <cstdint>

using namespace std;

uint64_t find_min(vector<uint64_t>& A, int s, int e, )
{
    uint64_t m = A[s];
    for (int i = s; i < e; i++)
    {
        if(m > A[i])
        {
            m = A[i];
        }
    }
    return m;
}

int main(){

    vector<int64_t> v {1,0 ,3,-1 ,2,-2, 5,6,-3};
    const int number_of_threads = 4;
    std::vector<std::future<uint64_t>> results(4);

    int number_of_elemenets = v.size() / number_of_threads;
    std::vector<std::thread> threads;

    for(uint64_t i = 0; i < number_of_threads; i++)
    {
        uint64_t s = i * number_of_elemenets;
        uint64_t e = (i + 1) * number_of_elemenets;
        if(i + 1 < number_of_threads)
            e = v.size();

        result.emplace_back(std::async(std::launch::async, find_min, &v, s, e));
    }

    auto min_sum =  [&] () {};

    auto ispis_nit = [&] () {};

    return 0;
}
