using AutoMapper;
using StackExchange.Redis;

try
{
    var configurationA = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfileA>());
    configurationA.AssertConfigurationIsValid();

    Console.WriteLine("ConfigurationA is valid");
}
catch (Exception ex)
{
    Console.WriteLine("ConfigurationA is INVALID");
}

try
{
    var configurationB = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfileB>());
    configurationB.AssertConfigurationIsValid();

    Console.WriteLine("ConfigurationB is valid");
}
catch (Exception ex)
{
    Console.WriteLine("ConfigurationB is INVALID");
}

try
{
    var configurationC = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfileC>());
    configurationC.AssertConfigurationIsValid();

    Console.WriteLine("ConfigurationC is valid");
}
catch (Exception ex)
{
    Console.WriteLine("ConfigurationC is INVALID");
}

record TestModelB(string Id, DateTimeOffset EventTime)
{
    private TestModelB()
        : this(string.Empty, default)
    {
    }
}

class AutomapperProfileA : Profile
{
    public AutomapperProfileA() 
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => DateTimeOffset.Parse(src["EventTime"])))
            .ForAllMembers(opt => opt.MapFrom(src => src[opt.DestinationMember.Name]));
}

class AutomapperProfileB : Profile
{
    public AutomapperProfileB()
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((src, _) => src.Id))
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom((src, _) => DateTimeOffset.Parse(src["EventTime"])))
            .ForAllMembers(opt => opt.MapFrom(src => src[opt.DestinationMember.Name]));
}

class AutomapperProfileC : Profile
{
    public AutomapperProfileC()
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((src, _) => src.Id))
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom((src, _) => DateTimeOffset.Parse(src["EventTime"])))
            .ForAllMembers(opt => opt.MapFrom((src, _) => src[opt.DestinationMember.Name]));
}