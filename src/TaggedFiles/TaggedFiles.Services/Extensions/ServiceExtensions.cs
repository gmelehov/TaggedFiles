using Microsoft.Extensions.DependencyInjection;
using TaggedFiles.Models.Services;

namespace TaggedFiles.Services.Extensions;


public static class ServiceExtensions
{



  public static IServiceCollection AddTaggedFilesServices(this IServiceCollection scoll) =>
    scoll
    .AddScoped<ITaggedFilesService, TaggedFilesService>()
    ;



}
