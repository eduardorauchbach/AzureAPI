using FluentValidation;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RauchTech.Common.Configuration;
using RauchTech.DataExtensions.AzureBlob;

namespace Demo.API.Domain.Model
{
    public class Candidate
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public string FileID { get; set; }
        public string File { get; private set; }

        public AzureBlobFile BlobFile { get; set; }

        public void LoadUrls(IConfiguration config)
        {
            if (!string.IsNullOrEmpty(FileID))
            {
                File = config.GetValue("Blob:BaseURL")[0] + FileID;
            }
        }

        public List<CandidateJob> CandidateJobs { get; set; }
    }

    public class ValidatorCandidate : AbstractValidator<Candidate>
    {
        public ValidatorCandidate()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Favor preencher o campo");
            RuleFor(x => x.Name).Must(x => x.Length <= 50).WithMessage("Tamanho máximo excedido: 50");
            RuleFor(x => x.FileID).Must(x => x.Length <= 50).WithMessage("Tamanho máximo excedido: 50");
        }
    }
}
