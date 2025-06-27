using Microsoft.EntityFrameworkCore;
using TaggedFiles.Data;
using TaggedFiles.Services.Extensions;
using TaggedFiles.WorkerService;


var builder = Host.CreateApplicationBuilder(args);
builder.Services
  .AddDbContext<TaggedFilesDbContext>(opts =>
  {
    opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaggedFiles;Trusted_Connection=True;MultipleActiveResultSets=true");
  })
  .AddHostedService<Worker>()
  .AddTaggedFilesServices();


var host = builder.Build();
host.Run();
