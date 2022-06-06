using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class CrudEF<T, TContext> : ICrud<T>, IDisposable
        where T : class
        where TContext : DbContext
    {
        protected TContext _context;

        public CrudEF(TContext context)
        {
            _context = context;
        }

        public async Task Add(T Objeto)
        {
            //using var data = new ApplicationDbContext(_OptionsBuilder);
            await _context.Set<T>().AddAsync(Objeto);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T Objeto)
        {
            //using var data = new ApplicationDbContext(_OptionsBuilder);
            _context.Set<T>().Remove(Objeto);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetEntityById(long Id)
        {
            //using var data = new ApplicationDbContext(_OptionsBuilder);
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<IEnumerable<T>> List()
        {
            //using var data = new ApplicationDbContext(_OptionsBuilder);
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task Update(T Objeto)
        {
            //using var data = new ApplicationDbContext(_OptionsBuilder);
            _context.Set<T>().Update(Objeto);
            await _context.SaveChangesAsync();
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Instantiate a SafeHandle instance.
        readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);



        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
    }
}
