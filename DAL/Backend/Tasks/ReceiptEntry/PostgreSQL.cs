﻿using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.DataAccess.Extensions;
using Frapid.Framework.Extensions;
using MixERP.Sales.ViewModels;
using Npgsql;

namespace MixERP.Sales.DAL.Backend.Tasks.ReceiptEntry
{
    public sealed class PostgreSQL : IReceiptEntry
    {
        public async Task<long> PostAsync(string tenant, SalesReceipt model)
        {
            string connectionString = FrapidDbServer.GetConnectionString(tenant);
            const string sql = @"SELECT * FROM sales.post_customer_receipt
                            (
                                @UserId, @OfficeId, @LoginId, @CustomerId, 
                                @CurrencyCode, @CashAccountId, @Amount, 
                                @ExchangeRateDebit, @ExchangeRateCredit, 
                                @ReferenceNumber, @StatementReference, 
                                @CostCenterId, @CashRepositoryId, 
                                @PostedDate::date, @BankAccountId, @PaymentCardId, @BankInstrumentCode, @BankTranCode
                            );";


            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithNullableValue("@UserId", model.UserId);
                    command.Parameters.AddWithNullableValue("@OfficeId", model.OfficeId);
                    command.Parameters.AddWithNullableValue("@LoginId", model.LoginId);
                    command.Parameters.AddWithNullableValue("@CustomerId", model.CustomerId);
                    command.Parameters.AddWithNullableValue("@CurrencyCode", model.CurrencyCode);
                    command.Parameters.AddWithNullableValue("@CashAccountId", model.CashAccountId);
                    command.Parameters.AddWithNullableValue("@Amount", model.Amount);
                    command.Parameters.AddWithNullableValue("@ExchangeRateDebit", model.DebitExchangeRate);
                    command.Parameters.AddWithNullableValue("@ExchangeRateCredit", model.CreditExchangeRate);

                    command.Parameters.AddWithNullableValue("@ReferenceNumber", model.ReferenceNumber);
                    command.Parameters.AddWithNullableValue("@StatementReference", model.StatementReference);


                    command.Parameters.AddWithNullableValue("@CostCenterId", model.CostCenterId);
                    command.Parameters.AddWithNullableValue("@CashRepositoryId", model.CashRepositoryId);
                    command.Parameters.AddWithNullableValue("@PostedDate", model.PostedDate);
                    command.Parameters.AddWithNullableValue("@BankAccountId", model.BankAccountId);
                    command.Parameters.AddWithNullableValue("@PaymentCardId", model.PaymentCardId);
                    command.Parameters.AddWithNullableValue("@BankInstrumentCode", model.BankInstrumentCode);
                    command.Parameters.AddWithNullableValue("@BankTranCode", model.BankTransactionCode);

                    connection.Open();
                    var awaiter = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    return awaiter.To<long>();
                }
            }
        }
    }
}