using Backend.AdapterModels;
using Backend.Services;
using Backend.SortModels;
using Domains.Models;
using BAL.Helpers;
using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Backend.Helpers
{
    public static class IdentityInsertHelper
    {
        #region IDENTITY_INSERT ASYNC

        public static async Task EnableIdentityInsertAsync<T>(this BackendDBContext context) => await SetIdentityInsertAsync<T>(context, true);
        public static async Task DisableIdentityInsertAsync<T>(this BackendDBContext context) => await SetIdentityInsertAsync<T>(context, false);

        private static async Task SetIdentityInsertAsync<T>([NotNull] BackendDBContext context, bool enable)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            //var entityType = context.Model.FindEntityType(typeof(T).Name);

            // We need dbcontext to access the models
            var models = context.Model;

            // Get all the entity types information
            var entityTypes = models.GetEntityTypes();
            // T is Name of class
            var entityType = entityTypes.First(t => t.ClrType == typeof(T));

            var value = enable ? "ON" : "OFF";
            try
            {
                var cmd = $"SET IDENTITY_INSERT {entityType.GetTableName()} {value}";
                await context.Database.ExecuteSqlRawAsync(cmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task SaveChangesWithIdentityInsertAsync<T>([NotNull] this BackendDBContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.EnableIdentityInsertAsync<T>();
            await context.SaveChangesAsync();
            await context.EnableIdentityInsertAsync<T>();
            await transaction.CommitAsync();
        }

        public static async Task SaveChangesWithoutIdentityInsertAsync<T>([NotNull] this BackendDBContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.EnableIdentityInsertAsync<T>();
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await context.EnableIdentityInsertAsync<T>();
            await transaction.CommitAsync();
        }
        #endregion 
    }
}