using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ProcurandoApartamento.Domain.Services.Interfaces;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LanguageExt;
using System;

namespace ProcurandoApartamento.Domain.Services
{
    public class ApartamentoService : IApartamentoService
    {
        protected readonly IApartamentoRepository _apartamentoRepository;

        public ApartamentoService(IApartamentoRepository apartamentoRepository)
        {
            _apartamentoRepository = apartamentoRepository;
        }

        public virtual async Task<Apartamento> Save(Apartamento apartamento)
        {
            await _apartamentoRepository.CreateOrUpdateAsync(apartamento);
            await _apartamentoRepository.SaveChangesAsync();
            return apartamento;
        }

        public virtual async Task<IPage<Apartamento>> FindAll(IPageable pageable)
        {
            var page = await _apartamentoRepository.QueryHelper()
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<Apartamento> FindOne(long id)
        {
            var result = await _apartamentoRepository.QueryHelper()
                .GetOneAsync(apartamento => apartamento.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _apartamentoRepository.DeleteByIdAsync(id);
            await _apartamentoRepository.SaveChangesAsync();
        }

        public virtual async Task<string> FindBestBlock(string[] estabelecimentos)
        {
            var result = string.Empty;
            var apartamentos = await _apartamentoRepository.GetAllAsync();
            estabelecimentos = estabelecimentos.Select(e => e.ToLower()).ToArray();

            if (estabelecimentos.Count() == 1 )
            {
                var estabelecimento = estabelecimentos.First();
                result = "Quadra " + apartamentos.Where(s => s.ApartamentoDisponivel == true
                    && s.Estabelecimento.ToLower() == estabelecimento
                    && s.EstabelecimentoExiste == true).LastOrDefault().Quadra.ToString();
            }
            else{

                var disponiveis = apartamentos.Where(s => s.ApartamentoDisponivel == true
                    && estabelecimentos.Contains(s.Estabelecimento.ToLower())
                    && s.EstabelecimentoExiste == true)
                    .OrderBy(s => Array.IndexOf(estabelecimentos, s.Estabelecimento.ToLower())).ToList();

                result = "Quadra " + disponiveis.Where(x => x.Estabelecimento.ToLower() == estabelecimentos.FirstOrDefault()).LastOrDefault().Quadra.ToString();
            }
            
            return result;
        }
    }
}
