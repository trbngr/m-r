using System;
using System.IO;

using Lokad.Cqrs;
using Lokad.Cqrs.Build.Client;
using Lokad.Cqrs.Build.Engine;
using Lokad.Cqrs.Core.Directory.Default;

using SimpleCQRS;
using SimpleCQRS.Domain;
using SimpleCQRS.Messages;
using SimpleCQRS.ReadModel;

using Autofac;

namespace CQRSGui
{
    public static class Bootstrapper
    {
        private static readonly FileStorageConfig storageConfig;

        static Bootstrapper()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_data");
            var storageDirectory = new DirectoryInfo(path);
            storageConfig = new FileStorageConfig(storageDirectory, "simplecqrs");
        }

        public static CqrsEngineHost Boot()
         {
            //Used by the MVC app to send messages and read entities. 
             ConfigureClient();

             //Configure the engine host to process commands and events.
             return ConfigureHost();
         }

        private static CqrsEngineHost ConfigureHost()
         {
             var builder = new CqrsEngineBuilder();

             builder.Domain(d =>
             {
                 d.InAssemblyOf<CreateInventoryItem>();
                 d.HandlerSample<IConsume<Define.Command>>(m => m.Consume(null));
             });

             builder.File(f =>
             {
                 f.AddFileSender(storageConfig, "events");
                 f.AddFileProcess(storageConfig, "commands", p =>
                 {
                     p.WhereFilter(x => x.WhereMessagesAre<Define.Command>());
                     p.DispatchAsCommandBatch();
                 });
                 f.AddFileProcess(storageConfig, "events", p=>
                 {
                     p.WhereFilter(x => x.WhereMessagesAre<DomainEvent>());
                     p.DispatchAsEvents();
                 });
             });

             builder.Storage(s =>
             {
                 s.TapeIsInFiles(Path.Combine(storageConfig.Folder.FullName, "eventstore"));
                 s.AtomicIsInFiles(storageConfig.Folder.FullName, b =>
                 {
                     b.WhereEntityIs<IEntity>();
                     b.WhereSingletonIs<ISingleton>();
                 });
                 s.StreamingIsInFiles(storageConfig.Folder.FullName);
             });

             builder.Advanced.ConfigureContainer(c =>
             {
                 c.RegisterType<ReadModelFacade>().As<IReadModelFacade>();
                 
                 c.RegisterType<Repository<InventoryItem>>().As<IRepository<InventoryItem>>();
                 
                 c.RegisterType<EventStore>().As<IEventStore>();
             });

             return builder.Build();
        }

        private static void ConfigureClient()
        {
            var builder = new CqrsClientBuilder();

            builder.File(f => f.AddFileSender(storageConfig, "commands"));

            builder.Domain(d => d.InAssemblyOf<CreateInventoryItem>());

            builder.Storage(s =>
            {
                s.AtomicIsInFiles(storageConfig.Folder.FullName, b =>
                {
                    b.WhereEntityIs<IEntity>();
                    b.WhereSingletonIs<ISingleton>();
                });
                s.StreamingIsInFiles(storageConfig.Folder.FullName);
            });

            builder.Advanced.ConfigureContainer(c=> c.RegisterType<ReadModelFacade>().As<IReadModelFacade>());

            var client = builder.Build();

            ServiceLocator.Bus = client.Resolve<IMessageSender>();
            ServiceLocator.ReadModel = client.Resolve<IReadModelFacade>();}
    }
}