using Microsoft.AspNetCore.Mvc;
using ClientManagement.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockQuoteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockQuoteController(AppDbContext context)
        {
            _context = context;
        }
        //-=====================Get===================================

        [HttpGet]
        public ActionResult<IEnumerable<StockQuote>> StockQuotes()
        {
            return _context.StockQuotes.ToList();
        }
        //-=====================Update&&Add===================================
        [HttpPost]
     
        public ActionResult<StockQuote> CreateClient(StockQuote newStockQuote)
        {
            if (newStockQuote.Id > 0)
            {
                // Update existing stock quote
                var existingStockQuote = _context.StockQuotes.Find(newStockQuote.Id);
                if (existingStockQuote == null)
                {
                    return NotFound();
                }
                existingStockQuote.V = newStockQuote.V;
                existingStockQuote.VW = newStockQuote.VW;
                existingStockQuote.O = newStockQuote.O;
                existingStockQuote.C = newStockQuote.C;
                existingStockQuote.H = newStockQuote.H;
                existingStockQuote.L = newStockQuote.L;
                existingStockQuote.T = newStockQuote.T;
                existingStockQuote.N = newStockQuote.N;
                _context.StockQuotes.Update(existingStockQuote);
                _context.SaveChanges();
                return Ok(existingStockQuote);
            }
            else
            {
                newStockQuote.Id = 0;
                newStockQuote.RegDate = DateTime.UtcNow;
                _context.StockQuotes.Add(newStockQuote);
                _context.SaveChanges();
                return CreatedAtAction(nameof(CreateClient), new { id = newStockQuote.Id }, newStockQuote);
            }
        }
        //-=====================Delete===================================
        [HttpDelete("{id}")]
        public IActionResult DeleteStockQuote(int id)
        {
            var stockQuote = _context.StockQuotes.Find(id);
            if (stockQuote == null)
            {
                return NotFound();
            }

            _context.StockQuotes.Remove(stockQuote);
            _context.SaveChanges();

            return NoContent();
        }


    }
}
