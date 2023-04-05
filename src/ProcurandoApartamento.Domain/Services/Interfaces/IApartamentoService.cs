using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ProcurandoApartamento.Domain;

namespace ProcurandoApartamento.Domain.Services.Interfaces
{
    public interface IApartamentoService
    {
        Task<Apartamento> Save(Apartamento apartamento);

        Task<IPage<Apartamento>> FindAll(IPageable pageable);

        Task<Apartamento> FindOne(long id);

        Task<string> FindBestBlock(string[] valores);

        Task Delete(long id);
    }
}
