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

/// <summary>
/// Fails validation due to first two properties being mapped using Expression
/// </summary>
class AutomapperProfileA : Profile
{
    public AutomapperProfileA() 
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Using Expression<Func>
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => DateTimeOffset.Parse(src["EventTime"]))) // Using Expression<Func>
            .ForAllMembers(opt => opt.MapFrom(src => src[opt.DestinationMember.Name])); // Using Expression<Func>
}

/// <summary>
/// Succeeds validation due to first two properties being mapped using Func and generic mapping done using Expression
/// </summary>
class AutomapperProfileB : Profile
{
    public AutomapperProfileB()
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((src, _) => src.Id)) // Using Func instead of Expression<Func>
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom((src, _) => DateTimeOffset.Parse(src["EventTime"]))) // Using Func instead of Expression<Func>
            .ForAllMembers(opt => opt.MapFrom(src => src[opt.DestinationMember.Name])); // Still using Expression<Func>
}

/// <summary>
/// Fails validation due to generic mapping being done using Func, not expression
/// </summary>
class AutomapperProfileC : Profile
{
    public AutomapperProfileC()
        => CreateMap<StreamEntry, TestModelB>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((src, _) => src.Id)) // Using Func instead of Expression<Func>
            .ForMember(dest => dest.EventTime, opt => opt.MapFrom((src, _) => DateTimeOffset.Parse(src["EventTime"]))) // Using Func instead of Expression<Func>
            .ForAllMembers(opt => opt.MapFrom((src, _) => src[opt.DestinationMember.Name])); // Using Func instead of Expression<Func>
}