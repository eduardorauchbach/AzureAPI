using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RauchTech.Common.Model
{
    public class PageModel
    {
        /// <summary>
        /// Página atual, sendo indice base 1
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Items desejados por página
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Página atual, sendo indice base 1
        /// </summary>
        public int PageCount { get { return TotalPages(); } }
        /// <summary>
        /// Quantidade total de itens nesse filtro
        /// </summary>
        public int ItemsCount { get; set; }
        /// <summary>
        /// Ordenado por propriedade
        /// </summary>
        public List<(string, bool)> OrderBy { get; set; }

        #region Helpers

        public string ToOrderByScript(string prefix = null)
        {
            PageModel page = this;

            string result = string.Empty;

            if (page?.OrderBy?.Count > 0)
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    prefix += ".";
                }

                result = $" ORDER BY {string.Join(", ", page.OrderBy.Select(x => prefix + x.Item1 + (x.Item2 ? " ASC" : " DESC")))}";
            }

            return result;
        }

        public string ToFetchScript()
        {
            PageModel page = this;

            string result = string.Empty;

            if (page.CurrentPage != 0 && page.PageSize > 0)
            {
                result = $" OFFSET {(page.CurrentPage - 1) * page.PageSize} ROWS FETCH NEXT {page.PageSize} ROWS ONLY";
            }

            return result;
        }

        public int TotalPages()
        {
            PageModel page = this;

            int result = 0;

            if (page.ItemsCount > 0 && page.PageSize > 0)
            {
                result=(page.ItemsCount / page.PageSize) + (page.ItemsCount % page.PageSize == 0 ? 0 : 1);
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Modelo de Paginação utilizado pela aplicação
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel<T> : PageModel where T : class
    {
        /// <summary>
        /// resultado
        /// </summary>
        public List<T> Items { get; set; }
    }
}
