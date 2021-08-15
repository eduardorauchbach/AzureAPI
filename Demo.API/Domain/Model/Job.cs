using FluentValidation;
using System.Collections.Generic;

namespace Demo.API.Domain.Model
{
    public class Job
    {
        #region Data Base Parse

        //(Optional, used to parse Db Column Names against Model Property Names)
        public static readonly Dictionary<string, string> ColumnsLibrary = new Dictionary<string, string>
        {
            { "id", "ID"},
            { "title", "Title"},
            { "description", "Description"}
        };

        #endregion

        public long ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class ValidatorJob : AbstractValidator<Job>
    {
        public ValidatorJob()
        {
            RuleFor(x => x.ID).NotEmpty().WithMessage("Favor preencher o campo");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Favor preencher o campo");
            RuleFor(x => x.Title).Must(x => x.Length <= 50).WithMessage("Tamanho máximo excedido: 50");
        }
    }
}
