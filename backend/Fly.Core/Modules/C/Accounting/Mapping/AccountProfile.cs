namespace Fly.Core.Modules.C.Accounting.Mapping;

//internal class AccountProfile : Profile
//{
//    public AccountProfile()
//    {
//        CreateMap<TEntity, DtoBase>()
//            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Guid))
//        ;
//        CreateMap<DtoBase, TEntity>()
//            .ForMember(des => des.Id, opt => opt.Ignore())
//            .ForMember(des => des.Guid, opt => opt.MapFrom(src => src.Id))
//        ;

//        CreateMap<TAccount, DtoAccount>()
//            .IncludeBase<TEntity, DtoBase>()

//        ;
//        CreateMap<DtoAccount, TAccount>()
//            .IncludeBase<DtoBase, TEntity>()
//        ; 
//    }
//}

//internal class GuidToIdentifierValueResolver : IValueResolver<DtoBase, TEntity, Identifier>
//{
//    private readonly IServiceBase<DtoBase, TEntity> _service;

//    public GuidToIdentifierValueResolver(IServiceBase<DtoBase, TEntity> service)
//    {
//        _service = service;
//    }

//    public Identifier Resolve(DtoBase source, TEntity destination, Identifier destMember, ResolutionContext context)
//    {
//        if (source.IsNew) return Identifier.Empty;
//        var id = _service.GetId(source.Id).Result;
//        return id.Value;
//    }
//}