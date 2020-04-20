using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using log4net;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace TesteDois.Repos
{
        public class DynamoDBHelper : IDisposable
    {
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        AmazonDynamoDBClient client;
            private static readonly ILog _logger = LogManager.GetLogger(typeof(DynamoDBHelper));
            private readonly string accessKeyId, secretKey, serviceUrl;
        
            public DynamoDBHelper(string accessKeyId, string secretKey, string serviceUrl)
            {
                this.accessKeyId = accessKeyId;
                this.secretKey = secretKey;
                this.serviceUrl = serviceUrl;
                client = GetClient();
            }

            /// <summary>
                    /// Initializes and returns the DynamoDBClient object
                    /// </summary>
                    /// <returns></returns>
            private AmazonDynamoDBClient GetClient()
            {
                if (client == null)
                {
                    try
                    {
                        // DynamoDB config object
                        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
                        {
                            // Set the endpoint URL
                            ServiceURL = serviceUrl
                        };
                        client = new AmazonDynamoDBClient(accessKeyId, secretKey, clientConfig);
                    }
                    catch (AmazonDynamoDBException ex)
                    { _logger.Error($"Error (AmazonDynamoDBException) creating dynamodb client", ex); }
                    catch (AmazonServiceException ex)
                    { _logger.Error($"Error (AmazonServiceException) creating dynamodb client", ex); }
                    catch (Exception ex)
                    { _logger.Error($"Error creating dynamodb client", ex); }
                }
                return client;
            }

            /// <summary>
                    /// Get all the records from the given table
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <returns></returns>
            public async Task<IList<T>> GetAll<T>()
            {
                var context = new DynamoDBContext(GetClient());
                // Here we are passing the ScanCoditions as empty to get all the rows
                List<ScanCondition> conditions = new List<ScanCondition>();
                return await context.QueryAsync<T>(conditions).GetRemainingAsync();
            }

            /// <summary>
                    /// Get the rows from the given table which maches the given key and conditions 
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <param name="keyValue">hash key value</param>
                    /// <param name="scanConditions">any other scan conditions</param>                                                             ₢  n/
                    /// <returns></returns>
            public async Task<IList<T>> GetRows<T>(object keyValue, List<ScanCondition> scanConditions = null)
            {
                var context = new DynamoDBContext(GetClient());
                DynamoDBOperationConfig config = null;

                if (scanConditions != null && scanConditions.Count > 0)
                {
                    config = new DynamoDBOperationConfig()
                    {
                        QueryFilter = scanConditions
                    };
                }
                return await context.QueryAsync<T>(keyValue, config).GetRemainingAsync();
            }

            /// <summary>
                    /// Get the rows from the given table which maches the given conditions 
                    /// </summary>
                    /// <typeparam name="T"> Table object</typeparam>
                    /// <param name="scanConditions"></param>
                    /// <returns></returns>
            public async Task<IList<T>> GetRows<T>(List<ScanCondition> scanConditions)
            {
                var context = new DynamoDBContext(GetClient());
                return await context.ScanAsync<T>(scanConditions).GetRemainingAsync();
            }

            /// <summary>
                    /// Gets a record which matches the given key value
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <param name="keyValue">Hash key value</param>
                    /// <returns></returns>
            public T Load<T>(object keyValue)
            {
                var context = new DynamoDBContext(GetClient());
                return context.LoadAsync<T>(keyValue).Result;
            }

            /// <summary>
                    /// Saves the given record in the table
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <param name="document">Record to save in the table</param>
                    /// <returns></returns>
            public async Task Save<T>(T document)
            {
                var context = new DynamoDBContext(GetClient());
                await context.SaveAsync(document);
            }


            /// <summary>
                    /// Deletes the given record in the table
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <param name="document">Record to be removed from the table</param>
                    /// <returns></returns>
            public async Task Delete<T>(T document)
            {
                var context = new DynamoDBContext(GetClient());
                await context.DeleteAsync(document);
            }

            /// <summary>
                    /// Saves batch of records in the table
                    /// </summary>
                    /// <typeparam name="T">Table object</typeparam>
                    /// <param name="documents">Records to be saved</param>
                    /// <returns></returns>
           
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
    }

    
}