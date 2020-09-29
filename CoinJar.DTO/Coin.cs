using CoinJar.Interface;
using System;

namespace CoinJar.DTO
{
    public class Coin : ICoin
    {
        decimal coinAmount;
        decimal coinValue;
        public decimal Amount { get => coinAmount; set => coinAmount = value; }
        public decimal Volume { get => coinValue; set => coinValue = value; }
    }
}
