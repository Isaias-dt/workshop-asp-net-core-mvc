using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            await ExistSalesRecordOfThisSellerAsync(id);
            try
            {
                var obj = _context.Seller.Find(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbe)
            {
                throw new IntegrityException(dbe.Message);
            }
        } 

        public async Task UpdateAsync(Seller seller)
        {
            bool hasAny = await _context.Seller.AnyAsync(s => s.Id == seller.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Seller not found!");
            }
            try
            {
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbe)
            {
                throw new DbConcurrencyException(dbe.Message);
            }
        }

        private async Task ExistSalesRecordOfThisSellerAsync(int id)
        {
            bool hasAny = await _context.SalesRecord.AnyAsync(obj => obj.Seller.Id == id);
            if (hasAny)
            {
                throw new IntegrityException(
                    "Cannot delete Seller without first deleted all reference data of Sales Record!");
            }
        }
    }
}
