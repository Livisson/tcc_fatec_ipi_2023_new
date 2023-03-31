using AutoMapper;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Responses;
using GestaoComercio.Domain.Entities;
using GestaoComercio.WebUI.Models.Fornecedor.Commands;
using GestaoComercio.WebUI.Models.Pedido.Commands;
using GestaoComercio.WebUI.Models.Precificacao.Commands;
using GestaoComercio.Application.Models.Despesa.Commands;
using GestaoComercio.WebUI.Models.Despesa.Command;
using GestaoComercio.Application.Models.Caixa.Commands;
using GestaoComercio.WebUI.Models.Caixa.Command;
using System.Collections.Generic;
using GestaoComercio.WebUI.Models.NomeProdutos.Commands;
using GestaoComercio.Application.Models.NomeProdutos.Commands;
using GestaoComercio.Application.Models.Usuario.Commands;
using GestaoComercio.WebUI.Models.Usuario.Commands;

namespace GestaoComercio.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<UsuarioDTO, UsuarioResponse>().ReverseMap();
            CreateMap<DespesaDTO, DespesaResponse>().ReverseMap();

            CreateMap<TelaPedidoResponse, PedidoDTO>()
                .ForMember(dest => dest.ValorCompra, map => map.MapFrom(src => src.ValorCompra))
                .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.DataVencimento, map => map.MapFrom(src => src.DataPagamento));

            CreateMap<PedidoDTO, TelaPedidoResponse>()
                .ForMember(dest => dest.ValorCompra, map => map.MapFrom(src => src.ValorCompra))
                .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.DataPagamento, map => map.MapFrom(src => src.DataVencimento));

            CreateMap<ProdutoDTO, TelaPedidoResponse>()
                .ForMember(dest => dest.NomeProduto, map => map.MapFrom(src => src.Nome))
                .ForMember(dest => dest.CodigoBarras, map => map.MapFrom(src => src.CodigoBarras));

            CreateMap<ProdutoDTO, TelaPrecificacaoResponse>()
                .ForMember(dest => dest.NomeProduto, map => map.MapFrom(src => src.Nome))
                .ForMember(dest => dest.CodigoBarras, map => map.MapFrom(src => src.CodigoBarras))
                .ForMember(dest => dest.ValorCompra, map => map.MapFrom(src => src.EspecificacoesDeProduto))
                .ForMember(dest => dest.Estoque, map => map.MapFrom(src => src.QtdEstoqueTotal))
                .ForMember(dest => dest.PerDesconto, map => map.MapFrom(src => src.PerDesconto))
                .ForMember(dest => dest.PerMargem, map => map.MapFrom(src => src.PerMargem))
                .ForMember(dest => dest.ValorSugerido, map => map.MapFrom(src => src.ValorSugerido))
                .ForMember(dest => dest.ValorVenda, map => map.MapFrom(src => src.ValorVenda));

            CreateMap<DespesaDTO, Despesa>().ReverseMap();
            CreateMap<CaixaDTO, Caixa>().ReverseMap();
            CreateMap<EspecificacaoProdutoDTO, EspecificacaoProduto>().ReverseMap();
            CreateMap<FornecedorDTO, Fornecedor>().ReverseMap();
            CreateMap<PedidoDTO, Pedido>().ReverseMap();
            CreateMap<ProdutoDTO, Produto>().ReverseMap();
            CreateMap<ProdutosVendaDTO, ProdutosVenda>().ReverseMap();
            CreateMap<UsuarioDTO, Usuario>().ReverseMap();
            CreateMap<NomeProdutosDTO, NomeProdutos>().ReverseMap();

            CreateMap<PostPedidoCommand, PostPedidoModel>().ReverseMap();
            CreateMap<PostPrecificacaoCommand, PostPrecificacaoModel>().ReverseMap();
            CreateMap<PostFornecedorCommand, PostFornecedorModel>().ReverseMap();
            CreateMap<PostDespesaCommand, PostDespesaModel>().ReverseMap();
            CreateMap<PostCaixaCommand, PostCaixaModel>().ReverseMap();
            CreateMap<PostNomeProdutosCommand, PostNomeProdutosModel>().ReverseMap();
            CreateMap<PostUsuarioCommand, PostUsuarioModel>().ReverseMap();

            CreateMap<PostFornecedorCommand, Fornecedor>().ReverseMap();
            CreateMap<PostDespesaCommand, Despesa>().ReverseMap();
            CreateMap<PostNomeProdutosCommand, NomeProdutos>().ReverseMap();
            CreateMap<PostUsuarioCommand, Usuario>().ReverseMap();

        }
    }
}

