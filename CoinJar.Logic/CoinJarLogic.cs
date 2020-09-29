using CoinJar.Interface;
using System;
using System.Reflection.Emit;
using Microsoft.Extensions.Caching.Memory;
using CoinJar.DTO;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.IO;
using CacheItemPriority = Microsoft.Extensions.Caching.Memory.CacheItemPriority;
using Newtonsoft.Json;

namespace CoinJar.Logic
{
    public class CoinJarLogic : ICoinJarLogic
    {
        private IMemoryCache _cache { get; set; }
        private MemoryCacheEntryOptions _cacheOptions { get; set; }

        private readonly string _totalCacheKey = "totalKey";

        public CoinJarLogic(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public void AddCoin(ICoin coin)
        {
            Coin _coin = null;
            decimal maxVolume = 42;
            decimal accumilatedVolume = 0;

            if (!_cache.TryGetValue<Coin>(_totalCacheKey, out _coin))
            {

                _cacheOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
                _coin = getCoinTotal();
                if (_coin == null)
                    _coin = new Coin();

                accumilatedVolume = _coin.Volume + coin.Volume;
                if (maxVolume > accumilatedVolume)
                {
                    _coin.Amount += coin.Amount;
                    _coin.Volume += coin.Volume;
                    _cache.Set<Coin>(_totalCacheKey, _coin, _cacheOptions);
                    this.saveCoin(_coin);
                }
            }
            else
            {
                _coin = _cache.Get<Coin>(_totalCacheKey);
                accumilatedVolume = _coin.Volume + coin.Volume;
                if (maxVolume > accumilatedVolume)
                {
                    _coin.Amount += coin.Amount;
                    _coin.Volume += coin.Volume;
                    _cache.Set<Coin>(_totalCacheKey, _coin, _cacheOptions);
                    this.saveCoin(_coin);
                }
            }
        }

        public decimal GetTotalAmount()
        {
            Coin _coin = null;
            if (!_cache.TryGetValue<Coin>(_totalCacheKey, out _coin))
            {
                _coin = getCoinTotal();

               if(_coin== null)
                _coin = new Coin();
       
                _coin.Amount = _coin.Amount;
            }
            else
            {
                _coin = _cache.Get<Coin>(_totalCacheKey);
            }

            return _coin.Amount;
        }

        public void Reset()
        {
            Coin coin = new Coin();
            _cache.Set<Coin>(_totalCacheKey, coin, _cacheOptions);
            saveCoin(coin);
        }

        private void saveCoin(Coin coin)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string fullPath = Path.Combine(folderPath, "kineticCache.txt");

                if (!File.Exists(fullPath))
                    File.Create(fullPath).Dispose();
 
                    using (StreamWriter outputFile = new StreamWriter(fullPath))
                    {
                        string json = JsonConvert.SerializeObject(coin);
                        outputFile.WriteLine(json);
                    }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Coin getCoinTotal()
        {
            try
            {
                Coin coin = null;
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string fullPath = Path.Combine(folderPath, "kineticCache.txt");

                if (File.Exists(fullPath))
                {
                    using (StreamReader outputFile = new StreamReader(fullPath))
                    {
                        coin = JsonConvert.DeserializeObject<Coin>(outputFile.ReadToEnd());                   
                    }
                }
               
                return coin;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}