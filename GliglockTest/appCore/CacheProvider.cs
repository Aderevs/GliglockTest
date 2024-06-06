using AutoMapper;
using GliglockTest.appCore.Interfaces;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GliglockTest.appCore
{
    public class CacheProvider : ICacheProvider
    {
        private readonly ITestsRepository _testsRepository;
        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;
        private const string CacheKeyTestsList = "TestsListPage";
        private const string CacheKeyCertainTest = "Tests";
        private const int NumberOfTestsInOnePage = 2;
        public CacheProvider(IMemoryCache cache, ITestsRepository testsRepository, IMapper mapper)
        {
            _cache = cache;
            _testsRepository = testsRepository;
            _mapper = mapper;
        }
        private async Task<TestsListPage> RefillCacheForPageAndGetTests(ushort pageNumber)
        {
            var tests = await FetchTestsPageFromDB(pageNumber);
            TestsListPage testsList = new TestsListPage
            {
                TestList = tests,
                PageNumber = pageNumber
            };
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(CacheKeyTestsList + pageNumber.ToString(), testsList, cacheEntryOptions);
            return testsList;
        }
        private async Task<TestsListPage> RefillCacheForPageAndGetTests() => await RefillCacheForPageAndGetTests(1);
        private async Task<appCore.Test> GetCertainTestAndAddToCache(Guid testId)
        {
            var test = await FetchTestFromDb(testId);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
               .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _cache.Set(CacheKeyCertainTest + testId.ToString(), test, cacheEntryOptions);
            return test;
        }
        private async Task<List<appCore.Test>> FetchTestsPageFromDB(int pageNumber)
        {
            var allTestsDb = await _testsRepository.GetPaginatedSolidTestsIncludeTeacherAsync(NumberOfTestsInOnePage, pageNumber);
            return _mapper.Map<List<appCore.Test>>(allTestsDb);
        }
        private async Task<appCore.Test> FetchTestFromDb(Guid testId)
        {
            var testDb = await _testsRepository.GetSolidTestByIdAsync(testId);
            var test = _mapper.Map<appCore.Test>(testDb);
            return test;
        }
        public async Task<TestsListPage> GetTestsForPageAsync(ushort page)
        {
            TestsListPage? testsListPage;
            if (!_cache.TryGetValue(CacheKeyTestsList + page.ToString(), out testsListPage))
            {
                testsListPage = await RefillCacheForPageAndGetTests(page);
            }
            return testsListPage;
        }
        public async Task<TestsListPage> GetTestsForPageAsync() => await GetTestsForPageAsync(1);
        public async Task<appCore.Test> GetCertainTestAsync(Guid testId)
        {
            appCore.Test? test;
            if (!_cache.TryGetValue(CacheKeyCertainTest + testId.ToString(), out test))
            {
                test = await GetCertainTestAndAddToCache(testId);
            }
            return test;
        }
    }
}
