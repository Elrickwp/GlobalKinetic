using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinJar.DTO;
using CoinJar.Interface;
using CoinJar.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinJar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinJarController : ControllerBase
    {
        private readonly ICoinJarLogic _coinJarLogic;

        public CoinJarController(ICoinJarLogic coinJarLogic)
        {
            _coinJarLogic = coinJarLogic;
        }


        [HttpGet]
        public decimal Get()
        {
            return _coinJarLogic.GetTotalAmount();
        }

        // POST: api/coinjar
        [HttpPost, Route("deposit")]
        public void Post([FromBody] Coin coin)
        {
            _coinJarLogic.AddCoin(coin);
        }

        // DELETE: api/coinjar/
        [HttpDelete]
        public void Reset()
        {
            _coinJarLogic.Reset();
        }
    }
}
