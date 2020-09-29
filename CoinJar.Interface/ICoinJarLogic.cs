using System;
using System.Collections.Generic;
using System.Text;

namespace CoinJar.Interface
{
   public interface ICoinJarLogic
    {
        void AddCoin(ICoin coin);
        decimal GetTotalAmount();
        void Reset();
    }
}
