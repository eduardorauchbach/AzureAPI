using Microsoft.Extensions.Configuration;
using RauchTech.DataExtensions.AzureBlob;
using System;
using System.IO;

namespace RauchTech.DataExtensions.AzureBlob
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly AzureBlobHelper _blobHelper;

        public AzureBlobService(IConfiguration configuration)
        {
            _blobHelper = new AzureBlobHelper(configuration);
        }

        #region Change Data

        public AzureBlobFile Insert(AzureBlobFile blobFile)
        {
            try
            {
                if (string.IsNullOrEmpty(blobFile.ID))
                {
                    if (!string.IsNullOrEmpty(blobFile?.Name) && blobFile?.Data != null)
                    {
                        blobFile.ID = Guid.NewGuid().ToString();
                        blobFile.ID += Path.GetExtension(blobFile.Name);
                        blobFile = _blobHelper.InsertOrUpdate(blobFile);
                    }
                    else
                    {
                        throw new Exception("Informações insuficientes");
                    }
                }
                else
                {
                    throw new Exception("ID diferente de vazio, avalie a utilização do PUT");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blobFile;
        }

        public AzureBlobFile Update(AzureBlobFile blobFile)
        {
            string oldID;

            try
            {
                if (string.IsNullOrEmpty(blobFile.ID))
                {
                    throw new Exception("ID vazio, avalie a utilização do POST");
                }
                else
                {
                    if (!string.IsNullOrEmpty(blobFile?.Name) && blobFile?.Data != null)
                    {
                        oldID = blobFile.ID;
                        _blobHelper.InsertOrUpdate(blobFile, oldID);
                    }
                    else
                    {
                        throw new Exception("Informações insuficientes");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blobFile;
        }

        public void Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID inválido");
                }
                else
                {
                    _blobHelper.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public AzureBlobFile Get(string id)
        {
            AzureBlobFile blobFile;

            try
            {
                blobFile = _blobHelper.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blobFile;
        }

        #endregion
    }
}
