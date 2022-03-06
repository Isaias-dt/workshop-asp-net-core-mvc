using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(sales => sales.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(sales => sales.Date <= maxDate.Value);
            }
            return await result
                .Include(sales => sales.Seller)
                .Include(sales => sales.Seller.Department)
                .OrderByDescending(sales => sales.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(sales => sales.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(sales => sales.Date <= maxDate.Value);
            }
            return await result
                .Include(sales => sales.Seller)
                .Include(sales => sales.Seller.Department)
                .OrderByDescending(sales => sales.Date)
                .GroupBy(sales => sales.Seller.Department)
                .ToListAsync();
        }
    }
}
